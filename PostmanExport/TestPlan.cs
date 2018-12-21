using Fiddler;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;

namespace JmeterExport.FiddlerExtensions
{
	public class TestPlan
	{
		private string filePath;

		private Session[] oSessions;

		public TestPlan()
		{
		}

		public TestPlan(Session[] oSessions)
		{
		}

		public TestPlan(Session[] oSessions, string filePath)
		{
			this.filePath = filePath;
			this.oSessions = oSessions;
		}

		private string getRequestPath(Session session)
		{
			return WebUtility.HtmlEncode(session.PathAndQuery);
		}

		private string getRequestMethod(Session session)
		{
			return session.oRequest.headers.HTTPMethod.ToUpper();
		}

		private string getRequestBody(Session session)
		{
			return WebUtility.HtmlEncode(session.GetRequestBodyAsString());
		}

		private string getProtocol(Session session)
		{
			return session.oRequest.headers.UriScheme.ToLower();
		}

		private bool isExistContentType(Session session)
		{
			return session.oRequest.headers.Exists("Content-Type");
		}

		private string getContentType(Session session)
		{
			//return session.oRequest.get_Item("Content-Type");
		    return session.oRequest["Content-Type"].ToString();
        }

		private string getIpAddress(Session session)
		{
			return session.hostname;
		}

		public string generateContent()
		{
			bool flag = this.oSessions.Length == 0;
			if (flag)
			{
				FiddlerApplication.Log.LogString("Error, There has no sessions, please check.");
				throw new Exception();
			}
			Session session = this.oSessions[0];
			string text = this.getIpAddress(session);
			string text2 = session.port.ToString();
			string encoding = "utf-8";
			string text3 = this.getProtocol(session);
			Element element = new Element();
			string text4 = element.addConfigTestElement(null, text, text2, encoding, text3, "HTTP请求默认值");
			text4 = element.addCookieManager(text4, "HTTP Cookie 管理器");
			text4 = element.addArguments(text4, "用户定义的变量");
			StringBuilder stringBuilder = new StringBuilder();
			Session[] array = this.oSessions;
			for (int i = 0; i < array.Length; i++)
			{
				Session session2 = array[i];
				string text5 = null;
				text5 = element.addJSONPathAssertion(text5, "$.stat", "OK", "验证响应结果");
				string requestPath = this.getRequestPath(session2);
				bool flag2 = text.Equals(this.getIpAddress(session2));
				if (flag2)
				{
					text = "";
				}
				else
				{
					text = this.getIpAddress(session2);
				}
				bool flag3 = text2.Equals(session2.port.ToString());
				if (flag3)
				{
					text2 = "";
				}
				else
				{
					text2 = session2.port.ToString();
				}
				bool flag4 = text3.Equals(this.getProtocol(session2));
				if (flag4)
				{
					text3 = "";
				}
				else
				{
					text3 = this.getProtocol(session2);
				}
				bool flag5 = this.isExistContentType(session2);
				string value;
				if (flag5)
				{
					string contentType = this.getContentType(session2);
					bool flag6 = !contentType.Contains("boundary") && !contentType.Contains("octet - stream") && !contentType.Contains("image") && !contentType.Contains("video") && !contentType.Contains("audio") && !contentType.Contains("tar") && !contentType.Contains("zip") && !contentType.Contains("rtf") && (!contentType.Contains("pdf") && !contentType.Contains("powerpoint")) && !contentType.Contains("x-compress") && !contentType.Contains("msword");
					if (flag6)
					{
						value = this.getRequestBody(session2);
					}
					else
					{
						value = "";
					}
				}
				else
				{
					value = this.getRequestBody(session2);
				}
				bool flag7 = requestPath.Contains("/unode/stor/uploadPart");
				if (flag7)
				{
					value = "";
				}
				text5 = element.surroundByHTTPSamplerProxy(text5, text, text2, text3, this.getRequestMethod(session2), requestPath, value, requestPath);
				stringBuilder.Append(text5);
			}
			string value2 = element.surroundByThreadGroup(stringBuilder.ToString(), "线程组");
			StringBuilder stringBuilder2 = new StringBuilder();
			stringBuilder2.Append(text4);
			stringBuilder2.Append(value2);
			text4 = element.addViewResultTree(stringBuilder2.ToString(), "察看结果树");
			text4 = element.addAssertionResult(text4, "断言结果");
			text4 = element.surroundByTestPlan(text4, "测试计划");
			FiddlerApplication.Log.LogString(text4);
			return element.addXmlHead(XDocument.Parse(text4).ToString());
		}

		public void saveAsJmeterScript()
		{
			Encoding encoding = new UTF8Encoding(true);
			string value = this.generateContent();
			StreamWriter streamWriter = null;
			try
			{
				streamWriter = new StreamWriter(this.filePath, false, encoding);
				streamWriter.Write(value);
				MessageBox.Show("Jmeter脚本导出成功 !", "提示", MessageBoxButtons.OK);
			}
			finally
			{
				bool flag = streamWriter != null;
				if (flag)
				{
					streamWriter.Close();
					streamWriter.Dispose();
				}
			}
		}
	}
}
