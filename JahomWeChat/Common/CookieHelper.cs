using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JahomWeChat.Common
{
	public class CookieHelper
	{
		static readonly string cookieName = "OPENID";

		public static void SetCookie(HttpContextBase context, string user)
		{
			HttpCookie cookie = new HttpCookie(cookieName);
			cookie.Value = context.Server.UrlEncode(user);
			context.Response.Cookies.Add(cookie);
		}

		public static string GetCookie(HttpContextBase context)
		{
			var cookie = context.Request.Cookies[cookieName];
			return cookie != null ? context.Server.UrlDecode(cookie.Value) : null;
		}

		public static void RemoveCookie(HttpContextBase context)
		{
			var cookie = context.Request.Cookies[cookieName];
			if (cookie != null)
			{
				cookie.Expires = DateTime.Now.AddDays(-1);
				context.Response.Cookies.Add(cookie);
			}
		}
	}
}