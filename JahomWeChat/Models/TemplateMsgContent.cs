using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JahomWeChat.Models
{
	public class TemplateMsgContent
	{
		public string OpenId { get; set; }
		public string Template_id { get; set; }
		public string Url { get; set; }
		public Dictionary<string, string> DicFirst { get; set; }
		public Dictionary<string, string> DicKeynote { get; set; }
	}
}