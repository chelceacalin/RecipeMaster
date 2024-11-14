using RecipeMaster.Model;
using System.Diagnostics;

namespace RecipeMaster;

public partial class FavoriteListPage : ContentPage
{
    public FavoriteListPage()
    {
        InitializeComponent();
        LoadFavoriteRecipes(); 
    }

    private void LoadFavoriteRecipes()
    {
        try
        {
            var favoriteRecipes = DatabaseManager.Instance.GetFavoriteRecipes();

            if (favoriteRecipes != null && favoriteRecipes.Count > 0)
            {
                Debug.WriteLine($"Loaded {favoriteRecipes.Count} favorite recipes into the view.");
                RecipesListView.ItemsSource = favoriteRecipes;
                var recipes = DatabaseManager.Instance.GetAllRecords<Recipe>();

                // Check if records are found and print details
                if (recipes != null && recipes.Count > 0)
                {
                    Debug.WriteLine("All Recipes:");
                    foreach (var recipe in recipes)
                    {
                        Debug.WriteLine($"ID: {recipe.Id}");
                        Debug.WriteLine($"Title: {recipe.Title}");
                        Debug.WriteLine($"Category: {recipe.Category}");
                        Debug.WriteLine($"Description: {recipe.Description}");
                        Debug.WriteLine($"Area: {recipe.Area}");
                        Debug.WriteLine($"MealThumb: {recipe.MealThumb}");
                        Debug.WriteLine($"Tags: {recipe.Tags}");
                        Debug.WriteLine($"Source: {recipe.Source}");
                        Debug.WriteLine($"YoutubeLink: {recipe.YoutubeLink}");
                        Debug.WriteLine($"IsFavorite: {recipe.IsFavorite}");
                        Debug.WriteLine("-----"); // Separator between recipes
                    }
                }
                else
                {
                    Debug.WriteLine("No recipes found in the database.");
                }
            }
            else
            {
                DisplayAlert("Info", "No favorite recipes found.", "OK");
                Debug.WriteLine("No favorite recipes found.");
            }
        }
        catch (Exception ex)
        {
            DisplayAlert("Error", "Failed to load favorite recipes: " + ex.Message, "OK");
            Debug.WriteLine($"Error loading favorite recipes: {ex.Message}");
        }
    }

    private async void OnBackClicked2(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private async void OnShowSavedRecipesClicked(object sender, EventArgs e)
    {
        
        await Navigation.PushAsync(new RecipesListPage());
    }

    private async void OnShowMainPageClicked(object sender, EventArgs e)
    {
        
        await Navigation.PopToRootAsync();
    }

    private async void OnEditClicked2(object sender, EventArgs e)
    {
        var button = sender as Button;
        var recipe = button?.CommandParameter as Recipe;

        if (recipe != null)
        {
            await Navigation.PushAsync(new RecipeEditPage(recipe));
        }
    }

    private async void OnDeleteClicked2(object sender, EventArgs e)
    {
        var button = sender as Button;
        var recipe = button?.CommandParameter as Recipe;

        if (recipe != null)
        {
            var confirm = await DisplayAlert("Confirm", $"Are you sure you want to remove {recipe.Title} from favorites?", "Yes", "No");

            if (confirm)
            {
                try
                {
                    DatabaseManager.Instance.RemoveFromFavorites(recipe.Id);
                    LoadFavoriteRecipes(); 
                    await DisplayAlert("Removed", $"{recipe.Title} was removed from favorites.", "OK");
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", $"Failed to remove {recipe.Title} from favorites: {ex.Message}", "OK");
                }
            }
        }
    }
}
