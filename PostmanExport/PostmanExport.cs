using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fiddler;

namespace PostmanExport
{
    [ProfferFormat("Postman", "导出Postman脚本文件")]
    public class PostmanExport : ISessionExporter, IDisposable
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public bool ExportSessions(string sExportFormat, Session[] oSessions, Dictionary<string, object> dictOptions, EventHandler<ProgressCallbackEventArgs> evtProgressNotifications)
        {
            throw new NotImplementedException();
            
        }
    }
}
