using System;
using System.Collections;
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
        private readonly HRContext Context;
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
            DataContext = employeeCollection;

            /*
             * Find employee by last name, if found
             * select and scroll into view in the DataGrid
             */
            var employee = (employeeCollection)
                .FirstOrDefault(emp => emp.LastName == "Russell");

            if (employee == null) return;

            EmployeeGrid.SelectedItem = employee;
            EmployeeGrid.ScrollIntoView(employee);

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
            Console.WriteLine(e.Action);
            if (e.Action != NotifyCollectionChangedAction.Remove) return;

            var employee = (Employees)e.OldItems[0];
            InformationDialog($"Index: {e.OldStartingIndex} - {employee.FirstName} {employee.LastName}", "Just removed");

        }
        /// <summary>
        /// Show some details on the currently selected employee when clicking
        /// the view button in the DataGrid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewCurrentEmployee(object sender, RoutedEventArgs e)
        {
            var employee = (Employees) (sender as Button)?.DataContext;

            var window = new DetailsWindow(employee) {Owner = this};
            window.ShowDialog();
            
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

            // nothing entered to search so remove an existing filter
            if (string.IsNullOrWhiteSpace(lastNameFilter))
            {
                cvs.Filter = null;
            }
            else
            {
                // do the filter
                cvs.Filter = item => 
                    item is Employees employees && (employees.LastName.StartsWith(lastNameFilter, 
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
                    entry.State == EntityState.Deleted || 
                    entry.State == EntityState.Modified || 
                    entry.State == EntityState.Added);

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
        /// <summary>
        /// Example for adding a new employee without going thru the hassle of
        /// rigging up user interface as there are plenty needed to collect
        /// required fields.
        ///
        /// Note no manager assigned, this means in the view button click we
        /// need to do a null check.
        /// </summary>
        private void AddHardCodedEmployee()
        {
            // create new employee
            var employee = new Employees()
            {
                FirstName = "Jim",
                LastName = "Lewis",
                Email = "jlewis@comcast.net",
                HireDate = new DateTime(2012, 3, 14),
                JobId = 4,
                Salary = 100000,
                DepartmentId = 9
            };

            // add and set state for change tracker
            Context.Entry(employee).State = EntityState.Added;

            // add employee to the grid
            ((ObservableCollection<Employees>)EmployeeGrid.ItemsSource).Add(employee);

        }
        /// <summary>
        /// Close this application
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

    }
}
