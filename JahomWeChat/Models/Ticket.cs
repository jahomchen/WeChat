using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JahomWeChat.Models
{
	public class Ticket
	{
		public string ticket { get; set; }
		public string expires_in { get; set; }
		public int ErrorCode { get; set; }
	}
}