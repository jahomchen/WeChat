using JahomWeChat.Common;
using System.IO;
using System.Web.Mvc;

namespace JahomWeChat.Controllers
{
    public class WeChatInterfaceController : Controller
    {
		public ContentResult WeChatInterfaceValid()
		{
			string result = string.Empty;
			if (Request.HttpMethod.ToLower() == "post")
			{
				Stream s = HttpContext.Request.InputStream;
				byte[] b = new byte[s.Length];
				s.Read(b, 0, (int)s.Length);
				var postStr = System.Text.Encoding.UTF8.GetString(b);

				if (!string.IsNullOrEmpty(postStr))
				{
					result = ControllerHelper.ResponseMsg(postStr);
				}
			}
			else
			{
				string echoStr = Request.QueryString["echoStr"];
				if (!string.IsNullOrEmpty(echoStr) && ControllerHelper.CheckSignature(HttpContext))
				{
					result = echoStr;
				}
			}

			return Content(result);
		}
	}
}