using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Xunit;

public class HomePageTests : IDisposable
{
    private readonly IWebDriver _driver;

    public HomePageTests()
    {
        var options = new ChromeOptions();
        options.AddArgument("--ignore-certificate-errors"); // ignoruj błędy certyfikatu SSL
        _driver = new ChromeDriver(options);
    }

    [Fact]
    public void HomePage_ShouldContainKeyRentInTitle()
    {
        var url = "https://localhost:7162";

        _driver.Navigate().GoToUrl(url);
        string title = _driver.Title;

        Assert.Contains("Key Rent", title);
    }

    public void Dispose()
    {
        _driver.Quit();
    }
}
