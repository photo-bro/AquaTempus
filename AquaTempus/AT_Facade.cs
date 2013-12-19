using System;

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


		public void OpenFile(string name, string path){
			m_FM.OpenFile (name, path);
		}

		public void CloseFile(){
			m_FM.CloseFile ();
		}

		public string GetTokenList(){
			m_ST.TokenizeString (m_FM.CurrentFileToString ());
			return m_ST.ToString ();
		}





	}
}

