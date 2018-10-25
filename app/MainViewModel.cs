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
        () => data.FileExists(GedcomPath), //can execute
        ()=> //execute
        {
            foreach (var p in data.GetPeopleFromGedcom(GedcomPath))
                People.Add(p);
            Output = $"We found {People.Count} people in {GedcomPath}!";
        }));

        private SimpleCommand findFile;
        public SimpleCommand FindFile => findFile ?? (findFile = new SimpleCommand(
            () => true,
            async () => GedcomPath = await data.FindFile()));


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