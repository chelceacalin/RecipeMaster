using SQLite;
using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using RecipeMaster.Model;

namespace RecipeMaster
{
    class DatabaseManager
    {
        private static DatabaseManager? _instance;
        private static readonly object _lock = new object();
        private readonly SQLiteConnection _connection;
        public string DbPath { get; private set; }

        public static DatabaseManager Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new DatabaseManager();
                    }
                    return _instance;
                }
            }
        }

        private DatabaseManager()
        {
            DbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "appdatabase.db");
            _connection = new SQLiteConnection(DbPath);
            Console.WriteLine($"Database path: {DbPath}");

            CreateTable<Recipe>(); // Tabela pentru rețete
            CreateTable<Favorite>(); // Tabela pentru favorite
        }

        public void CreateTable<T>() where T : new()
        {
            try
            {
                _connection.CreateTable<T>();
                Debug.WriteLine($"Table {typeof(T).Name} created successfully.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error creating table: {ex.Message}");
            }

        }

        public T GetRecordById<T>(int id) where T : new()
        {
            try
            {
                var record = _connection.Find<T>(id);
                Debug.WriteLine(record != null
                    ? $"Successfully retrieved record with ID {id} from {typeof(T).Name}."
                    : $"Record with ID {id} not found in {typeof(T).Name}.");
                return record;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error retrieving record by ID from {typeof(T).Name}: {ex.Message}");
                return default;
            }
        }
        public void InsertRecord<T>(T record)
        {
            try
            {
                if (record is Recipe recipe)
                {
                    recipe.MealThumb = CleanUriPrefix(recipe.MealThumb);

                    var existingRecipe = _connection.Table<Recipe>().FirstOrDefault(r => r.Title == recipe.Title && r.MealThumb == recipe.MealThumb);
                    if (existingRecipe != null)
                    {
                        Debug.WriteLine($"Recipe with title '{recipe.Title}' already exists. Skipping insert.");
                        return;
                    }
                }

                _connection.Insert(record);
                Debug.WriteLine($"Successfully inserted record into {typeof(T).Name}.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error inserting record into {typeof(T).Name}: {ex.Message}");
            }
        }

        public List<T> GetAllRecords<T>() where T : new()
        {
            try
            {
                var records = _connection.Table<T>().ToList();

                if (typeof(T) == typeof(Recipe))
                {
                    foreach (var recipe in records.Cast<Recipe>())
                    {
                        recipe.MealThumb = CleanUriPrefix(recipe.MealThumb);
                    }
                }

                Debug.WriteLine($"Successfully retrieved {records.Count} records from {typeof(T).Name}.");
                return records;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error retrieving records from {typeof(T).Name}: {ex.Message}");
                return new List<T>();
            }
        }

        private string CleanUriPrefix(string uri)
        {
            return string.IsNullOrEmpty(uri) ? uri : uri.Replace("Uri: ", "");
        }

        public void UpdateRecord<T>(T record)
        {
            try
            {
                _connection.Update(record);
                Debug.WriteLine($"Successfully updated record in {typeof(T).Name}.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error updating record in {typeof(T).Name}: {ex.Message}");
            }
        }

        public void DeleteRecord<T>(T record)
        {
            try
            {
                _connection.Delete(record);
                Debug.WriteLine($"Successfully deleted record from {typeof(T).Name}.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error deleting record from {typeof(T).Name}: {ex.Message}");
            }
        }

        public List<T> Query<T>(string query, params object[] args) where T : new()
        {
            try
            {
                var records = _connection.Query<T>(query, args);
                Debug.WriteLine($"Successfully executed query on {typeof(T).Name}.");
                return records;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error executing query on {typeof(T).Name}: {ex.Message}");
                return new List<T>(); 
            }
        }

        public void AddToFavorites(int recipeId)
        {
            try
            {
                
                var existingFavorite = _connection.Table<Favorite>().FirstOrDefault(f => f.RecipeId == recipeId);
                if (existingFavorite == null)
                {
                    var favorite = new Favorite { RecipeId = recipeId };
                    _connection.Insert(favorite);
                    Debug.WriteLine($"Successfully added recipe ID {recipeId} to favorites.");
                }
                else
                {
                    Debug.WriteLine($"Recipe ID {recipeId} is already in favorites.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error adding recipe ID {recipeId} to favorites: {ex.Message}");
            }
        }

        public bool RemoveFromFavorites(int recipeId)
        {
            try
            {
                if (_connection == null)
                {
                    Debug.WriteLine("Database connection is null. Cannot remove favorite.");
                    return false;
                }

                var favorite = _connection.Table<Favorite>().FirstOrDefault(f => f.RecipeId == recipeId);

                if (favorite != null)
                {
                    _connection.Delete(favorite);
                    Debug.WriteLine($"Successfully removed recipe ID {recipeId} from favorites.");
                    return true;
                }
                else
                {
                    Debug.WriteLine($"Recipe ID {recipeId} is not in favorites.");
                    return false;
                }
            }
            catch (SQLiteException sqlEx)
            {
                Debug.WriteLine($"SQLite error while removing recipe ID {recipeId} from favorites: {sqlEx.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unexpected error while removing recipe ID {recipeId} from favorites: {ex.Message}");
                return false;
            }
        }


        public List<Recipe> GetFavoriteRecipes()
        {
            try
            {
                
                var favoriteRecipeIds = _connection.Table<Favorite>().Select(f => f.RecipeId).ToList();

                
                var favoriteRecipes = _connection.Table<Recipe>().Where(r => favoriteRecipeIds.Contains(r.Id)).ToList();
                Debug.WriteLine($"Successfully retrieved {favoriteRecipes.Count} favorite recipes.");
                return favoriteRecipes;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error retrieving favorite recipes: {ex.Message}");
                return new List<Recipe>();
            }
        }

        /*
         * Exemple:
         * Creare tabel: DatabaseManager.Instance.CreateTable<Recipe>();

         * Inserarea unui nou Recipe
        var recipe = new Recipe
        {
            Title = "Pancakes",
            Category = "Dessert",
            Area = "American",
            Description = "Delicious pancakes with syrup",
            Tags = "breakfast, sweet",
            MealThumb = "some_url",
            Source = "recipe_source",
            YoutubeLink = "youtube_link"
        };

         DatabaseManager.Instance.InsertRecord(recipe);

        * Inserarea unui alt model (de exemplu, `AnotherModel`)
            var anotherModelInstance = new AnotherModel
            DatabaseManager.Instance.InsertRecord(anotherModelInstance);

        * Citirea tuturor retetelor
        var allRecipes = DatabaseManager.Instance.GetAllRecords<Recipe>();
        * Citirea tuturor din AnotherModel
        var allOtherRecords = DatabaseManager.Instance.GetAllRecords<AnotherModel>();
         */
    }
}
