using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfApp1.Models;
using Path = System.IO.Path;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for DetailsWindow.xaml
    /// </summary>
    public partial class DetailsWindow : Window
    {
        private Employees _employee;

        public DetailsWindow()
        {
            InitializeComponent();
        }

        public DetailsWindow(Employees employee)
        {
            InitializeComponent();
            _employee = employee;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var manager = _employee?.Manager;

            if (_employee != null)
            {
                FirstNameBox.Text = _employee.FirstName;
                LastNameBox.Text = _employee.LastName;
            }

            ManagerNameBox.Text = $"{manager?.FirstName} {manager?.LastName}";

        }
    }
}
