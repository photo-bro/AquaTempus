using System;
using System.Collections.Generic;

namespace AquaTempus
{
	public class SetParser //: Singleton<this>
	{
		///// Singleton Stuff
		private static SetParser c_spInstance;
		private static object c_spLock = typeof(SetParser);

		private SetParser ()
		{
		}

		/// <summary>
		/// Return static singleton instance of class
		/// </summary>
		/// <value>The instance.</value>
		public static SetParser Instance {
			get {
				lock (c_spLock) {
					if (c_spInstance == null)
						c_spInstance = new SetParser ();
					return c_spInstance;
				} // lock
			} // get
		}

		LinkedList<Set> m_llSetList;
		SetToken m_curToken;
		List<SetToken> m_Tokens;
		int m_iTP = 0;

		public void ParseToSetList (List<SetToken> tokens)
		{
			m_Tokens = tokens;
			m_iTP = 0;
			m_curToken = null;

			m_llSetList = new LinkedList<Set> ();

			SetParse ();
		}

		/// <summary>
		/// Parse SetToken list matching the following BNF:
		/// SetList ::= Integer Mult Integer Interval Stroke {Comment}
		/// Interval ::= {{Digit} Digit : } {Digit} Digit : Digit Digit
		/// Stroke  ::= Word
		/// Comment ::= {Word} | {{Word} Word}
		/// Word	::= Letter | Word Letter
		/// Letter  ::= {any latin letter}
		/// Integer ::= Digit | Integer Digit
		/// Digit   ::= 0|1| ... | 9
		/// Mult	::= x|X
		/// </summary>
		private void SetParse ()
		{
			int number, distance;
			string interval, stroke, comment;
		
			m_curToken = NextToken ();
			while ( m_curToken != null && m_curToken.Type != SetTokenType.EOF) {
				comment = ""; // reset comment

				number = int.Parse (m_curToken.Value);
				Match (SetTokenType.INTEGER);

				Match (SetTokenType.MULT);

				distance = int.Parse (m_curToken.Value);
				Match (SetTokenType.INTEGER);

				interval = m_curToken.Value;
				Match (SetTokenType.INTERVAL);

				stroke = m_curToken.Value;
				Match (SetTokenType.WORD);

				// Comments are optional
				if (m_curToken.Type == SetTokenType.WORD) {
					// Comments
					int curLine = m_curToken.LineNumber;
					for (; m_curToken != null && m_curToken.Type != SetTokenType.EOF &&
						m_curToken.LineNumber != curLine + 1; Match (SetTokenType.WORD))
						comment += m_curToken.Value + " ";
				} // if
				m_llSetList.AddLast (new Set (number, distance, stroke, comment, interval));
			} // main loop
		}

		private bool Match (SetTokenType type)
		{
			if (m_curToken.Type == type) {
				m_curToken = NextToken ();
				return true;
			} else
				throw new Exception (string.Format ("Invalid token - Expecting : {0}", type));
		}

		private SetToken NextToken ()
		{
			if (m_iTP + 1 < m_Tokens.Count)
				return m_Tokens [m_iTP++];
			else
				return null;
		}

		public LinkedList<Set> CurrentSetList {
			get { 
				if (m_llSetList == null)
					return null;
				return m_llSetList;
			} // get
		}
	}
}

