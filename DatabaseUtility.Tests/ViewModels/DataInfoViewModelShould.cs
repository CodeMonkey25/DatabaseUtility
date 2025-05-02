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
        Mock<ILoggerService> loggerServiceMock = new();
        Mock<ISettingsService> settingsServiceMock = new();
        settingsServiceMock.Setup(mock => mock.Get()).Returns(new Settings());
        Mock<IDatabaseService> databaseServiceMock = new();
        Mock<IDataInfoFileService> dataInfoFileServiceMock = new();

        // Act
        DataInfoViewModel _ = new(loggerServiceMock.Object, settingsServiceMock.Object, databaseServiceMock.Object, dataInfoFileServiceMock.Object);
        
        // Assert
        settingsServiceMock.Verify(mock => mock.Get(), Times.Once);
    }
    
    [Fact]
    public void CallGetDataInfoUponStartup()
    {
        // Arrange
        Mock<ILoggerService> loggerServiceMock = new();
        Mock<ISettingsService> settingsServiceMock = new();
        settingsServiceMock.Setup(mock => mock.Get()).Returns(new Settings());
        Mock<IDatabaseService> databaseServiceMock = new();
        Mock<IDataInfoFileService> dataInfoFileServiceMock = new();
        dataInfoFileServiceMock.Setup(mock => mock.GetDataInfo(DataInfoViewModel.DefaultDataInfoFilePath)).Returns(("", ""));

        // Act
        DataInfoViewModel _ = new(loggerServiceMock.Object, settingsServiceMock.Object, databaseServiceMock.Object, dataInfoFileServiceMock.Object);
        
        // Assert
        dataInfoFileServiceMock.Verify(mock => mock.GetDataInfo(DataInfoViewModel.DefaultDataInfoFilePath), Times.Once);
    }
    
    [Fact]
    public void CallSaveDataInfoUponUpdateButtonClick()
    {
        // Arrange
        const string serverName = "serverName";
        const string databaseName = "databaseName";
        Mock<ILoggerService> loggerServiceMock = new();
        Mock<ISettingsService> settingsServiceMock = new();
        settingsServiceMock.Setup(mock => mock.Get()).Returns(new Settings());
        Mock<IDatabaseService> databaseServiceMock = new();
        Mock<IDataInfoFileService> dataInfoFileServiceMock = new();
        dataInfoFileServiceMock.Setup(mock => mock.GetDataInfo(DataInfoViewModel.DefaultDataInfoFilePath)).Returns(("", ""));
        dataInfoFileServiceMock.Setup(mock => mock.SaveDataInfo(DataInfoViewModel.DefaultDataInfoFilePath, serverName, databaseName));
        DataInfoViewModel sut = new(loggerServiceMock.Object, settingsServiceMock.Object, databaseServiceMock.Object, dataInfoFileServiceMock.Object)
        {
            SelectedDatabaseServer = serverName,
            SelectedDatabaseName = databaseName,
        };

        // Act
        sut.UpdateDataInfo();
        
        // Assert
        dataInfoFileServiceMock.Verify(mock => mock.SaveDataInfo(DataInfoViewModel.DefaultDataInfoFilePath, serverName, databaseName), Times.Once);
    }
    
    [Fact]
    public void CallGetDatabaseNamesUponServerChange()
    {
        // Arrange
        const string serverName = "serverName";
        Mock<ILoggerService> loggerServiceMock = new();
        Mock<ISettingsService> settingsServiceMock = new();
        settingsServiceMock.Setup(mock => mock.Get()).Returns(new Settings());
        Mock<IDatabaseService> databaseServiceMock = new();
        databaseServiceMock.Setup(mock => mock.GetDatabaseNames(serverName)).Returns(new List<string>());
        Mock<IDataInfoFileService> dataInfoFileServiceMock = new();
        DataInfoViewModel sut = new(loggerServiceMock.Object, settingsServiceMock.Object, databaseServiceMock.Object, dataInfoFileServiceMock.Object);

        // Act
        sut.SelectedDatabaseServer = serverName;
        
        // Assert
        databaseServiceMock.Verify(mock => mock.GetDatabaseNames(serverName), Times.Exactly(1));
    }
}