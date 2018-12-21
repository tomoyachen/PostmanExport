using Fiddler;
using System;
using System.Collections.Generic;

namespace PostmanExport.FiddlerExtensions
{
	[ProfferFormat("Postman Script (Postman脚本)", "Export Postman Script. \r\n导出Postman脚本")]
	public class PostmanExporter : ISessionExporter, IDisposable
	{
		public void Dispose()
		{
		}

		public bool ExportSessions(string sExportFormat, Session[] oSessions, Dictionary<string, object> dictOptions, EventHandler<ProgressCallbackEventArgs> evtProgressNotifications)
		{
			bool flag = true;
			string text = Utilities.ObtainSaveFilename("Export As" + sExportFormat, "Postman Script (*.jmx)|*.jmx");
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
