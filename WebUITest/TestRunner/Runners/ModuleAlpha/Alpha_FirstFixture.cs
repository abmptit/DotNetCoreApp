namespace TestRunner.ModuleAlpha
{
    using NUnit.Framework;
    using Selenium;
    using System.Configuration;

    [Parallelizable(ParallelScope.All)]
    public class Alpha_FirstFixture
    {
        [Test]
        public void Alpha_FirstFixture_FirstTest()
        {
            System.Threading.Thread.Sleep(4000);
        }

        [Test]
        public void SearchWithGoogle()
        {
            string jsonFile = "TestBooks/Simple/SearchWithGoogle.json";
            string contextFolder = "";
            string scenarioFolder = "";
            string sitemapFolder = "Sitemap";
            SeleniumLauncher.ExecuteTestFromJson(jsonFile, contextFolder, scenarioFolder, sitemapFolder);
        }

        [Test]
        public void Alpha_FirstFixture_SecondTest()
        {
            System.Threading.Thread.Sleep(8000);
        }
    }
}
