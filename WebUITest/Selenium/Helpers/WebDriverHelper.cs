namespace Selenium.Helpers
{
    using Common.Helpers;
    using Common.Models;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Chrome;
    using OpenQA.Selenium.Remote;
    using System;
    using System.Drawing;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    public static class WebDriverHelper
    {
        public static int FetchRetries => ConfigHelper.GetIntValue("SeleniumClient.Fetch.MaxRetries", 1);
        public static int RetrySleepInterval => ConfigHelper.GetIntValue("SeleniumClient.Fetch.RetrySleepInterval", 500);

        public static IWebDriver CreateSession()
        {
            ChromeOptions options = new ChromeOptions();
            options.AddArgument(string.Format("--lang={0}", CultureInfo.CurrentCulture));
            DesiredCapabilities desiredCapabilities = new DesiredCapabilities("chrome", string.Empty, new OpenQA.Selenium.Platform(OpenQA.Selenium.PlatformType.Any));
            desiredCapabilities.SetCapability(ChromeOptions.Capability,
                string.Format(CultureInfo.InvariantCulture, "--lang={0}", CultureInfo.CurrentCulture));

            var projectOutputDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            //var chromeDriverPath = Environment.CurrentDirectory + SeleniumConfig.ChromeDriverLocation;
            var chromeDriverPath = SeleniumConfig.ChromeDriverLocation;

            IWebDriver webDriver = SeleniumConfig.GridEnabled ?
                new RemoteWebDriver(SeleniumConfig.SeleniumHubEndPoint, desiredCapabilities) :
                new ChromeDriver(chromeDriverPath, options, TimeSpan.FromSeconds(60));
            return webDriver;
        }

        public static void ResizeWindow(this IWebDriver webDriver, Size? windowSize)
        {
            if (windowSize != null)
            {
                webDriver.Manage().Window.Size = windowSize.Value;
            }
            else
            {
                webDriver.Manage().Window.Maximize();
            }
        }

        public static void SmartClick(this IWebDriver webDriver, Selector selector)
        {
            var element = webDriver.UniqueElement(selector);
            if (element.Enabled)
            {
                element.Click();
            }
        }

        public static void SetText(this IWebDriver webDriver, Selector selector, string value)
        {
            var element = webDriver.UniqueElement(selector);
            element.SendKeys(value);
        }

        public static void AssertTextEqual(this IWebDriver webDriver, Selector selector, string value)
        {
            var element = webDriver.UniqueElement(selector);

            if (!element.Text.Equals(value))
            {
                throw new Exception($"{selector.Name} Text equal to {element.Text} is not like excpeted {value}");
            }
        }

        public static void TakeScreenshot(this IWebDriver webDriver, string fileName)
        {
            var screenshotsLocations = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), SeleniumConfig.ScreenshotLocation);
            var sessionId = ((RemoteWebDriver)webDriver).SessionId;
            var screenshotFullPath = Path.Combine(screenshotsLocations, $"{sessionId}");
            var screenshot = ((ITakesScreenshot)webDriver).GetScreenshot();
            var filePath = Path.Combine(screenshotFullPath, fileName);
            var directoryName = Path.GetDirectoryName(filePath);

            if (!Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }

            if (File.Exists(filePath))
            {
                throw new Exception($"Save screenshot : the file {filePath}  already exist");
            }
            screenshot.SaveAsFile(filePath);
        }

        public static IWebElement UniqueElement(this IWebDriver webDriver, Selector selector)
        {
            IWebElement element = null;
            Exception exception = null;
            int retry = 0;
            while (element == null && retry < FetchRetries)
            {
                var by = ByHelper.Construct(selector);
                var elements = webDriver.FindElements(by);
                var displayedElements = elements.Where(e => e.Displayed);
                if (displayedElements.Count() > 1)
                {
                    exception = new InvalidSelectorException($"{selector.Name} return more than 1 displayed element");
                }
                element = displayedElements.FirstOrDefault();
                System.Threading.Thread.Sleep(RetrySleepInterval);
                retry++;
            }
            if (element == null)
            {
                if (exception == null)
                {
                    throw new InvalidSelectorException($"{selector.Name} no element to return with this selector");
                }
                throw exception;
            }

            return element;
        }
    }
}
