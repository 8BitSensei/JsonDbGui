using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using JsonDbGui.Models;
using ReactiveUI;
using System.Reactive;
using System.Threading.Tasks;

namespace JsonDbGui.ViewModels
{
    public class CollectionCardViewModel : ViewModelBase
    {
        private const string LocalIcon = "folder_regular";
        private const string GithubIcon = "question_regular";
        private const string S3Icon = "question_regular";
        private const string UnknownIcon = "question_regular";

        private MainWindowViewModel _main;
        public string CollectionName { get; private set; }
        public string CollectionDescription { get; private set; }
        public StreamGeometry? CollectionIcon { get; private set; }

        public ReactiveCommand<Unit, Unit> OpenCollection { get; }
        public ReactiveCommand<Unit, Unit> RemoveCollection { get; }

        public CollectionCardViewModel(MainWindowViewModel main, Collection collectionData)
        {
            _main = main;
            CollectionName = collectionData.CollectionName;
            CollectionDescription = collectionData.CollectionDescription;

            switch (collectionData.CollectionType)
            {
                case (CollectionType.Local):
                    CollectionIcon = GetIconForName(LocalIcon);
                    break;
                case (CollectionType.Github):
                    CollectionIcon = GetIconForName(GithubIcon);
                    break;
                case (CollectionType.S3):
                    CollectionIcon = GetIconForName(S3Icon);
                    break;
                default:
                    CollectionIcon = GetIconForName(UnknownIcon);
                    break;
            }

            OpenCollection = ReactiveCommand.CreateFromTask(OpenCollectionImpl);
            RemoveCollection = ReactiveCommand.CreateFromTask(RemoveCollectionImpl);
        }

        private static StreamGeometry? GetIconForName(string iconKey)
        {
            Application.Current!.TryFindResource(iconKey, out var res);
            StreamGeometry? navigationItemIcon = null;
            if (res is not null)
                navigationItemIcon = (StreamGeometry)res;

            return navigationItemIcon;
        }

        public Task OpenCollectionImpl()
        {
            //_main.SetCurrentPage(new NavigationItem(typeof(SubPageViewModel), "settings_regular"));
            return Task.CompletedTask;
        }

        public Task RemoveCollectionImpl()
        {
            return Task.CompletedTask;
        }
    }
}
