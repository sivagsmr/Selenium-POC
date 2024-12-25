using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Safari;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Edge;

namespace Selenium_POC.Helpers
{
    public class CrossBrowserHelper
    {
        public static IEnumerable<Dictionary<string, string>> BrowserCombinations()
        {
            yield return new Dictionary<string, string>
        {
            {"browserName", "Chrome"},
            {"browser_version", "latest"},
            {"os", "Windows"},
            {"os_version", "10"},
            {"name", "Chrome on Windows"}
        };
            yield return new Dictionary<string, string>
        {
            {"browserName", "Firefox"},
            {"browser_version", "latest"},
            {"os", "Windows"},
            {"os_version", "10"},
            {"name", "Firefox on Windows"}
        };
            yield return new Dictionary<string, string>
        {
            {"browserName", "iPhone"},
            {"device", "iPhone 15"},
            {"realMobile", "true"},
            {"os_version", "15"},
            {"name", "iPhone Chrome"}
        };
            yield return new Dictionary<string, string>
        {
            {"browserName", "Android"},
            {"device", "Samsung Galaxy S22"},
            {"realMobile", "true"},
            {"os_version", "12"},
            {"name", "Samsung Chrome"}
        };
            yield return new Dictionary<string, string>
        {
            {"browserName", "Edge"},
            {"browser_version", "latest"},
            {"os", "Windows"},
            {"os_version", "10"},
            {"name", "Edge on Windows"}
        };
        }

        public static IWebDriver CreateDriver(string browserName, Dictionary<string, string> capabilities)
        {
            DriverOptions options;


            switch (browserName.ToLower())
            {
                case "chrome":
                    var chromeOptions = new ChromeOptions();

                    options = chromeOptions;
                    break;

                case "firefox":
                    var firefoxOptions = new FirefoxOptions();

                    options = firefoxOptions;
                    break;

                case "iphone":
                    var iphoneOptions = new ChromeOptions();

                    options = iphoneOptions;
                    break;

                case "edge":
                    var edgeOptions = new EdgeOptions();
                    options = edgeOptions;
                    break;

                case "android":
                    var androidOptions = new ChromeOptions();
                    options = androidOptions;
                    break;

                default:
                    throw new ArgumentException($"Unsupported browser: {browserName}");
            }


            string username = "sivamohan_YRHrOA";
            string accessKey = "DRNu4SHpH2NDazzY8xRJ";
            string remoteUrl = $"https://{username}:{accessKey}@hub-cloud.browserstack.com/wd/hub";

            return new RemoteWebDriver(new Uri(remoteUrl), options);
        }

    }
}
