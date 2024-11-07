using SQLite;

namespace RecipeMaster.Model
{
    public class Favorite
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Indexed]
        public int RecipeId { get; set; }
    }
}