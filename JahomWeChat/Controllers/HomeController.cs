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
		public JsonResult AddSomething(Record record)
		{
			try
			{
				var user = HttpContext.Items["USER"] as User;
				record.IsCompleted = true;
				record.UserId = user.ID;
				record.UserName = user.UserName;
				record.Summary = record.Content.Substring(0, 10);
				record.CreateTime = DateTime.Now;
				record.ModifyTime = DateTime.Now;
				jahomDBContext.Record.Add(record);
				jahomDBContext.SaveChanges();
			}
			catch (Exception ex)
			{
				Logger.Error(ex.Message);
				Logger.Error(ex.StackTrace);
				return Json(new ResultMsg() { IsSuccess = false, Msg = ex.Message });
			}

			return Json(new ResultMsg() { IsSuccess = true, Msg = record.ID.ToString() });
		}

		[GetUser]
		public ActionResult MyStory()
		{
			var user = HttpContext.Items["USER"] as User;
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
			return View();
		}
	}
}