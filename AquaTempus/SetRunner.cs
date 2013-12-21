using System;
using System.Collections.Generic;

namespace AquaTempus
{
	public class SetRunner
	{
		///// Singleton Stuff
		private static SetRunner c_srInstance;
		private static object c_srLock = typeof(SetParser);

		private SetRunner ()
		{
		}

		/// <summary>
		/// Return static singleton instance of class
		/// </summary>
		/// <value>The instance.</value>
		public static SetRunner Instance {
			get {
				lock (c_srLock) {
					if (c_srInstance == null)
						c_srInstance = new SetRunner ();
					return c_srInstance;
				} // lock
			} // get
		}

		private Clock m_SetClock;

		private LinkedList<Set> m_llSetList;
		private LinkedList<Set> m_llnCurSet;

		public Set CurrentSet {
			get{ return null; }
		}

		public void Start(){
		}

		public void Pause(){
		}

		public void Stop(){
		}

		public void Next(){
		}

		public void Previous(){
		}






	}
}

