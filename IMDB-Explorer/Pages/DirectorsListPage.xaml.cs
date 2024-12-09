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
    /// Interaction logic for DirectorsListPage.xaml
    /// </summary>
    public partial class DirectorsListPage : Page
    {
        private readonly ImdbContext _context = new ImdbContext();
        private CollectionViewSource directorsViewSource = new CollectionViewSource();

        public DirectorsListPage()
        {
            InitializeComponent();

            /* 
             * This will load all the Names
            //directorsViewSource = (CollectionViewSource)FindResource(nameof(directorsViewSource));

            // Use the dbContext to tell EF to load the data we want to use on this page
            //_context.Names.Load();

            // Set the viewsource data source to use the Products data collection (dbset)
            //directorsViewSource.Source = _context.Names.Local.ToObservableCollection();
            */

            // Only Load directors
            LoadDirectors();
        }

        private void LoadDirectors()
        {
            var directors = _context.Names
                                    .Where(n => n.PrimaryProfession != null && n.PrimaryProfession.Contains("director"))
                                    .ToList();

            directorsListView.ItemsSource = directors;
        }
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            var searchQuery = textSearch.Text.ToLower();

            var directors = _context.Names
                                    .Where(n => n.PrimaryProfession != null && n.PrimaryProfession.Contains("director")
                                            && n.PrimaryName.ToLower().Contains(searchQuery))
                                    .ToList();

            directorsListView.ItemsSource = directors;
        }
    }
}
