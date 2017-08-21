using JahomWeChat.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JahomWeChat.Filter
{
	public class GetUserAttribute:ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			base.OnActionExecuting(filterContext);

			var context = filterContext.HttpContext;
			var code = context.Request.Params["code"];
			var user = ControllerHelper.GetUserInfoByCodeOrCookie(context, code);
			if (user.ID != Guid.Empty)
			{
				HttpContext.Current.Items["USER"] = user;
			}
			else
			{
				filterContext.Result = new EmptyResult();
				filterContext.HttpContext.Response.Redirect("~/Home/AddUser?openId=" + user.OpenId);
			}
		}
	}
}