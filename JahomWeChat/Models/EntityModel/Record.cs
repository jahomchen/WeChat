using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace JahomWeChat.Models.EntityModel
{
	
	public class Record
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid ID { get; set; }
		public DateTime CreateTime { get; set; }
		public DateTime ModifyTime { get; set; }
		public Guid UserId { get; set; }
		public string UserName { get; set; }
		public string Title { get; set; }
		public string Content { get; set; }
		public string Summary { get; set; }
		public string Tips { get; set; }
		public bool IsCompleted { get; set; }

		public bool IsSpecial { get; set; }
	}
}