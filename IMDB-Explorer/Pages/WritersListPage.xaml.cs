using IMDB_Explorer.Data;
using Microsoft.EntityFrameworkCore;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IMDB_Explorer.Pages
{
    /// <summary>
    /// Interaction logic for WritersListPage.xaml
    /// </summary>
    public partial class WritersListPage : Page
    {

        // Create a dbContext to use to access the database
        private readonly ImdbContext _context = new ImdbContext();

        // Create a CollectionViewSource to hold the list of Names
        private CollectionViewSource writerViewSource = new CollectionViewSource();
        
        public WritersListPage()
        {
            // This will load all the Names
            InitializeComponent();


            // Use the dbContext to tell EF to load the data we want to use on this page
            LoadInitialData();

        }

        private void LoadInitialData()
        {
            //Linq
            writerViewSource.Source = _context.Principals 
                .Where(p => p.JobCategory == "writer")
                .Join(_context.Names,
                      principal => principal.NameId,
                      name => name.NameId,
                      (principal, name) => new
                      {
                          ID = name.NameId,
                          Name = name.PrimaryName,
                          BirthYear = name.BirthYear,
                          DeathYear = name.DeathYear
                      })
                .Distinct()
                .OrderBy(x => x.Name)
                .ToList();

            //Bind the initial list of Names to the listbox
            listWriterSearchResults.ItemsSource = writerViewSource.View;
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            //Linq
            //Defining our LINQ Query 

            var query = _context.Principals
            .Where(p => p.JobCategory == "writer")
            .Join(_context.Names,
              principal => principal.NameId,
              name => name.NameId,
            (principal, name) => new
            {
              ID = name.NameId,
              Name = name.PrimaryName,
              BirthYear = name.BirthYear,
              DeathYear = name.DeathYear

            })
            .Where(n => n.Name.ToLower().Contains(textSearch.Text.ToLower()))
            .Distinct()
            .OrderBy(n => n.Name)
            .ToList();

            //Update the listbox with the results of the query
            listWriterSearchResults.ItemsSource = query;

        }
    }
}
