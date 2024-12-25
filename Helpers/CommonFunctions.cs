using OpenQA.Selenium;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Selenium_POC.Helpers
{
    public class CommonFunctions
    {
        private IWebDriver driver;

        public CommonFunctions(IWebDriver driver)
        {
            this.driver = driver;
        }

        public static void PrintRepeatedWordsWithCount(List<string> strings)
        {
            Dictionary<string, int> wordCount = new Dictionary<string, int>();
            
            //identifying any words that are repeated more than twice
            foreach (string str in strings)
            {
                string[] words = str.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                foreach (string word in words)
                {
                    string lowercaseWord = word.ToLower(); 
                    if (wordCount.ContainsKey(lowercaseWord))
                    {
                        wordCount[lowercaseWord]++;
                    }
                    else
                    {
                        wordCount[lowercaseWord] = 1;
                    }
                }
            }

            //Printing each repeated word along with the count of its occurrences.
            Console.WriteLine("Repeated Words with Counts:");

            foreach (var kvp in wordCount.Where(kvp => kvp.Value > 1))
            {
                Console.WriteLine($"{kvp.Key}: {kvp.Value}");
            }
        }


       
        public static List<Article> ScrapeArticles(IWebDriver driver)
        {
            var articles = new List<Article>();

            var articleLinks = new List<string>();

            var articleElements = driver.FindElements(By.XPath("//article"));          

            for (int i = 0; i < 5; i++)
            {
                var title = articleElements[i].FindElement(By.CssSelector("h2")).Text;

                var content = driver.FindElement(By.CssSelector("p")).Text;

                //Printing the title and content of each article in Spanish.
                Console.WriteLine($"Title : {title}");

                Console.WriteLine($"Content : {content}");

                var articleLink = articleElements[i].FindElement(By.TagName("a")).GetAttribute("href");

                articles.Add(new Article { Title = title, Content = content });

                articleLinks.Add(articleLink);

            }

            foreach (var link in articleLinks)
            {

                try
                {
                    driver.Navigate().GoToUrl(link);

                    string imagePath = null;

                    foreach (var article in articles)
                    {
                        try
                        {
                            var imgUrl = driver.FindElement(By.XPath("//article[header//*[text()='" + article.Title + "']]//img")).GetAttribute("src");

                            //If available, download and saveing the cover image of each article to your local machine.
                            imagePath = SaveImage(imgUrl, article.Title);
                        }
                        catch
                        {
                            continue;
                        }
                    }


                    articles.Add(new Article { ImagePath = imagePath });
                    driver.Navigate().Back();
                }
                catch { continue; }
            }


            return articles;
        }

        private static string SaveImage(string url, string title)
        {
            var fileName = Regex.Replace(title, "[^a-zA-Z0-9]", "_") + ".jpg";

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), fileName);

            using var client = new System.Net.WebClient();

            client.DownloadFile(url, filePath);

            return filePath;
        }

        public static List<string> TranslateHeaders(List<string> article)
        {
            var translatedHeaders = new List<string>();

            foreach (var item in article)
            {
                if (item != null)
                {
                    //Translation API 
                    var client = new RestClient("https://rapid-translate-multi-traduction.p.rapidapi.com");

                    var request = new RestRequest("/t", Method.Post);

                    request.AddHeader("x-rapidapi-key", "8a441acac8msh79b81c52b70cecep15f452jsn0e8a8378fc5b");

                    request.AddHeader("x-rapidapi-host", "rapid-translate-multi-traduction.p.rapidapi.com");

                    request.AddHeader("Content-Type", "application/json");

                    //Translate the title of each article to English.
                    var requestBody = new
                    {
                        from = "es",
                        to = "en",
                        q = item
                    };

                    request.AddJsonBody(requestBody);

                    var response = client.Execute(request);

                    translatedHeaders.Add(response.Content);

                    //Printing the translated headers.
                    Console.WriteLine(response.Content);
                }
            }
            return translatedHeaders;
        }
    }
}
