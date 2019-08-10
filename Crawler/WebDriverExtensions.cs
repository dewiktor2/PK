using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Crawler
{
    public static class WebDriverExtensions
    {
        public static IWebElement FindElementExtension(this IWebDriver driver, By by, int timeoutInSeconds)
        {
            if (timeoutInSeconds > 0)
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
                return wait.Until(driv => driv.FindElement(by));
            }
            return driver.FindElement(by);
        }

        public static bool ElementExist(this IWebDriver driver, By by)
        {
            try
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(0.1));
                var myElement = wait.Until(x => x.FindElement(by));
                return myElement.Displayed;
            }
            catch
            {
                return false;
            }
        }

        public static ICollection<IWebElement> FindElementsExtension(this IWebDriver driver, By by, int timeoutInSeconds)
        {
            if (timeoutInSeconds > 0)
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
                return wait.Until(driv => (driv.FindElements(by).Any()) ? driv.FindElements(by) : null);
            }
            return driver.FindElements(by);
        }
    }
}
