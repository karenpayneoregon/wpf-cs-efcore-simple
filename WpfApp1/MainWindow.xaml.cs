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
using System.Collections.Specialized;
using System.ComponentModel;
using Microsoft.EntityFrameworkCore;
using WpfApp1.Contexts;
using WpfApp1.Models;
using static WpfApp1.Classes.Dialogs;
using MessageBox = System.Windows.MessageBox;

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

                employeeCollection.CollectionChanged += EmployeeCollection_CollectionChanged;
            }

        }
        /// <summary>
        /// Example of gaining access to a deleted employee
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EmployeeCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems != null)
            {
                //Console.WriteLine(e.OldStartingIndex);
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
        /// <summary>
        /// Prompt to remove the current row in the DataGrid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Grid_PreviewCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            var grid = (DataGrid)sender;
            if (e.Command != DataGrid.DeleteCommand) return;

            if (grid.SelectedItem is Employees employee)
            {
                var employeeName = $"{employee.FirstName} {employee.LastName}";
                if (!Question($"Delete {employeeName}", "Confirm Delete"))
                {
                    e.Handled = true;
                }
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

    }
}
