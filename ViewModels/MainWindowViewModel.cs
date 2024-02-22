using JsonDbGui.Models;

namespace JsonDbGui.ViewModels
{
    public class MainWindowViewModel : NavigationViewModelBase
    {
        private const string CollectionIcon = "home_regular";

        public MainWindowViewModel() : base()
        {
            var startPage = new NavigationItem(typeof(CollectionsPageViewModel), CollectionIcon);
            AddPageHistory(startPage);
            AddListItem(startPage);
            SetCurrentPage(startPage);
        }
    }
}
