using System;
using System.IO;

namespace AquaTempus
{
	/// ********************
	/// Platform implementations of filer
	/// See abstract class for function descriptions
	/// ********************

	public class Filer
	{
		private static Filer c_fInstance;
		private static object c_fLock = typeof(Filer);

		private Filer ()
		{
		}

		/// <summary>
		/// Return static singleton instance of class
		/// </summary>
		/// <value>The instance.</value>
		public static Filer Instance {
			get {
				lock (c_fLock) {
					if (c_fInstance == null)
						c_fInstance = new Filer ();
					return  c_fInstance;
				} // lock
			} // get
		}

		StreamReader m_sr;
		StreamWriter m_sw;

		public  bool OpenFile (string name, string path)
		{
			try {
				m_sr = new StreamReader (path + "//" + name);
			} catch (Exception e) {
				Console.WriteLine (e.ToString ());
				return false;
			}
			return true;
		}

		public  bool CloseFile ()
		{
			try {
				m_sr.Close ();
			} catch (Exception e) {
				Console.WriteLine (e.ToString ());
				return false;
			}
			return true;
		}

		public  bool CreateFile (string name, string path, string content)
		{
			try {
				m_sw = new StreamWriter (path + "//" + name);
				m_sw.Write (content);
				m_sw.Close ();
			} catch (Exception e) {
				Console.WriteLine (e.ToString ());
				return false;
			}
			return true;			
		}

		public string FileToString ()
		{
			try {
				string s = m_sr.ReadToEnd();
				// reset streamreader
				// Credit: stackoverflow.com/questions/6467853/return-streamreader-to-beginning-when-his-basestream-has-bom
				m_sr.BaseStream.Position = 0;
				m_sr = new StreamReader(m_sr.BaseStream, m_sr.CurrentEncoding, false);
				return s;
			} catch (Exception e) {
				Console.WriteLine (e.ToString ());
				return "";
			}
		}
	}
}

