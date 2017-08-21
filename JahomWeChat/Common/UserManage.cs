using JahomWeChat.Models.EntityModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace JahomWeChat.Common
{
	public class UserManage
	{
		private const string urlGetUserInfoByAccessTokenNameAndOpenId = "https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid={1}&lang=zh_CN";
		private const string urlGetOpenIdListByAccessTokenNameAndOpenId = "https://api.weixin.qq.com/cgi-bin/user/get?access_token={0}&next_openid={1}";

		public static User GetUserInfoByAccessTokenNameAndOpenId(string accessTokenName, string openId)
		{
			User user = null;
			string responseContent = string.Empty;
			byte[] data = null;
			string url = string.Format(urlGetUserInfoByAccessTokenNameAndOpenId, accessTokenName, openId);
			bool ret = HttpManager.Request(url, out responseContent, WebRequestMethods.Http.Get, data: data);
			if (!string.IsNullOrWhiteSpace(responseContent))
			{
				user = new User();
				user = JsonConvert.DeserializeAnonymousType<User>(responseContent, user);
			}
			return user;
		}

		/// <summary>
		/// openId:第一个拉取的OPENID，不填默认从头开始拉取
		/// </summary>
		/// <param name="accessTokenName"></param>
		/// <param name="openId"></param>
		/// <returns></returns>
		public static string GetOpenIdListByAccessTokenNameAndOpenId(string accessTokenName, string openId = "")
		{
			string strJson = string.Empty;
			string responseContent = string.Empty;
			byte[] data = null;
			string url = string.Format(urlGetOpenIdListByAccessTokenNameAndOpenId, accessTokenName, openId);
			bool ret = HttpManager.Request(url, out responseContent, WebRequestMethods.Http.Get, data: data);
			if (!string.IsNullOrWhiteSpace(responseContent))
			{
				strJson = responseContent;
			}
			return strJson;
		}

		public static string GetUnionIDUrl(string originalUrl)
		{
			string url = "https://open.weixin.qq.com/connect/oauth2/authorize?appid={0}&redirect_uri={1}&response_type=code&scope=snsapi_base&state=1#wechat_redirect";
			return string.Format(url, ConstString.AppId, originalUrl);
		}

	}
}