using DatabaseUtility.Models;
using DatabaseUtility.Services;
using DatabaseUtility.ViewModels;
using Moq;
using Shouldly;

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
        ServersViewModel _ = new(settingsServiceMock.Object);
        
        // Assert
        settingsServiceMock.Verify(mock => mock.Get(), Times.Once);
    }

    [Fact]
    public void AddServerUponAddServerButtonClick()
    {
        // Arrange
        Mock<ISettingsService> settingsServiceMock = new();
        settingsServiceMock.Setup(mock => mock.Get()).Returns(new Settings());
        ServersViewModel sut = new(settingsServiceMock.Object)
        {
            DatabaseServers = ["Server #1", "Server #2"],
            DatabaseServer = "Server #3",
        };

        // Act
        sut.AddServer();

        // Assert
        sut.DatabaseServers.ShouldBe(["Server #1", "Server #2", "Server #3"]);
    }

    [Fact]
    public void RemoveServerUponRemoveServerButtonClick()
    {
        // Arrange
        Mock<ISettingsService> settingsServiceMock = new();
        settingsServiceMock.Setup(mock => mock.Get()).Returns(new Settings());
        ServersViewModel sut = new(settingsServiceMock.Object)
        {
            DatabaseServers = ["Server #1", "Server #2", "Server #3"],
            SelectedDatabaseServerIndex = 2,
        };

        // Act
        sut.RemoveServer();

        // Assert
        sut.DatabaseServers.ShouldBe(["Server #1", "Server #2"]);
    }
    
    [Fact]
    public void MoveServerUpUponMoveUpButtonClick()
    {
        // Arrange
        Mock<ISettingsService> settingsServiceMock = new();
        settingsServiceMock.Setup(mock => mock.Get()).Returns(new Settings());
        ServersViewModel sut = new(settingsServiceMock.Object)
        {
            DatabaseServers = ["Server #1", "Server #2", "Server #3"],
            SelectedDatabaseServerIndex = 2,
        };

        // Act
        sut.MoveServerUp();

        // Assert
        sut.DatabaseServers.ShouldBe(["Server #1", "Server #3", "Server #2"]);
    }
    
    [Fact]
    public void DoNothingUpUponMoveUpButtonClickOnFirstItem()
    {
        // Arrange
        Mock<ISettingsService> settingsServiceMock = new();
        settingsServiceMock.Setup(mock => mock.Get()).Returns(new Settings());
        ServersViewModel sut = new(settingsServiceMock.Object)
        {
            DatabaseServers = ["Server #1", "Server #2", "Server #3"],
            SelectedDatabaseServerIndex = 0,
        };

        // Act
        sut.MoveServerUp();

        // Assert
        sut.DatabaseServers.ShouldBe(["Server #1", "Server #2", "Server #3"]);
    }
    
    [Fact]
    public void MoveServerDownUponMoveDownButtonClick()
    {
        // Arrange
        Mock<ISettingsService> settingsServiceMock = new();
        settingsServiceMock.Setup(mock => mock.Get()).Returns(new Settings());
        ServersViewModel sut = new(settingsServiceMock.Object)
        {
            DatabaseServers = ["Server #1", "Server #2", "Server #3"],
            SelectedDatabaseServerIndex = 1,
        };

        // Act
        sut.MoveServerDown();

        // Assert
        sut.DatabaseServers.ShouldBe(["Server #1", "Server #3", "Server #2"]);
    }
    
    [Fact]
    public void DoNothingUpUponMoveDownButtonClickOnLastItem()
    {
        // Arrange
        Mock<ISettingsService> settingsServiceMock = new();
        settingsServiceMock.Setup(mock => mock.Get()).Returns(new Settings());
        ServersViewModel sut = new(settingsServiceMock.Object)
        {
            DatabaseServers = ["Server #1", "Server #2", "Server #3"],
            SelectedDatabaseServerIndex = 2,
        };

        // Act
        sut.MoveServerDown();

        // Assert
        sut.DatabaseServers.ShouldBe(["Server #1", "Server #2", "Server #3"]);
    }
    
    [Fact]
    public void SaveSettingsUponSaveButtonClick()
    {
        // Arrange
        Settings settings = new()
        {
            DatabaseServers = ["Server #1", "Server #2", "Server #3"],
        };
        Mock<ISettingsService> settingsServiceMock = new();
        settingsServiceMock.Setup(mock => mock.Get()).Returns(settings);
        settingsServiceMock.Setup(mock => mock.Save(settings));
        ServersViewModel sut = new(settingsServiceMock.Object)
        {
            DatabaseServers = ["Server #4", "Server #5", "Server #6"],
        };

        // Act
        sut.Save();

        // Assert
        settings.DatabaseServers.ShouldBe(["Server #4", "Server #5", "Server #6"]);
        settingsServiceMock.Verify(mock => mock.Save(settings), Times.Once);
    }
    
    [Fact]
    public void RevertSettingsUponCancelButtonClick()
    {
        // Arrange
        Settings settings = new()
        {
            DatabaseServers = ["Server #1", "Server #2", "Server #3"],
        };
        Mock<ISettingsService> settingsServiceMock = new();
        settingsServiceMock.Setup(mock => mock.Get()).Returns(settings);
        settingsServiceMock.Setup(mock => mock.Save(settings));
        ServersViewModel sut = new(settingsServiceMock.Object)
        {
            DatabaseServers = ["Server #4", "Server #5", "Server #6"],
        };

        // Act
        sut.Cancel();

        // Assert
        sut.DatabaseServers.ShouldBe(["Server #1", "Server #2", "Server #3"]);
        settingsServiceMock.Verify(mock => mock.Save(settings), Times.Never);
    }
}