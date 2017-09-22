using JahomWeChat.Common;
using JahomWeChat.DataAccess;
using JahomWeChat.Models.EntityModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JahomPersonalWechat.Common.DailyQuestionMsg
{
	public class SendData : BaseSend
	{
		public SendData(string templateId = null)
		{
			Template_id = templateId ?? Template_id;
		}

		protected override void SendCore(string openId, Record record)
		{
			templateMsgContent.OpenId = openId;
			templateMsgContent.Url = "http://www.jahom.site/home/RecordDetail?recordId=" + record.ID;
			templateMsgContent.Template_id = Template_id;
			templateMsgContent.DicFirst = new Dictionary<string, string>();
			templateMsgContent.DicFirst.Add(string.Format("{0}\\n", record.Title), "#173177");
			templateMsgContent.DicKeynote = new Dictionary<string, string>();
			templateMsgContent.DicKeynote.Add(string.Format("{0}\\n", record.Summary), "#173177");

			ExcuteSend();
		}
	}
}
