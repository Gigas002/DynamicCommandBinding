using System.Windows;
using DynamicCommandBinding.Views;
using Prism.Ioc;

namespace DynamicCommandBinding
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell() => Container.Resolve<MainView>();

        protected override void RegisterTypes(IContainerRegistry containerRegistry) { }
    }
}
