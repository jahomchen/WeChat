using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace JahomWeChat.Common
{
	public class Logger
	{
		public static void Error(string errorStr)
		{
			var directoryPath = @"c:\error\" + DateTime.Now.Date.ToString("yyyyMMdd") + @"\";
			var fileName = "log.txt";

			if (!Directory.Exists(directoryPath))
			{
				Directory.CreateDirectory(directoryPath);
			}
			File.AppendAllText(directoryPath + @"\" + fileName, DateTime.Now + ": " + errorStr + "\r\n", Encoding.UTF8);

		}

		public static void Record(string recordStr)
		{
			var directoryPath = @"c:\error\";
			var fileName = "Record.txt";

			if (!Directory.Exists(directoryPath))
			{
				Directory.CreateDirectory(directoryPath);
			}
			File.AppendAllText(directoryPath + @"\" + fileName, DateTime.Now + ": " + recordStr + "\r\n\r\n", Encoding.UTF8);

		}
	}
}