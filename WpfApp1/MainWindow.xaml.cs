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
using WpfApp1.Classes;
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
        private HRContext Context;
        public MainWindow()
        {
            InitializeComponent();
            Context = new HRContext();
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
            var employeeCollection = new ObservableCollection<Employees>(Context.Employees.AsQueryable());

            EmployeeGrid.ItemsSource = employeeCollection;

            employeeCollection.CollectionChanged += EmployeeCollection_CollectionChanged;

        }
        /// <summary>
        /// Informational
        /// Example of gaining access to a deleted employee
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EmployeeCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.OldItems == null) return;
            if (e.Action != NotifyCollectionChangedAction.Remove) return;

            var employee = (Employees)e.OldItems[0];
            InformationDialog($"Index: {e.OldStartingIndex} - {employee.FirstName} {employee.LastName}", "Just removed");
        }

        private void ViewCurrentEmployee(object sender, RoutedEventArgs e)
        {
            var employee = (Employees) (sender as Button)?.DataContext;
            MessageBox.Show(
                $"{employee.EmployeeId}: " + 
                           $"{employee.FirstName} " + 
                           $"{employee.LastName}", "Current Employee", 
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
        /// <summary>
        /// Save changes by prompting
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveChangesButton_Click(object sender, RoutedEventArgs e)
        {
           
            try
            {
                // we only have to deal with deletes and modified items
                var affectedCount = Context.ChangeTracker.Entries().Count(entry => 
                    entry.State == EntityState.Deleted || entry.State == EntityState.Modified);

                if (affectedCount > 0)
                {
                    if (Question("Save changes?"))
                    {
                        Context.SaveChanges();
                    }
                }
                else
                {
                    InformationDialog("Nothing to save.");
                }
            }
            catch (Exception ex)
            {
                ExceptionDialog("Something went wrong", "Ooops", ex);
            }
        }
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

    }
}
