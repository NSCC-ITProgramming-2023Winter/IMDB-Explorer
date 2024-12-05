using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Collections.ObjectModel;
using IMDB_Explorer.Data;

namespace IMDBApp.Pages
{
    /// <summary>
    /// Interaction logic for VideoListPage.xaml
    /// </summary>
    public partial class VideoListPage : Page
    {
        IMDBContext context = new IMDBContext();
        CollectionViewSource videoViewSource = new CollectionViewSource();

        public VideoListPage()
        {
            InitializeComponent();

            // Tie the markup xaml viewsource object to the code behind viewsource object(C#)
            videoViewSource = (CollectionViewSource)FindResource(nameof(videoViewSource));

            // Use the dbContext to tell EF to load the data we want to use on this page
            context.Videos.Load();

            // Set the viewsource data source to use the Videos data collection (dbset)
            videoViewSource.Source = context.Videos.Local.ToObservableCollection();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string searchText = textSearch.Text.Trim();

            // USE LINQ
            // Define the query to search video titles based on the search text
            var query = from video in context.Videos
                        where video.Title.Contains(searchText)
                        orderby video.ReleaseYear descending
                        select new
                        {
                            video.Title,
                            video.ReleaseYear,
                            video.Genre
                        };

            // Execute the query
            videoViewSource.Source = query.ToList();
        }
    }
}
