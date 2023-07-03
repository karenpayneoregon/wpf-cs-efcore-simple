using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace FrameworkCanExecuteExample.Classes
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private string _connectionString;

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand ConfirmCommand { get; }
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


        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void Confirm(object parameter)
        {
            Debug.WriteLine($"Text for ConnectionString is '{ConnectionString}'");
            MessageBox.Show("xx");
        }

        private bool CanConfirm(object parameter) 
            => IsValidConnection();

        /// <summary>
        /// Here is just enough code to demonstrate doing starter validation which
        /// needs a good deal more to validate a connection string.
        /// </summary>
        /// <returns></returns>
        private bool IsValidConnection()
        {
            return !string.IsNullOrWhiteSpace(_connectionString) && _connectionString.Length > 5;
        }
    }
}