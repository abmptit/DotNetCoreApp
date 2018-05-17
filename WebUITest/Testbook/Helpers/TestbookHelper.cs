namespace Testbook.Helpers
{
    using System.Reflection;
    using System.IO;
    using System.Linq;
    using System.Collections.Generic;
    using Common.Helpers;
    using Common.Models;
    using Common.Enums;

    public static class TestBookHelper
    {
        public static Test ReadTestFromJson(string jsonFile)
        {
            var projectOutputDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var jsonFileAbsolutePath = Path.Combine(
                projectOutputDirectory,
                jsonFile);

            Test test = JsonHelper.DeserializeObject<Test>(jsonFileAbsolutePath);
            test.FilePath = jsonFileAbsolutePath;
            test.FileName = Path.GetFileName(jsonFileAbsolutePath);

            return test;
        }

        public static Test ConvertFromPageObject(this Test test, SiteMap.Models.SiteMap sitemap)
        {
            foreach (var step in test.Steps)
            {
                step.ConvertFromPageObject(sitemap);
            }
            return test;
        }

        public static Step ConvertFromPageObject(this Step step, SiteMap.Models.SiteMap sitemap)
        {
            foreach (var property in step.GetType().GetProperties())
            {
                if (property.PropertyType == typeof(string))
                {
                    string value = property.GetValue(step, null)?.ToString();

                    if (value != null && value.StartsWith("#"))
                    {
                        string[] pathElements = value.Substring(1).Split('.');
                        var pageKey = pathElements[0];
                        string propertyPath = value.Substring(1 + 1 + pageKey.Length);
                        var page = sitemap.GetPage(pageKey);
                        var pageProperties = page.GetType().GetProperties();
                        var stepProperties = step.GetType().GetProperties();
                        var currentProperty = stepProperties.FirstOrDefault(p => p.Name == propertyPath);
                        var selectorValue = ExpandoHelper.GetDynamicMember(page, propertyPath);
                        if (selectorValue is string)
                        {
                            step.Param = (string)selectorValue;
                        }
                        if (selectorValue is Selector)
                        {
                            step.Selector = (Selector)selectorValue;
                        }
                    }
                }
            }
            return step;
        }

        public static void SaveTestToJson(Test test, string filePath = null)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                filePath = test.FilePath;
            }
            var directoryName = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(directoryName) && !Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }

            JsonHelper.SaveObjectIntoFile(test, filePath);
        }

        public static Test ConvertScenarioToElementarySteps(this Test test, ContextLoader contextLoader, ScenarioLoader scenarioLoader)
        {
            //foreach (var step in test.Steps)
            for (int index = 0; index < test.Steps.Count; index++)
            {
                if (test.Steps[index].Type == StepType.EXECUTE_SCENARIO)
                {
                    var elementarySteps = test.Steps[index].ConvertScenarioToElementarySteps(scenarioLoader);
                    var contextToApply = contextLoader.GetContext(test.Steps[index].Value);
                    elementarySteps.ForEach(s => s.ApplyScenarioContext(contextToApply));
                    test.Steps.Remove(test.Steps[index]);
                    test.Steps.InsertRange(index, elementarySteps);
                }
            }
            return test;
        }

        public static List<Step> ConvertScenarioToElementarySteps(this Step step, ScenarioLoader scenarioLoader)
        {
            var scenario = scenarioLoader.GetScenario(step.Param);
            //foreach (var scenarioStep in scenario.Steps)
            for (int index = 0; index < scenario.Steps.Count; index++)
            {
                if (scenario.Steps[index].Type == StepType.EXECUTE_SCENARIO)
                {
                    var elementarySteps = scenario.Steps[index].ConvertScenarioToElementarySteps(scenarioLoader);
                    var contextToApply = ContextLoader.Instance.GetContext(scenario.Steps[index].Value);
                    elementarySteps.ForEach(s => s.ApplyScenarioContext(contextToApply));
                    scenario.Steps.Remove(scenario.Steps[index]);
                    scenario.Steps.InsertRange(index, elementarySteps);
                }
            }
            return scenario.Steps;
        }

        public static void ApplyScenarioContext(this Step step, Context context)
        {
            var value = step.Value;
            if (value != null && value.StartsWith("$"))
            {
                string propertyPath = value.Substring(1);
                var contextValue = (string)ExpandoHelper.GetDynamicMember(context, propertyPath);
                step.Value = contextValue;
            }
        }

        public static Test InsertScreenshotSteps(this Test test)
        {
            //foreach (var step in test.Steps)
            var steps = new List<Step>(test.Steps);
            int insertionIndex = 0;
            for (int index = 0; index < steps.Count; index++)
            {
                var currentStep = test.Steps[index];
                if (currentStep.TakeScreenshotBefore && currentStep.Type != StepType.TAKE_SCREENSHOT && currentStep.Type != StepType.CREATE_SESSION)
                {
                    test.Steps.Insert(insertionIndex, new Step()
                    {
                        Name = "TAKE_SCREENSHOT",
                        TakeScreenshotBefore = false,
                        TakeScreenshotAfter = false,
                        Description = $"Screenshot before action {currentStep.Name} {currentStep.Param} {currentStep.Value}",
                        Value = $"{test.Name}\\{test.Name}_{index}_0before.png"
                    });
                    insertionIndex++;
                }
                if (currentStep.TakeScreenshotAfter && currentStep.Type != StepType.TAKE_SCREENSHOT)
                {
                    test.Steps.Insert(insertionIndex + 1, new Step()
                    {
                        Name = "TAKE_SCREENSHOT",
                        TakeScreenshotBefore = false,
                        TakeScreenshotAfter = false,
                        Description = $"Screenshot after action {currentStep.Name} {currentStep.Param}",
                        Value = $"{test.Name}\\{test.Name}_{index}_1after.png"
                    });
                    insertionIndex++;
                }
                insertionIndex++;
            }
            return test;
        }
    }
}
