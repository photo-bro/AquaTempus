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

//			tbConsole.Value = "Hello and welcome to Aqua:Tempus ! ";
		
		}

		#endregion

		#region WindowEvents

		AT_Facade m_at = AT_Facade.Instance;

		public override void AwakeFromNib ()
		{
			///////
			/// Glorified Event "Handler"
			///////
			/// 

			base.AwakeFromNib ();
			SetRunner sr = SetRunner.Instance;

			NSApplication.CheckForIllegalCrossThreadCalls = false;
			updateGUI ();
			NSApplication.CheckForIllegalCrossThreadCalls = true;
			;

			// for countdown timer
			Timer tickTimer = new Timer (1000);
			int iSec = 0;
			// credit for disabling cross thread calls in NSApplication:
			// http://stackoverflow.com/questions/19795522/monomac-create-context-in-bacground-thread
			// Disable UIKit thread checks for a couple of methods

			/////
			/// Pause Button
			/////
			btnPause.Activated += (object sender, EventArgs e) => {
				// Check if a file has been opened
				if (!m_at.FileOpen ())
					return;

				// call for pause
				sr.Pause ();
				// stop ticktimer
				tickTimer.Stop ();
			};

			/////
			/// Stop Button
			/////
			btnStop.Activated += (object sender, EventArgs e) => {
				// Check if a file has been opened
				if (!m_at.FileOpen ())
					return;

				// call for stop (resets set pointer)
				sr.Stop ();

				// stop and reset ticktimer
				tickTimer.Stop ();
				iSec = 0;
				tickTimer = new Timer (1000);

			};

			/////
			/// Start Button
			/////
			btnStart.Activated += (object sender, EventArgs e) => {
				// Check if a file has been opened
				if (!m_at.FileOpen ())
					return;

				// start timers
				sr.Start ();
				tickTimer.Start ();

				// update gui
				updateGUI ();
			};

			/////
			/// Next Button
			/////
			btnNext.Activated += (object sender, EventArgs e) => {
				// Check if a file has been opened
				if (!m_at.FileOpen ())
					return;
			
				// call next set
				sr.Next ();

				// reset tickTimer
				iSec = 0;
				tickTimer = new Timer (1000);
				tickTimer.Start ();

				// update gui
				updateGUI ();
			};

			/////
			/// Previous Button
			/////
			btnPrev.Activated += (object sender, EventArgs e) => {
				// Check if a file has been opened
				if (!m_at.FileOpen ())
					return;

				// call previous set
				sr.Previous ();

				// reset tickTimer
				iSec = 0;
				tickTimer = new Timer (1000);
				tickTimer.Start ();

				// update gui
				updateGUI ();
			};

			/////
			/// tick
			/// countdown clock tick
			/////
			tickTimer.Elapsed += (object sender, ElapsedEventArgs e) => {
				NSApplication.CheckForIllegalCrossThreadCalls = false;

				lbTimeRemain.StringValue = Set.IntervalToString (sr.CurrentSet.IntervalInt - iSec++);

				NSApplication.CheckForIllegalCrossThreadCalls = true;
			};

			/////
			/// Set Completed
			///   Reset countdown clock
			/////
			sr.SetEnded += (object source, SetEndArgs e) => {
				NSApplication.CheckForIllegalCrossThreadCalls = false;

				lbStroke.StringValue = string.Format ("{0}x{1} {2} on {3}-- Ended", e.CurrentSet.Number,
					e.CurrentSet.Distance, e.CurrentSet.Stroke, e.CurrentSet.Interval);
				Console.WriteLine (string.Format ("{0}x{1} {2} on {3} -- Ended", e.CurrentSet.Number,
					e.CurrentSet.Distance, e.CurrentSet.Stroke, e.CurrentSet.Interval));
				iSec = 0;

				NSApplication.CheckForIllegalCrossThreadCalls = true;

			};

			/////
			/// ResetCount
			///  reset countdown clock
			/////
			sr.ResetCount += (object source, EventArgs e) => {
				NSApplication.CheckForIllegalCrossThreadCalls = false;
			
				updateGUI ();

				NSApplication.CheckForIllegalCrossThreadCalls = true;

				iSec = 0;
				tickTimer = new Timer (1000);
				tickTimer.Start ();
			};

		}

		#endregion

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
			lbStats.StringValue = ""; // TODO
			lbStroke.StringValue = curSet.Stroke;
			lbDistRemain.StringValue = string.Format ("{0}x{1}"
				, sr.CurrentSet.Number - sr.CurrentNum 
				, sr.CurrentSet.Distance);

			// highlight current set in tableview
			tbvSetList.DeselectAll (this);
			tbvSetList.SelectRow ((tbvSetList.DataSource as TableViewHandler).getSetRow (curSet), true);

		}
	}
}

