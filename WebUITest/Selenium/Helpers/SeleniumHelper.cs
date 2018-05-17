namespace Selenium.Helpers
{
    using System;
    using Common.Enums;
    using Common.Helpers;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Chrome;

    public class SeleniumHelper
    {
        private const string _driverLocation = "Selenium.webdriver.director";
        private const string _chromeVersion = "Selenium.chromedrive";

        IWebDriver webDriver;

        public void CreateSession(BrowserType browserType)
        {
            switch (browserType)
            {
                case (BrowserType.CHROME):
                    {
                        webDriver = BuildChromeDriver();
                        break;
                    }
                default:
                    throw new NotSupportedException(
                        string.Format("The browser type {0} is not supported.", browserType.ToString()));
            }

        }

        private IWebDriver BuildChromeDriver()
        {
            var location = $"{ConfigHelper.GetSizeValue(_driverLocation)}\\ChromeDriver\\{ConfigHelper.GetSizeValue(_chromeVersion)}";
            var timeout = TimeSpan.FromSeconds(60);
            ChromeOptions options = new ChromeOptions();
            return new ChromeDriver(location, options, timeout);
        }
    }
}
