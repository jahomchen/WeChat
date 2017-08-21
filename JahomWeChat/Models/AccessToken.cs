using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JahomWeChat.Models
{
	public class AccessToken
	{
		public string access_token { get; set; }
		public int expires_in { get; set; }
		public string RefreshAccessTokenName { get; set; }
		public string Scope { get; set; }
		public string openId { get; set; }
	}
}