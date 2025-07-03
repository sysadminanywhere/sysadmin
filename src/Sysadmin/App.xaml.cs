using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sysadmin.Models;
using Sysadmin.Services;
using SysAdmin.ActiveDirectory;
using SysAdmin.ActiveDirectory.Services.Ldap;
using SysAdmin.Services;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;
using Wpf.Ui;

namespace Sysadmin
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        // The.NET Generic Host provides dependency injection, configuration, logging, and other services.
        // https://docs.microsoft.com/dotnet/core/extensions/generic-host
        // https://docs.microsoft.com/dotnet/core/extensions/dependency-injection
        // https://docs.microsoft.com/dotnet/core/extensions/configuration
        // https://docs.microsoft.com/dotnet/core/extensions/logging
        private static readonly IHost _host = Host
            .CreateDefaultBuilder()
            .ConfigureAppConfiguration(c => { c.SetBasePath(Path.GetDirectoryName(Assembly.GetEntryAssembly()!.Location)); })
            .ConfigureServices((context, services) =>
            {
                // App Host
                services.AddHostedService<ApplicationHostService>();

                // Page resolver service
                services.AddSingleton<IPageService, PageService>();

                // Theme manipulation
                services.AddSingleton<IThemeService, ThemeService>();

                // TaskBar manipulation
                services.AddSingleton<ITaskBarService, TaskBarService>();

                // Service containing navigation, same as INavigationWindow... but without window
                services.AddSingleton<INavigationService, NavigationService>();

                // Main window with navigation
                services.AddScoped<INavigationWindow, Views.Windows.MainWindow>();
                services.AddScoped<ViewModels.MainWindowViewModel>();

                // Views and ViewModels
                services.AddScoped<Views.Pages.DashboardPage>();
                services.AddScoped<ViewModels.DashboardViewModel>();
                services.AddScoped<Views.Pages.SettingsPage>();
                services.AddScoped<ViewModels.SettingsViewModel>();

                services.AddScoped<Views.Pages.ComputersPage>();
                services.AddScoped<Views.Pages.ContactsPage>();
                services.AddScoped<Views.Pages.GroupsPage>();
                services.AddScoped<Views.Pages.PrintersPage>();
                services.AddScoped<Views.Pages.ReportsPage>();
                services.AddScoped<Views.Pages.UsersPage>();
                services.AddScoped<Views.Pages.LoginPage>();
                services.AddScoped<Views.Pages.UserPage>();
                services.AddScoped<Views.Pages.ComputerPage>();
                services.AddScoped<Views.Pages.ContactPage>();
                services.AddScoped<Views.Pages.GroupPage>();
                services.AddScoped<Views.Pages.PrinterPage>();
                services.AddScoped<Views.Pages.ReportPage>();
                services.AddScoped<Views.Pages.AddComputerPage>();
                services.AddScoped<Views.Pages.AddContactPage>();
                services.AddScoped<Views.Pages.AddGroupPage>();
                services.AddScoped<Views.Pages.AddUserPage>();
                services.AddScoped<Views.Pages.EditComputerPage>();
                services.AddScoped<Views.Pages.EditContactPage>();
                services.AddScoped<Views.Pages.EditGroupPage>();
                services.AddScoped<Views.Pages.EditUserPage>();
                services.AddScoped<Views.Pages.UserOptionsPage>();
                services.AddScoped<Views.Pages.ResetPasswordPage>();
                services.AddScoped<Views.Pages.EventsPage>();
                services.AddScoped<Views.Pages.HardwarePage>();
                services.AddScoped<Views.Pages.PerformancePage>();
                services.AddScoped<Views.Pages.ProcessesPage>();
                services.AddScoped<Views.Pages.ServicesPage>();
                services.AddScoped<Views.Pages.SoftwarePage>();

                services.AddScoped<ViewModels.ComputersViewModel>();
                services.AddScoped<ViewModels.ContactsViewModel>();
                services.AddScoped<ViewModels.GroupsViewModel>();
                services.AddScoped<ViewModels.PrintersViewModel>();
                services.AddScoped<ViewModels.ReportsViewModel>();
                services.AddScoped<ViewModels.ReportViewModel>();
                services.AddScoped<ViewModels.UsersViewModel>();
                services.AddScoped<ViewModels.LoginViewModel>();
                services.AddScoped<ViewModels.UserViewModel>();
                services.AddScoped<ViewModels.ComputerViewModel>();
                services.AddScoped<ViewModels.ContactViewModel>();
                services.AddScoped<ViewModels.GroupViewModel>();
                services.AddScoped<ViewModels.PrinterViewModel>();
                services.AddScoped<ViewModels.AddUserViewModel>();
                services.AddScoped<ViewModels.AddComputerViewModel>();
                services.AddScoped<ViewModels.AddContactViewModel>();
                services.AddScoped<ViewModels.AddGroupViewModel>();
                services.AddScoped<ViewModels.EditUserViewModel>();
                services.AddScoped<ViewModels.EditComputerViewModel>();
                services.AddScoped<ViewModels.EditContactViewModel>();
                services.AddScoped<ViewModels.EditGroupViewModel>();
                services.AddScoped<ViewModels.UserOptionsViewModel>();
                services.AddScoped<ViewModels.ResetPasswordViewModel>();
                services.AddScoped<ViewModels.EventsViewModel>();
                services.AddScoped<ViewModels.HardwareViewModel>();
                services.AddScoped<ViewModels.PerformanceViewModel>();
                services.AddScoped<ViewModels.ProcessesViewModel>();
                services.AddScoped<ViewModels.ServicesViewModel>();
                services.AddScoped<ViewModels.SoftwareViewModel>();

                services.AddSingleton<ISettingsService, SettingsService>();
                services.AddSingleton<IStateService, StateService>();
                services.AddSingleton<IExchangeService, ExchangeService>();

                // Configuration
                services.Configure<AppConfig>(context.Configuration.GetSection(nameof(AppConfig)));
            }).Build();

        /// <summary>
        /// Gets registered service.
        /// </summary>
        /// <typeparam name="T">Type of the service to get.</typeparam>
        /// <returns>Instance of the service or <see langword="null"/>.</returns>
        public static T GetService<T>()
            where T : class
        {
            return _host.Services.GetService(typeof(T)) as T;
        }

        /// <summary>
        /// Occurs when the application is loading.
        /// </summary>
        private async void OnStartup(object sender, StartupEventArgs e)
        {
            await _host.StartAsync();
        }

        /// <summary>
        /// Occurs when the application is closing.
        /// </summary>
        private async void OnExit(object sender, ExitEventArgs e)
        {
            await _host.StopAsync();

            _host.Dispose();
        }

        /// <summary>
        /// Occurs when an exception is thrown by an application but not handled.
        /// </summary>
        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            // For more info see https://docs.microsoft.com/en-us/dotnet/api/system.windows.application.dispatcherunhandledexception?view=windowsdesktop-6.0
        }

        public static IServer? SERVER = null;           //NOSONAR
        public static ICredential? CREDENTIAL = null;   //NOSONAR
        public static ADContainers? CONTAINERS = null;  //NOSONAR

    }
}