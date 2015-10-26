using System;
using System.Windows;
using AddressBook.Factories;
using AddressBook.Repositories;
using AddressBook.Services;
using AddressBook.ViewModels;
using Autofac;

namespace AddressBook
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IContainer _container;
        private readonly ContainerBuilder _containerBuilder;

        public App()
        {
            _containerBuilder = new ContainerBuilder();

            // Register MVVM dependencies
            (new RX_MVVM.Bootstrapper()).Register(_containerBuilder);

            // Register Viewmodels
            _containerBuilder.RegisterType<MainViewModel>().As<IMainViewModel>().SingleInstance();
            _containerBuilder.RegisterType<PersonViewModelFactory>().As<IPersonViewModelFactory>().SingleInstance();
            _containerBuilder.RegisterType<DialogService>().As<IDialogService>().SingleInstance();
            _containerBuilder.RegisterType<PersonRepository>().As<IPersonRepository>().SingleInstance();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            try
            {
                ShutdownMode = ShutdownMode.OnMainWindowClose;
                _container = _containerBuilder.Build();

                var mainWidow = new MainWindow {DataContext = _container.Resolve<IMainViewModel>()};
                MainWindow = mainWidow;
                mainWidow.Show();
                base.OnStartup(e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
            
        }
    }
}
