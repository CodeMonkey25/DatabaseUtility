using DatabaseUtility.Models;
using DatabaseUtility.Services;
using DatabaseUtility.ViewModels;
using Moq;

namespace DatabaseUtility.Tests.ViewModels;

public class SettingsViewModelShould
{
    [Fact]
    public void FetchSettingsUponStartup()
    {
        // Arrange
        Mock<ISettingsService> settingsServiceMock = new();
        settingsServiceMock.Setup(mock => mock.Get()).Returns(new Settings());

        // Act
        SettingsViewModel _ = new(settingsServiceMock.Object);
        
        // Assert
        settingsServiceMock.Verify(mock => mock.Get(), Times.Once);
    }
    
    [Fact]
    public void SaveSettingsUponSaveButtonClick()
    {
        // Arrange
        Settings settings = new();
        Mock<ISettingsService> settingsServiceMock = new();
        settingsServiceMock.Setup(mock => mock.Get()).Returns(settings);
        SettingsViewModel sut = new(settingsServiceMock.Object);
        
        // Act
        sut.Save();
        
        // Assert
        settingsServiceMock.Verify(mock => mock.Save(settings), Times.Once);
    }
    
    [Fact]
    public void CancelSettingsUponCancelButtonClick()
    {
        // Arrange
        Settings settings = new();
        Mock<ISettingsService> settingsServiceMock = new();
        settingsServiceMock.Setup(mock => mock.Get()).Returns(settings);
        SettingsViewModel sut = new(settingsServiceMock.Object);
        
        // Act
        sut.Cancel();
        
        // Assert
        settingsServiceMock.Verify(mock => mock.Save(settings), Times.Never);
    }
}