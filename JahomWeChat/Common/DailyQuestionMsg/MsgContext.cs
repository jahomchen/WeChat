using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JahomPersonalWechat.Common.DailyQuestionMsg
{
	public class MsgContext
	{
		private BaseSend sendMsg;

		public MsgContext(BaseSend sendMsg)
		{
			this.sendMsg = sendMsg;
		}

		public List<string> Process()
		{
			string sendTimeHour = System.Web.Configuration.WebConfigurationManager.AppSettings["sendTimeHour"];
			while (true)
			{
				var now = DateTime.Now;
				if (now.Hour.ToString() == sendTimeHour && now.Minute == 0)
				{
					return sendMsg.Send();
				}
				System.Threading.Thread.Sleep(60000);
			}
		}
	}
}
