using System;
using System.IO;

namespace AquaTempus
{
	public class FilerFacade
	{
		OperatingSystem m_OS = Environment.OSVersion;
		PlatformID m_pID = m_OS.Platform;

		public static Filer CurrentPlatformFiler ()
		{
			// Credit:
			// http://stackoverflow.com/questions/9129491/c-sharp-compiled-in-mono-detect-os
			switch (m_pID) {
			case PlatformID.Win32NT:
			case PlatformID.Win32S:
			case PlatformID.Win32Windows:
			case PlatformID.WinCE:
				return Windows_Filer.Instance;
			case PlatformID.Unix:
				return OSX_Filer.Instance;
			default:
				Console.WriteLine ("Invalid Platform / Operating System");
				return null;
			} // switch
		}

		public abstract class Filer
		{
			///// Singleton Stuff
			private static Filer c_fInstance;
			private static object c_fLock = typeof(Filer);

			private Filer ()
			{
			}

			public static Filer Instance {
				get {
					lock (c_fLock) {
						if (c_fInstance == null)
							c_fInstance = new Filer ();
						return c_fInstance;
					} // lock
				} // get
			}

			protected StreamWriter m_sw;
			protected StreamReader m_sr;

			public abstract bool OpenFile (string name, string path);

			public abstract bool CloseFile ();

			public abstract bool CreateFile (string name, string path, string content);

			public abstract string FileToString ();
		}

		public class OSX_Filer : Filer
		{
			public bool OpenFile (string name, string path)
			{
				try {
					m_sr = new StreamReader (path + "//" + name);
				} catch (Exception e) {
					Console.WriteLine (e.ToString ());
					return false;
				}
				return true;
			}

			public bool CloseFile ()
			{
				try {
					m_sr.Close ();
				} catch (Exception e) {
					Console.WriteLine (e.ToString ());
					return false;
				}
				return true;
			}

			public bool CreateFile (string name, string path, string content)
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

			public string FileToString(){
				try{
					return m_sr.ReadToEnd();
				}
				catch(Exception e){
					Console.WriteLine (e.ToString ());
					return "";
				}
			}
		}

		public class Windows_Filer : Filer
		{
		}
	}
