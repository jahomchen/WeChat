using JahomWeChat.DataAccess;
using JahomWeChat.Models;
using JahomWeChat.Models.EntityModel;
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

		protected override void SendCore(string openId)
		{
			var user = jahomDBContext.User.FirstOrDefault(u => u.OpenId == openId);
			var userTips = user.Tips.Split(' ');
			Record record = null;
			var recordForUser = new List<Record>();

			var records = jahomDBContext.Record;
			foreach (var uTip in userTips)
			{
				recordForUser.AddRange(records.Where(r => r.Tips.Contains(user.Tips)));
			}

			if (record == null)
			{
				var random = new Random().Next(0, records.Count() - 1);
				record = records.OrderBy(c => c.CreateTime).Skip(random).Take(1).FirstOrDefault();
			}

			templateMsgContent.OpenId = OpenId;
			templateMsgContent.Url = "http://www.jahom.site/home/RecordDetail?recordId=" + record.ID;
			templateMsgContent.Template_id = Template_id;
			templateMsgContent.DicFirst = new Dictionary<string, string>();
			templateMsgContent.DicFirst.Add("内容精选\\n", "#173177");
			templateMsgContent.DicKeynote = new Dictionary<string, string>();
			templateMsgContent.DicKeynote.Add(string.Format("{0}\\n", record.Title), "#173177");

			ExcuteSend();
		}
	}
}
