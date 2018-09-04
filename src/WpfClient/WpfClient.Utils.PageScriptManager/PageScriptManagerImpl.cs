using HandlebarsDotNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfClient.Utils.PageScriptManager
{
    public class PageScriptManagerImpl : IPageScriptManager
    {

        private readonly ICredentialsManager _credentialsManager;

        public PageScriptManagerImpl(ICredentialsManager credentialsManager)
        {
            _credentialsManager = credentialsManager ?? throw new ArgumentNullException(nameof(credentialsManager));


        }
        private Dictionary<string, ExtScript> container;

        public Dictionary<string, ExtScript> Container
        {
            get
            {
                if (container == null)
                {
                    InitializeContainer(Path.Combine(Environment.CurrentDirectory, "PageScripts"));
                }

                return container;
            }
            set { container = value; }
        }

        public void InitializeContainer(string sourcePath)
        {
            Dictionary<string, ExtScript> result = null;
            var isExistSourceDirectory = Directory.Exists(sourcePath);

            //if (!isExistSourceDirectory) 

            var allFiles = Directory.GetFiles(sourcePath);

            foreach (var item in allFiles)
            {
                var newExtScript = new ExtScript();

                var fileNameSegment = Path.GetFileNameWithoutExtension(item);
                var absPathValue = NormalizeFileName(fileNameSegment);

                using (var fs = File.Open(item, FileMode.Open))
                {
                    using (var sr = new StreamReader(fs))
                    {
                        newExtScript.Script = sr.ReadToEnd();
                    }
                }
                if (absPathValue.Contains("{{") && absPathValue.Contains("}}"))
                {
                    newExtScript.ScriptType = ExtScriptType.Template;

                    absPathValue = absPathValue.TrimStart('{');
                    absPathValue = absPathValue.TrimEnd('}');
                }

                if (result == null)
                    result = new Dictionary<string, ExtScript>();

                result.Add(absPathValue, newExtScript);
            }

            Container = result;
        }

        private string NormalizeFileName(string fileName)
        {
            var result = fileName;

            result = result.Replace("--", "/");
            result = result.Replace('-', '.');

            return result;
        }

        public async Task<string> ResolveScriptAsync(Uri uri)
        {
            string result = null;
            var cred = await _credentialsManager.CurrentCredentials;

            if (cred == null)
                return result;

            var isExist = Container.ContainsKey(uri.AbsolutePath);

            if (isExist == false)
                return Container["/"].Script;

            var extScript = Container[uri.AbsolutePath];

            if (extScript.ScriptType == ExtScriptType.Origin)
                return extScript.Script;

            var template = Handlebars.Compile(extScript.Script);
            result = template(cred);

            return result;
        }
    }
}
