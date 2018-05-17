namespace Selenium.Helpers
{
    using Common.Enums;
    using Common.Models;
    using OpenQA.Selenium;

    public static class ByHelper
    {
        public static By Construct(Selector selector)
        {
            switch (selector.SelectorType)
            {
                case (SelectorType.ID):
                    {
                        return By.Id(string.Format(selector.Text, selector.Args));
                    }
                case (SelectorType.CSS):
                    {
                        return By.CssSelector(string.Format(selector.Text, selector.Args));
                    }
                case (SelectorType.XPATH):
                    {
                        return By.XPath(string.Format(selector.Text, selector.Args));
                    }
                default:
                    throw new InvalidSelectorException($"{selector.Text} is not valid");
            }
        }
    }
}
