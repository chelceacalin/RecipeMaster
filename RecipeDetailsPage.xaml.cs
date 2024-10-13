using RecipeMaster.Model;
using System.Diagnostics;

namespace RecipeMaster;

public partial class RecipeDetailsPage : ContentPage
{
    // Constructor unic pentru RecipeDetailsPage
    public RecipeDetailsPage(Recipe recipe)
    {
        InitializeComponent();

        // Populează pagina cu detaliile rețetei
        RecipeTitle.Text = recipe.Title ?? "No title available";
        RecipeCategory.Text = recipe.Category ?? "No category";
        RecipeArea.Text = recipe.Area ?? "No area";

        if (!string.IsNullOrEmpty(recipe.MealThumb))
        {
            RecipeImage.Source = recipe.MealThumb; // Setează imaginea dacă există
        }
        else
        {
            RecipeImage.IsVisible = false; // Ascunde imaginea dacă nu este disponibilă
        }


        Debug.WriteLine($"Recipe details:\n" +
                  $"Title: {recipe.Title}\n" +
                  $"Category: {recipe.Category}\n" +
                  $"Area: {recipe.Area}\n" +
                  $"Tags: {recipe.Tags}\n" +
                  $"Source: {recipe.Source}\n" +
                  $"YouTube: {recipe.YoutubeLink}");

        RecipeDescription.Text = recipe.Description ?? "No description available";

        // Asigură-te că sunt setate valorile pentru Tags, Source și YouTube
        RecipeTags.Text = !string.IsNullOrEmpty(recipe.Tags) ? recipe.Tags : "No tags available";
        RecipeSource.Text = !string.IsNullOrEmpty(recipe.Source) ? recipe.Source : "No source available";
        RecipeYoutubeLink.Text = !string.IsNullOrEmpty(recipe.YoutubeLink) ? recipe.YoutubeLink : "No YouTube link available";
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }


    // Metoda pentru tap pe RecipeSource
    private async void OnSourceTapped(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(RecipeSource.Text) && RecipeSource.Text != "No source available")
        {
            try
            {
                Uri uri = new Uri(RecipeSource.Text);
                await Launcher.OpenAsync(uri); // Deschide URL-ul în browser
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Unable to open source link: " + ex.Message, "OK");
            }
        }
    }

    // Metoda pentru tap pe RecipeYoutubeLink
    private async void OnYoutubeTapped(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(RecipeYoutubeLink.Text) && RecipeYoutubeLink.Text != "No YouTube link available")
        {
            try
            {
                Uri uri = new Uri(RecipeYoutubeLink.Text);
                await Launcher.OpenAsync(uri); // Deschide URL-ul în browser
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Unable to open YouTube link: " + ex.Message, "OK");
            }
        }
    }
}
