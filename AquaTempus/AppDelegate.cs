using System;
using System.Drawing;
using MonoMac.Foundation;
using MonoMac.AppKit;
using MonoMac.ObjCRuntime;

namespace AquaTempus
{
	public partial class AppDelegate : NSApplicationDelegate
	{
		MainWindowController mainWindowController;
		AT_Facade m_at = AT_Facade.Instance;
		SetRunner m_sr = SetRunner.Instance;

		public AppDelegate ()
		{
		}

		public override void FinishedLaunching (NSObject notification)
		{
			mainWindowController = new MainWindowController ();
			mainWindowController.Window.MakeKeyAndOrderFront (this);

			MainWindow winMain = mainWindowController.Window;

			/////
			/// SaveAs File Menu Item
			/////
			miSaveAs.Activated += (object sender, EventArgs e) => {
				SharedWindowMethods.SaveCurrentSetFilePanel();
			};

			/////
			/// Open File Menu Item
			/////
			miOpen.Activated += (object sender, EventArgs e) => {
				// Call for openPanel
				// Credit user: rjm
				// http://forums.xamarin.com/discussion/3876/regression-in-nsopenpanel
				NSOpenPanel openPanel = new NSOpenPanel ();
				openPanel.Begin (((int result) => {
					try {
						if (openPanel.Url != null) {
							// get path
							var file = openPanel.Url.Path;

							// open file
							m_at.OpenFile(System.IO.Path.GetFileName(file), System.IO.Path.GetDirectoryName(file));

							// parse open file
							m_at.InitSet();

							// init SetRunner
							m_sr.Init(m_at.SetList());

							// update table
							winMain.tbvSetList.DataSource = new TableViewHandler(m_at.SetListTable());

						}
					} finally {
						openPanel.Dispose ();
					}
				}));
			};


		}
	}
}

