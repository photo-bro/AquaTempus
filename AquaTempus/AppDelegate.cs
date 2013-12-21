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
				NSSavePanel savePanel = new NSSavePanel ();
				savePanel.Begin (((int result) => {
					try {
						if (savePanel.Url != null) {
							var urlString = savePanel.Url.Path;

							if (!string.IsNullOrEmpty (urlString)) {
								m_at.SaveCurrentSet (System.IO.Path.GetFileName (urlString),
									System.IO.Path.GetDirectoryName (urlString));
							} // string not null
						} // url not null
					} finally {
						savePanel.Dispose ();
					} // finally
				}));
			};

			/////
			/// Open File Menu Item
			/////
			miOpen.Activated += (object sender, EventArgs e) => {
				// Open file prompt
				// Credit user: rjm
				// http://forums.xamarin.com/discussion/3876/regression-in-nsopenpanel
				NSOpenPanel openPanel = new NSOpenPanel ();
				openPanel.Begin (((int result) => {
					try {
						if (openPanel.Url != null) {
							var urlString = openPanel.Url.Path;

							if (!string.IsNullOrEmpty (urlString)) {
								m_at.OpenFile (System.IO.Path.GetFileName (urlString),
									System.IO.Path.GetDirectoryName (urlString));
								winMain.tbConsole.Value = urlString + " Opened" + Environment.NewLine;

								winMain.tbConsole.Value += m_at.GetTokenList ();

							}
						}
					} finally {
						openPanel.Dispose ();
					}
				}));

			};


		}
	}
}

