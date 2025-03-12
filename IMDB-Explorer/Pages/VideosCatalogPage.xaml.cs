using IMDB_Explorer.Data;
using IMDB_Explorer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace IMDB_Explorer.Pages
{
    /// <summary>
    /// Interaction logic for VideosCatalogPage.xaml
    /// </summary>
    public partial class VideosCatalogPage : Page
    {
        private readonly ImdbContext _context = new ImdbContext();

        public VideosCatalogPage()
        {
            InitializeComponent();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            // Get search input
            var searchText = textSearch.Text.Trim();

            // If the input is empty, display a prompt or clear the result
            if (string.IsNullOrEmpty(searchText))
            {
                videosCatalogListVew.ItemsSource = null;
                MessageBox.Show("Please enter a search term.", "No Input", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            // Push the query to the database and load the relevant data directly

            _context.Database.SetCommandTimeout(180); // Set timeout to 180 seconds
            var query = _context.Names
                .AsNoTracking()
                .Where(name => name.PrimaryName.Contains(searchText)) 
                .Include(name => name.Titles)  // Explicitly load Titles
                .GroupBy(name => name.PrimaryName.Substring(0, 1).ToUpper()) 
                .Select(group => new
                {
                    Index = group.Key,
                    ArtCount = group.Count(),
                    name = group.Select(n => new
                    {
                        n.NameId,
                        n.PrimaryName,
                        Titles = n.Titles.Select(t => new { t.PrimaryTitle }).ToList()
                    }).ToList()
                })
                .ToList(); // Query Execution

            // Checking Titles data
            //foreach (var item in query)
            //{
            //    foreach (var name in item.name)
            //    {
            //        Debug.WriteLine($"Name: {name.PrimaryName}, Titles Count: {name.Titles.Count}");
            //    }
            //}

            videosCatalogListVew.ItemsSource = query;
        }
    }
}