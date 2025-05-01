﻿using DatabaseUtility.Models;
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
}