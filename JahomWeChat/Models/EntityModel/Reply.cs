using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace JahomWeChat.Models.EntityModel
{
	public class Reply
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid ID { get; set; }
		public DateTime CreateTime { get; set; }
		public DateTime ModifyTime { get; set; }
		public Guid PId { get; set; }
		public Guid RecordId { get; set; }
		public Guid FromUserId { get; set; }
		public string FromUserName { get; set; }
		public Guid ToUserId { get; set; }
		public string ToUserName { get; set; }
		public string Message { get; set; }
	}
}