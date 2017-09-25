using JahomPersonalWechat.Common.DailyQuestionMsg;
using JahomWeChat.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace JahomWeChat
{
	public class MvcApplication : HttpApplication
	{
		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);

			Logger.Error("网站启动了:" + DateTime.Now);

			AccessTokenManage.GetAccessTokenName(true);
			ButtonHelp.InitButton();

			Task.Factory.StartNew(() =>
			{
				var msgContext = new MsgContext(new SendData());
				while (true)
				{
					msgContext.Process();
					Thread.Sleep(60000 * 1435);
				}
			});
		}

		protected void Application_Error(Object sender, EventArgs e)
		{
			Exception lastError = Server.GetLastError();
			if (lastError != null)
			{
				Logger.Error(lastError.Message);
				Logger.Error(lastError.StackTrace);
			}
			Server.ClearError();
			Response.Write("网站出现错误：" + lastError.Message + "\r\n" + lastError.StackTrace);
		}
	}

}
