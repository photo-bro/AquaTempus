//using System;
//using System.Timers;
//
//namespace AquaTempus
//{
//	public class Clock
//	{
//		Timer m_SetTimer;
//		Timer m_Tick;
//		int m_secOnClock;
//
//		public Clock (double interval = 1000)
//		{
//			m_SetTimer = new Timer (interval);
//			m_Tick = new Timer (1000); // 1 second interval
//			m_secOnClock = 0;
//
//		}
//
//		public void Test ()
//		{
//		
//			m_Tick.Elapsed += new ElapsedEventHandler (Tick);
//		
//			m_Tick.Enabled = true;
//
//		
//		}
//
//		public int Seconds {
//			get{ return m_secOnClock; }
//		}
//
//
//		// Reference:
//		// Delegates and Events
//		// http://msdn.microsoft.com/en-us/library/orm-9780596521066-01-17.aspx
//		public delegate int SetTimer (object source, ElapsedEventArgs e);
//
//
//
//
//	}
//}
//
