using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace JahomWeChat.Models.EntityModel
{
	public class User
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid ID { get; set; }
		public DateTime CreateTime { get; set; }
		public DateTime ModifyTime { get; set; }
		public string OpenId { get; set; }
		public string UserName { get; set; }
		public string Email { get; set; }
		public string Sign { get; set; }
		public int Gender { get; set; }
		public string Tips { get; set; }
	}
}