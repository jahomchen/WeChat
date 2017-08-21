using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace JahomWeChat.Common
{
	public class HttpManager
	{
		public static readonly Encoding RequestEncoding = Encoding.UTF8;
		public static readonly Encoding ResponseEncoding = Encoding.UTF8;

		/// <summary>
		/// 向微信服务器提交数据，并获取微信服务器响应的内容
		/// </summary>
		/// <param name="url">服务器地址</param>
		/// <param name="responseContent">返回响应内容</param>
		/// /// <param name="httpMethod">http方法</param>
		/// <param name="data">数据</param>
		/// <returns>返回是否提交成功</returns>
		public static bool Request(string url, out string responseContent,
			string httpMethod = WebRequestMethods.Http.Get, string data = null)
		{
			byte[] bytes = string.IsNullOrEmpty(data) ? null : RequestEncoding.GetBytes(data);
			return Request(url, out responseContent, httpMethod, (byte[])bytes);
		}

		/// <summary>
		/// 向微信服务器提交数据，并获取微信服务器响应的内容
		/// </summary>
		/// <param name="url">服务器地址</param>
		/// <param name="responseContent">返回响应内容</param>
		/// /// <param name="httpMethod">http方法</param>
		/// <param name="data">数据</param>
		/// <returns>返回是否提交成功</returns>
		public static bool Request(string url, out string responseContent,
			string httpMethod = WebRequestMethods.Http.Get, byte[] data = null)
		{
			byte[] responseData;
			responseContent = string.Empty;
			bool success = Request(url, out responseData, httpMethod, data);
			if (success && responseData != null && responseData.Length > 0)
				responseContent = ResponseEncoding.GetString(responseData);
			return success;
		}

		/// <summary>
		/// 向微信服务器提交数据，并获取微信服务器响应的数据
		/// </summary>
		/// <param name="url">服务器地址</param>
		/// <param name="responseData">返回响应数据</param>
		/// /// <param name="httpMethod">http方法</param>
		/// <param name="data">数据</param>
		/// <returns>返回是否提交成功</returns>
		public static bool Request(string url, out byte[] responseData,
			string httpMethod = WebRequestMethods.Http.Get, byte[] data = null)
		{
			bool success = false;
			responseData = null;
			Stream requestStream = null;
			HttpWebResponse response = null;
			Stream responseStream = null;
			MemoryStream ms = null;
			try
			{
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
				request.Method = httpMethod;
				if (data != null && data.Length > 0)
				{
					request.ContentLength = data.Length;
					requestStream = request.GetRequestStream();
					requestStream.Write(data, 0, data.Length);
				}
				response = (HttpWebResponse)request.GetResponse();
				//由于微信服务器的响应有时没有正确设置ContentLength，这里不检查ContentLength
				//if (response.ContentLength > 0)
				{
					ms = new MemoryStream();
					responseStream = response.GetResponseStream();
					int bufferLength = 2048;
					byte[] buffer = new byte[bufferLength];
					int size = responseStream.Read(buffer, 0, bufferLength);
					while (size > 0)
					{
						ms.Write(buffer, 0, size);
						size = responseStream.Read(buffer, 0, bufferLength);
					}
					responseData = ms.ToArray();
				}
				success = true;
			}
			finally
			{
				if (requestStream != null)
					requestStream.Close();
				if (responseStream != null)
					responseStream.Close();
				if (ms != null)
					ms.Close();
				if (response != null)
					response.Close();
			}
			return success;
		}

	}
}