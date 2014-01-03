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

		private FileManager ()
		{
		}

		public static FileManager Instance {
			get {
				lock (c_fmLock) {
					if (c_fmInstance == null)
						c_fmInstance = new FileManager ();
					return c_fmInstance;
				} // lock
			} // get
		}
		// Instance
		private Filer m_Filer = Filer.Instance;
		private string m_sProgramName = "AquaTempus";
		private string m_sProgramVer = "1.00";
		// version 1.00 because it works! (sorta)
		private string m_sLastFileName = "TestSet.txt";
		private string m_sLastFilePath = "//Users//jharm//Desktop";

		/// <summary>
		/// Get/Set name of last file
		/// </summary>
		/// <value>The last name of the last file.</value>
		public string LastFileName {
			get { return m_sLastFileName; }
			set { m_sLastFileName = value; }
		}

		/// <summary>
		/// Get/Set path of last file
		/// </summary>
		/// <value>The last file path.</value>
		public string LastFilePath {
			get { return m_sLastFilePath; }
			set { m_sLastFilePath = value; }
		}

		/// <summary>
		/// Get/Set current Program Name
		/// </summary>
		/// <value>The name of the program.</value>
		public string ProgramName {
			get { return m_sProgramName; }
			set { m_sProgramName = value; }
		}

		/// <summary>
		/// Get current Program version
		/// </summary>
		/// <value>The program version.</value>
		public string ProgramVersion {
			get { return m_sProgramVer; }
		}

		/// <summary>
		/// Open file at path / name
		/// </summary>
		/// <returns><c>true</c>, if file was opened, <c>false</c> otherwise.</returns>
		/// <param name="name">Name.</param>
		/// <param name="path">Path.</param>
		public bool OpenFile (string name, string path)
		{
			return m_Filer.OpenFile (name, path);
		}

		/// <summary>
		/// Open last opened file
		/// </summary>
		/// <returns><c>true</c>, if file was opened, <c>false</c> otherwise.</returns>
		public bool OpenFile ()
		{
			return m_Filer.OpenFile (LastFileName, LastFilePath);
		}

		/// <summary>
		/// Check if a file is currently opened
		/// </summary>
		/// <returns><c>true</c>, if open filed was opened, <c>false</c> otherwise.</returns>
		public bool FileOpen ()
		{
			return m_Filer.FileOpen ();
		}

		/// <summary>
		/// Close current file
		/// </summary>
		/// <returns><c>true</c>, if file was closed, <c>false</c> otherwise.</returns>
		public bool CloseFile ()
		{
			return m_Filer.CloseFile ();
		}

		/// <summary>
		/// Parse current file to string
		/// </summary>
		/// <returns>The current open file to string.</returns>
		public string CurrentFileToString ()
		{
			return m_Filer.FileToString ();
		}

		/// <summary>
		/// Create text containing content at path/name
		/// </summary>
		/// <returns><c>true</c>, if file was created, <c>false</c> otherwise.</returns>
		/// <param name="path">Path.</param>
		/// <param name="name">Name.</param>
		/// <param name="content">Content.</param>
		public bool CreateFile (string path, string name, string content)
		{
			return m_Filer.CreateFile (name, path, content);
		}
	}
	// FileManager
}
// namespace AquaTempus

