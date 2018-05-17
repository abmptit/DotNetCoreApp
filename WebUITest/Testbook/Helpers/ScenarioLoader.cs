namespace Testbook.Helpers
{
    using Common.Helpers;
    using Common.Models;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;

    public class ScenarioLoader
    {
        public string ScenarioFolder { get; set; }

        private static volatile ScenarioLoader instance;
        private static object syncRoot = new Object();
        private static object syncDic = new Object();

        private ScenarioLoader() { }

        public static ScenarioLoader Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new ScenarioLoader();
                    }
                }

                return instance;
            }
        }

        public Dictionary<string, Scenario> DicoScenario = new Dictionary<string, Scenario>();

        public Scenario GetScenario(string scenarioKey)
        {
            lock (syncDic)
            {
                if (!DicoScenario.ContainsKey(scenarioKey))
                {
                    DicoScenario.Add(scenarioKey, this.LoadScenarioFromJson($"{scenarioKey}.json"));
                }
            }
            return DicoScenario[scenarioKey];
        }

        private Scenario LoadScenarioFromJson(string jsonFile)
        {
            if (string.IsNullOrEmpty(ScenarioFolder))
            {
                throw new Exception("No folder defined for the context files.");
            }

            var projectOutputDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var jsonFileAbsolutePath = Path.Combine(
                projectOutputDirectory,
                $"{ScenarioFolder}\\{jsonFile}");
            Scenario page = JsonHelper.DeserializeObject<Scenario>(jsonFileAbsolutePath);
            return page;
        }
    }
}
