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
			if (oSessions.Length < 1)
			{
			    MessageBox.Show("没有任何可以导出的会话 !", "提示", MessageBoxButtons.OK);
                result = false;
			}else if (filePath == null || filePath == "")
			{
                result = false;
            }
			else if (oSessions.Length > 5000)
			{
			    MessageBox.Show("当前会话数量超过5000条，请移除部分会话后再导出 !", "提示", MessageBoxButtons.OK);
                result = false;
			}
            else 
			{
				try
				{
					Collection col = new Collection(oSessions);
				    FiddlerApplication.Log.LogString("filePath >>>" + filePath);
                    saveAsPostmanScript(col.generateContent(), filePath);
				    MessageBox.Show("导出成功 !", "提示", MessageBoxButtons.OK);
                }
				catch (Exception ex)
				{
					FiddlerApplication.Log.LogString("导出脚本出错，错误信息如下：");
					FiddlerApplication.Log.LogString(ex.Message);
					FiddlerApplication.Log.LogString(ex.StackTrace);
				    MessageBox.Show("导出失败 !\r\n" + ex, "提示", MessageBoxButtons.OK);
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
