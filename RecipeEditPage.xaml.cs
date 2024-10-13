using RecipeMaster.Model;
using System.Diagnostics;

namespace RecipeMaster;

public partial class RecipeEditPage : ContentPage
{
    private Recipe _recipe;

    public RecipeEditPage(Recipe recipe)
    {
        InitializeComponent();
        _recipe = recipe;

        TitleEntry.Text = _recipe.Title;
        CategoryEntry.Text = _recipe.Category;
        ImageUrlEntry.Text = _recipe.MealThumb;

        if (!string.IsNullOrEmpty(_recipe.MealThumb))
        {
            RecipeImage.Source =_recipe.MealThumb.Replace("Uri: ","");
        }
        else
        {
            RecipeImage.Source = ""; 
        }

        BindingContext = _recipe;
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        try
        {
            _recipe.Title = TitleEntry.Text;
            _recipe.Category = CategoryEntry.Text;
            _recipe.MealThumb = ImageUrlEntry.Text;

            DatabaseManager.Instance.UpdateRecord(_recipe);
            await DisplayAlert("Success", "Recipe updated successfully", "OK");

            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to save the recipe: {ex.Message}", "OK");
        }
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync(); 
    }

    private async void OnGoToMainPageClicked(object sender, EventArgs e)
    {
        await Navigation.PopToRootAsync();
    }
}
