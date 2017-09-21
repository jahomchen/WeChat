using JahomWeChat.DataAccess;
using JahomWeChat.Models;
using JahomWeChat.Models.EntityModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace JahomWeChat.Common
{
	public class ControllerHelper
	{
		public static bool CheckSignature(HttpContextBase context)
		{
			string signature = context.Request.QueryString["signature"].ToString();
			string timestamp = context.Request.QueryString["timestamp"].ToString();
			string nonce = context.Request.QueryString["nonce"].ToString();
			string[] ArrTmp = { ConstString.Token, timestamp, nonce };

			string str = string.Format("验证的时候，signature ={0},timestamp={1},nonce={2}", signature, timestamp, nonce);

			Array.Sort(ArrTmp);     //字典排序
			string tmpStr = string.Join("", ArrTmp);
			tmpStr = TicketManage.SHA1(tmpStr);
			tmpStr = tmpStr.ToLower();

			return tmpStr == signature;
		}

		public static string ResponseMsg(string postStr)
		{
			XmlDocument postObj = new XmlDocument();
			postObj.LoadXml(postStr);
			string FromUserName = GetValueByTagName(postObj, "FromUserName");
			string ToUserName = GetValueByTagName(postObj, "ToUserName");
			string Content = "欢迎关注Jahom";
			string printMsgType = "text";

			var textpl = "<xml><ToUserName><![CDATA[" + FromUserName + "]]></ToUserName>" +
				"<FromUserName><![CDATA[" + ToUserName + "]]></FromUserName>" +
				"<CreateTime>" + ConvertDateTimeInt(DateTime.Now) + "</CreateTime><MsgType><![CDATA[" + printMsgType + "]]></MsgType>" +
				"<Content><![CDATA[" + Content + "]]></Content><FuncFlag>0</FuncFlag></xml> ";

			return textpl;
		}

		public static User GetUserInfoByCodeOrCookie(HttpContextBase context, string code = null)
		{
			JahomDBContext jahomDBContext = new JahomDBContext();

			if (string.IsNullOrEmpty(code))
			{
				var userStr = CookieHelper.GetCookie(context);
				if (!string.IsNullOrEmpty(userStr))
				{
					return JsonConvert.DeserializeObject<User>(userStr);
				}
				return null;
			}
			else
			{
				AccessToken accToken = AccessTokenManage.GetAccessTokenNameByCode(code);
				User user = jahomDBContext.User.FirstOrDefault(u => u.OpenId == accToken.openId);
				if (user != null)
				{
					CookieHelper.SetCookie(context, JsonConvert.SerializeObject(user));
				}
				else
				{
					user = new User() { OpenId = accToken.openId };
				}

				return user;
			}
		}

		public static string GetRecordSummary(string recordContent)
		{
			var summaryWithHtmlTag = recordContent.Substring(0, 20);
			return System.Text.RegularExpressions.Regex.Replace(summaryWithHtmlTag, "<[^>]*>", "");
		}

		public static Record GetMatchedRecord(string openId)
		{
			JahomDBContext jahomDBContext = new JahomDBContext();

			var records = jahomDBContext.Record.ToList();
			var random = new Random().Next(0, records.Count() - 1);
			var record = records.OrderBy(c => c.CreateTime).Skip(random).Take(1).FirstOrDefault();

			return record;
		}

		#region 私有方法

		static string GetValueByTagName(XmlDocument postObj, string tagName)
		{
			string retValue = string.Empty;
			var retValueList = postObj.GetElementsByTagName(tagName);
			for (int i = 0; i < retValueList.Count; i++)
			{
				if (retValueList[i].ChildNodes[0].NodeType == System.Xml.XmlNodeType.CDATA)
				{
					retValue = retValueList[i].ChildNodes[0].Value;
				}
			}

			return retValue;
		}

		static int ConvertDateTimeInt(DateTime time)
		{
			System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
			return (int)(time - startTime).TotalSeconds;
		}

		#endregion
	}
}