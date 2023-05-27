using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
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
using windowsApp.Models;
using windowsApplication.Data;

namespace windowsApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<User> users = new List<User>();
        List<AppFile> appFiles = new List<AppFile>();
        private FileSystemWatcher watcher = new FileSystemWatcher();
        public MainWindow()
        {
            InitializeComponent();
            // Filepath concerned
            watcher.Path = "C:\\Users\\User\\Desktop\\pulsenics\\windowsApplication\\windowsApplication\\testDirectory";
            // Assumption: We only want to be notified of changes to file names, directory names, and when files are written to.
            watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;

            // Add event handlers.
            watcher.Created += OnChanged;
            watcher.Changed += OnChanged;
            watcher.Deleted += OnChanged;
            watcher.Renamed += OnChanged;

            // Start monitoring.
            watcher.EnableRaisingEvents = true;


            // Filepath concerned
            // var filePath = "C:\\Users\\User\\Desktop\\pulsenics\\windowsApplication\\windowsApplication\\testDirectory";

            // Task B: Save filename and details into SQL database
            // SaveFilesDetails(filePath);
        }
        void OnChanged(object sender, FileSystemEventArgs e)
        {
            // Task G: If a file is changed, the application should know the details (name, extension, created date, last modified date) and update the modified date.
            FileInfo fileInfo = new FileInfo(e.FullPath);
            MessageBox.Show($"File: {e.FullPath} {e.ChangeType} " + 
                $"Name: {fileInfo.Name} " +
                $"Extension: {fileInfo.Extension} " +
                $"Created: {fileInfo.CreationTime} " +
                $"Last Modified: {fileInfo.LastWriteTime} ");
        }
            private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void BtnGetUsers_Click(object sender, RoutedEventArgs e)
        {
            GetUsers();
        }
       
        void GetUsers()
        {
            var db = new windowsAppContext();
            users = db.Users.ToList();
            dgUsers.ItemsSource = users;
            LblUsers.Content = $"User(s): {users.Count()}";
        }
        // Task A: List all the files inside the folder based on the AppFiles SQL database
        private void BtnGetAppFiles_Click(object sender, RoutedEventArgs e)
        {
            GetAppFiles();
        }
        void GetAppFiles()
        {
            var db = new windowsAppContext();
            appFiles = db.AppFiles.ToList();

            // Task C: Search files by filename from the filename textbox input

            if (string.IsNullOrEmpty(TbxFileName.Text))
            {
                var filtered = appFiles.Where(file => file.FileName.ToLower().Contains(TbxFileName.Text.ToLower()));
                dgAppFiles.ItemsSource = filtered;
                LblAppFiles.Content = $"File(s): {filtered.Count()}";
            }
            else
            {
                dgAppFiles.ItemsSource = appFiles;
                LblAppFiles.Content = $"File(s): {appFiles.Count()}";
            }
            
        }
        void SaveFilesDetails(string path)
        {
            var context = new windowsAppContext();
            DirectoryInfo directory = new DirectoryInfo(path);
            foreach (var file in directory.GetFiles())
            {
                AppFile directoryFile = new AppFile()
                {
                    FileName = file.Name,
                    FileExtension = file.Extension,
                    FileCreatedTime = file.CreationTime,
                    FileLastModifiedTime = file.LastWriteTime
                };
                context.AppFiles.Add(directoryFile);
            }
            context.SaveChanges();
            MessageBox.Show("Saved changes to SQL database.");
        }
    }
}