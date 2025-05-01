using DatabaseUtility.Models;
using DatabaseUtility.Services;
using DatabaseUtility.Utility;
using DatabaseUtility.ViewModels;
using Moq;
using Shouldly;

namespace DatabaseUtility.Tests.ViewModels;

public class MainWindowViewModelShould
{
    [Fact]
    public void DisplaysDataInfoViewUponStartup()
    {
        // Arrange
        Mock<IViewFactory> viewFactoryMock = new();
        viewFactoryMock.Setup(mock => mock.CreateView(ViewTypes.DataInfo)).Returns(new DataInfoViewModel());
        
        // Act
        MainWindowViewModel sut = new(viewFactoryMock.Object);
        
        // Assert
        sut.ContentViewModel.ShouldBeOfType<DataInfoViewModel>();
        viewFactoryMock.Verify(mock => mock.CreateView(ViewTypes.DataInfo), Times.Once);
    }
    
    [Fact]
    public void DisplaysSettingsViewUponSettingsButtonClick()
    {
        // Arrange
        Mock<ISettingsService> settingsServiceMock = new();
        settingsServiceMock.Setup(mock => mock.Get()).Returns(new Settings());
        Mock<IViewFactory> viewFactoryMock = new();
        viewFactoryMock.Setup(mock => mock.CreateView(ViewTypes.DataInfo)).Returns(new DataInfoViewModel());
        viewFactoryMock.Setup(mock => mock.CreateView(ViewTypes.Settings)).Returns(new SettingsViewModel(settingsServiceMock.Object));

        MainWindowViewModel sut = new(viewFactoryMock.Object);
        
        // Act
        sut.SwitchToSettings();
        
        // Assert
        viewFactoryMock.Verify(mock => mock.CreateView(ViewTypes.Settings), Times.Once);
        sut.ContentViewModel.ShouldBeOfType<SettingsViewModel>();
    }
    
    [Fact]
    public void DisplaysServersViewUponServersButtonClick()
    {
        // Arrange
        Mock<IViewFactory> viewFactoryMock = new();
        viewFactoryMock.Setup(mock => mock.CreateView(ViewTypes.DataInfo)).Returns(new DataInfoViewModel());
        viewFactoryMock.Setup(mock => mock.CreateView(ViewTypes.Servers)).Returns(new ServersViewModel());
        
        MainWindowViewModel sut = new(viewFactoryMock.Object);
        
        // Act
        sut.SwitchToServers();
        
        // Assert
        sut.ContentViewModel.ShouldBeOfType<ServersViewModel>();
        viewFactoryMock.Verify(mock => mock.CreateView(ViewTypes.Servers), Times.Once);
    }
}