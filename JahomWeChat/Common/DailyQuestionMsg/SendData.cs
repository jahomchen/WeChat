﻿using JahomWeChat.DataAccess;
using JahomWeChat.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JahomPersonalWechat.Common.DailyQuestionMsg
{
	public class SendData : BaseSend
	{
		JahomDBContext jahomDBContext = new JahomDBContext();

		public SendData(string openId = null, string templateId = null)
		{
			OpenId = openId;
			Template_id = templateId ?? Template_id;
		}

		protected override List<string> SendCore(string openId)
		{
			var user = jahomDBContext.User.FirstOrDefault(u => u.OpenId == openId);
			var records = jahomDBContext.Record;
			var record = records.FirstOrDefault(r => r.Tips.Contains(user.Tips));
			if (record == null)
			{
				var random = new Random().Next(0, records.Count() - 1);
				record = records.OrderBy(c => c.CreateTime).Skip(random).Take(1).FirstOrDefault();
			}
			if (record != null)
			{
				templateMsgContent.SendDataInfo = new SendDataInfo() { OpenId = openId, Url = "http://www.jahom.site/home/RecordDetail?recordId=" + record.ID, UserId = user.ID };
				templateMsgContent.Template_id = Template_id;
				templateMsgContent.DicFirst = new Dictionary<string, string>();
				templateMsgContent.DicFirst.Add("内容精选\\n", "#173177");
				templateMsgContent.DicKeynote = new Dictionary<string, string>();
				templateMsgContent.DicKeynote.Add(string.Format("{0}\\n", record.Title), "#173177");

				return ExcuteSend();
			}
			else
			{
				return new List<string>() { "GetSendDataInfo is Null" };
			}
		}
	}
}
