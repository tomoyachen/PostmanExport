using Fiddler;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace PostmanExport.FiddlerExtensions
{
	[ProfferFormat("Postman脚本文件 v1.0", "导出Postman脚本文件 (基于collection v2.1.0) \r\n作者: tomoya_chen")]
	public class PostmanExporter : ISessionExporter, IDisposable
	{
		public void Dispose()
		{
		}

		public bool ExportSessions(string sExportFormat, Session[] oSessions, Dictionary<string, object> dictOptions, EventHandler<ProgressCallbackEventArgs> evtProgressNotifications)
		{
			bool flag = true;
			string filePath = Utilities.ObtainSaveFilename("Export As" + sExportFormat, "Postman Script (*.json)|*.json");
			bool result;
			if (filePath == null)
			{
				result = false;
			}
			else
			{
				try
				{
					Collection col = new Collection(oSessions);
				    saveAsPostmanScript(col.generateContent(), filePath);
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

        public void saveAsPostmanScript(string content, string filePath)
        {
            Encoding encoding = new UTF8Encoding(true);
            StreamWriter streamWriter = null;
            try
            {
                streamWriter = new StreamWriter(filePath, false, encoding);
                streamWriter.Write(content);
                MessageBox.Show("导出成功 !", "提示", MessageBoxButtons.OK);
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
