using System;

/* for NSAlert */
using MonoMac.Foundation;
using MonoMac.AppKit;
using MonoMac.ObjCRuntime;

namespace AquaTempus
{
	public enum AQ_EXCEPTION_CODE
	{
		NONE = 0,
		FILE_NOT_OPEN = 1
	}

	public class AQ_Exceptions : Exception
	{
		///// Singleton Stuff
		private static AQ_Exceptions c_erInstance;
		private static object c_erLock = typeof(AQ_Exceptions);

		/// <summary>
		/// Does nothing
		/// </summary>
		private AQ_Exceptions ()
		{
		}

		/// <summary>
		/// Return static singleton instance of class
		/// </summary>
		/// <value>The instance.</value>
		public static AQ_Exceptions Instance {
			get {
				lock (c_erLock) {
					if (c_erInstance == null)
						c_erInstance = new AQ_Exceptions ();
					return c_erInstance;
				} // lock
			} // get
		}

		/// <summary>
		/// Error log
		/// </summary>
		private static string m_sLog = "";

		/// <summary>
		/// Log Exception/Error. To display alert requires source NSWindow
		/// </summary>
		/// <param name="exception">Exception.</param>
		/// <param name="message">Message.</param>
		/// <param name="displayAlert">If set to <c>true</c> display alert.</param>
		/// <param name="source">Source.</param>
		public static void AQ_Exception (AQ_EXCEPTION_CODE exception, string message, bool displayAlert = true, NSWindow source = null)
		{
			// build entry
			string sExceptionEntry = string.Format ("{0} {1}{2}Exception: {3}{2}{4}"
				, DateTime.Now.ToShortDateString()
				, DateTime.Now.ToShortTimeString()
				, Environment.NewLine
				, exception
				, message);

			// add to log
			m_sLog += sExceptionEntry + Environment.NewLine;

			// display alert
			if (displayAlert) {
				// create and display alert
				NSAlert alert = new NSAlert ();
				alert.AlertStyle = NSAlertStyle.Warning;
				alert.MessageText = sExceptionEntry;
				alert.BeginSheet (source);
			}
		
		}

		/// <summary>
		/// Returns exception log
		/// </summary>
		/// <returns>The exception log.</returns>
		public static string AQ_Exception_Log ()
		{
			return m_sLog;
		}
	}
}

