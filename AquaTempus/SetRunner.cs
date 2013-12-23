using System;
using System.Collections.Generic;
using System.Timers;

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

		private LinkedList<Set> m_llSetList;
		private LinkedListNode<Set> m_llnCurSet;
		private bool m_bRun = false;
		private Timer m_Tick;
		private int m_icurTime;

		public event SetEndHandler SetEnded;
		public event TickHandler Ticked;

		public Set CurrentSet {
			get{ return null; }
		}

		public void Init (LinkedList<Set> SetList)
		{
			m_llSetList = SetList;
			m_llnCurSet = m_llSetList.First;
		}

		public void Start ()
		{
			if (m_llSetList == null)
				return;
			if (!m_bRun) {
				m_bRun = true;
				Run ();
			} else if (m_Tick != null)
				m_Tick.Start ();
		}

		public void Pause ()
		{
			if (m_llSetList == null)
				return;
			if (m_Tick != null)
				m_Tick.Stop ();
		}

		public void Stop ()
		{
			if (m_llSetList == null)
				return;

			if (m_Tick != null)
				m_Tick.Stop ();
			m_bRun = false;
			m_llnCurSet = m_llSetList.First;

		}

		public void Next ()
		{
			if (m_llSetList == null)
				return;
			m_llnCurSet = m_llnCurSet.Next;
		}

		public void Previous ()
		{
			if (m_llSetList == null)
				return;
			m_llnCurSet = m_llnCurSet.Previous;
		}
		// Reference:
		// Delegates and Events
		// http://msdn.microsoft.com/en-us/library/orm-9780596521066-01-17.aspx
//		public delegate int SetTimer (object source, ElapsedEventArgs e);

		//SetTimer st = SetTimer;

		private void Run ()
		{
			m_Tick = new Timer ();
			while (m_llnCurSet != null && m_bRun) {
				m_Tick.Start ();

				m_Tick.Elapsed += new ElapsedEventHandler (SetTimeEvent);
			}
		}

		private void SetTimeEvent (object source, ElapsedEventArgs e)
		{
			m_icurTime++;

			if (m_icurTime == m_llnCurSet.Value.IntervalInt) {
				// Set end event (to be listened by Window
				SetEnded (this, e);
					
				m_icurTime = 0;
				m_llnCurSet = m_llnCurSet.Next;
			
			}
			// tick event 
			Ticked (this, e);


		}
	}
	// SetRunner class ^
	public delegate void SetEndHandler (object source, ElapsedEventArgs e);
	public delegate void TickHandler (object source, ElapsedEventArgs e);
}

