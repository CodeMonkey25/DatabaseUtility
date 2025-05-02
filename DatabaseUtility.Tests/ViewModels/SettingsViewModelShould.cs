using DatabaseUtility.Models;
using DatabaseUtility.Services;
using DatabaseUtility.ViewModels;
using Moq;
using Shouldly;

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
        Settings settings = new()
        {
            DatabaseNameFilter = "old filter",
            ShowOnlyRM12Databases = true,
            ShowOnlyDefaultLocations = true,
            DatabaseUser = "old_name",
            DatabasePassword = "old_password"       
        };
        Mock<ISettingsService> settingsServiceMock = new();
        settingsServiceMock.Setup(mock => mock.Get()).Returns(settings);
        SettingsViewModel sut = new(settingsServiceMock.Object);
        
        // Act
        sut.DatabaseNameFilter = "new filter";
        sut.ShowOnlyRM12Databases = false;
        sut.ShowOnlyDefaultLocations = false;
        sut.DatabaseUser = "new_name";
        sut.DatabasePassword = "new_password";
        sut.Save();
        
        // Assert
        settingsServiceMock.Verify(mock => mock.Save(settings), Times.Once);
        settings.DatabaseNameFilter.ShouldBe(sut.DatabaseNameFilter);
        settings.ShowOnlyRM12Databases.ShouldBe(sut.ShowOnlyRM12Databases);
        settings.ShowOnlyDefaultLocations.ShouldBe(sut.ShowOnlyDefaultLocations);
        settings.DatabaseUser.ShouldBe(sut.DatabaseUser);
        settings.DatabasePassword.ShouldBe(sut.DatabasePassword);       
    }
    
    [Fact]
    public void CancelSettingsUponCancelButtonClick()
    {
        // Arrange
        Settings settings = new()
        {
            DatabaseNameFilter = "old filter",
            ShowOnlyRM12Databases = true,
            ShowOnlyDefaultLocations = true,
            DatabaseUser = "old_name",
            DatabasePassword = "old_password"       
        };
        Mock<ISettingsService> settingsServiceMock = new();
        settingsServiceMock.Setup(mock => mock.Get()).Returns(settings);
        SettingsViewModel sut = new(settingsServiceMock.Object);
        
        // Act
        sut.DatabaseNameFilter = "new filter";
        sut.ShowOnlyRM12Databases = false;
        sut.ShowOnlyDefaultLocations = false;
        sut.DatabaseUser = "new_name";
        sut.DatabasePassword = "new_password";
        sut.Cancel();
        
        // Assert
        settingsServiceMock.Verify(mock => mock.Save(settings), Times.Never);
        sut.DatabaseNameFilter.ShouldBe(settings.DatabaseNameFilter);
        sut.ShowOnlyRM12Databases.ShouldBe(settings.ShowOnlyRM12Databases);
        sut.ShowOnlyDefaultLocations.ShouldBe(settings.ShowOnlyDefaultLocations);
        sut.DatabaseUser.ShouldBe(settings.DatabaseUser);
        sut.DatabasePassword.ShouldBe(settings.DatabasePassword);       
    }
}