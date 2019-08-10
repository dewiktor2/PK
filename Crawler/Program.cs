using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Crawler
{
    class Program
    {

        static void Main(string[] args)
        {
            WebCrawler crawler = new WebCrawler("http://suw.biblos.pk.edu.pl/userHomepage&uId=722& rel=BPP-author", new ChromeDriver(DriverPath.path));
            crawler.NavigateToPage();
            crawler.SetAuthorPublicationNumber();
            crawler.totalPages = (crawler.publicationsNumber + crawler.pageSize - 1) / crawler.pageSize;
            crawler.GetAuthorPublications();

            foreach (var item in crawler.publications)
            {
                Console.WriteLine(item);
            }
           
        }
    }
}
