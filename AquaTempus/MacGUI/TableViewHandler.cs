using System;
using System.Collections.Generic;
using MonoMac.Foundation;
using MonoMac.AppKit;

namespace AquaTempus
{
	[Register ("TableViewHandler")]
	public class TableViewHandler : NSTableViewDataSource
	{
		List<List<string>> m_tableData;

		/// <summary>
		/// Constructor
		/// Data is in the format of data [column] [row]
		/// </summary>
		/// <param name="data">Data.</param>
		public TableViewHandler (List<List<string>> data)
		{
			m_tableData = data;
		}

		public override int GetRowCount (NSTableView tableView)
		{
			return m_tableData.Count;
		}

		public override NSObject GetObjectValue (NSTableView table, NSTableColumn col, int row)
		{
			int column;

			// Determine which column is being selected
			switch (col.HeaderCell.Title) {
			case "Count":
				column = 0;
				break;
			case "Number":
				column = 1;
				break;
			case "Distance":
				column = 2;
				break;
			case "Interval":
				column = 3;
				break;
			case "Stroke":
				column = 4;
				break;
			case "Comment":
				column = 5;
				break;
			default:
				break;
			}
			return new NSString (m_tableData [column] [row]);
		}
	}
}

