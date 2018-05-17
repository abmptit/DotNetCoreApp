namespace TestRunner.Runners
{
    using NUnit.Framework;
    using Selenium;

    [SetUpFixture]
    public class SetUpClassModuleAlpha
    {
        [OneTimeSetUp]
        public void Initialize()
        {
            string contextFolder = "Inputs/Contexts";
            string scenarioFolder = "Inputs/Scenarios";
            string sitemapFolder = "Inputs/Sitemap";

            SeleniumLauncher.InitializeEnvironment(contextFolder, scenarioFolder, sitemapFolder);
        }

        [OneTimeTearDown]
        public void TearDown()
        {

        }
    }
}
