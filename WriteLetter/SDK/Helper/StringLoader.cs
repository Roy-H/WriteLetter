using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AppCore.SDK.Helper
{
    public class StringLoader
    {
        private static StringLoader instance;
        private static object syncRoot = new object();
        public static StringLoader Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                        {
                            instance = new StringLoader();
                        }
                    }
                }
                return instance;

            }
        }

        public string Load(string id)
        {
            lock (syncRoot)
            {
                //var a = Windows.ApplicationModel.Resources.Core.ResourceContext.GetForCurrentView().QualifierValues;
                var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();
                var b = resourceLoader.GetString(id);
                return b;
            }
        }

        public List<ResourceItem> Parse()
        {
            var reswFileContents = File.ReadAllText("Resources.resw");
            var target = new ResourceParser(reswFileContents);
            var actual = target.Parse();
            return actual;
        }
    }

    #region Process Resw file
    public interface IResourceParser
    {
        string ReswFileContents { get; set; }
        List<ResourceItem> Parse();
    }

    public class ResourceItem
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string Comment { get; set; }
    }


    
    public class ResourceParser
    {
        public ResourceParser(string reswFileContents)
        {
            ReswFileContents = reswFileContents;
        }

        public string ReswFileContents { get; set; }

        public List<ResourceItem> Parse()
        {
            var doc = XDocument.Parse(ReswFileContents);

            var list = new List<ResourceItem>();

            foreach (var element in doc.Descendants("data"))
            {
                if (element.Attributes().All(c => c.Name != "name"))
                    continue;

                var item = new ResourceItem();

                var nameAttribute = element.Attribute("name");
                if (nameAttribute != null)
                    item.Name = nameAttribute.Value;

                if (element.Descendants().Any(c => c.Name == "value"))
                {
                    var valueElement = element.Descendants("value").FirstOrDefault();
                    if (valueElement != null)
                        item.Value = valueElement.Value;
                }

                if (element.Descendants().Any(c => c.Name == "comment"))
                {
                    var commentElement = element.Descendants("comment").FirstOrDefault();
                    if (commentElement != null)
                        item.Comment = commentElement.Value;
                }

                list.Add(item);
            }

            return list;
        }
    }
    #endregion
}
