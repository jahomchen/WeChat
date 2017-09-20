﻿using JahomWeChat.DataAccess;
using JahomWeChat.Models.EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JahomPersonalWechat.Common.DailyQuestionMsg
{
	public class SendData : BaseSend
	{
		JahomDBContext jahomDBContext = new JahomDBContext();

		public SendData(string templateId = null)
		{
			Template_id = templateId ?? Template_id;
		}

		protected override void SendCore(string openId)
		{
			var records = jahomDBContext.Record.ToList();
			var random = new Random().Next(0, records.Count() - 1);
			var record = records.OrderBy(c => c.CreateTime).Skip(random).Take(1).FirstOrDefault();

			templateMsgContent.OpenId = openId;
			templateMsgContent.Url = "http://www.jahom.site/home/RecordDetail?recordId=" + record.ID;
			templateMsgContent.Template_id = Template_id;
			templateMsgContent.DicFirst = new Dictionary<string, string>();
			templateMsgContent.DicFirst.Add(string.Format("{0}\\n",record.Title), "#173177");
			templateMsgContent.DicKeynote = new Dictionary<string, string>();
			templateMsgContent.DicKeynote.Add(string.Format("{0}\\n", record.Summary), "#173177");

			ExcuteSend();
		}
	}
}
