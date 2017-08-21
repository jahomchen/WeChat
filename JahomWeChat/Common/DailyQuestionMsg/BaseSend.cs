using JahomWeChat.Common;
using JahomWeChat.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace JahomPersonalWechat.Common.DailyQuestionMsg
{
	public class BaseSend
	{
		const string urlSendTemplateMsg = "https://api.weixin.qq.com/cgi-bin/message/template/send?access_token={0}";

		public string Template_id = ConstString.DefaultTemplateId;
		public TemplateMsgContent templateMsgContent = new TemplateMsgContent();
		public string OpenId { get; set; }

		public List<string> Send()
		{
			var result = new List<string>();
			var lsOpenId = new List<string>();
			var ret = UserManage.GetOpenIdListByAccessTokenNameAndOpenId(AccessTokenManage.GetAccessTokenName());
			JObject Jo = (JObject)JsonConvert.DeserializeObject(ret);
			JArray Jarows = JArray.Parse(Jo["data"]["openid"].ToString());
			for (int i = 0; i < Jarows.Count; i++)
			{
				lsOpenId.Add(Jarows[i].ToString());
			}

			if (string.IsNullOrEmpty(OpenId))
			{
				lsOpenId.ForEach(p => result.AddRange(SendCore(p)));
			}
			else
			{
				result.AddRange(SendCore(lsOpenId.FirstOrDefault(p => p.ToLower() == OpenId.ToLower())));
			}

			if (result == null || result.Count == 0)
			{
				result.Add("没有找到匹配的OpenId");
			}

			return result;
		}

		protected virtual List<string> SendCore(string openId)
		{
			return null;
		}

		protected List<string> ExcuteSend()
		{
			List<string> result = new List<string>();
			string first = @"first"": {""value"":""#"",""color"":""$"" },";
			string keynote1 = @"""keyword1"":{""value"":""#"", ""color"":""$"" }";
			string keynote2 = @"""keyword2"":{""value"":""#"", ""color"":""$"" },";
			string sendTemplateMsgContent = @"{ ""touser"":""$"",""template_id"":""#"",""url"":""%"",""data"":{""*""}}";
			string responseContent = string.Empty;
			string url = string.Format(urlSendTemplateMsg, AccessTokenManage.GetAccessTokenName());
			first = first.Replace("#", templateMsgContent.DicFirst.First().Key).Replace("$", templateMsgContent.DicFirst.First().Value);
			keynote1 = keynote1.Replace("#", templateMsgContent.DicKeynote1.First().Key).Replace("$", templateMsgContent.DicKeynote1.First().Value);
			keynote2 = keynote2.Replace("#", templateMsgContent.DicKeynote2.First().Key).Replace("$", templateMsgContent.DicKeynote2.First().Value);
			string dataTemp = first + keynote1 + "," + keynote2;

			foreach (var item in templateMsgContent.OpenIdRealList)
			{
				string sendTemplateMsg = sendTemplateMsgContent.Replace("$", item.OpenId).Replace("#", templateMsgContent.Template_id).Replace("%", item.Url).Replace("*", dataTemp);
				sendTemplateMsg = sendTemplateMsg.Remove(sendTemplateMsg.LastIndexOf('"'), 1).Remove(sendTemplateMsg.LastIndexOf(','), 1);
				HttpManager.Request(url, out responseContent, WebRequestMethods.Http.Post, data: sendTemplateMsg);
				while (responseContent.Contains("40001"))//token失效码
				{
					url = string.Format(urlSendTemplateMsg, AccessTokenManage.GetAccessTokenName(true));
					HttpManager.Request(url, out responseContent, WebRequestMethods.Http.Post, data: sendTemplateMsg);
				}

				result.Add(responseContent);
			}

			return result;
		}
	}
}
