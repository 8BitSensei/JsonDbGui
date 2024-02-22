using JsonDb;
using ReactiveUI;
using System.Reactive;
using System.Threading.Tasks;

namespace JsonDbGui.ViewModels
{
    public class RemoveDialogViewModel : ViewModelBase
    {
        private const string CollectionsDbName = "LocalCollections"; //TODO: Move into AppSettings

        private MainWindowViewModel _main;
        private readonly IJsonDb _jsonDb;
        private readonly string _collectionName;

        private IJsonCollection<Models.Collection>? _jsonDbCollection;

        public ReactiveCommand<Unit, Unit> Delete { get; }
        public ReactiveCommand<Unit, Unit> Cancel { get; }

        public RemoveDialogViewModel(MainWindowViewModel main, string collectionName) 
        {
            _main = main;
            var jsonDb = App.GetService<IJsonDbFactory>();
            _jsonDb = jsonDb.GetJsonDb();
            _collectionName = collectionName;

            Cancel = ReactiveCommand.Create(CancelImpl);
            Delete = ReactiveCommand.CreateFromTask(DeleteImpl);
        }

        public void CancelImpl() => _main.IsDialogOpen = false;

        public async Task DeleteImpl()
        {
            if (_jsonDbCollection is null)
                _jsonDbCollection = await _jsonDb.GetCollectionAsync<Models.Collection>(CollectionsDbName);

            _jsonDbCollection.Remove(x => x.CollectionName.Equals(_collectionName));
            await _jsonDbCollection.WriteAsync();
            _main.IsDialogOpen = false;
            _main.SetCurrentPage(new Models.NavigationItem(typeof(CollectionsPageViewModel), "home_regular")); //TODO: This is a hacky way to do it, but I think I'll need to change a lot to do it well
        }
        
    }
}
