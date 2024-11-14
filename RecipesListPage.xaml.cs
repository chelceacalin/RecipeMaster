using RecipeMaster.Model;
using System.Diagnostics;

namespace RecipeMaster;

public partial class RecipesListPage : ContentPage
{
    public RecipesListPage()
    {
        InitializeComponent();
        LoadSavedRecipes();
    }

    private void LoadSavedRecipes()
    {
        var recipes = DatabaseManager.Instance.GetAllRecords<Recipe>();

        foreach (var recipe in recipes)
        {
            recipe.IsFavorite = DatabaseManager.Instance.GetAllRecords<Favorite>().Any(f => f.RecipeId == recipe.Id);

            if (!Uri.IsWellFormedUriString(recipe.MealThumb, UriKind.Absolute))
            {
                recipe.MealThumb = "default_image_path";
            }
        }
        RecipesListView.ItemsSource = recipes;
    }
  
    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private async void OnShowFavoriteClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new FavoriteListPage());
    }

    private async void OnShowMainPageClicked(object sender, EventArgs e)
    {
        await Navigation.PopToRootAsync();
    }

    private async void OnEditClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var recipe = button?.CommandParameter as Recipe;

        if (recipe != null)
        {
            await Navigation.PushAsync(new RecipeEditPage(recipe));
        }
    }

    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var recipe = button?.CommandParameter as Recipe;

        if (recipe != null)
        {
            bool confirm = await DisplayAlert("Confirm", $"Are you sure you want to delete {recipe.Title}?", "Yes", "No");
            if (confirm)
            {
                DatabaseManager.Instance.DeleteRecord(recipe);
                LoadSavedRecipes();
                await DisplayAlert("Deleted", $"{recipe.Title} has been deleted.", "OK");
            }
        }
    }

    private void OnToggleFavoriteClicked(object sender, EventArgs e)
    {
        var button = sender as Button;
        var recipe = button?.CommandParameter as Recipe;

        if (recipe != null)
        {
            bool isFavorite = DatabaseManager.Instance.GetAllRecords<Favorite>().Any(f => f.RecipeId == recipe.Id);

            if (isFavorite)
            {
                DatabaseManager.Instance.RemoveFromFavorites(recipe.Id);
                Debug.WriteLine($"Removed recipe ID {recipe.Id} from favorites.");
            }
            else
            {
                DatabaseManager.Instance.AddToFavorites(recipe.Id);
                Debug.WriteLine($"Added recipe ID {recipe.Id} to favorites.");
            }

            recipe.IsFavorite = !isFavorite;
        }
    }
}
