using JahomWeChat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace JahomWeChat.Common
{
	public class TwoDimensionCodeManage
	{
		const string urlGetTwoDimensionCodeByTickt = "https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket={0}";
		const string urlQRCode = "https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token={0}";
		static string persitenceStr = @"{""action_name"": ""QR_LIMIT_STR_SCENE"", ""action_info"": { ""scene"": { ""scene_str"":""#""}}}";

		public static string CreateUrl(string inputId, string codeType)
		{
			string dataFormat = persitenceStr.Replace("#", inputId + "&" + codeType);
			string tokenName = AccessTokenManage.GetAccessTokenName();
			string responseContent = string.Empty;
			string urlCreateQR = string.Format(urlQRCode, tokenName);
			HttpManager.Request(urlCreateQR, out responseContent, WebRequestMethods.Http.Post, data: dataFormat);
			Ticket tick = Newtonsoft.Json.JsonConvert.DeserializeObject<Ticket>(responseContent);
			string strTicket = System.Web.HttpUtility.UrlEncode(tick.ticket);
			string url = string.Format(urlGetTwoDimensionCodeByTickt, strTicket);
			return url;
		}
	}
}