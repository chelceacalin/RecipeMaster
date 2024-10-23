using SQLite;
using System.ComponentModel;

namespace RecipeMaster.Model
{
    public class Recipe : INotifyPropertyChanged
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        private string _title;
        public string Title
        {
            get => _title;
            set
            {
                if (_title != value)
                {
                    _title = value;
                    OnPropertyChanged(nameof(Title));
                }
            }
        }

        private string _description;
        public string Description
        {
            get => _description;
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged(nameof(Description));
                }
            }
        }

        private string _category;
        public string Category
        {
            get => _category;
            set
            {
                if (_category != value)
                {
                    _category = value;
                    OnPropertyChanged(nameof(Category));
                }
            }
        }

        private string _area;
        public string Area
        {
            get => _area;
            set
            {
                if (_area != value)
                {
                    _area = value;
                    OnPropertyChanged(nameof(Area));
                }
            }
        }

        private string _mealThumb;
        public string MealThumb
        {
            get => _mealThumb;
            set
            {
                if (_mealThumb != value)
                {
                    _mealThumb = value;
                    OnPropertyChanged(nameof(MealThumb));
                }
            }
        }

        private string _tags;
        public string Tags
        {
            get => _tags;
            set
            {
                if (_tags != value)
                {
                    _tags = value;
                    OnPropertyChanged(nameof(Tags));
                }
            }
        }

        private string _youtubeLink;
        public string YoutubeLink
        {
            get => _youtubeLink;
            set
            {
                if (_youtubeLink != value)
                {
                    _youtubeLink = value;
                    OnPropertyChanged(nameof(YoutubeLink));
                }
            }
        }

        private string _source;
        public string Source
        {
            get => _source;
            set
            {
                if (_source != value)
                {
                    _source = value;
                    OnPropertyChanged(nameof(Source));
                }
            }
        }

        // Implementarea INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    public class Ingredients : INotifyPropertyChanged
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        //ingredient name
        private string _ingredientName;
        public string IngredientName
        {
            get => _ingredientName;
            set
            {
                if(_ingredientName != value)
                {
                    _ingredientName = value;
                    OnPropertyChanged(nameof(_ingredientName));
                }
            }
        }

        // ingredient quantity
        private string _quantity;
        public string Quantity
        {
            get => Quantity;
            set
            {
                if (_quantity != value)
                {
                    _quantity = value;
                    OnPropertyChanged(nameof(Quantity));
                }


            }
        }

        //unit of measurement
        private string _unit;
        public string Unit
        {
            get => _unit;
            set
            {
                if (Unit != value)
                {
                    _unit = value;
                    OnPropertyChanged(nameof(Unit));
                }
            }
        }
        //Recipe ID which is foreign key to recipe table

        private int _recipeId;
        public int RecipeId
        {
            get => _recipeId;
            set
            {
                if(RecipeId != value)
                {
                    _recipeId = value;
                    OnPropertyChanged(nameof(RecipeId));
                }
            }
        }

        // Implementarea INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

            protected void OnPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        
    }

}
