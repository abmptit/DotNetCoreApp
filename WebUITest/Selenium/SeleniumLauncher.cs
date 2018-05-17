namespace Selenium
{
    using System;
    using Common.Enums;
    using Common.Helpers;
    using Common.Models;
    using OpenQA.Selenium;
    using Selenium.Helpers;
    using Testbook.Helpers;

    public static class SeleniumLauncher
    {
        public static string CreateJsonSampleTest()
        {
            Test newTest = new Test() { Name = "My First Test", Description = "This Test ..." };
            Step createSession = new Step() { Name = "CREATE_SESSION", Description = "This step open the browser" };
            newTest.Steps.Add(createSession);
            string jsonTest = JsonHelper.SerializeObject(newTest);
            return jsonTest;
        }

        public static void Execute(this Test test)
        {
            test.Measure.StartDate = DateTime.Now;
            string sessionId = string.Empty;
            IWebDriver testWebDriver = null;
            try
            {
                foreach (Step step in test.Steps)
                {
                    if (!string.IsNullOrEmpty(sessionId))
                    {
                        step.SessionId = sessionId;
                    }
                    step.Execute(ref testWebDriver);
                    if (sessionId != step.SessionId)
                    {
                        sessionId = step.SessionId;
                    }
                }
            }
            catch (Exception ex)
            {
                test.Failed = true;
                test.StackTrace = ex.Message;
                throw;
            }
            finally
            {
                test.Measure.EndDate = DateTime.Now;
                test.WriteJsonReport(SeleniumConfig.ReportLocation);
                testWebDriver.Close();
            }
        }

        public static void Execute(this Step step, ref IWebDriver webDriver)
        {
            try
            {
                step.Measure.StartDate = DateTime.Now;
                switch (step.Type)
                {
                    case (StepType.CREATE_SESSION):
                        webDriver = WebDriverHelper.CreateSession();
                        var sessionId = ((OpenQA.Selenium.Remote.RemoteWebDriver)webDriver).SessionId;
                        webDriver.ResizeWindow(SeleniumConfig.BrowserSize);
                        step.SessionId = sessionId.ToString();
                        break;
                    case (StepType.NAVIGATE_URL):
                        //webDriver.Url = step.Param;
                        webDriver.Navigate().GoToUrl(step.Param);
                        break;
                    case (StepType.CLICK_BUTTON):
                        webDriver.SmartClick(step.Selector);
                        break;
                    case (StepType.SET_TEXT):
                        webDriver.SetText(step.Selector, step.Value);
                        break;
                    case (StepType.ASSERT_TEXT):
                        webDriver.AssertTextEqual(step.Selector, step.Value);
                        break;
                    case (StepType.TAKE_SCREENSHOT):
                        webDriver.TakeScreenshot(step.Value);
                        break;
                    case (StepType.RESIZE_WINDOW):
                        webDriver.ResizeWindow(SeleniumConfig.BrowserSize);
                        break;
                    case StepType.EXECUTE_SCENARIO:
                        throw new InvalidOperationException("Selenium launcher execute only elementary step");
                    default:
                        throw new NotImplementedException();

                }
                step.Measure.EndDate = DateTime.Now;
            }
            catch (Exception ex)
            {
                step.Failed = true;
                step.StackTrace = ex.Message;
                throw ex;
            }
        }

        public static ITest ExecuteTestFromJson(string jsonFile)
        {
            var test = TestBookHelper.ReadTestFromJson(jsonFile);
            test.ConvertScenarioToElementarySteps(ContextLoader.Instance, ScenarioLoader.Instance);
            test.InsertScreenshotSteps();
            test.ConvertFromPageObject(SiteMap.Models.SiteMap.Instance);
         
            TestBookHelper.SaveTestToJson(test, $"{test.FilePath.Replace(".json", "-conv.json")}");
            test.Execute();
            TestBookHelper.SaveTestToJson(test, $"{test.FilePath.Replace(".json", "-result.json")}");
            return (ITest)test;
        }

        public static void InitializeEnvironment(string contextFolder, string scenarioFolder, string sitemapFolder)
        {
            ContextLoader.Instance.ContextFolder = contextFolder;
            ScenarioLoader.Instance.ScenarioFolder = scenarioFolder;
            SiteMap.Models.SiteMap.Instance.SiteMapFolder = sitemapFolder;
        }
    }
}
