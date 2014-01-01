using System;
using System.Collections.Generic;
using System.Timers;

namespace AquaTempus
{
	/// <summary>
	/// Data for SetEnd events, such as the current Set object
	/// </summary>
	public class SetEndArgs : EventArgs
	{
		public Set CurrentSet;

		public SetEndArgs (Set currentSet)
		{
			CurrentSet = currentSet;
		}
	}
	// Event handlers for SetRunner events
	public delegate void SetEndHandler (object source, SetEndArgs e);
	public delegate void ResetCountHandler (object source, EventArgs e);
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
		// ******************* //
		// TODO


		private LinkedList<Set> m_llSetList;
		private LinkedListNode<Set> m_llnCurSet;
		private bool m_bRun = false;
		private Timer m_SetTimer;
		private int m_iNum;

		public event SetEndHandler SetEnded;
		public event ResetCountHandler ResetCount;

		public Set CurrentSet {
			get{ return (m_llnCurSet == null) ? null : m_llnCurSet.Value; }
		}

		public int CurrentNum {
			get { return m_iNum; }
		}

		public void Init (LinkedList<Set> SetList)
		{
			m_llSetList = SetList;
			m_llnCurSet = m_llSetList.First;
		}

		public void Start ()
		{
			// Check if there is a set to run
			if (m_llSetList == null)
				return;

			// Check if not running, if so start running
			if (!m_bRun) {
				m_bRun = true;
				Run ();
			} 
			// else if timer is constructed start
			else if (m_SetTimer != null)
				m_SetTimer.Start ();
		}

		public void Pause ()
		{
			// Check for running set
			if (m_llSetList == null)
				return;
			// Stop timer
			if (m_SetTimer != null)
				m_SetTimer.Stop ();
		}

		public void Stop ()
		{
			// Check for running set
			if (m_llSetList == null)
				return;
			// Stop timer
			if (m_SetTimer != null)
				m_SetTimer.Stop ();
			// reset timer and setlist to head
			m_bRun = false;
			m_llnCurSet = m_llSetList.First;

		}

		public void Next ()
		{
			// Check for running set
			if (m_llSetList == null)
				return;

			// check if there is a next set
			if (m_llnCurSet.Next == null)
				return;

			// Point to next set
			m_llnCurSet = m_llnCurSet.Next;

			// Reset, init, and start timer
			ResetAndStartSetTimer (m_llnCurSet.Value.IntervalInt * 1000);
			m_iNum = 0;
		}

		public void Previous ()
		{
			// Check for running set
			if (m_llSetList == null)
				return;

			// check if there is a previous set
			if (m_llnCurSet.Previous == null)
				return;

			// Point to previous set
			m_llnCurSet = m_llnCurSet.Previous;

			// Reset, init, and start timer	
			ResetAndStartSetTimer (m_llnCurSet.Value.IntervalInt * 1000);
			m_iNum = 0;
		}
		// Reference:
		// Delegates and Events
		// http://msdn.microsoft.com/en-us/library/orm-9780596521066-01-17.asp
		private void Run ()
		{
			m_SetTimer = new Timer (m_llnCurSet.Value.IntervalInt * 1000);
			if (m_llnCurSet != null && m_bRun) {
				m_SetTimer.Start ();

				m_SetTimer.Elapsed += new ElapsedEventHandler (SetTimeEvent);
			}
		}

		private void SetTimeEvent (object source, ElapsedEventArgs e)
		{
			// Check if the current set is over
			if (++m_iNum >= m_llnCurSet.Value.Number) {

				// reset set num count
				m_iNum = 0;

				// Point to next set
				m_llnCurSet = m_llnCurSet.Next;

				// Create SetEnded event (for set that just ended)
				SetEnded (this, new SetEndArgs (m_llnCurSet.Previous.Value));

				// check if end of set list
				if (m_llnCurSet == null) {
					// stop timer
					m_SetTimer.Stop ();

					// Create event to reset countdown clock
					ResetCount (this, new EventArgs ());

					// exit
					return;
				}
			}

			// adjust interval
			ResetAndStartSetTimer (m_llnCurSet.Value.IntervalInt * 1000);
			m_SetTimer.Elapsed += new ElapsedEventHandler (SetTimeEvent);

			// Create event to reset countdown clock
			ResetCount (this, new EventArgs ());
		}

		private void ResetAndStartSetTimer (int interval)
		{
			m_SetTimer.Dispose ();
			m_SetTimer = new Timer (interval);
			m_SetTimer.Start ();
		}
	}
	// SetRunner class ^
}

