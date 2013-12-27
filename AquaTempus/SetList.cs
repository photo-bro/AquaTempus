using System;
using System.Collections.Generic;


namespace AquaTempus
{
	public class Set
	{
		public int Number;
		public int Distance;
		public string Stroke;
		public string Comment;
		private int m_iInterval;

		public string Interval {
			get {  
				return IntervalToString (m_iInterval);
			} // get
			set { 
				m_iInterval = ParseStringInterval (value);
			} // set 
		}

		public int IntervalInt {
			get { return m_iInterval; }
		}

		public static string IntervalToString(int interval){
			if (interval < 3659) // 60:59 -> 60min 59sec
				return string.Format ("{0}:{1}", (int)(interval / 60), interval % 60);
		
		
			return "";
		}

		public static int ParseStringInterval (string interval)
		{
			if (interval.Length == 5) { // 00:00
				int min = int.Parse (interval.Substring (0, 2));
				int sec = int.Parse (interval.Substring (3, 2));
				return min * 60 + sec;
			} else
				return 0;
		}

		public Set (int number, int distance, string stroke, string comment, string interval)
		{
			Number = number;
			Distance = distance;
			Stroke = stroke;
			Comment = comment;
			Interval = interval;
		}
	}
}

