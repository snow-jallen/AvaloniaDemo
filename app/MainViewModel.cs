using Data;
using Interfaces;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;

namespace Demo
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel() : this(new DefaultDataService())
        { }

        public MainViewModel(IDataService data)
        {
            this.data = data ?? throw new ArgumentNullException(nameof(data));
            People = new ObservableCollection<Person>();
        }

        private string gedcomPath;
        public string GedcomPath
        {
            get=>gedcomPath;
            set
            {
                gedcomPath=value;
                OnPropertyChanged(nameof(GedcomPath));
                LoadGedcom.RaiseCanExecuteChanged();
            }
        }

        private string output;
        public string Output
        {
            get=>output;
            set
            {
                output = value;
                OnPropertyChanged(nameof(Output));
            }
        }

        private readonly IDataService data;
        private SimpleCommand loadGedcom;
        public SimpleCommand LoadGedcom => loadGedcom ?? (loadGedcom = new SimpleCommand(
        () => !IsBusy && data.FileExists(GedcomPath), //can execute
        async ()=> //execute
        {
            Output = "Loading...";
            IsBusy = true;
            foreach (var p in await data.GetPeopleFromGedcomAsync(GedcomPath))
                People.Add(p);
            Output = $"We found {People.Count} people in {GedcomPath}!";
            IsBusy = false;
        }));

        private SimpleCommand findFile;
        public SimpleCommand FindFile => findFile ?? (findFile = new SimpleCommand(
            () => !IsBusy,
            async () =>
            {
                GedcomPath = await data.FindFileAsync();
                LoadGedcom.RaiseCanExecuteChanged();
            }));

        private bool isBusy;
        public bool IsBusy
        {
            get => isBusy;
            set
            {
                isBusy = value;
                OnPropertyChanged(nameof(IsBusy));
                LoadGedcom.RaiseCanExecuteChanged();
                FindFile.RaiseCanExecuteChanged();
            }
        }

        private void OnPropertyChanged(string propertyName)=>PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<Person> People { get; private set; }

        private Person selectedPerson;
        public Person SelectedPerson
        {
            get => selectedPerson;
            set
            {
                selectedPerson = value;
                OnPropertyChanged(nameof(SelectedPerson));
            }
        }

    }
}