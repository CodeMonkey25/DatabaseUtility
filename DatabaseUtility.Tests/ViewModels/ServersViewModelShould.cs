using DatabaseUtility.Models;
using DatabaseUtility.Services;
using DatabaseUtility.ViewModels;
using Moq;

namespace DatabaseUtility.Tests.ViewModels;

public class ServersViewModelShould
{
    [Fact]
    public void FetchSettingsUponStartup()
    {
        // Arrange
        Mock<ISettingsService> settingsServiceMock = new();
        settingsServiceMock.Setup(mock => mock.Get()).Returns(new Settings());

        // Act
        ServersViewModel sut = new(settingsServiceMock.Object);
        
        // Assert
        settingsServiceMock.Verify(mock => mock.Get(), Times.Once);
    }
}