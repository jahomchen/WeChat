using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JahomWeChat.Models
{
	public class TemplateMsgContent
	{
		public List<string> OpenIdList { get; set; }
		public SendDataInfo SendDataInfo { get; set; }
		public string Template_id { get; set; }
		public string url { get; set; }
		public Dictionary<string, string> DicFirst { get; set; }
		public Dictionary<string, string> DicKeynote { get; set; }
	}
}