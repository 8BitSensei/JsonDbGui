using DialogHostAvalonia;
using JsonDbGui.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;

namespace JsonDbGui.ViewModels
{
    public abstract class NavigationViewModelBase : ViewModelBase
    {
        private bool _isPaneOpen;
        private bool _subPage;
        private bool _isDialogOpen;
        private NavigationItem? _currentMenuSelection;

        private ViewModelBase? _currentPage;
        private List<NavigationItem>? _navigationHistory = new();

        public ReactiveCommand<Unit, Unit> OpenPane { get; }
        public ReactiveCommand<Unit, Unit> RevertPage { get; }
        public ObservableCollection<NavigationItem> Items { get; } = new();

        public NavigationViewModelBase()
        {
            OpenPane = ReactiveCommand.Create(OpenPaneImpl);
            RevertPage = ReactiveCommand.Create(RevertPageImpl);
            this.WhenAnyValue(x => x.SelectedItem)
                .Subscribe(x =>
                {
                    SetCurrentPage(x);
                    AddPageHistory(x);
                });
        }

        public async Task ShowDialog(object content) => await DialogHost.Show(content);

        public void OpenPaneImpl() => IsPaneOpen = !_isPaneOpen;

        public void AddPageHistory(NavigationItem navItem) 
        {
            if(navItem is not null)
                _navigationHistory?.Add(navItem);
        }

        public void AddListItem(NavigationItem navItem) 
        { 
            if(navItem is not null)
                Items?.Add(navItem); 
        }

        public void RevertPageImpl()
        {
            if (_navigationHistory?.Count > 1)
            {
                SetCurrentPage(_navigationHistory.ElementAt(_navigationHistory.Count - 2));
                _navigationHistory.Remove(_navigationHistory.ElementAt(_navigationHistory.Count - 1));
            }
        }

        public bool SetCurrentPage(NavigationItem navItem)
        {
            if (navItem is not null)
            {
                var instance = Activator.CreateInstance(navItem.ModelType, this);
                if (instance is not null)
                {
                    IsPaneOpen = false;
                    CurrentPage = (ViewModelBase)instance;
                    if (!Items.Contains(navItem))
                        SubPage = true;
                    else
                        SubPage = false;

                    return true;
                }
            }

            return false;
        }

        public bool IsPaneOpen
        {
            get => _isPaneOpen;
            set => this.RaiseAndSetIfChanged(ref _isPaneOpen, value);
        }

        public bool SubPage
        {
            get => _subPage;
            set => this.RaiseAndSetIfChanged(ref _subPage, value);
        }

        public bool IsDialogOpen
        {
            get => _isDialogOpen;
            set => this.RaiseAndSetIfChanged(ref _isDialogOpen, value);
        }

        public ViewModelBase? CurrentPage
        {
            get => _currentPage;
            set => this.RaiseAndSetIfChanged(ref _currentPage, value);
        }

        public NavigationItem SelectedItem
        {
            get => _currentMenuSelection!;
            set => this.RaiseAndSetIfChanged(ref _currentMenuSelection, value);
        }
    }
}
