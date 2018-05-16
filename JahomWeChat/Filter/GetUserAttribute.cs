using JahomWeChat.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JahomWeChat.Filter
{
	public class GetUserAttribute : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			base.OnActionExecuting(filterContext);

			var context = filterContext.HttpContext;
			var code = context.Request.Params["code"];
			var user = ControllerHelper.GetUserInfoByCodeOrCookie(context, code);
			if (user == null)
			{
				filterContext.Result = new EmptyResult();
				filterContext.HttpContext.Response.Write("<p>用户不存在,请点击订阅号下面两个按钮\"我的故事\",\"写点什么\"中的任意一个,添加用户名称.</p>");
			}
			else if (user.ID == Guid.Empty)
			{
				filterContext.Result = new EmptyResult();
				filterContext.HttpContext.Response.Redirect("~/Home/AddUser?openId=" + user.OpenId);
			}
			else
			{
				HttpContext.Current.Items["USER"] = user;
			}
		}
	}
}