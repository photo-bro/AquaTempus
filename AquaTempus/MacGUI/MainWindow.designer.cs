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
	[Register ("MainWindow")]
	partial class MainWindow
	{
		[Outlet]
		MonoMac.AppKit.NSButton btnNext { get; set; }

		[Outlet]
		MonoMac.AppKit.NSButton btnPause { get; set; }

		[Outlet]
		MonoMac.AppKit.NSButton btnPrev { get; set; }

		[Outlet]
		MonoMac.AppKit.NSButton btnStart { get; set; }

		[Outlet]
		MonoMac.AppKit.NSButton btnStop { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTextField lbDistRemain { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTextField lbNote { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTextField lbStats { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTextField lbStroke { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTextField lbTimeRemain { get; set; }

		[Outlet]
		MonoMac.AppKit.NSTabView tabvMain { get; set; }

		[Outlet]
		public MonoMac.AppKit.NSTextView tbConsole { get; private set; }

		[Outlet]
		public MonoMac.AppKit.NSTableView tbvSetList { get; private set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (tabvMain != null) {
				tabvMain.Dispose ();
				tabvMain = null;
			}

			if (btnNext != null) {
				btnNext.Dispose ();
				btnNext = null;
			}

			if (btnPause != null) {
				btnPause.Dispose ();
				btnPause = null;
			}

			if (btnPrev != null) {
				btnPrev.Dispose ();
				btnPrev = null;
			}

			if (btnStart != null) {
				btnStart.Dispose ();
				btnStart = null;
			}

			if (btnStop != null) {
				btnStop.Dispose ();
				btnStop = null;
			}

			if (lbDistRemain != null) {
				lbDistRemain.Dispose ();
				lbDistRemain = null;
			}

			if (lbNote != null) {
				lbNote.Dispose ();
				lbNote = null;
			}

			if (lbStats != null) {
				lbStats.Dispose ();
				lbStats = null;
			}

			if (lbStroke != null) {
				lbStroke.Dispose ();
				lbStroke = null;
			}

			if (lbTimeRemain != null) {
				lbTimeRemain.Dispose ();
				lbTimeRemain = null;
			}

			if (tbConsole != null) {
				tbConsole.Dispose ();
				tbConsole = null;
			}

			if (tbvSetList != null) {
				tbvSetList.Dispose ();
				tbvSetList = null;
			}
		}
	}

	[Register ("MainWindowController")]
	partial class MainWindowController
	{
		
		void ReleaseDesignerOutlets ()
		{
		}
	}
}
