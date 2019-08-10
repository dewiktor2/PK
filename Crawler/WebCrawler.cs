using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;

namespace Crawler
{
    public class WebCrawler
    {
        public string url { get; set; }
        public IWebDriver webDriver { get; set; }
        public ICollection<int> publications { get; set; }
        public int publicationsNumber { get; set; } = 0;
        public int totalPages { get; set; } = 0;
        public int currentPage { get; set; } = 0;
        public readonly int pageSize = 20;

        public WebCrawler(string Url, IWebDriver WebDriver)
        {
            this.url = Url;
            this.webDriver = WebDriver;
            publications = new List<int>();
        }

        private void ChangePage()
        {
            webDriver.FindElementExtension(By.LinkText("następne"), 3600).Click();
        }

        public IWebDriver NavigateToPage()
        {
            webDriver.Navigate().GoToUrl(url);
            return webDriver;
        }

        public void SetAuthorPublicationNumber()
        {
            string publikacjeAutora = webDriver.FindElementExtension(By.XPath("//h4[contains(.,'Publikacje autora:')]"), 3600).Text;
            var split = publikacjeAutora.Split(":");
            int number = 0;
            if (split.Count() > 0)
            {
                int.TryParse(split[1], out number);
                publicationsNumber = number;
            }
        }

        internal void GetAuthorPublications()
        {
            if (totalPages > 0)
            {
                int publicationsNumberTemp = publicationsNumber;
                for (int page = 1; page < totalPages + 1; page++)
                {
                    int recordsOnPage = GetRecordsPerPage(page, ref publicationsNumberTemp);
                    List<IWebElement> textfields = new List<IWebElement>();
                    textfields = webDriver.FindElementsExtension(By.CssSelector("a[href*='resourceDetailsBPP&rId=']"), 3600).ToList();
                    foreach (IWebElement field in textfields)
                    {
                        var link = field.GetAttribute("href");
                        AddPublication(link);
                    }
                    if (publicationsNumberTemp > 0)
                    {
                        ChangePage();
                    }
                }
            }
        }

        private void AddPublication(string link)
        {
            var splitLink = link.Split("rId=");
            int pubId = 0;
            int.TryParse(splitLink[1], out pubId);
            if (pubId > 0)
            {
                publications.Add(pubId);
            }
        }

        private int GetRecordsPerPage(int currentPage, ref int publicationsNumberTemp)
        {
            int modulo = publicationsNumberTemp % pageSize;
            if (modulo == 0)
            {
                publicationsNumberTemp = publicationsNumberTemp - 20;
                return pageSize;
            }
            else
            {
                publicationsNumberTemp = publicationsNumberTemp - publicationsNumberTemp;
                return publicationsNumberTemp;
            }
        }
    }
}
