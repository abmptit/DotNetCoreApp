namespace Selenium.Helpers
{
    using Common.Helpers;
    using Common.Models;
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    public static class ReportHelper
    {
        public static void WriteJsonReport(this Test test, string reportLocation)
        {
            var idReport = test.Steps.Select(s => s.SessionId).First();
            var assembly = Assembly.GetExecutingAssembly();
            var reportsLocation = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), SeleniumConfig.ReportLocation, idReport);
            var reportFile = Path.Combine(reportsLocation, $"report-{ test.FileName}");

            if (!Directory.Exists(reportsLocation))
            {
                Directory.CreateDirectory(reportsLocation);
            }

            if (File.Exists(reportFile))
            {
                throw new Exception($"Save json report : the file {reportFile}  already exist");
            }

            JsonHelper.SaveObjectIntoFile(test, reportFile);
        }
    }
}
