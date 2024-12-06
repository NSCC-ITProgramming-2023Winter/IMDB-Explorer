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

            LoadInitialData(); // Only load initial data for better performance

            videosViewSource.Source = _context.Videos.Local.ToObservableCollection();
        }

        private void LoadInitialData()
        {
            // Load 100 rows of video data along with related genres
            videosViewSource.Source = _context.Videos
                .Include(v => v.Genre) // Explicitly include related Genre data
                .OrderBy(v => v.Title)
                .Take(100)
                .ToList();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            // Perform search and group results by genre
            var query =
                from video in _context.Videos
                where video.Title.Contains(textSearch.Text)
                group video by video.Genre into videoGroup
                select new
                {
                    Genre = videoGroup.Key,
                    VideoCount = videoGroup.Count(),
                    Videos = videoGroup.ToList()
                };

            // Update the ListView's ItemsSource with the query results
            videoListView.ItemsSource = query.ToList();
        }
    }
}
