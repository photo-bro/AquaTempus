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

		public void OpenFile ()
		{
			m_FM.OpenFile (m_FM.LastFileName, m_FM.LastFilePath);
		}

		public bool FileOpen()
		{
			return m_FM.FileOpen ();
		}

		/// <summary>
		/// Open last file
		/// </summary>
		public void CloseFile ()
		{
			m_FM.CloseFile ();
		}

		public void SaveCurrentSet (string name, string path)
		{
			m_FM.CreateFile (path, name, m_FM.CurrentFileToString ());
		}

		public string GetTokenList ()
		{
			m_ST.TokenizeString (m_FM.CurrentFileToString ());
			return m_ST.ToString ();
		}

		public List<List<string>> SetListTable ()
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

		public void InitSet(){
			m_ST.TokenizeString (m_FM.CurrentFileToString ());
			m_SP.ParseToSetList (m_ST.TokenList ());
		}

		public LinkedList<Set> SetList ()
		{
			if (m_SP.CurrentSetList == null)
				InitSet ();

			return m_SP.CurrentSetList;
		}
	}
}

