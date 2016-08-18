using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PowerUp
{
    public class EmbeddedResourceLoader
    {
        private readonly string _resourceFolder;
        private readonly Assembly _assembly;
        private readonly Encoding _encoding;

        public EmbeddedResourceLoader(Encoding encode = null, string resourceFolder = "Resources")
        {
            _encoding = encode ?? Encoding.UTF8;
            _resourceFolder = resourceFolder;
            _assembly = Assembly.GetCallingAssembly();
        }

        public String Load(string resourceNameOnly)
        {
            var resourceName = string.Format("{0}.{1}.{2}", _assembly.GetName().Name, _resourceFolder, resourceNameOnly);
            using (var stream = _assembly.GetManifestResourceStream(resourceName))
            {
                using (var reader = new StreamReader(stream, _encoding))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        public IEnumerable<String> LoadLines(string resourceNameOnly)
        {
            var resourceContents = Load(resourceNameOnly);
            return resourceContents.Split(Environment.NewLine.First());
        }
    }
}
