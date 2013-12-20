using System;
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

		AT_Facade m_at = AT_Facade.Instance;

		public override void AwakeFromNib ()
		{
			base.AwakeFromNib ();


			//tbvSetList.DataSource = new TableViewHandler (m_at.SetListTable());


			// Start button
			btnStart.Activated += (object sender, EventArgs e) => {

				// Open file prompt
				// Credit user: rjm
				// http://forums.xamarin.com/discussion/3876/regression-in-nsopenpanel
				NSOpenPanel openPanel = new NSOpenPanel ();
				openPanel.BeginSheet (this, ((int result) => {
					try {
						if (openPanel.Url != null) {
							var urlString = openPanel.Url.Path;

							if (!string.IsNullOrEmpty (urlString)) {
								m_at.OpenFile (System.IO.Path.GetFileName (urlString),
									System.IO.Path.GetDirectoryName (urlString));
								tbConsole.Value = urlString + " Opened" + Environment.NewLine;

								tbConsole.Value += m_at.GetTokenList ();

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

