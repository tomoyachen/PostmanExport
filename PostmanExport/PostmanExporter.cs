using Fiddler;
using System;
using System.Collections.Generic;

namespace PostmanExport.FiddlerExtensions
{
	[ProfferFormat("Postman脚本文件", "导出Postman脚本文件 (基于collection v2.1.0) \r\n作者: tomoya_chen")]
	public class PostmanExporter : ISessionExporter, IDisposable
	{
		public void Dispose()
		{
		}

		public bool ExportSessions(string sExportFormat, Session[] oSessions, Dictionary<string, object> dictOptions, EventHandler<ProgressCallbackEventArgs> evtProgressNotifications)
		{
			bool flag = true;
			string text = Utilities.ObtainSaveFilename("Export As" + sExportFormat, "Postman Script (*.json)|*.json");
			bool flag2 = text == null;
			bool result;
			if (flag2)
			{
				result = false;
			}
			else
			{
				try
				{
					Collection col = new Collection(oSessions, text);
				    col.saveAsPostmanScript();
				}
				catch (Exception ex)
				{
					FiddlerApplication.Log.LogString("导出脚本出错，错误信息如下：");
					FiddlerApplication.Log.LogString(ex.Message);
					FiddlerApplication.Log.LogString(ex.StackTrace);
					flag = false;
				}
				result = flag;
			}
			return result;
		}
	}
}
