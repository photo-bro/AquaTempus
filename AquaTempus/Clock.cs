using System;
using System.Timers;

namespace AquaTempus
{
	public class Clock
	{
		Timer m_Timer;
		int m_secOnClock;



		public Clock (double interval = 1000)
		{
			m_Timer = new Timer (interval);
			m_secOnClock = 0;

		}

		public void Test ()
		{
		
			m_Timer.Elapsed += new ElapsedEventHandler (ClockTick );
		
			m_Timer.Enabled = true;

		
		}

		public int Seconds {
			get{ return m_secOnClock; }
		}
		// Timer Events
		private static void TimerEnd (object source, ElapsedEventArgs e)
		{
		
		
		}

		private static void TimerSecTick (object source, ElapsedEventArgs e)
		{
	

		
		
		}
	}
}

