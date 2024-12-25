using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Safari;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections;
using NUnit.Framework.Interfaces;
using Selenium_POC.Helpers;
using OpenQA.Selenium.Support.UI;

namespace Selenium_POC.Tests {

    [TestFixture]
    [Parallelizable(ParallelScope.Children)]
    public class BrowserStackTest
    {
        private IWebDriver driver;


        [TestCaseSource(typeof(CrossBrowserHelper), nameof(CrossBrowserHelper.BrowserCombinations))]
        public void WebScraping(Dictionary<string, string> capabilities)
        {
            string browserName = capabilities["browserName"]; ;

            Console.WriteLine(browserName);
            IWebDriver driver=null;
            try
            {
                driver = CrossBrowserHelper.CreateDriver(browserName, capabilities);

                driver.Manage().Window.Maximize();

                driver.Navigate().GoToUrl("https://elpais.com/");

                string htmlSource = driver.PageSource;

                string expectedLang = "es-ES";

                Assert.IsTrue(htmlSource.Contains($"<html lang=\"{expectedLang}\""), "website's text is displayed in Spanish.");

                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

                IWebElement htmlElement = wait.Until(d => d.FindElement(By.Id("didomi-notice-agree-button")));

                driver.FindElement(By.Id("didomi-notice-agree-button")).Click();

                driver.FindElement(By.XPath("//a[@href='https://elpais.com/opinion/' and @cmp-ltrk-idx='1' ]")).Click();

                var articles = CommonFunctions.ScrapeArticles(driver);

                var trnsHeaders = new List<string>();


                foreach (var article in articles)
                {
                    trnsHeaders.Add(article.Title);

                }
                var translatedTittle = CommonFunctions.TranslateHeaders(trnsHeaders);

                CommonFunctions.PrintRepeatedWordsWithCount(translatedTittle);
            }
            finally
            {
                driver.Quit();
                Console.WriteLine("Completed");
            }
        }


    }

    
}
