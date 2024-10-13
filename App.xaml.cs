using RecipeMaster.Model;

namespace RecipeMaster
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Table Creations 
            DatabaseManager.Instance.CreateTable<Recipe>();

            MainPage = new NavigationPage(new RecipesPage());
        }
    }
}
