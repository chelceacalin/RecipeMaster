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

        private bool _isFavorite;
        public bool IsFavorite
        {
            get => _isFavorite;
            set
            {
                if (_isFavorite != value)
                {
                    _isFavorite = value;
                    OnPropertyChanged(nameof(IsFavorite));
                }
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
