using System;
using System.Reflection;

namespace AquaTempus
{
	// Credit:
	// Brenda Bell
	// http://stackoverflow.com/questions/12962499/how-to-do-inheritance-with-singleton
	public class Singleton<T> where T : class
	{
		/// <summary>
		/// Default constructor - does nothing
		/// </summary>
		protected Singleton ()
		{
		}

		/// <summary>
		/// Return static instance of class
		/// </summary>
		/// <value>The instance.</value>
		public static T Instance {
			get { return SingletonFactory.instance; }
		}
		// Class for returning a single instance of a class
		class SingletonFactory
		{
			/// <summary>
			/// Static instance of class
			/// </summary>
			internal static readonly T instance;

			static SingletonFactory ()
			{
				ConstructorInfo constructor = typeof(T).GetConstructor (
					                              BindingFlags.Instance | BindingFlags.NonPublic,
					                              null, new System.Type[0],
					                              new ParameterModifier[0]);

				if (constructor == null)
					throw new Exception (
						"Target type is missing private or protected no-args constructor: type=" +
						typeof(T).FullName);
				try {
					instance = constructor.Invoke (new object[0]) as T;
				} // try
				catch (Exception e) {
					throw new Exception (
						"Failed to create target: type=" + typeof(T).FullName, e);
				} // catch
			}
		}
	}
}

