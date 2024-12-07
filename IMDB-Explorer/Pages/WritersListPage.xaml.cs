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
        

        private ImdbContext _context = new ImdbContext();
        CollectionViewSource writerViewSource = new CollectionViewSource();
        
        public WritersListPage()
        {
            InitializeComponent();

            //Tie the markup viewsource object to the C# code viewsource object
            writerViewSource = (CollectionViewSource)FindResource(nameof(writerViewSource));


            LoadInitialData();


            //Use the dbContext to tell EF to load data from a junction between Principal and Name tables for our Writers data
            //_context.Names.Load();
            // Bind the data to the grid
            //writerViewSource.Source = writers;

            //Bind the initial list of Names to the listbox
            //listWriterSearchResults.ItemsSource = writerViewSource;
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
                          Name = name.PrimaryName,
                          BirthYear = name.BirthYear,
                          DeathYear = name.DeathYear
                      })
                .Distinct()
                .OrderBy(x => x.Name)
                .ToList();
        }

        private void textSearch_TextChanged(object sender, TextChangedEventArgs e)
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
