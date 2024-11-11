using RecipeMaster.Model;
using System.Diagnostics;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace RecipeMaster;

public partial class RecipesPage : ContentPage
{
    HttpClient _httpClient = new HttpClient();
    private Recipe _randomRecipe = new Recipe();
    private DatabaseManager dbManager = DatabaseManager.Instance; // Instanța DatabaseManager

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

 
    private async void OnShowFavoriteClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new FavoriteListPage());
    }


    private async void OnStatisticsPageClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new StatisticsPage());
    }

    private async void OnShowCreditsPageClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CreditsPage());
        }

    private async Task<ImageSource> GetImageFromUrl(string imageUrl)
    {
        try
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync(imageUrl);
            if (response.IsSuccessStatusCode)
            {
                var stream = await response.Content.ReadAsStreamAsync();
                return ImageSource.FromStream(() => stream);
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error downloading image: {ex.Message}");
        }
        return null;
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
                    RandomRecipeImage.Source = await GetImageFromUrl(_randomRecipe.MealThumb);
                }

                bool isFavorite = dbManager.GetFavoriteRecipes().Any(f => f.Id == _randomRecipe.Id);
                UpdateFavoriteButtonText(isFavorite);
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

  
    private void ToggleFavoriteStatus(object sender, EventArgs e)
    {
        if (_randomRecipe != null)
        {
          
            var existingRecipe = dbManager.GetRecordById<Recipe>(_randomRecipe.Id);
            if (existingRecipe == null)
            {
                
                dbManager.InsertRecord(_randomRecipe);
                Debug.WriteLine($"Added recipe ID {_randomRecipe.Id} to Recipe table.");
            }

           
            bool isFavorite = dbManager.GetAllRecords<Favorite>().Any(f => f.RecipeId == _randomRecipe.Id);

            if (isFavorite)
            {
                
                dbManager.RemoveFromFavorites(_randomRecipe.Id);
                Debug.WriteLine($"Removed recipe ID {_randomRecipe.Id} from favorites.");
            }
            else
            {
                
                dbManager.AddToFavorites(_randomRecipe.Id);
                Debug.WriteLine($"Added recipe ID {_randomRecipe.Id} to favorites.");
            }

           
            UpdateFavoriteButtonText(!isFavorite);
        }
    }


   
    private void UpdateFavoriteButtonText(bool isFavorite)
    {
        FavoriteButton.Text = isFavorite ? "Remove from Favorites" : "Add to Favorites";
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
        public string? strSource { get; set; }
    }

    public class MealResponse
    {
        public List<Meal> meals { get; set; }
    }
}
