using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace FrameworkCanExecuteExample.Classes
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private string _connectionString;

        public event PropertyChangedEventHandler PropertyChanged;

        public MainViewModel() => ConfirmCommand = new RelayCommand(Confirm, CanConfirm);

        public string ConnectionString
        {
            get => _connectionString;
            set
            {
                _connectionString = value;
                OnPropertyChanged();
            }
        }

        public ICommand ConfirmCommand { get; }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Confirm(object parameter)
        {
            Console.WriteLine();
        }

        private bool CanConfirm(object parameter)
        {
            return !string.IsNullOrWhiteSpace(_connectionString);
        }
    }
}