using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using SysAdmin.ActiveDirectory.Services.Ldap;
using SysAdmin.Controls;
using SysAdmin.Services;
using SysAdmin.Services.Dialogs;
using SysAdmin.Views;
using SysAdmin.Views.Computers;
using SysAdmin.Views.Contacts;
using SysAdmin.Views.Groups;
using SysAdmin.Views.Users;
using System;
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

            ISettingsService settings = App.Current.Services.GetService<ISettingsService>();

            // Get theme choice from LocalSettings.
            App.Current.RequestedTheme = (ApplicationTheme)settings.ThemeSetting;

            if (settings.UserDisplayNameFormat == null)
                settings.UserDisplayNameFormat = @"(?<FirstName>\S+) (?<LastName>\S+)";

            if (settings.UserLoginPattern == null)
                settings.UserLoginPattern = @"(?<FirstName>\S+) (?<LastName>\S+)";

            if (settings.UserLoginFormat == null)
                settings.UserLoginFormat = @"${FirstName}.${LastName}";
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
            services.AddTransient<IResetPasswordDialog, ResetPasswordDialog>();

            services.AddTransient<IQuestionDialogService, QuestionDialog>();
            services.AddTransient<IRenameDialogService, RenameDialog>();

            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<INotificationService, NotificationService>();
            services.AddSingleton<IBusyService, BusyService>();

            services.AddSingleton<ISettingsService, SettingsService>();

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
