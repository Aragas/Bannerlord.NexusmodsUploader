using Bannerlord.NexusmodsUploader.Options;

using CommandLine;

using Newtonsoft.Json;

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;

using SeleniumExtras.WaitHelpers;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Bannerlord.NexusmodsUploader
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            CodePagesEncodingProvider.Instance.GetEncoding(437);
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            Parser.Default.ParseArguments<UploadOptions>(args)
                .WithParsed(Upload);
        }

        private static void Upload(UploadOptions options)
        {
            if (!File.Exists(options.FilePath))
            {
                throw new FileNotFoundException("File does not exist!", options.FilePath);
            }

            Directory.CreateDirectory("Screenshots");
            RemoteWebDriver driver;
            if (isLocal)
            {
                driver = new ChromeDriver(new ChromeOptions {BrowserVersion = "83.0.4103.3900"});
            }
            else
            {
                var chromeOptions = new ChromeOptions();
                chromeOptions.AddArguments("start-maximized");
                driver = new RemoteWebDriver(new Uri("http://localhost:4444/wd/hub"), chromeOptions) { FileDetector = new LocalFileDetector() };
            }

            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));

            driver.Navigate().GoToUrl("https://nexusmods.com");

            driver.Manage().Cookies.DeleteAllCookies();
            var cookies = Environment.GetEnvironmentVariable("NEXUSMODS_COOKIES_JSON");
            Console.WriteLine($"Length of env cookies {cookies?.Length ?? 0}");
            foreach (var cookieEntry in JsonConvert.DeserializeObject<List<CookieEntry>>(cookies ?? "[]"))
                driver.Manage().Cookies.AddCookie(new Cookie(cookieEntry.Id, cookieEntry.Value, cookieEntry.Domain, cookieEntry.Path, cookieEntry.Date.LocalDateTime));

            driver.Navigate().GoToUrl($"https://www.nexusmods.com/{options.GameId}/mods/edit/?id={options.ModId}&step=files");

            Console.WriteLine("Checking file_name...");
            var file_name = driver.FindElement(By.XPath("//*[@id=\"add-files\"]/div[1]/div[1]/div/input"));
            file_name.SendKeys(options.Name);

            Console.WriteLine("Checking file_version...");
            var file_version = driver.FindElement(By.XPath("//*[@id=\"add-files\"]/div[1]/div[2]/div/input"));
            file_version.SendKeys(options.Version);

            if (options.IsLatest)
            {
                Console.WriteLine("Checking update_version...");
                var update_version = driver.FindElement(By.Id("update-version"));
                update_version.Click();
            }

            if (options.IsNewOfExisting)
            {
                try
                {
                    Console.WriteLine("Checking new_existing_version...");
                    var new_existing_version = driver.FindElement(By.Id("new-existing-version"));
                    new_existing_version.Click();
                }
                catch (NoSuchElementException) { }
            }

            Console.WriteLine("Checking file_description...");
            var file_description = driver.FindElement(By.Id("file-description"));
            file_description.SendKeys(options.Description);

            driver.GetScreenshot().SaveAsFile("Screenshots/d_upload.png");
            Console.WriteLine("Checking input_file...");
            var input_file = driver.FindElement(By.Id("add_file_browse")).FindElements(By.XPath(".//*")).First();
            input_file.SendKeys(options.FilePath);
            driver.GetScreenshot().SaveAsFile("Screenshots/i_upload.png");
            Console.WriteLine("Waiting for upload_success...");
            wait.Until(ExpectedConditions.ElementExists(By.Id("upload_success")));
            driver.GetScreenshot().SaveAsFile("Screenshots/s_upload.png");

            Console.WriteLine("Checking js_save_file...");
            var js_save_file = driver.FindElement(By.Id("js-save-file"));
            js_save_file.Submit();

            //driver.GetScreenshot().SaveAsFile("Screenshots/save.png");
            //Console.WriteLine("Checking save button...");
            //var save_button = driver.FindElement(By.XPath("//*[@id=\"top-save-button-container\"]/li[1]/a"));
            //save_button.Click();

            Console.WriteLine("Waiting 10000ms...");
            var now = DateTime.Now;
            wait.Until(wd => (DateTime.Now - now) - TimeSpan.FromMilliseconds(10000) > TimeSpan.Zero);

            driver.GetScreenshot().SaveAsFile("Screenshots/done.png");
            Console.WriteLine("Done!");
            driver.Dispose();
        }
    }
}