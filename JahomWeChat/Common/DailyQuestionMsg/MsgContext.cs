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

		public void Process()
		{
			string sendTimeHour = System.Web.Configuration.WebConfigurationManager.AppSettings["sendTimeHour"];
			string sendTimeMinite = System.Web.Configuration.WebConfigurationManager.AppSettings["sendTimeMinite"];
			while (true)
			{
				var now = DateTime.Now;
				if (now.Hour.ToString() == sendTimeHour && now.Minute.ToString() == sendTimeMinite)
				{
					sendMsg.Send();
				}
				System.Threading.Thread.Sleep(55000);
			}
		}
	}
}
