using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Selenium_UI_Tests
{
    public class Contact_Book_UI_Tests
    {
        private WebDriver driver;
        private const string url = "https://contactbook.nakov.repl.co";

        [SetUp]
        public void OpenBrowser()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
        }

        [TearDown]
        public void CloseBrowser()
        {
            driver.Quit();
        }
        [Test]
        public void Test_First_Contact()
        {
            driver.Navigate().GoToUrl(url);
            driver.FindElement(By.CssSelector("[href='/contacts']")).Click();           
            string firstName = driver.FindElement(By.CssSelector(" [href= '/contacts/1'] .fname > td")).Text;
            string lastName = driver.FindElement(By.CssSelector("[href='/contacts/1'] .lname > td")).Text;

            Assert.AreEqual("Steve", firstName);
            Assert.AreEqual("Jobs", lastName);
        }

        [Test]
        public void Test_Find_Contact_By_KeyWord()
        {
            string keyword = "albert";
            driver.Navigate().GoToUrl(url);
            driver.FindElement(By.CssSelector("[href='/contacts/search']")).Click();
            driver.FindElement(By.Id("keyword")).SendKeys(keyword);
            driver.FindElement(By.Id("search")).Click();
            string firstName = driver.FindElement(By.CssSelector(".fname > td")).Text;
            string lastName = driver.FindElement(By.CssSelector(".lname > td")).Text;

            Assert.AreEqual("Albert", firstName);
            Assert.AreEqual("Einstein", lastName);
        }

        [Test]
        public void Test_Invalid_Keword_Search()
        {
            string keyword = "invalid2635";
            driver.Navigate().GoToUrl(url);
            driver.FindElement(By.CssSelector("[href='/contacts/search']")).Click();
            driver.FindElement(By.Id("keyword")).SendKeys(keyword);
            driver.FindElement(By.Id("search")).Click();

            string result = driver.FindElement(By.Id("searchResult")).Text;
            Assert.AreEqual("No contacts found.", result);
        }

        [Test]
        public void Test_Create_Contact_Invalid_Data()
        {
            string firstName = string.Empty;
            string lastName = "Ivanov";
            string phone = "123456";

            driver.Navigate().GoToUrl(url);
            driver.FindElement(By.XPath("//a[.='Create']")).Click();
            driver.FindElement(By.Id("firstName")).SendKeys(firstName);
            driver.FindElement(By.Id("lastName")).SendKeys(lastName);
            driver.FindElement(By.Id("phone")).SendKeys(phone);

            driver.FindElement(By.Id("create")).Click();

            string error = driver.FindElement(By.CssSelector(".err")).Text;

            Assert.AreEqual("Error: First name cannot be empty!", error);
        }

        [Test]
        public void Test_Create_Contact_Valid_Data()
        {
            string firstName = "Ivan" + DateTime.Now.Ticks;
            string lastName = "Ivanov" + DateTime.Now.Ticks;
            string email = $"{firstName}@{lastName}.com";
            string phone = $"{DateTime.Now.Ticks}";

            driver.Navigate().GoToUrl(url);
            driver.FindElement(By.XPath("//a[.='Create']")).Click();
            driver.FindElement(By.Id("firstName")).SendKeys(firstName);
            driver.FindElement(By.Id("lastName")).SendKeys(lastName);
            driver.FindElement(By.Id("email")).SendKeys(email);
            driver.FindElement(By.Id("phone")).SendKeys(phone);

            driver.FindElement(By.Id("create")).Click();

            driver.FindElement(By.XPath("//a[.='Search']")).Click();
            driver.FindElement(By.Id("keyword")).SendKeys(firstName);
            driver.FindElement(By.Id("search")).Click();
            string result = driver.FindElement(By.Id("searchResult")).Text;

            Assert.AreEqual("1 contacts found.", result);
            string firstNameResult = driver.FindElement(By.CssSelector("[href='/contacts/48'] .fname > td")).Text;
            string lastNameResult = driver.FindElement(By.CssSelector(".lname > td")).Text;

            Assert.AreEqual(firstName, firstNameResult);
            Assert.AreEqual(lastName, lastNameResult);            
        }
    }
}