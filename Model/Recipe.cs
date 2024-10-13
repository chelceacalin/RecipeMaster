

using SQLite;

namespace RecipeMaster.Model
{
    public class Recipe
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string Area { get; set; } 
        public string MealThumb { get; set; }
        public string Tags { get; set; }
        public string YoutubeLink { get; set; } 
        public string Source { get; set; } 
    }
}
