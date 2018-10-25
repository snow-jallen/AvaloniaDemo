using System;
using System.ComponentModel;

namespace Demo
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private string theirName;
        public string TheirName
        {
            get=>theirName;
            set
            {
                theirName=value;
                OnPropertyChanged(nameof(TheirName));
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

        private SimpleCommand buttonCommand;
        public SimpleCommand ButtonCommand => buttonCommand ?? (buttonCommand = new SimpleCommand(()=>
        {
            Output = $"You typed {TheirName}, thanks!";
            TheirName=String.Empty;
        }));

        private void OnPropertyChanged(string propertyName)=>PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        public event PropertyChangedEventHandler PropertyChanged;
    }
}