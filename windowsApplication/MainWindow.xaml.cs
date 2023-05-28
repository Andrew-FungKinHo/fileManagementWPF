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
        }
        void OnChanged(object sender, FileSystemEventArgs e)
        {
            // Task F: When a file is changed outside the application, we need to automatically update the database information.
            // Show that a file has been changed, created, or deleted.
            // Example: "File: file\path\to\file Created"
            var context = new windowsAppContext();
            FileInfo fileInfo = new FileInfo(e.FullPath);
            MessageBox.Show($"File: {e.FullPath} {e.ChangeType} ");
            switch (e.ChangeType)
            {
                case WatcherChangeTypes.Changed:
                    // update database entry for this recenetly updated file. 
                    var changedFileConcerned = context.AppFiles
                                    .Where(p => p.FileCreatedTime == fileInfo.CreationTime)
                                    .FirstOrDefault();

                    if (changedFileConcerned is AppFile)
                    {
                        changedFileConcerned.FileName = fileInfo.Name;
                        changedFileConcerned.FileExtension = fileInfo.Extension;
                        changedFileConcerned.FileCreatedTime = fileInfo.CreationTime;
                        changedFileConcerned.FileLastModifiedTime = fileInfo.LastWriteTime;
                    }
                    context.SaveChanges();
                    break;
                case WatcherChangeTypes.Created:
                    // create a new database entry for this newly created file.
                    AppFile newFile = new AppFile()
                    {
                        FileName = fileInfo.Name,
                        FileExtension = fileInfo.Extension,
                        FileCreatedTime = fileInfo.CreationTime,
                        FileLastModifiedTime = fileInfo.LastWriteTime
                    };
                    context.AppFiles.Add(newFile);
                    context.SaveChanges();
                    break;
                case WatcherChangeTypes.Deleted:
                    // remove the database entry for this recently deleted file.
                    String deletedFileName = System.IO.Path.GetFileName(e.FullPath);
                    var deletedFileConcerned = context.AppFiles
                                    .Where(p => p.FileName == deletedFileName)
                                    .FirstOrDefault();
                    if (deletedFileConcerned is AppFile)
                    {
                        context.Remove(deletedFileConcerned);
                    }
                    context.SaveChanges();

                    break;
                case WatcherChangeTypes.Renamed:
                    // update database entry for this recenetly updated file. 
                    var renamedFileConcerned = context.AppFiles
                                    .Where(p => p.FileCreatedTime == fileInfo.CreationTime)
                                    .FirstOrDefault();

                    if (renamedFileConcerned is AppFile)
                    {
                        renamedFileConcerned.FileName = fileInfo.Name;
                        renamedFileConcerned.FileExtension = fileInfo.Extension;
                        renamedFileConcerned.FileCreatedTime = fileInfo.CreationTime;
                        renamedFileConcerned.FileLastModifiedTime = fileInfo.LastWriteTime;
                    }
                    context.SaveChanges();
                    break;
                default:
                    // Task G: If a file is changed, the application should know the details (name, extension, created date, last modified date) and update the modified date.
                    MessageBox.Show($"File: {e.FullPath} {e.ChangeType} " +
                    $"Name: {fileInfo.Name} " +
                    $"Extension: {fileInfo.Extension} " +
                    $"Created: {fileInfo.CreationTime} " +
                    $"Last Modified: {fileInfo.LastWriteTime} ");
                    break;
            }

        }
        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void BtnCreateUser_Click(object sender, RoutedEventArgs e)
        {
            var editUser = new EditUser();
            editUser.Owner = this;
            editUser.ShowEmptyUserForm();
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

        private void DgRowUsers_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var row = sender as DataGridRow;
            var user = row.DataContext as User;
            var editUser = new EditUser();
            editUser.Owner = this;
            editUser.ShowUser(user);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Task B: Save filename and details into SQL database
            // SaveFilesDetails("C:\\Users\\User\\Desktop\\pulsenics\\windowsApplication\\windowsApplication\\testDirectory");

            GetUsers();
            GetAppFiles();

        }

        private void DgRowAppFiles_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var row = sender as DataGridRow;
            var appfile = row.DataContext as AppFile;
            var editAppFile = new EditAppFile();
            editAppFile.Owner = this;
            editAppFile.ShowAppFile(appfile);
        }
    
    }
}