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
            _context.Names.Load();
            //_context.Artists.Load();
            //_context.Albums.Load();
            //_context.Tracks.Load();

            videosCatalogViewSource.Source = _context.Names.Local.ToObservableCollection();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            //Define a grouping query to get grouped category data
            var query =
                from name in _context.Names
                where name.PrimaryName.Contains(textSearch.Text)
                select new
                {
                    name.NameId,
                    name.PrimaryName,
                    name.BirthYear,
                    name.DeathYear
                };

            //Execture the query against the db and assign it as the data source for the listview
            videosCatalogListVew.ItemsSource = query.ToList();
        }
    }
}
