namespace TestRunner.Runners
{
    using NUnit.Framework;
    using Selenium;
    using System.Configuration;

    [Parallelizable(ParallelScope.All)]
    public class Alpha_FirstFixture
    {
        [Test]
        public void HelloTalentConnectAndSearch()
        {
            string jsonFile = "Inputs/TestBooks/WithScenario/HelloTalentConnectAndSearch.json";
            SeleniumLauncher.ExecuteTestFromJson(jsonFile);
        }

        [Test]
        public void SearchWithGoogle()
        {
            string jsonFile = "Inputs/TestBooks/Simple/SearchWithGoogle.json";
            SeleniumLauncher.ExecuteTestFromJson(jsonFile);
        }
    }
}
