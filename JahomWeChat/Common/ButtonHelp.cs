using JahomWeChat.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace JahomWeChat.Common
{
	public class ButtonHelp
	{
		public static void InitButton()
		{
			var menuPath = AppDomain.CurrentDomain.BaseDirectory + "Common/Menu.txt";
			var munuStr= File.ReadAllText(menuPath);
			var munuUri = "https://api.weixin.qq.com/cgi-bin/menu/create?access_token={0}";

			string url = string.Format(munuUri, AccessTokenManage.GetAccessTokenName());
			var responseContent = string.Empty;
			HttpManager.Request(url, out responseContent, WebRequestMethods.Http.Post, data: munuStr);
			var res = Newtonsoft.Json.JsonConvert.DeserializeObject<WCResult>(responseContent);

			if (res.errcode=="40001")//token失效码
			{
				url = string.Format(munuUri, AccessTokenManage.GetAccessTokenName(true));
				HttpManager.Request(url, out responseContent, WebRequestMethods.Http.Post, data: munuStr);
			}
		}
	}
}