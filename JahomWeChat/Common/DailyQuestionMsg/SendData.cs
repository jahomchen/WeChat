using JahomWeChat.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace JahomPersonalWechat.Common.DailyQuestionMsg
{
	public class SendData : BaseSend
	{
		public SendData(string openId = null, string templateId = null)
		{
			OpenId = openId;
			Template_id = templateId ?? Template_id;
		}

		protected override List<string> SendCore(string openId)
		{
			var dataInfos = new List<SendDataInfo>() { new SendDataInfo() { OpenId = openId } };//TODO 数据库获取数据。
			if (dataInfos != null)
			{
				templateMsgContent.OpenIdRealList = new List<SendDataInfo>();
				templateMsgContent.OpenIdRealList.AddRange(dataInfos);
				templateMsgContent.Template_id = Template_id;
				templateMsgContent.DicFirst = new Dictionary<string, string>();
				templateMsgContent.DicFirst.Add("新消息通知\\n", "#173177");
				templateMsgContent.DicKeynote1 = new Dictionary<string, string>();
				templateMsgContent.DicKeynote1.Add(" 你收到了一个新的通知，赶紧打开吧\\n", "#173177");
				templateMsgContent.DicKeynote2 = new Dictionary<string, string>();
				templateMsgContent.DicKeynote2.Add(DateTime.Now.ToString("yyyy-MM-dd"), "#173177");

				return ExcuteSend();
			}
			else
			{
				return new List<string>() { "GetSendDataInfo is Null" };
			}
		}
	}
}
