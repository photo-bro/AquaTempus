using System;
using System.Collections.Generic;
using System.Timers;

namespace AquaTempus
{

	public class TickArgs : EventArgs
	{
		int m_iRemainingTime;

		public string TimeRemaining {
			get { 
				return string.Format ("{0}:{1}", (int)(m_iRemainingTime / 60), m_iRemainingTime % 60); }
		}

		public TickArgs(int TimeRemaining){
			m_iRemainingTime = TimeRemaining;
		}
	}

	public class SetEndArgs : EventArgs
	{
		public Set CurrentSet;

		public SetEndArgs(Set currentSet){
			CurrentSet = currentSet;
		}
	}

	public delegate void SetEndHandler (object source, SetEndArgs e);
	public delegate void TickHandler (object source, TickArgs e);

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
		private int m_iNum;

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
			// check if set interval has elapsed
			if (m_icurTime >= m_llnCurSet.Value.IntervalInt) {
				// Interval up, reset time
				m_icurTime = 0;
				m_iNum++;
				// Check if set over
				if (m_iNum >= m_llnCurSet.Value.Number) {

					// Set end event to be listened by Window
					SetEnded (this, new SetEndArgs (m_llnCurSet.Value));

					m_llnCurSet = m_llnCurSet.Next;
				}
			}

			// tick event - to update countdown timer 
			Ticked (this, new TickArgs(m_llnCurSet.Value.IntervalInt - m_icurTime));
		}
	}
	// SetRunner class ^
}

