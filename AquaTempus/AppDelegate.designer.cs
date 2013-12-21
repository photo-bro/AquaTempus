// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using MonoMac.Foundation;
using System.CodeDom.Compiler;

namespace AquaTempus
{
	[Register ("AppDelegate")]
	partial class AppDelegate
	{
		[Outlet]
		MonoMac.AppKit.NSMenuItem miOpen { get; set; }

		[Outlet]
		MonoMac.AppKit.NSMenuItem miSave { get; set; }

		[Outlet]
		MonoMac.AppKit.NSMenuItem miSaveAs { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (miOpen != null) {
				miOpen.Dispose ();
				miOpen = null;
			}

			if (miSave != null) {
				miSave.Dispose ();
				miSave = null;
			}

			if (miSaveAs != null) {
				miSaveAs.Dispose ();
				miSaveAs = null;
			}
		}
	}
}
