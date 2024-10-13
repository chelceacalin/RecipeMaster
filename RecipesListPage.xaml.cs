using RecipeMaster.Model;

namespace RecipeMaster;

public partial class RecipesListPage : ContentPage
{
    public RecipesListPage()
    {
        InitializeComponent();
        LoadRecipes();
    }

    private void LoadRecipes()
    {
        try
        {
            var recipes = DatabaseManager.Instance.GetAllRecords<Recipe>();

            if (recipes != null && recipes.Count > 0)
            {
                foreach (var recipe in recipes)
                {
                    if (!string.IsNullOrEmpty(recipe.MealThumb))
                    {
                        recipe.MealThumb = recipe.MealThumb.Replace("Uri: ", "");
                    }
                }
                RecipesListView.ItemsSource = recipes;
            }
            else
            {
                DisplayAlert("Info", "No recipes found.", "OK");
            }
        }
        catch (Exception ex)
        {
            DisplayAlert("Error", "Failed to load recipes: " + ex.Message, "OK");
        }
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
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
            var confirm = await DisplayAlert("Confirm", $"Are you sure you want to delete {recipe.Title}?", "Yes", "No");

            if (confirm)
            {
                try
                {
                    DatabaseManager.Instance.DeleteRecord(recipe);
                    LoadRecipes();
                    await DisplayAlert("Deleted", $"{recipe.Title} was deleted.", "OK");
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", $"Failed to delete {recipe.Title}: {ex.Message}", "OK");
                }
            }
        }
    }
}
