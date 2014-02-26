using System;
using System.Timers;
using System.Collections.Generic;
using System.Linq;
using MonoMac.Foundation;
using MonoMac.AppKit;
using MonoMac.ObjCRuntime;

namespace AquaTempus
{
	public partial class MainWindow : MonoMac.AppKit.NSWindow
	{

		#region Constructors

		// Called when created from unmanaged code
		public MainWindow (IntPtr handle) : base (handle)
		{
			Initialize ();
		}
		// Called when created directly from a XIB file
		[Export ("initWithCoder:")]
		public MainWindow (NSCoder coder) : base (coder)
		{
			Initialize ();
		}
		// Shared initialization code
		void Initialize ()
		{
		}

		#endregion

		#region WindowEvents

		AT_Facade m_at = AT_Facade.Instance;
		int m_iSec = 0;
		bool m_bCountDescending = true;

		public override void AwakeFromNib ()
		{
			///////
			/// Glorified Event "Handler"
			///////
			/// All outside calls from outside this class that update the GUI
			/// need to be wrapped around the:
			/// NSApplication.CheckForIllegalCrossThreadCalls = false/true;
			/// for the updates to properly work. 

			// *************************** //
			// TODO
			//
			// Make each button function properly
			//  - Pause (Doesnt reset tickTimer)
			//  - Next / Prev (Reset timers properly)

			base.AwakeFromNib ();
			SetRunner sr = SetRunner.Instance;

			NSApplication.CheckForIllegalCrossThreadCalls = false;
			updateGUI ();
			NSApplication.CheckForIllegalCrossThreadCalls = true;


			// Component settings
			tbvSetList.AllowsColumnSelection = false;

			/////
			/// TBV clicked
			/////
			tbvSetList.Activated += (object sender, EventArgs e) => {
				updateTBV ();
			};

			// for countdown timer
			Timer tickTimer = new Timer (1000);
			// credit for disabling cross thread calls in NSApplication:
			// http://stackoverflow.com/questions/19795522/monomac-create-context-in-bacground-thread
			// Disable UIKit thread checks for a couple of methods

			/////
			/// Pause Button
			/////
			btnPause.Activated += (object sender, EventArgs e) => {
				// Check if a file has been opened
				if (!m_at.FileOpen ()) {	
					alertFileNotOpen ();
					return;
				}

				// call for pause
				sr.Pause ();

				// stop ticktimer
				tickTimer.Stop ();

				// update gui
				NSApplication.CheckForIllegalCrossThreadCalls = false;
				updateGUI ();
				NSApplication.CheckForIllegalCrossThreadCalls = true;
			};

			/////
			/// Stop Button
			/////
			btnStop.Activated += (object sender, EventArgs e) => {
				// Check if a file has been opened
				if (!m_at.FileOpen ()) {
					alertFileNotOpen ();
					return;
				}

				// call for stop (resets set pointer)
				sr.Stop ();

				// stop and reset ticktimer
				tickTimer.Stop ();
				m_iSec = 0;
				tickTimer = new Timer (1000);

				// update gui
				NSApplication.CheckForIllegalCrossThreadCalls = false;
				updateGUI ();
				NSApplication.CheckForIllegalCrossThreadCalls = true;

			};

			/////
			/// Start Button
			/////
			btnStart.Activated += (object sender, EventArgs e) => {
				// Check if a file has been opened
				if (!m_at.FileOpen ()) {
					alertFileNotOpen ();
					return;
				}

				// start timers
				sr.Start ();
				tickTimer.Start ();

				// update gui
				NSApplication.CheckForIllegalCrossThreadCalls = false;
				updateGUI ();
				NSApplication.CheckForIllegalCrossThreadCalls = true;

			};

			/////
			/// Next Button
			/////
			btnNext.Activated += (object sender, EventArgs e) => {
				// Check if a file has been opened
				if (!m_at.FileOpen ()) {
					alertFileNotOpen ();
					return;
				}

				// stop tickTimer
				tickTimer.Stop ();
				tickTimer.Dispose ();
				m_iSec = 0;

				// call next set
				sr.Next ();

				// reset tickTimer
				tickTimer = new Timer (1000);
				tickTimer.Start ();

				// update gui
				NSApplication.CheckForIllegalCrossThreadCalls = false;
				updateGUI ();
				NSApplication.CheckForIllegalCrossThreadCalls = true;
			};

			/////
			/// Previous Button
			/////
			btnPrev.Activated += (object sender, EventArgs e) => {
				// Check if a file has been opened
				if (!m_at.FileOpen ()) {
					alertFileNotOpen ();
					return;
				}

				// stop tickTimer
				tickTimer.Stop ();
				tickTimer.Dispose ();
				m_iSec = 0;

				// call previous set
				sr.Previous ();

				// reset tickTimer
				tickTimer = new Timer (1000);
				tickTimer.Start ();

				// update gui
				NSApplication.CheckForIllegalCrossThreadCalls = false;
				updateGUI ();
				NSApplication.CheckForIllegalCrossThreadCalls = true;
			};


			/////
			/// tick
			/// countdown clock tick
			/////
			tickTimer.Elapsed += (object sender, ElapsedEventArgs e) => {
				NSApplication.CheckForIllegalCrossThreadCalls = false;

				updateCountDown ();
				NSApplication.CheckForIllegalCrossThreadCalls = true;

			};

			/////
			/// Set Completed
			///   Reset countdown clock
			/////
			sr.SetEnded += (object source, SetEndArgs e) => {
				Console.WriteLine (string.Format ("{0}-- Ended", e.CurrentSet.ToString ()));

				m_iSec = 0;
			};

			/////
			/// ResetCount
			///  reset countdown clock
			/////
			sr.ResetCount += (object source, EventArgs e) => {
				NSApplication.CheckForIllegalCrossThreadCalls = false;
			
				updateGUI ();

				NSApplication.CheckForIllegalCrossThreadCalls = true;

				m_iSec = 0;
				tickTimer = new Timer (1000);
				tickTimer.Start ();
			};

			/////
			/// Tab Selected
			/////
			tabvMain.DidSelect += (object sender, NSTabViewItemEventArgs e) => {
				// load error log into console
				tbConsole.Value = AQ_Exceptions.AQ_Exception_Log ();
			};

		}

		#endregion

		#region GUI_Updates

		/// <summary>
		/// Update GUI elements. Needs to be wrapped in:
		/// NSApplication.CheckForIllegalCrossThreadCalls = false/true;
		/// to work properly.
		/// </summary>
		public void updateGUI ()
		{
			SetRunner sr = SetRunner.Instance;
			Set curSet = sr.CurrentSet;

			if (curSet == null) {
				// "blank" values
				lbDistRemain.StringValue = "0x000";
				lbNote.StringValue = "------------";
				lbStats.StringValue = "------------";
				lbStroke.StringValue = "------------";
				lbTimeRemain.StringValue = "--:--";
				return;
			}

			// labels to be updated
			lbNote.StringValue = curSet.Comment;
			lbStats.StringValue = string.Format ("Total Distance Swam: {0}yds{1}Total Time Elapsed: {2}"
				, sr.TotalDistanceSwam
				, Environment.NewLine
				, Set.IntervalToString (sr.TotalTimeElapsed));
			lbStats.SizeToFit ();
			lbStroke.StringValue = curSet.Stroke;
			lbDistRemain.StringValue = string.Format ("{0}x{1}"
				, sr.CurrentSet.Number - sr.CurrentNum 
				, sr.CurrentSet.Distance);

			updateTBV ();
			updateCountDown ();

		}

		/// <summary>
		/// Update main TBV values. Needs to be wrapped in:
		/// NSApplication.CheckForIllegalCrossThreadCalls = false/true;
		/// to work properly. 
		/// </summary>
		public void updateTBV ()
		{
			// highlight current set in tableview
			tbvSetList.DeselectAll (this);
			try {
				tbvSetList.SelectRow ((tbvSetList.DataSource as TableViewHandler).getSetRow (SetRunner.Instance.CurrentSet), true);
			} catch (Exception e) {
				// do nothing
				Console.WriteLine (e.ToString ());
			}
		}

		/// <summary>
		/// Update countdown clock. Needs to be wrapped in:
		/// NSApplication.CheckForIllegalCrossThreadCalls = false/true;
		/// to work properly.
		/// </summary>
		public void updateCountDown ()
		{
			if (m_bCountDescending)
				lbTimeRemain.StringValue = Set.IntervalToString (SetRunner.Instance.CurrentSet.IntervalInt - m_iSec++);
			else
				lbTimeRemain.StringValue = Set.IntervalToString (m_iSec++);

		}

		#endregion

		/// <summary>
		/// Alert for FileNotOpen. Wrapper function of AQ_Exception class.
		/// </summary>
		private void alertFileNotOpen ()
		{
			AQ_Exceptions.AQ_Exception (AQ_EXCEPTION_CODE.FILE_NOT_OPEN
				, "File not opened. Please open a set file"
				, true
				, this);
			return;
		}
	}
}

