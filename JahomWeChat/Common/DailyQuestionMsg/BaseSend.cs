﻿using JahomWeChat.Common;
using JahomWeChat.DataAccess;
using JahomWeChat.Models;
using JahomWeChat.Models.EntityModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace JahomPersonalWechat.Common.DailyQuestionMsg
{
	public class BaseSend
	{
		const string urlSendTemplateMsg = "https://api.weixin.qq.com/cgi-bin/message/template/send?access_token={0}";

		public string Template_id = ConstString.DefaultTemplateId;
		public TemplateMsgContent templateMsgContent = new TemplateMsgContent();

		public void Send()
		{
			try
			{
				var lsOpenId = new List<string>();
				var ret = UserManage.GetOpenIdListByAccessTokenNameAndOpenId(AccessTokenManage.GetAccessTokenName());
				JObject Jo = (JObject)JsonConvert.DeserializeObject(ret);
				JArray Jarows = JArray.Parse(Jo["data"]["openid"].ToString());
				for (int i = 0; i < Jarows.Count; i++)
				{
					lsOpenId.Add(Jarows[i].ToString());
				}
				var record = ControllerHelper.GetMatchedRecord();
				lsOpenId.ForEach(p => SendCore(p, record));
			}
			catch (Exception ex)
			{
				Logger.Error("给用户发送通知时失败:" + ex);
			}
		}

		protected virtual void SendCore(string openId, Record record)
		{
		}

		protected void ExcuteSend()
		{
			string responseContent = string.Empty;
			string url = string.Format(urlSendTemplateMsg, AccessTokenManage.GetAccessTokenName());

			string first = @"first"": {""value"":""#"",""color"":""$"" },";
			string keynote = @"""keyword1"":{""value"":""#"", ""color"":""$"" }";
			string sendTemplateMsgContent = @"{ ""touser"":""$"",""template_id"":""#"",""url"":""%"",""data"":{""*""}}";
			first = first.Replace("#", templateMsgContent.DicFirst.First().Key).Replace("$", templateMsgContent.DicFirst.First().Value);
			keynote = keynote.Replace("#", templateMsgContent.DicKeynote.First().Key).Replace("$", templateMsgContent.DicKeynote.First().Value);
			string dataTemp = first + keynote;
			string sendTemplateMsg = sendTemplateMsgContent.Replace("$", templateMsgContent.OpenId).Replace("#", templateMsgContent.Template_id).Replace("%", templateMsgContent.Url).Replace("*", dataTemp);
			sendTemplateMsg = sendTemplateMsg.Remove(sendTemplateMsg.LastIndexOf('"'), 1);
			HttpManager.Request(url, out responseContent, WebRequestMethods.Http.Post, data: sendTemplateMsg);
			if (responseContent.Contains("40001"))//token失效码
			{
				url = string.Format(urlSendTemplateMsg, AccessTokenManage.GetAccessTokenName(true));
				HttpManager.Request(url, out responseContent, WebRequestMethods.Http.Post, data: sendTemplateMsg);
			}
		}
	}
}
