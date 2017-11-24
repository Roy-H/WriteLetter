using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Windows.Storage;

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
                var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();
                var result = resourceLoader.GetString(id);
                return result;
            }
        }

        public async Task<List<ResourceItem>> Parse(string language = @"/en-US/")
        {
            List<ResourceItem> actual = null;
           
            var file =  await FolderHelper.Instance.PickupFile();
            var text =await FileIO.ReadTextAsync(file);
            //var reswFileContents = File.ReadAllText(basePath + language + "Resources.resw");
            var target = new ResourceParser(text);
            actual = target.Parse();
                
            return actual;
        }

        public async void Append(List<ResourceItem> items,string language)
        {
            var fileName = basePath + language + "Resources.resw";

            var file = await FolderHelper.Instance.PickupFile();
            var text = await FileIO.ReadTextAsync(file);
            //var reswFileContents = File.ReadAllText(fileName);
            var target = new ResourceAppend(text, fileName);
            target.Append(items);
        }

        string basePath = @"D:\Code\UWP\WriteLetter\WriteLetter\Strings\";
        string LanguagePath = @"/en-US/";
        string[] SupportedLanguage = new string[] { @"/en-US/", @"/zh-CN/" };

        public async void AsyncStrings()
        {
            var items = await Parse();
            var dict = new Dictionary<string, ResourceItem>();
            foreach (var item in items)
            {
                if (!dict.ContainsKey(item.Name))
                {
                    dict.Add(item.Name,item);
                }
            }

            for (int i = 0; i < SupportedLanguage.Length; i++)
            {
                var allStrings =await Parse(language: SupportedLanguage[i]);

                var rest = dict.Where(stuff => { return !allStrings.Any(str => str.Name == stuff.Key); }).Select(stu => stu.Value).ToList();
                //var rest = allStrings.Where(stuff => { return !dict.ContainsKey(stuff.Name); }).ToList();
                if (rest.Count == 0)
                    continue;
                Append(rest, SupportedLanguage[i]);
            }            
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

    public class ResourceAppend
    {
        public ResourceAppend(string reswFileContents,string fileName)
        {
            ReswFileContents = reswFileContents;
            FileName = fileName;
        }

        public string ReswFileContents { get; set; }

        public string FileName { get; set; }

        public async void Append(IEnumerable<ResourceItem> items)
        {
            var doc = XDocument.Parse(ReswFileContents);
            
            var list = new List<ResourceItem>();

            
            foreach (var item in items)
            {
                var e = new XElement("data");
                e.SetAttributeValue("name", item.Name);

                e.Add(new XElement("value") { Value = item.Value });
                e.Add(new XElement("comment") { Value = item.Comment==null?"en-US": item.Comment });               
                doc.Root.Add(e);
            }
           
            var writter = doc.CreateWriter();

            await Task.Factory.StartNew(() => 
            {
                
                var file = File.CreateText(FileName);
                doc.Save(file);
            });
        }
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
