using IMDB_Explorer.Data;
using IMDB_Explorer.Models;
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
    /// Interaction logic for VideosCatalogPage.xaml
    /// </summary>
    public partial class VideosCatalogPage : Page
    {
        private readonly ImdbContext _context = new ImdbContext();
        private CollectionViewSource videosCatalogViewSource = new CollectionViewSource();
        public VideosCatalogPage()
        {
            InitializeComponent();
            videosCatalogViewSource = (CollectionViewSource)FindResource(nameof(videosCatalogViewSource));

            //Load data from the database
            //_context.Names.Load(); This will load all the data,  too slow
            //_context.Artists.Load();
            //_context.Albums.Load();
            //_context.Tracks.Load();

            LoadInitialData(); // use this function, only load 100 row data

            videosCatalogViewSource.Source = _context.Names.Local.ToObservableCollection();
        }

        private void LoadInitialData()
        {
            // Load only 100 pieces of data for the initial
            videosCatalogViewSource.Source = _context.Names
                .Include(n => n.Titles) // Explicit Loading of related data (Titles)
                .OrderBy(n => n.NameId)
                .Take(100) // Control the number of loads
                .ToList();
        }


        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            //Define a grouping query to get grouped category data
            var query =
                from name in _context.Names
                where name.PrimaryName.Contains(textSearch.Text)
                group name by name.PrimaryName.ToUpper().Substring(0, 1) into nameGroup
                select new
                {
                    Index = nameGroup.Key,
                    ArtCount = nameGroup.Count().ToString(),
                    name = nameGroup.ToList<Name>()
                };
            //select new
            //{
            //    name.NameId,
            //    name.PrimaryName,
            //    name.BirthYear,
            //    name.DeathYear
            //};

            //Execture the query against the db and assign it as the data source for the listview
            videosCatalogListVew.ItemsSource = query.ToList();
        }
    }
}
