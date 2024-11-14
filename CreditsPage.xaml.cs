using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;

namespace RecipeMaster
{
    public partial class CreditsPage : ContentPage
    {
        public CreditsPage()
        {
            InitializeComponent();
        #if ANDROID
            DownloadDocxButton.IsVisible = false;
        #endif
        }

        private async void OnBackClicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }

        private async void OnMainPageClicked(object sender, EventArgs e)
        {
            await Navigation.PopToRootAsync();
        }

        private async void OnDownloadDocxClicked(object sender, EventArgs e)
        {
            await SaveAndOpenFile("RecipeMaster.Resources.Docs.Proiect-PDM-RECIPEMASTER.docx", "Proiect-PDM-RECIPEMASTER.docx");
        }

        private async void OnDownloadPdfClicked(object sender, EventArgs e)
        {
            await SaveAndOpenFile("RecipeMaster.Resources.Docs.Proiect-PDM-RECIPEMASTER.pdf", "Proiect-PDM-RECIPEMASTER.pdf");
        }

        private async Task SaveAndOpenFile(string resourceName, string fileName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            using Stream stream = assembly.GetManifestResourceStream(resourceName);

            if (stream == null)
            {
                await DisplayAlert("Error", $"Resource {resourceName} not found.", "OK");
                return;
            }

            string filePath = Path.Combine(FileSystem.CacheDirectory, fileName);
            using FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            await stream.CopyToAsync(fileStream);

            await Launcher.Default.OpenAsync(new OpenFileRequest
            {
                File = new ReadOnlyFile(filePath)
            });
        }
    }
}
