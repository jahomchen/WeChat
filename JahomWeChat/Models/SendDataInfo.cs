using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JahomWeChat.Models
{
	public class SendDataInfo
	{
		public int UserId { get; set; }
		public string OpenId { get; set; }
		public string Url { get; set; }
		public string Content { get; set; }
	}
}