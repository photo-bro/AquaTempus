using System;

namespace AquaTempus
{
	/// <summary>
	/// Responsible for actinos related to opening, maintainging, and closing
	/// the active file
	/// </summary>
	public class FileManager
	{
		///// Singleton Stuff
		private static FileManager c_fmInstance;
		private static object c_fmLock = typeof(FileManager);

		private FileManager() {}

		public static FileManager Instance {
			get {
				lock (c_fmLock) {
					if (c_fmInstance == null)
						c_fmInstance = new FileManager ();
					return c_fmInstance;
				} // lock
			} // get
		} // Instance

		private Filer m_Filer = Filer.Instance;


		private string m_sProgramName = "AquaTempus";
		private string m_sProgramVer = ".01";

		private string m_sLastFileName;
		private string m_sLastFilePath;


		public string LastFileName {
			get { return m_sLastFileName; }
			set { m_sLastFileName = value; }
		}

		public string LastFilePath {
			get { return m_sLastFilePath; }
			set { m_sLastFilePath = value; }
		}

		public string ProgramName {
			get { return m_sProgramName; }
			set { m_sProgramName = value; }
		}

		public string ProgramVersion {
			get { return m_sProgramVer; }
		}

		public bool OpenFile(string name, string path){
			return m_Filer.OpenFile(name, path);
		}

		public bool CloseFile ()
		{
			return m_Filer.CloseFile ();
		}

		public string CurrentFileToString(){
			return m_Filer.FileToString ();
		}

		public bool CreateFile (string path, string name, string content)
		{
			return false;
			//return m_Filer.FileToString (path, name, content);
		}
	}
	// FileManager
} // namespace AquaTempus

