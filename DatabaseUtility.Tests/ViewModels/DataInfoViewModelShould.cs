using DatabaseUtility.Models;
using DatabaseUtility.Services;
using DatabaseUtility.ViewModels;
using Moq;

namespace DatabaseUtility.Tests.ViewModels;

public class DataInfoViewModelShould
{
    [Fact]
    public void FetchSettingsUponStartup()
    {
        // Arrange
        Mock<ISettingsService> settingsServiceMock = new();
        settingsServiceMock.Setup(mock => mock.Get()).Returns(new Settings());
        Mock<IDatabaseService> databaseServiceMock = new();
        Mock<ILoggerService> loggerServiceMock = new();

        // Act
        DataInfoViewModel sut = new(settingsServiceMock.Object, databaseServiceMock.Object, loggerServiceMock.Object);
        
        // Assert
        settingsServiceMock.Verify(mock => mock.Get(), Times.Once);
    }
}