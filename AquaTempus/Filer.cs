using System;
using System.IO;

namespace AquaTempus
{
	public class FilerFacade
	{
		OperatingSystem m_OS = Environment.OSVersion;
		PlatformID m_pID = m_OS.Platform;

		/// <summary>
		/// Returns the proper Filer instance depending on platform
		/// </summary>
		/// <returns>The platform filer.</returns>
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

			private Filer (){}

			/// <summary>
			/// Return static singleton instance of class
			/// </summary>
			/// <value>The instance.</value>
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

			/// <summary>
			/// Open file through internal StreamReader object
			/// </summary>
			/// <returns><c>true</c>, if file was opened, <c>false</c> otherwise.</returns>
			/// <param name="name">Name.</param>
			/// <param name="path">Path.</param>
			public abstract bool OpenFile (string name, string path);

			/// <summary>
			/// Close file in internal StreamReader object
			/// </summary>
			/// <returns><c>true</c>, if file was closed, <c>false</c> otherwise.</returns>
			public abstract bool CloseFile ();

			/// <summary>
			/// Create a new text based file 
			/// </summary>
			/// <returns><c>true</c>, if file was created, <c>false</c> otherwise.</returns>
			/// <param name="name">Name.</param>
			/// <param name="path">Path.</param>
			/// <param name="content">Content.</param>
			public abstract bool CreateFile (string name, string path, string content);

			/// <summary>
			/// Return open file's content as string
			/// </summary>
			/// <returns>The to string.</returns>
			public abstract string FileToString ();
		}

		/// ********************
		/// Platform implementations of filer
		/// See abstract class for function descriptions
		/// ********************

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
