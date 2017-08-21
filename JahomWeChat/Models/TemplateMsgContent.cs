using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JahomWeChat.Models
{
	public class TemplateMsgContent
	{
		public List<string> OpenIdList { get; set; }
		public List<SendDataInfo> OpenIdRealList { get; set; }
		public string Template_id { get; set; }
		public string url { get; set; }
		public Dictionary<string, string> DicFirst { get; set; }
		public Dictionary<string, string> DicKeynote1 { get; set; }
		public Dictionary<string, string> DicKeynote2 { get; set; }
		public Dictionary<string, string> DicKeynote3 { get; set; }
		public Dictionary<string, string> DicRemark { get; set; }
	}
}