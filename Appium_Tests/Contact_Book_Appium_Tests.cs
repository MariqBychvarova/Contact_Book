using NUnit.Framework;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Enums;
using OpenQA.Selenium.Appium.Windows;
using System;

namespace Appium_Tests
{
    public class Contact_Book_Appium_Tests
    {
        private const string AppiumServer = "http://127.0.0.1:4723/wd/hub";
        private WindowsDriver<WindowsElement> driver;
        private AppiumOptions options;
        private const string url = "https://contactbook.nakov.repl.co/api";

        [SetUp]
        public void Setup()
        {
            options = new AppiumOptions() { PlatformName = "Windows" };
            options.AddAdditionalCapability(MobileCapabilityType.App, @"C:\Users\PC\Desktop\Exercises\QA Automation\Trial_Exam\ContactBook-DesktopClient.exe");
            driver = new WindowsDriver<WindowsElement>(new Uri(AppiumServer), options);
        }

        [TearDown]
        public void Quit()
        {
            driver.Quit();
        }

        [Test]
        public void Test1()
        {
            string keyword = "steve";

            WindowsElement textBox =driver.FindElementByAccessibilityId("textBoxApiUrl");
            textBox.Clear();
            textBox.SendKeys(url);
            driver.FindElementByAccessibilityId("buttonConnect").Click();

            string windowsName = driver.WindowHandles[0];
            driver.SwitchTo().Window(windowsName);

            WindowsElement textBoxSearch=driver.FindElementByAccessibilityId("textBoxSearch");          
            textBoxSearch.SendKeys(keyword);
            WindowsElement buttonSearch=driver.FindElementByAccessibilityId("buttonSearch");
            buttonSearch.Click();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            string resultCount = driver.FindElementByName("Contacts found: 1").Text;

            Assert.AreEqual("Contacts found: 1", resultCount);

            string firstName = driver.FindElementByName("Row 0\"]/Edit[@Name=\"FirstName Row 0, Not sorted.").Text;

            string lastName = driver.FindElementByName("Row 0\"]/Edit[@Name=\"LastName Row 0, Not sorted.").Text;

            Assert.AreEqual(keyword, firstName);
            Assert.AreEqual("Jobs", lastName);
        }
       
    }
}