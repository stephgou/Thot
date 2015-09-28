using Doc.Search.App.Extensions;
using Doc.Search.App.Models;
using Microsoft.Azure.AppService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Popups;

namespace Doc.Search.App.ViewModels
{
    class SearchViewModel : BaseViewModel
    {
        private DocSearchAPI _docSearchAPI;

        public Predicate<object> CanExecuteDelegate { get; set; }

        public ObservableCollection<DocumentProperties> Documents { get; private set; }
       
        public SearchViewModel()
        {
            Documents = new ObservableCollection<DocumentProperties>();
            _docSearchAPI = App.ViewModel.DocSearchAPI;
        }

        private string _searchText = "";
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                this.SearchCommand.RaiseCanExecuteChanged();
            }
        }

        private RelayCommand _searchCommand;
        public RelayCommand SearchCommand
        {
            get
            {
                if (_searchCommand == null)
                    _searchCommand = new RelayCommand(  
                           () =>
                           {
                               LaunchSearch();
                           },
                        () => (SearchText.Length != 0)
                    );
                return _searchCommand;
            }
        }

        public async void LaunchSearch()
        {
            try
            {
                // To avoid "error message - the potentially dangerous Request.Path value was detected from the client (*)"
                var length = SearchText.Length - 1;
                if (SearchText == "*")
                    SearchText = "#@!";
                if (SearchText.LastIndexOf('*') == length)
                    SearchText = SearchText.Substring(0,length);

                var response = await _docSearchAPI.Search.SearchbyNameAsync(SearchText);
                while (Documents.Count() != 0)
                    Documents.Remove(Documents[0]);
                foreach (var doc in response)
                {
                    Documents.Add(new DocumentProperties(async document =>
                    {
                        var updateResult = await _docSearchAPI.Search.Update(document.ID, 
                            (int) document.DownloadsCounter);
                        await Task.Delay(1000);
                       
                        LaunchSearch();
                    })
                    {
                        BlobType = doc.BlobType,
                        Container = doc.Container,
                        ContentType = doc.ContentType,
                        LastModified = doc.LastModified,
                        Name = doc.Name,
                        Size = doc.Size,
                        ID = doc.ID,
                        Url = doc.Url,
                        DownloadsCounter = doc.DownloadsCounter
                    });
                }
            }
            catch (Exception e)
            {
                var dialog = new MessageDialog(e.Message);

                dialog.Commands.Add(new UICommand("OK"));
                await dialog.ShowAsync();
            }
        }
    }
}
