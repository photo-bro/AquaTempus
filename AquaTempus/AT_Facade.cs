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








	}
}

