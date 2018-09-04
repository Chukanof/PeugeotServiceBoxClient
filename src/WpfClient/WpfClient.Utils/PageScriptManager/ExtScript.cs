using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfClient.Utils.PageScriptManager
{
    public class ExtScript
    {
        public ExtScript()
        {
            ScriptType = ExtScriptType.Origin;
        }
        public ExtScriptType ScriptType { get; set; }
        public string Script { get; set; }
    }
}
