using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

// regex
namespace AquaTempus
{
	public class SetToken
	{
		public SetTokenType Type;
		public string Value;
		public int LineNumber;

		public SetToken (SetTokenType type, string tokvalue, int linenum)
		{
			Type = type;
			Value = tokvalue;
			LineNumber = linenum;
		}

		public override string ToString ()
		{
			return string.Format ("Line {0}:  Token {1} : {2}", LineNumber, Type, Value);
		}
	}

	public enum SetTokenType
	{
		WORD = 0,
		// any string that is not any other token
		INTEGER = 1,
		// any integer
		INTERVAL = 2,
		// any integer followed by a colon and another integer 00:00
		MULT = 3,
		// X or x
		EOF = 4
		// EndOfFile
	}

	public class SetTokenizer
	{
		///// Singleton Stuff
		private static SetTokenizer c_stInstance;
		private static object c_stLock = typeof(SetTokenizer);

		private SetTokenizer ()
		{
		}

		/// <summary>
		/// Return static singleton instance of class
		/// </summary>
		/// <value>The instance.</value>
		public static SetTokenizer Instance {
			get {
				lock (c_stLock) {
					if (c_stInstance == null)
						c_stInstance = new SetTokenizer ();
					return c_stInstance;
				} // lock
			} // get
		}

		///// Tokenizer Stuff
		string m_sRawFile;
		List<string> m_lsLines = new List<string> ();
		List<SetToken> m_ltTokens = new List<SetToken> ();

		/// <summary>
		/// Convert string into list of SetTokens
		/// </summary>
		/// <param name="setfile">Setfile.</param>
		public void TokenizeString (string setfile)
		{
			m_sRawFile = setfile;

			// Split raw file into "words" or "blocks"
			m_lsLines.AddRange (m_sRawFile.Split (new [] { Environment.NewLine }, StringSplitOptions.None));

			int iLineCount = -1;

			// Loop through lines 
			foreach (string line in m_lsLines) {
				iLineCount++;

				// skip line if empty
				if (line.Length == 0)
					continue;
				// skip comment lines
				if (line [0] == '#')
					continue;

				// Loop through blocks in each line
				foreach (string s in line.Split(new[]{' '}, StringSplitOptions.RemoveEmptyEntries)) {
					SetToken st;
					int temp = 0;
					// comment reached, goto next line
					if (s.Contains ("#"))
						break;

					// MULT
					if (s.ToString ().ToUpper() [0] == 'X' && s.Length == 1)
						st = new SetToken (SetTokenType.MULT, s, iLineCount);
			
					// INTEGER
					else if (int.TryParse (s, out temp))
						st = new SetToken (SetTokenType.INTEGER, s, iLineCount);

					// INTERVAL
					else if (Regex.IsMatch (s, "[0-9]*[0-9]*:*[0-9]*[0-9]:[0-9][0-9]"))
						st = new SetToken (SetTokenType.INTERVAL, s, iLineCount);

					// WORD, assume anything else is a word
					else
						st = new SetToken (SetTokenType.WORD, s, iLineCount);

					m_ltTokens.Add (st);
				} // foreach block
			} // foreach line
			m_ltTokens.Add (new SetToken (SetTokenType.EOF, "EOF", iLineCount));
		}

		/// <summary>
		/// Returns class token list. If string hasn't been tokenized, null is returned.
		/// </summary>
		/// <returns>The list.</returns>
		public List<SetToken> TokenList ()
		{
			return m_ltTokens;
		}

		/// <summary>
		/// Returns formatted string of tokenized file
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="AquaTempus.SetTokenizer"/>.</returns>
		public override string ToString ()
		{
			string s = "";
			m_ltTokens.ForEach ((SetToken t) => s += t.ToString () + Environment.NewLine);
			return s;
		}
	}
}

