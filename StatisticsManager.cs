using RecipeMaster.Model;

namespace RecipeMaster
{
    public class StatisticsManager
    {
        private List<Recipe> _recipes;

        public StatisticsManager(List<Recipe> recipes)
        {
            _recipes = recipes;
        }

        public Dictionary<string, int> GetRecipesByCategory()
        {
            return _recipes
                .GroupBy(r => r.Category ?? "Unknown")
                .ToDictionary(g => g.Key, g => g.Count());
        }

        public Dictionary<string, int> GetRecipesByArea()
        {
            return _recipes
                .GroupBy(r => r.Area ?? "Unknown")
                .ToDictionary(g => g.Key, g => g.Count());
        }

        public int GetRecipesWithTagsCount()
        {
            return _recipes.Count(r => !string.IsNullOrEmpty(r.Tags));
        }

        public int GetTotalRecipesCount()
        {
            return _recipes.Count;
        }
    }
}
