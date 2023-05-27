﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
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
        public MainWindow()
        {
            InitializeComponent();

            // Filepath concerned
            // var filePath = "C:\\Users\\User\\Desktop\\pulsenics\\windowsApplication\\windowsApplication\\testDirectory";

            // Task B: Save filename and details into SQL database
            // SaveFilesDetails(filePath);
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
            dgAppFiles.ItemsSource = appFiles;
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