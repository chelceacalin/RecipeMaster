using RecipeMaster.Model;
using System.Diagnostics;

namespace RecipeMaster
{
    public partial class StatisticsPage : ContentPage
    {
        public StatisticsPage()
        {
            InitializeComponent();
            List<Recipe> recipes = DatabaseManager.Instance.GetAllRecords<Recipe>();
            var statsManager = new StatisticsManager(recipes);

            var categoryStats = statsManager.GetRecipesByCategory();
            var areaStats = statsManager.GetRecipesByArea();

            GeneratePieChart(categoryStats);
            GenerateBarChart(areaStats);
        }

        private async void OnBackToMainPageClicked(object sender, EventArgs e)
        {
            await Navigation.PopToRootAsync();
        }

        private async void OnBackToListViewClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RecipesListPage());
        }

        private void GeneratePieChart(Dictionary<string, int> data)
        {
            PieChartLayout.Children.Clear();
            int total = data.Values.Sum();

            foreach (var entry in data)
            {
                double percentage = (double)entry.Value / total;

              
                var pieSlice = new BoxView
                {
                    WidthRequest = percentage * 200, 
                    HeightRequest = 30,
                    BackgroundColor = GetRandomColor(),
                    HorizontalOptions = LayoutOptions.FillAndExpand,
                    CornerRadius = 15
                };

                
                var label = new Label
                {
                    Text = $"{entry.Key}: {percentage:P1}",
                    FontSize = 14,
                    HorizontalOptions = LayoutOptions.Center,
                    TextColor = Color.FromRgb(220, 220, 220),
                    FontAttributes = FontAttributes.Bold
                };

                PieChartLayout.Children.Add(label);
                PieChartLayout.Children.Add(pieSlice);
            }
        }

        private void GenerateBarChart(Dictionary<string, int> data)
        {
            BarChartLayout.Children.Clear();
            foreach (var entry in data)
            {
                var label = new Label
                {
                    Text = $"{entry.Key}: {entry.Value}",
                    FontSize = 14,
                    HorizontalOptions = LayoutOptions.Start,
                    TextColor = Color.FromRgb(220, 220, 220),
                    FontAttributes = FontAttributes.Bold
                };

                var bar = new BoxView
                {
                    HeightRequest = 20,
                    WidthRequest = entry.Value * 10, 
                    BackgroundColor = GetRandomColor(),
                    HorizontalOptions = LayoutOptions.Start,
                    CornerRadius = 10
                };

                BarChartLayout.Children.Add(label);
                BarChartLayout.Children.Add(bar);
            }
        }

        private Color GetRandomColor()
        {
            Random rand = new Random();
            return Color.FromRgb(rand.Next(100, 256), rand.Next(100, 256), rand.Next(100, 256));
        }
    }
}
