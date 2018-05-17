namespace Testbook.Helpers
{
    using Common.Helpers;
    using Common.Models;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;

    public class ContextLoader
    {
        public string ContextFolder { get; set; }

        private static volatile ContextLoader instance;
        private static object syncRoot = new Object();
        private static object syncDic = new Object();

        private ContextLoader() { }

        public static ContextLoader Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new ContextLoader();
                    }
                }

                return instance;
            }
        }

        public Dictionary<string, Context> DicoContext = new Dictionary<string, Context>();

        public Context GetContext(string contextKey)
        {
            lock (syncDic)
            {
                if (!DicoContext.ContainsKey(contextKey))
                {
                    DicoContext.Add(contextKey, this.LoadContextFromJson($"{contextKey}.json"));
                }
            }
            return DicoContext[contextKey];
        }

        private Context LoadContextFromJson(string jsonFile)
        {
            if (string.IsNullOrEmpty(ContextFolder))
            {
                throw new Exception("No folder defined for the context files.");
            }

            var projectOutputDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var jsonFileAbsolutePath = Path.Combine(
                projectOutputDirectory,
                $"{ContextFolder}\\{jsonFile}");
            Context context = JsonHelper.DeserializeObject<Context>(jsonFileAbsolutePath);
            return context;
        }

    }
}
