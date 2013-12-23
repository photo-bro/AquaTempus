using System;
using System.Timers;
using System.Collections.Generic;
using System.Linq;
using MonoMac.Foundation;
using MonoMac.AppKit;

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


			/////
			/// Pause Button
			/////
			btnPause.Activated += (object sender, EventArgs e) => {
				lbStroke.StringValue = "btnPause.Activated";
				m_at.OpenFile();

				tbvSetList.DataSource = new TableViewHandler (m_at.SetListTable ());

			};


			/////
			/// Start Button
			/////
			btnStart.Activated += (object sender, EventArgs e) => {

				m_at.OpenFile();
				// Parse file
				sr.Init (m_at.SetList ());

				sr.Start ();
				sr.SetEnded += (object source, ElapsedEventArgs ee) => {
					System.Console.WriteLine ("Set Ended");
				};
				sr.Ticked += (object source, ElapsedEventArgs ee) => {
					lbTimeRemain.StringValue = ee.SignalTime.Second.ToString ();
					Console.WriteLine (ee.SignalTime.Second);
				};
			};

		}

		#endregion



	}
}

