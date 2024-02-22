using JsonDb;
using JsonDbGui.Models;
using ReactiveUI;
using System.Reactive;
using System.Threading.Tasks;

namespace JsonDbGui.ViewModels
{
    public class AddCollectionPageViewModel : ViewModelBase
    {
        private const string CollectionsDbName = "LocalCollections"; //TODO: Move into AppSettings

        private MainWindowViewModel _main;
        private readonly IJsonDb _jsonDb;
        private IJsonCollection<Collection>? _jsonDbCollection;

        private string? _name;
        private string? _description;
        private int _type;

        public ReactiveCommand<Unit, Unit> Add { get; }
        public ReactiveCommand<Unit, Unit> Cancel { get; }

        public AddCollectionPageViewModel(MainWindowViewModel main)
        {
            _main = main;
            var jsonDb = App.GetService<IJsonDbFactory>();
            _jsonDb = jsonDb.GetJsonDb();

            Add = ReactiveCommand.CreateFromTask(AddImpl);
            Cancel = ReactiveCommand.Create(CancelImpl);
        }

        public async Task AddImpl() 
        {
            var newCollection = new Collection
            {
                CollectionName = _name!,
                CollectionDescription = _description!,
                CollectionType = CollectionType.Local //TODO: Need to get binding on the ComboBox working
            };

            _jsonDbCollection ??= await _jsonDb.GetCollectionAsync<Collection>(CollectionsDbName);

            _jsonDbCollection!.Add(newCollection);
            await _jsonDbCollection.WriteAsync();
            _main.RevertPageImpl();
        }

        public void CancelImpl()
        {
            _main.RevertPageImpl();
        }

        public string? Name
        {
            get => _name;
            set => this.RaiseAndSetIfChanged(ref _name, value);
        }

        public string? Description
        {
            get => _description;
            set => this.RaiseAndSetIfChanged(ref _description, value);
        }

        public int Type
        {
            get => _type;
            set => this.RaiseAndSetIfChanged(ref _type, value);
        }
    }
}
