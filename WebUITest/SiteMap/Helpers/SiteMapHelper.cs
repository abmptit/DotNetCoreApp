namespace SiteMap.Helpers
{
    using SiteMap.Models;
    using System.IO;
    using System.Reflection;

    public static class SiteMapHelper
    {
        public static Page LoadPageFromJson(string jsonFile)
        {
            var projectOutputDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var jsonFileAbsolutePath = Path.Combine(
                projectOutputDirectory,
                $"sitemap\\{jsonFile}");
            Page page = null;//JsonHelper.DeserializeObject<Page>(jsonFileAbsolutePath);
            return page;
        }
    }
}
