using System;
using System.Drawing;
using MonoMac.Foundation;
using MonoMac.AppKit;
using MonoMac.ObjCRuntime;

namespace AquaTempus
{
	public class SharedWindowMethods : MonoMac.AppKit.AppKitFramework
	{
		private static AT_Facade m_at = AT_Facade.Instance;

		public static string OpenFilePanel ()
		{
			// Open file prompt
			// Credit user: rjm
			// http://forums.xamarin.com/discussion/3876/regression-in-nsopenpanel
			NSOpenPanel openPanel = new NSOpenPanel ();
			string s = "";
			openPanel.Begin (((int result) => {
				try {
					if (openPanel.Url != null) 
						s = openPanel.Url.Path;
						
				} finally {
					openPanel.Dispose ();
				}
			}));
			return s;
		}

		public static void SaveCurrentSetFilePanel ()
		{
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
		}
	}
}

