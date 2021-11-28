using NUnit.Framework;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System.Text;
using System;
using System.Threading;

namespace npo5
{
    public class Tests
    {
        IWebDriver driver;

        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
        }

        [TearDown]
        public void TearDown()
        {
            driver.Quit();
        }

        [Test]
        public void Test1()
        {
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl("http://ushachi.edu.by/ru/main.aspx?guid=63191");
            Thread.Sleep(1000);
            Assert.AreEqual("Гостевая книга © Отдел по образованию Ушачского райисполкома", driver.Title);
            var userNameField = driver.FindElement(By.Id("field_name"));
           
            var responseField = driver.FindElement(By.Id("field_text"));

            var btnLeaveComment = driver.FindElement(By.Id("__tab_Tabs_13011_ctl01"));

            btnLeaveComment.Click();

            userNameField.SendKeys(RandomString(10, true));
            responseField.SendKeys(RandomString(10, true));
            
            var captchaImage = driver.FindElement(By.XPath("//*[@id=\"pmgimg\"]/img")).GetAttribute("src");

            var captchaField = driver.FindElement(By.Id("pmgtext"));

            var captchaString = captchaImage.Substring(captchaImage.IndexOf("guid=")+5,5);

            captchaField.SendKeys(captchaString);

            var sendButton = driver.FindElement(By.ClassName("link-button"));
           
            sendButton.Click();
            Thread.Sleep(1000);
            var successMessage = driver.FindElement(By.XPath("//*[@id=\"AjaxLoad\"]")).Text;
            Assert.AreEqual(successMessage, "Ваше сообщение отправлено. Оно появится в гостевой после проверки администратором.");
        }

        private readonly Random random = new Random();

        public string RandomString(int size, bool lowerCase = false)
        {
            var builder = new StringBuilder(size);
            char offset = lowerCase ? 'a' : 'A';
            const int lettersOffset = 26;
            for (var i = 0; i < size; i++)
            {
                var @char = (char)random.Next(offset, offset + lettersOffset);
                builder.Append(@char);
            }
            return lowerCase ? builder.ToString().ToLower() : builder.ToString();
        }
    }
}