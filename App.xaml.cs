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

            AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
            {
                HandleGlobalException(e.ExceptionObject as Exception);
            };
            TaskScheduler.UnobservedTaskException += (sender, e) =>
            {
                HandleGlobalException(e.Exception);
                e.SetObserved();

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

        private void HandleGlobalException(Exception ex)
        {
            Debug.WriteLine($"Unhandled exception: {ex?.Message}");
            ShowInitializationError();
        }

        private void ShowInitializationError()
        {
            Application.Current?.Dispatcher.Dispatch(async () =>
            {
                await MainPage?.DisplayAlert("Error", "An error occurred during initialization. Redirecting to the main page.", "OK");

                MainPage = new NavigationPage(new RecipesPage());
            });
        }
    }
}
