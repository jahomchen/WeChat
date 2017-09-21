using JahomWeChat.Common;
using JahomWeChat.DataAccess;
using JahomWeChat.Filter;
using JahomWeChat.Models;
using JahomWeChat.Models.EntityModel;
using JahomWeChat.Models.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JahomWeChat.Controllers
{
	public class HomeController : Controller
	{
		JahomDBContext jahomDBContext = new JahomDBContext();

		public ActionResult Index()
		{
			var records = jahomDBContext.Record.Where(r => r.IsCompleted).OrderByDescending(r => r.ModifyTime).
				Select(r => (new RecordSummary() { ID = r.ID, Title = r.Title, Summary = r.Summary })).ToList();
			ViewBag.records = records;
			return View();
		}

		[GetUser]
		public ActionResult AddSomething()
		{
			return View();
		}

		[HttpPost]
		[GetUser]
		[ValidateInput(false)]
		public ActionResult AddSomething(Record record)
		{
			try
			{
				var user = HttpContext.Items["USER"] as User;
				record.IsCompleted = true;
				record.UserId = user.ID;
				record.UserName = user.UserName;
				record.Summary = ControllerHelper.GetRecordSummary(record.Content);
				record.CreateTime = DateTime.Now;
				record.ModifyTime = DateTime.Now;
				jahomDBContext.Record.Add(record);
				jahomDBContext.SaveChanges();
			}
			catch (Exception ex)
			{
				Logger.Error(ex.Message);
				Logger.Error(ex.StackTrace);
			}

			return RedirectToAction("MyStory");
		}

		[GetUser]
		public ActionResult MyStory()
		{
			var user = HttpContext.Items["USER"] as User;
			ViewBag.Sign = user.Sign;
			var records = jahomDBContext.Record.Where(r => r.UserId == user.ID && r.IsCompleted).
				OrderByDescending(r => r.ModifyTime).Select(r => (new RecordSummary() { ID = r.ID, Title = r.Title, Summary = r.Summary })).ToList();
			ViewBag.records = records;
			return View();
		}

		[HttpGet]
		public ActionResult AddUser(string openId)
		{
			ViewBag.openId = openId;
			return View();
		}

		[HttpPost]
		public ActionResult AddUser(User user)
		{
			user.CreateTime = DateTime.Now;
			user.ModifyTime = DateTime.Now;
			jahomDBContext.User.Add(user);
			jahomDBContext.SaveChanges();
			CookieHelper.SetCookie(HttpContext, JsonConvert.SerializeObject(user));
			return RedirectToAction("Index");
		}

		public ActionResult RecordDetail(Guid recordId)
		{
			var record = jahomDBContext.Record.FirstOrDefault(r => r.ID == recordId);
			ViewBag.record = record;
			var replys = jahomDBContext.Reply.Where(r => r.RecordId == record.ID).ToList();
			ViewBag.Relpys = replys;
			return View();
		}

		[GetUser]
		public ActionResult AddReply(Reply reply)
		{
			var user = HttpContext.Items["USER"] as User;
			reply.FromUserId = user.ID;
			reply.FromUserName = user.UserName;
			reply.CreateTime = DateTime.Now;
			reply.ModifyTime = DateTime.Now;
			jahomDBContext.Reply.Add(reply);
			jahomDBContext.SaveChanges();

			return RedirectToRoute(new { controller = "Home", action = "RecordDetail", recordId = reply.RecordId });
		}


		[HttpGet]
		public ActionResult Admin(string key)
		{
			if (key == "jahom")
			{
				var admin = jahomDBContext.User.FirstOrDefault(u => u.OpenId == ConstString.AdminOpenId);
				CookieHelper.SetCookie(HttpContext, JsonConvert.SerializeObject(admin));
				return View("");
			}
			else
			{
				return Content("error");
			}
		}

		[HttpPost]
		public ActionResult AddRecordByAdmin(Record record)
		{
			var adminStr = CookieHelper.GetCookie(HttpContext);
			if (!string.IsNullOrEmpty(adminStr))
			{
				var admin = JsonConvert.DeserializeObject<User>(adminStr);
				record.IsCompleted = true;
				record.UserId = admin.ID;
				record.UserName = admin.UserName;
				record.Summary = ControllerHelper.GetRecordSummary(record.Content);
				record.CreateTime = DateTime.Now;
				record.ModifyTime = DateTime.Now;
				jahomDBContext.Record.Add(record);
				jahomDBContext.SaveChanges();

				var records = jahomDBContext.Record.OrderBy(r => r.CreateTime).ToList();
				ViewBag.Records = records;
				return View();
			}
			else
			{
				return Content("error");
			}
		}

	}
}