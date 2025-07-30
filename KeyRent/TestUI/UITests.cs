using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Xunit;
using System;
using System.IO;

namespace KeyRent.UiTests
{
    public class AdminPanelTests : IDisposable
    {
        private readonly IWebDriver driver;
        private readonly WebDriverWait wait;
        private readonly string baseUrl = "https://localhost:7162"; 

        public AdminPanelTests()
        {
            var options = new ChromeOptions();
            options.AddArgument("--headless");  
            options.AddArgument("--no-sandbox");
            options.AddArgument("--disable-dev-shm-usage");

            string driverPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Drivers");
            driver = new ChromeDriver(driverPath, options);

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
        }

        public void Dispose()
        {
            driver.Quit();
            driver.Dispose();
        }

        private IWebElement WaitForElement(By by)
        {
            return wait.Until(driver =>
            {
                try
                {
                    var element = driver.FindElement(by);
                    return element.Displayed ? element : null;
                }
                catch (NoSuchElementException)
                {
                    return null;
                }
            });
        }

        [Fact]
        public void Strona_Glowna_Laduje_Sie_Poprawnie()
        {
            driver.Navigate().GoToUrl(baseUrl + "/");

      
            var header = WaitForElement(By.TagName("h1"));
            Assert.NotNull(header);
            Assert.True(header.Displayed);

            Assert.Contains("Key Rent", driver.Title);
        }

        [Fact]
        public void Rejestracja_Z_Blednym_Haslem()
        {
            driver.Navigate().GoToUrl(baseUrl + "/Identity/Account/Register");

            var emailInput = WaitForElement(By.Id("Input_Email"));
            emailInput.SendKeys("niepoprawny@user.pl");

            var passwordInput = WaitForElement(By.Id("Input_Password"));
            passwordInput.SendKeys("123");

            var confirmPasswordInput = WaitForElement(By.Id("Input_ConfirmPassword"));
            confirmPasswordInput.SendKeys("123");

            var registerButton = WaitForElement(By.Id("registerSubmit"));
            registerButton.Click();

            wait.Until(d => d.PageSource.Contains("The Password must be at least 6 and at max 100 characters long."));

            var pageSource = driver.PageSource;
            Assert.Contains("The Password must be at least 6 and at max 100 characters long.", pageSource);
        }

        [Fact]
        public void Rejestracja_Z_Poprawnymi_Danymi()
        {
            var uniqueEmail = $"test{Guid.NewGuid().ToString().Substring(0, 6)}@mail.com";

            driver.Navigate().GoToUrl(baseUrl + "/Identity/Account/Register");

            var emailInput = WaitForElement(By.Id("Input_Email"));
            emailInput.SendKeys(uniqueEmail);

            var passwordInput = WaitForElement(By.Id("Input_Password"));
            passwordInput.SendKeys("Test123!");

            var confirmPasswordInput = WaitForElement(By.Id("Input_ConfirmPassword"));
            confirmPasswordInput.SendKeys("Test123!");

            var registerButton = WaitForElement(By.Id("registerSubmit"));
            registerButton.Click();

            wait.Until(d => !d.PageSource.Contains("registerSubmit"));

            var pageSource = driver.PageSource;
            Assert.DoesNotContain("The Password must be at least", pageSource);
            Assert.DoesNotContain("registerSubmit", pageSource);
        }

        [Fact]
        public void Logowanie_()
        {
            Logowanie("test@o2.pl", "Test1!");

            var logoutLink = WaitForElement(By.LinkText("Logout"));
            Assert.NotNull(logoutLink);
            Assert.Contains("Logout", driver.PageSource);
        }

        private void Logowanie(string email, string password)
        {
            driver.Navigate().GoToUrl(baseUrl + "/Identity/Account/Login");

            var emailInput = WaitForElement(By.Id("Input_Email"));
            emailInput.SendKeys(email);

            var passwordInput = WaitForElement(By.Id("Input_Password"));
            passwordInput.SendKeys(password);

            var loginButton = WaitForElement(By.Id("login-submit"));
            loginButton.Click();

            WaitForElement(By.LinkText("Logout"));
        }
        [Fact]
        public void Dostep_Do_Listy_Uzytkownikow_W_Panelu_Admina()
        {
           
            Logowanie("test@o2.pl", "Test1!");

          
            driver.Navigate().GoToUrl(baseUrl + "/Admin");

         
            var tabela = WaitForElement(By.CssSelector("table.table.table-bordered.table-hover"));

            Assert.NotNull(tabela);
            Assert.Contains("Imiê", tabela.Text);
            Assert.Contains("Nazwisko", tabela.Text);
            Assert.Contains("Email", tabela.Text);
        }


    }
}
