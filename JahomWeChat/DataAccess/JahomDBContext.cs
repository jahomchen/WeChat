using JahomWeChat.Models.EntityModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace JahomWeChat.DataAccess
{
	public class JahomDBContext : DbContext
	{
		//static JahomDBContext()
		//{
		//	Database.SetInitializer(new CreateDatabaseIfNotExists<JahomDBContext>());
		//}


		public DbSet<Record> Record { get; set; }
		public DbSet<User> User { get; set; }
		public DbSet<Reply> Reply { get; set; }
	}
}