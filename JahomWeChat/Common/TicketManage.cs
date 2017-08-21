using JahomWeChat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Web.Security;

namespace JahomWeChat.Common
{
	public class TicketManage
	{
		private const string urlJsapiTicket = "https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token={0}&type=jsapi";
		private static string[] strArr = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };

		public static string GetJsapiTicket()
		{
			var jsapiTicketRedisStr = HttpRuntime.Cache.Get(ConstString.AppId);
			if (jsapiTicketRedisStr == null)
			{
				string responseContent = string.Empty;
				string tokenName = AccessTokenManage.GetAccessTokenName();
				string url = string.Format(urlJsapiTicket, tokenName);
				bool res = HttpManager.Request(url, out responseContent, WebRequestMethods.Http.Post, data: "");
				var ticketObj = Newtonsoft.Json.JsonConvert.DeserializeObject<Ticket>(responseContent);
				if (ticketObj.ErrorCode == 0)
				{
					jsapiTicketRedisStr = ticketObj.ticket;
					HttpRuntime.Cache.Insert(ConstString.AppId, jsapiTicketRedisStr, null, DateTime.Now.AddSeconds(Convert.ToInt32(ticketObj.expires_in) - 100), Cache.NoSlidingExpiration);
				}
			}

			return jsapiTicketRedisStr.ToString();
		}

		public static string GetRandomStr()
		{
			StringBuilder sb = new StringBuilder();
			Random rd = new Random();

			for (int i = 0; i < 15; i++)
			{
				sb.Append(strArr[rd.Next(0, strArr.Count())]);
			}
			return sb.ToString();
		}

		public static long GetTimeStamp()
		{
			return (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;

		}

		public static string SHA1(string str)
		{
			return FormsAuthentication.HashPasswordForStoringInConfigFile(str, "SHA1");
		}
	}
}