using RecipeMaster.Model;
using System.Net.Http.Json;

namespace RecipeMaster;

public partial class RecipesPage : ContentPage
{
    HttpClient _httpClient = new HttpClient();
    private Recipe _randomRecipe=new Recipe();
    public RecipesPage()
    {
        InitializeComponent();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();

        LoadRandomRecipe(); 
    }


    private async void OnShowRecipesClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new RecipesListPage());
    }
    private async void OnStatisticsPageClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new StatisticsPage());
    }
    private async Task LoadRandomRecipe()
    {
        try
        {
            string randomRecipeLink = "http://www.themealdb.com/api/json/v1/1/random.php";
            var response = await _httpClient.GetFromJsonAsync<MealResponse>(randomRecipeLink);

            if (response != null && response.meals != null && response.meals.Count > 0)
            {
                var meal = response.meals[0];
                _randomRecipe = new Recipe
                {
                    Title = meal.strMeal,
                    Category = meal.strCategory,
                    Description = meal.strInstructions,
                    MealThumb = meal.strMealThumb,
                    YoutubeLink = meal.strYoutube,
                    Area = meal.strArea,
                    Tags = meal.strTags,
                    Source = meal.strSource
                };

                RandomRecipeTitle.Text = _randomRecipe.Title;
                RandomRecipeCategory.Text = _randomRecipe.Category;

                if (!string.IsNullOrEmpty(_randomRecipe.MealThumb))
                {
                    RandomRecipeImage.Source = _randomRecipe.MealThumb;
                }
            }
            else
            {
                await DisplayAlert("Error", "No recipe found.", "OK");
            }
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine($"Request error: {e.Message}");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "Failed to load the random recipe: " + ex.Message, "OK");
        }
    }

    private async void OnDetailsClicked(object sender, EventArgs e)
    {
        if (_randomRecipe != null)
        {
            await Navigation.PushAsync(new RecipeDetailsPage(_randomRecipe));
        }
        else
        {
            await DisplayAlert("Error", "No recipe loaded", "OK");
        }
    }

    private async void OnRefreshClicked(object sender, EventArgs e)
    {
        await LoadRandomRecipe();
    }

    public class Meal
    {
        public string strMeal { get; set; }
        public string strCategory { get; set; }
        public string strArea { get; set; }
        public string strInstructions { get; set; }
        public string strMealThumb { get; set; }
        public string strTags { get; set; }
        public string strYoutube { get; set; }
        public string strSource { get; set; }
    }

    public class MealResponse
    {
        public List<Meal> meals { get; set; }
    }
}