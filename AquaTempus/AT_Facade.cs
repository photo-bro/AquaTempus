using System;
using System.Collections.Generic;

namespace AquaTempus
{
	public class AT_Facade
	{
		///// Singleton Stuff
		private static AT_Facade c_atInstance;
		private static object c_atLock = typeof(AT_Facade);

		private AT_Facade ()
		{
		}

		/// <summary>
		/// Return static singleton instance of class
		/// </summary>
		/// <value>The instance.</value>
		public static AT_Facade Instance {
			get {
				lock (c_atLock) {
					if (c_atInstance == null)
						c_atInstance = new AT_Facade ();
					return c_atInstance;
				} // lock
			} // get
		}

		FileManager m_FM = FileManager.Instance;
		SetTokenizer m_ST = SetTokenizer.Instance;
		SetParser m_SP = SetParser.Instance;

		public void OpenFile (string name, string path)
		{
			m_FM.LastFileName = name;
			m_FM.LastFilePath = path;
			m_FM.OpenFile (name, path);
		}

		/// <summary>
		/// Open last file
		/// </summary>
		public void OpenFile ()
		{
			m_FM.OpenFile (m_FM.LastFileName, m_FM.LastFilePath);
		}

		/// <summary>
		/// Check if a file is currently opened
		/// </summary>
		/// <returns><c>true</c>, if open filed was opened, <c>false</c> otherwise.</returns>
		public bool FileOpen ()
		{
			return m_FM.FileOpen ();
		}

		/// <summary>
		/// Closes the current file
		/// </summary>
		public void CloseFile ()
		{
			m_FM.CloseFile ();
		}

		/// <summary>
		/// Saves current Set List at path / name
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="path">Path.</param>
		public void SaveCurrentSet (string name, string path)
		{
			m_FM.CreateFile (path, name, m_FM.CurrentFileToString ());
		}

		/// <summary>
		/// Tokenizes current file and returns as formatted string
		/// </summary>
		/// <returns>The token list.</returns>
		public string GetTokenList ()
		{
			m_ST.TokenizeString (m_FM.CurrentFileToString ());
			return m_ST.ToString ();
		}

		/// <summary>
		/// Tokenizes and parses current file. Returns as a "table" in the following format:
		/// List<List<string>>
		/// Outer List is columns, 6 indexes
		/// Index 0: Count
		/// Index 1: Num
		/// Index 2: Distance
		/// Index 3: Interval
		/// Index 4: Stroke
		/// Index 5: Comment
		/// 
		/// Inner List is rows, the values
		/// </summary>
		/// <returns>The set list table. See summary for format</returns>
		public List<List<string>> GetSetListTable ()
		{
			m_ST.TokenizeString (m_FM.CurrentFileToString ());
			m_SP.ParseToSetList (m_ST.TokenList ());
			// Create the table
			List<List<string>> SetTable = new List<List<string>> ();
			for (int j = 0; j < 6; ++j)
				SetTable.Add (new List<string> ());

			int i = 0;
			foreach (Set st in m_SP.CurrentSetList) {
				SetTable [0].Add ((++i).ToString ());			
				SetTable [1].Add (st.Number.ToString ());	
				SetTable [2].Add (st.Distance.ToString ());	
				SetTable [3].Add (st.Interval);	
				SetTable [4].Add (st.Stroke);	
				SetTable [5].Add (st.Comment);	
			}
			return SetTable;
		}

		/// <summary>
		/// Tokenize and Parse open file
		/// </summary>
		public void InitSet ()
		{
			m_ST.TokenizeString (m_FM.CurrentFileToString ());
			m_SP.ParseToSetList (m_ST.TokenList ());
		}

		/// <summary>
		/// Gets Set List for the current file. If file has not been initialized,
		/// it calls InitSet.
		/// </summary>
		/// <returns>The set list.</returns>
		public LinkedList<Set> GetSetList ()
		{
			if (m_SP.CurrentSetList == null)
				InitSet ();

			return m_SP.CurrentSetList;
		}
	}
}

