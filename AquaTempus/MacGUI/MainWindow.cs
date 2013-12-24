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

			base.AwakeFromNib ();
			SetRunner sr = SetRunner.Instance;

			// credit 
			// http://stackoverflow.com/questions/19795522/monomac-create-context-in-bacground-thread
			// Disable UIKit thread checks for a couple of methods




			/////
			/// Pause Button
			/////
			btnPause.Activated += (object sender, EventArgs e) => {
				lbStroke.StringValue = "btnPause.Activated";
				m_at.OpenFile ();

				tbvSetList.DataSource = new TableViewHandler (m_at.SetListTable ());

			};

			/////
			/// Stop Button
			/////
			btnStop.Activated += (object sender, EventArgs e) => {
				sr.Stop ();

			};

			/////
			/// Start Button
			/////
			btnStart.Activated += (object sender, EventArgs e) => {

				m_at.OpenFile ();
				// Parse file
				sr.Init (m_at.SetList ());

				sr.Start ();

			};

			/////
			/// Set Completed
			///   Reset countdown clock
			/////
			sr.SetEnded += (object source, SetEndArgs e) => {
				NSApplication.CheckForIllegalCrossThreadCalls = false;

				lbStroke.StringValue = string.Format("{0}x{1} {2} on {3}-- Ended", e.CurrentSet.Number,
					e.CurrentSet.Distance, e.CurrentSet.Stroke, e.CurrentSet.Interval);
//				lbStroke.SizeToFit();
//				Console.WriteLine(string.Format("{0}x{1} {2} on {3} -- Ended", e.CurrentSet.Number,
//					e.CurrentSet.Distance, e.CurrentSet.Stroke, e.CurrentSet.Interval));
				NSApplication.CheckForIllegalCrossThreadCalls = true;

			};

			/////
			/// Tick (one second)
			/// 
			/////
			sr.Ticked += (object source, TickArgs e) => {
				NSApplication.CheckForIllegalCrossThreadCalls = false;

				lbTimeRemain.StringValue = e.TimeRemaining;
//				lbTimeRemain.SizeToFit();
				Console.WriteLine(e.TimeRemaining);
				NSApplication.CheckForIllegalCrossThreadCalls = true;
			};

		}

		#endregion

	}
}

