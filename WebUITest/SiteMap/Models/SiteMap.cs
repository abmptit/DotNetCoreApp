namespace SiteMap.Models
{
    using Common.Helpers;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;

    public class SiteMap
    {
        public string SiteMapFolder { get; set; }

        private static volatile SiteMap instance;
        private static object syncRoot = new Object();
        private static object syncDic = new Object();

        private SiteMap() { }

        public static SiteMap Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new SiteMap();
                    }
                }

                return instance;
            }
        }

        public Dictionary<string, Page> Pages = new Dictionary<string, Page>();

        public Page GetPage(string pageKey)
        {
            lock (syncDic)
            {
                if (!Pages.ContainsKey(pageKey))
                {
                    Pages.Add(pageKey, LoadPageFromJson($"{pageKey}.json"));
                }
            }
            return Pages[pageKey];
        }


        private Page LoadPageFromJson(string jsonFile)
        {
            var projectOutputDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var jsonFileAbsolutePath = Path.Combine(
                projectOutputDirectory,
                $"{SiteMapFolder}\\{jsonFile}");
            Page page = JsonHelper.DeserializeObject<Page>(jsonFileAbsolutePath);
            return page;
        }

    }
}
