using System;
using System.Collections.Generic;
using System.Text;
using Realms;

namespace Awesome.Player.Models
{
	//public class Notes : RealmObject
	//{
	//	[PrimaryKey]
	//	public string MediaUri { get; set; }
	//	public IList<Note> notes { get; }
	//}

	public class Note : RealmObject
	{
		public string MediaUri { get; set; }
		public string Text { get; set; }
		public double Position { get; set; }
		public double Duration { get; set; }
	}

	
}
