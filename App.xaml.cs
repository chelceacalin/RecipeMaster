using RecipeMaster.Model;
using Microsoft.Maui.Controls;
using System.Diagnostics;

namespace RecipeMaster
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Set up a global unhandled exception handler
            AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
            {
                HandleGlobalException(e.ExceptionObject as Exception);
            };
            TaskScheduler.UnobservedTaskException += (sender, e) =>
            {
                HandleGlobalException(e.Exception);
                e.SetObserved(); // Prevents the application from terminating due to unobserved task exceptions
            };

            try
            {
                DatabaseManager.Instance.CreateTable<Recipe>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Database initialization failed: {ex.Message}");
                ShowInitializationError();
            }

            try
            {
                MainPage = new NavigationPage(new RecipesPage());
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Navigation setup failed: {ex.Message}");
                ShowInitializationError();
            }
        }

        // Method to handle global exceptions
        private void HandleGlobalException(Exception ex)
        {
            Debug.WriteLine($"Unhandled exception: {ex?.Message}");
            ShowInitializationError();
        }

        private void ShowInitializationError()
        {
            Application.Current?.Dispatcher.Dispatch(async () =>
            {
                // Display error message to the user
                await MainPage?.DisplayAlert("Error", "An error occurred during initialization. Redirecting to the main page.", "OK");

                // Navigate back to the main page to avoid app termination
                MainPage = new NavigationPage(new RecipesPage());
            });
        }
    }
}
