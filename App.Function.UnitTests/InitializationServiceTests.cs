using App.Function.Configurations;
using App.Infrastructure.Storage;
using Azure.Storage.Queues;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace App.Function.UnitTests;

public class InitializationServiceTests
{
    private readonly Mock<ILogger<InitializationService>> _loggerMock;
    private readonly Mock<IOptions<ConnectionStringOptions>> _optionsMock;
    private readonly InitializationService _initializationService;

    public InitializationServiceTests()
    {
        _loggerMock = new Mock<ILogger<InitializationService>>();
        _optionsMock = new Mock<IOptions<ConnectionStringOptions>>();
        _optionsMock.Setup(o => o.Value).Returns(new ConnectionStringOptions { AzureStorage = "UseDevelopmentStorage=true" });

        _initializationService = new InitializationService(_optionsMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task StartAsync_ShouldLogInformationAndCreateQueue()
    {
        // Arrange
        var queueClientFactoryMock = new Mock<QueueClientFactory>("UseDevelopmentStorage=true");
        var queueClientMock = new Mock<QueueClient>();
        queueClientFactoryMock.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(queueClientMock.Object);
        queueClientMock.Setup(q => q.CreateIfNotExistsAsync(null, It.IsAny<CancellationToken>())).Returns(It.IsAny<Task>);

        // Act
        await _initializationService.StartAsync(CancellationToken.None);

        // Assert
        _loggerMock.Verify(logger => logger.LogInformation("Starting initialization checks prior to the function starting ..."), Times.Once);
        queueClientFactoryMock.Verify(f => f.CreateClient("money-manager"), Times.Once);
        queueClientMock.Verify(q => q.CreateIfNotExistsAsync(null, It.IsAny<CancellationToken>()), Times.Once);
        _loggerMock.Verify(logger => logger.LogInformation(" ... done!"), Times.Once);
    }

    [Fact]
    public async Task StartAsync_ShouldLogErrorOnException()
    {
        // Arrange
        var queueClientFactoryMock = new Mock<QueueClientFactory>("UseDevelopmentStorage=true");
        var queueClientMock = new Mock<QueueClient>();
        queueClientFactoryMock.Setup(f => f.CreateClient(It.IsAny<string>())).Returns(queueClientMock.Object);
        queueClientMock.Setup(q => q.CreateIfNotExistsAsync(null, It.IsAny<CancellationToken>())).ThrowsAsync(new Exception("Test exception"));

        // Act
        await _initializationService.StartAsync(CancellationToken.None);

        // Assert
        _loggerMock.Verify(logger => logger.LogInformation("Starting initialization checks prior to the function starting ..."), Times.Once);
        _loggerMock.Verify(logger => logger.LogError(It.IsAny<Exception>(), "Failed to initialize any initialization-startup logic."), Times.Once);
    }

    //[Fact]
    //public async Task StopAsync_ShouldCompleteTask()
    //{
    //    // Act
    //    var result = await _initializationService.StopAsync(CancellationToken.None);

    //    // Assert
    //    Assert.Equal(Task.CompletedTask, result);
    //}
}
