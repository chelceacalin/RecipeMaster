using RecipeMaster.Model;

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

                // Create a BoxView for each "slice" of the pie chart
                var pieSlice = new BoxView
                {
                    WidthRequest = percentage * 300,  // Simulate width based on percentage
                    HeightRequest = 50,
                    BackgroundColor = GetRandomColor(),
                    HorizontalOptions = LayoutOptions.FillAndExpand
                };

                // Add a label for each slice
                var label = new Label
                {
                    Text = $"{entry.Key}: {percentage:P1}",
                    FontSize = 12,
                    HorizontalOptions = LayoutOptions.Center,
                    TextColor = Color.FromRgb(255, 255, 255)
                };

                PieChartLayout.Children.Add(pieSlice);
                PieChartLayout.Children.Add(label);
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
                    TextColor = Color.FromRgb(255, 255, 255)
                };

                var bar = new BoxView
                {
                    HeightRequest = entry.Value * 10,  // Adjust height based on value
                    WidthRequest = 200,
                    BackgroundColor = GetRandomColor(),
                    HorizontalOptions = LayoutOptions.Start
                };

                BarChartLayout.Children.Add(label);
                BarChartLayout.Children.Add(bar);
            }
        }

        private Color GetRandomColor()
        {
            Random rand = new Random();
            return Color.FromRgb(rand.Next(256), rand.Next(256), rand.Next(256));
        }
    }
}
