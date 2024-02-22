using JsonDb;
using JsonDbGui.Models;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;

namespace JsonDbGui.ViewModels
{
    public class CollectionsPageViewModel : ViewModelBase
    {
        private const string CollectionsDbName = "LocalCollections"; //TODO: Move into AppSettings

        private readonly IJsonDb _jsonDb;
        
        private IJsonCollection<Models.Collection>? _jsonDbCollection;
        private MainWindowViewModel _main;
        private bool _isBusy;

        public ObservableCollection<CollectionCardViewModel> Collections { get; set; } = new();

        public ReactiveCommand<Unit, Unit> AddCollection { get; }

        public CollectionsPageViewModel(MainWindowViewModel main)
        {
            var jsonDb = App.GetService<IJsonDbFactory>();
            _jsonDb = jsonDb.GetJsonDb();
            _main = main;

            InitialiseCollections();

            AddCollection = ReactiveCommand.Create(AddCollectionImpl);
        }

        public async void InitialiseCollections() 
        {
            if(_jsonDbCollection is null)
                _jsonDbCollection = await _jsonDb.GetCollectionAsync<Models.Collection>(CollectionsDbName);

            for (var i = 0; i < _jsonDbCollection?.Count(); i++) 
            {
                var dbItem = _jsonDbCollection.ElementAtOrDefault(i);
                if (dbItem is not null)
                {
                    if (i > Collections.Count() - 1)
                        Collections.Add(new CollectionCardViewModel(_main, dbItem));
                    else
                        Collections[i] = new CollectionCardViewModel(_main, dbItem);
                }
                else 
                {
                    if(i <= Collections.Count() - 1)
                        Collections.RemoveAt(i);
                }
                    
            }

        }

        public void AddCollectionImpl()
        {
            _main.SetCurrentPage(new NavigationItem(typeof(AddCollectionPageViewModel)));
        }

        public bool IsBusy
        {
            get => _isBusy;
            set => this.RaiseAndSetIfChanged(ref _isBusy, value);
        }
    }
}
