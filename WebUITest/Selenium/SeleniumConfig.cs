namespace Selenium
{
    using Common.Helpers;
    using System;
    using System.Drawing;

    //public static class SeleniumConfig
    //{
    //    public static string SeleniumHubAddress => ConfigHelper.GetStringValue("Selenium.hub.Address");
    //    public static string SeleniumHubPort => ConfigHelper.GetStringValue("Selenium.hub.Port");

    //    public static Uri SeleniumHubEndPoint => new Uri(string.Format("http://{0}:{1}/wd/hub", SeleniumConfig.SeleniumHubAddress, SeleniumConfig.SeleniumHubPort), UriKind.Absolute);

    //    public static bool GridEnabled => ConfigHelper.GetBoolValue("Selenium.Grid.Enabled");

    //    public static string ChromeDriverLocation => ConfigHelper.GetStringValue("Selenium.ChromeDriver.Location", "C:\\Talentsoft.SeleniumNode\\wrapper\\lib");

    //    public static string ScreenshotLocation => ConfigHelper.GetStringValue("Selenium.Screenshot.Location");

    //    public static string ReportLocation => ConfigHelper.GetStringValue("Selenium.Report.Location");

    //    public static Size? BrowserSize => ConfigHelper.GetSizeValue("Selenium.Browser.Size");
    //}


    public static class SeleniumConfig
    {
        public static string SeleniumHubAddress => "localhost";
        public static string SeleniumHubPort => "4444";

        public static Uri SeleniumHubEndPoint => new Uri(string.Format("http://{0}:{1}/wd/hub", SeleniumConfig.SeleniumHubAddress, SeleniumConfig.SeleniumHubPort), UriKind.Absolute);

        public static bool GridEnabled => false;

        public static string ChromeDriverLocation => "C:\\Talentsoft.SeleniumNode\\wrapper\\lib";

        public static string ScreenshotLocation => "Screenshots";

        public static string ReportLocation => "Reports";

        public static Size? BrowserSize => new Size(1024, 788);
    }
}
