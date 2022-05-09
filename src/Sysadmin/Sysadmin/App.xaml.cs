using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using SysAdmin.ActiveDirectory.Services.Ldap;
using SysAdmin.Controls;
using SysAdmin.Services;
using SysAdmin.Services.Dialogs;
using SysAdmin.Views.Computers;
using SysAdmin.Views.Contacts;
using SysAdmin.Views.Groups;
using SysAdmin.Views.Users;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SysAdmin
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            Services = ConfigureServices();
            this.InitializeComponent();

            // Get theme choice from LocalSettings.
            object value = ApplicationData.Current.LocalSettings.Values["ThemeSetting"];

            if (value != null)
                App.Current.RequestedTheme = (ApplicationTheme)(int)value;

            if (ApplicationData.Current.LocalSettings.Values["UserDisplayNameFormat"] == null)
                ApplicationData.Current.LocalSettings.Values["UserDisplayNameFormat"] = @"(?<FirstName>\S+) (?<LastName>\S+)";

            if (ApplicationData.Current.LocalSettings.Values["UserLoginPattern"] == null)
                ApplicationData.Current.LocalSettings.Values["UserLoginPattern"] = @"(?<FirstName>\S+) (?<LastName>\S+)";

            if (ApplicationData.Current.LocalSettings.Values["UserLoginFormat"] == null)
                ApplicationData.Current.LocalSettings.Values["UserLoginFormat"] = @"${FirstName}.${LastName}";
        }

        /// <summary>
        /// Gets the current <see cref="App"/> instance in use
        /// </summary>
        public new static App Current => (App)Application.Current;

        public static IServer SERVER = null;
        public static ICredential CREDENTIAL = null;

        /// <summary>
        /// Gets the <see cref="IServiceProvider"/> instance to resolve application services.
        /// </summary>
        public IServiceProvider Services { get; }

        /// <summary>
        /// Configures the services for the application.
        /// </summary>
        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            services.AddTransient<IAddComputerDialogService, AddComputerDialog>();
            services.AddTransient<IEditComputerDialogService, EditComputerDialog>();

            services.AddTransient<IAddContactDialogService, AddContactDialog>();
            services.AddTransient<IEditContactDialogService, EditContactDialog>();

            services.AddTransient<IAddGroupDialogService, AddGroupDialog>();
            services.AddTransient<IEditGroupDialogService, EditGroupDialog>();

            services.AddTransient<IAddUserDialogService, AddUserDialog>();
            services.AddTransient<IEditUserDialogService, EditUserDialog>();
            services.AddTransient<IUserOptionsDialogService, UserOptionsDialog>();

            services.AddTransient<IQuestionDialogService, QuestionDialog>();

            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<INotificationService, NotificationService>();
            services.AddSingleton<IBusyService, BusyService>();

            return services.BuildServiceProvider();
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="args">Details about the launch request and process.</param>
        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            m_window = new MainWindow();
            m_window.Activate();
        }

        private static Window m_window;

        public static Window GetMainWindow()
        {
            return m_window;
        }
    }
}
