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
			// Credit: User: spurious.thoughts
			// http://stackoverflow.com/questions/463642/what-is-the-best-way-to-convert-seconds-into-hourminutessecondsmilliseconds

			TimeSpan t = TimeSpan.FromSeconds( interval );

			return string.Format("{0:D2}:{1:D2}", 
				t.Minutes, 
				t.Seconds);
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

		public override string ToString ()
		{
			return string.Format ("{0}x{1} {2} on {3} {4}", Number, Distance, Stroke, IntervalToString(IntervalInt), Comment);
		}
	}
}

