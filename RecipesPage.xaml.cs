using Newtonsoft.Json.Linq;
using RecipeMaster.Model;
using SQLite;
using System.Net.Http.Json;

namespace RecipeMaster;

public partial class RecipesPage : ContentPage
{
	HttpClient _httpClient = new HttpClient();
    private Recipe _randomRecipe;
    public RecipesPage()
	{
		InitializeComponent();
         LoadRandomRecipe();
    }

    private async Task LoadRandomRecipe()
    {
        try
        {
            string randomRecipeLink = "http://www.themealdb.com/api/json/v1/1/random.php";
            var response = await _httpClient.GetFromJsonAsync<MealResponse>(randomRecipeLink);

            if (response != null && response.meals != null && response.meals.Count > 0)
            {
                var meal = response.meals[0]; // Prima rețetă din listă
                _randomRecipe = new Recipe
                {
                    Title = meal.strMeal,
                    Category = meal.strCategory,
                    Description = meal.strInstructions,
                    MealThumb = meal.strMealThumb,
                    YoutubeLink = meal.strYoutube,
                    Area= meal.strArea,
                    Tags= meal.strTags,
                    Source=meal.strSource
                };

                // Actualizează interfața utilizatorului cu datele obținute
                RandomRecipeTitle.Text = _randomRecipe.Title; // Setează titlul rețetei
                RandomRecipeCategory.Text = _randomRecipe.Category; // Setează categoria rețetei

                if (!string.IsNullOrEmpty(_randomRecipe.MealThumb))
                {
                    RandomRecipeImage.Source = _randomRecipe.MealThumb; // Setează imaginea rețetei
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
            // Navighează către pagina de detalii și transmite rețeta selectată
           await Navigation.PushAsync(new RecipeDetailsPage(_randomRecipe));
        }
        else
        {
            await DisplayAlert("Error", "No recipe loaded", "OK");
        }
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

    private async void OnSaveRandomRecipeClicked(object sender, EventArgs e)
    {
        //using (var connection = new SQLiteConnection(App.DatabasePath))
        //{
        //    connection.CreateTable<Recipe>();
        //    connection.Insert(_randomRecipe);  // Salvează rețeta random cu toate câmpurile
        //}
        //await DisplayAlert("Succes", "Rețeta a fost salvată în baza de date", "OK");
    }


}