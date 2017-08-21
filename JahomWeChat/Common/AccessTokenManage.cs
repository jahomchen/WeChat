using JahomWeChat.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Caching;

namespace JahomWeChat.Common
{
	public class AccessTokenManage
	{
		const string urlGetAccessTokenName = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}";
		const string urlGetAccessTokenNameByCode = "https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={1}&code={2}&grant_type=authorization_code";

		public static string GetAccessTokenName(bool getNewToken = false)
		{
			var tokenRedisStr = HttpRuntime.Cache.Get(ConstString.AppId);
			if (tokenRedisStr == null || getNewToken)
			{
				var accessToken = CreateAccessToken();
				if (accessToken != null)
				{
					tokenRedisStr = accessToken.access_token;
					HttpRuntime.Cache.Insert(ConstString.AppId, tokenRedisStr, null, DateTime.Now.AddSeconds(7000), Cache.NoSlidingExpiration, CacheItemPriority.Normal, (k, v, r) => { GetAccessTokenName(true); });
				}
			}
			return tokenRedisStr.ToString();
		}

		public static AccessToken GetAccessTokenNameByCode(string code)
		{
			return CreateAccessToken(code);
		}

		static AccessToken CreateAccessToken(string code = null)
		{
			AccessToken accessToken = null;
			string responseContent = string.Empty;
			byte[] data = null;
			string url = string.IsNullOrEmpty(code) ? string.Format(urlGetAccessTokenName, ConstString.AppId, ConstString.AppSecret) : string.Format(urlGetAccessTokenNameByCode, ConstString.AppId, ConstString.AppSecret, code);
			bool ret = HttpManager.Request(url, out responseContent, WebRequestMethods.Http.Get, data: data);
			if (!string.IsNullOrWhiteSpace(responseContent))
			{
				accessToken = new AccessToken();
				accessToken = JsonConvert.DeserializeAnonymousType(responseContent, accessToken);
			}
			return accessToken;
		}

	}
}