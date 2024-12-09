using IMDB_Explorer.Data;
using IMDB_Explorer.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace IMDB_Explorer.Pages
{
    /// <summary>
    /// Interaction logic for VideoListPage.xaml
    /// </summary>
    public partial class VideoListPage : Page
    {
        private readonly ImdbContext _context = new ImdbContext();
        private CollectionViewSource videosViewSource = new CollectionViewSource();

        public VideoListPage()
        {
            InitializeComponent();
            videosViewSource = (CollectionViewSource)FindResource(nameof(videosViewSource));

            // Use the dbContext to tell EF to load the data we want to use on this page
            _context.Titles.Load();

            // Set the viewsource data source to use the Products data collection (dbset)
            videosViewSource.Source = _context.Titles.Local.ToObservableCollection();

        }


        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string searchText = textSearch.Text.Trim();

            // USE LINQ
            // Define the query
            var query = from title in _context.Titles
                        where title.PrimaryTitle.Contains(searchText)
                        orderby title.TitleId
                        select title;


            // Execute the query
            videosViewSource.Source = query.ToList();
        }
    }
}