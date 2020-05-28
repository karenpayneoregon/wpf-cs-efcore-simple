using System;
using System.Collections.Generic;
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
using System.Collections.ObjectModel;
using System.ComponentModel;
using Microsoft.EntityFrameworkCore;
using WpfApp1.Contexts;
using WpfApp1.Models;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        bool _hasShown;

        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);

            if (_hasShown)
            {
                return;
            }

            _hasShown = true;

            using (var context = new HRContext())
            {
                var employeeCollection = new ObservableCollection<Employees>(context.Employees.AsQueryable());
                
                EmployeeGrid.ItemsSource = employeeCollection;
            }

        }
        private void ViewCurrentEmployee(object sender, RoutedEventArgs e)
        {
            var currentEmployee = (Employees) (sender as Button)?.DataContext;
            MessageBox.Show(
                $"{currentEmployee.EmployeeId}: " + 
                           $"{currentEmployee.FirstName} " + 
                           $"{currentEmployee.LastName}", "Current Employee", 
                MessageBoxButton.OK, 
                MessageBoxImage.Information);

        }

        /// <summary>
        /// Perform iterative case insensitive search on employee last name
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LastNameSearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var tb = (TextBox)sender;
            var lastNameFilter = tb.Text;
            var cvs = CollectionViewSource.GetDefaultView(EmployeeGrid.ItemsSource);

            if (string.IsNullOrWhiteSpace(lastNameFilter))
            {
                cvs.Filter = null;
            }
            else
            {
                cvs.Filter = item => 
                    item is Employees employees && 
                   (employees.LastName.StartsWith(lastNameFilter, 
                       StringComparison.InvariantCultureIgnoreCase));
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
