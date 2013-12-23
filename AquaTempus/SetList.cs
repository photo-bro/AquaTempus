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
				if (m_iInterval < 3659) // 60:59 -> 60min 59sec
				return string.Format ("{0}:{1}", (int)(m_iInterval / 60), m_iInterval % 60);
				else
					return "";
			} // get
			set { 
				m_iInterval = ParseInterval (value);
			} // set 
		}

		public int IntervalInt {
			get { return m_iInterval; }
		}


		private int ParseInterval (string interval)
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

