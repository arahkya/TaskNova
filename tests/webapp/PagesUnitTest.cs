using Microsoft.AspNetCore.Mvc.Testing;


namespace Arahk.TaskNova.WebApp.Test;

public class PagesUnitTest(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory = factory;

    [Theory]
    [InlineData("/")]
    [InlineData("/create-task")]
    public async Task Get_EndpointsReturnSuccessAndCorrectContentType(string url)
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync(url);

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal("text/html; charset=utf-8", response.Content.Headers.ContentType!.ToString());
    }
}
