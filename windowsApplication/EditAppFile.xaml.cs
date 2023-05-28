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
using System.Windows.Shapes;
using windowsApp.Models;
using windowsApplication.Data;

namespace windowsApplication
{
    /// <summary>
    /// Interaction logic for EditAppFile.xaml
    /// </summary>
    public partial class EditAppFile : Window
    {
        List<User> users = new List<User>();
        public EditAppFile()
        {
            InitializeComponent();
            var db = new windowsAppContext();
            users = db.Users.ToList();
            
            listbxUsersAssign.ItemsSource = users;
            listbxUsersAssign.DisplayMemberPath = "Name";
           
           
            
            
        }

        public AppFile AppFile { get; set; }

        public void ShowAppFile(AppFile appfile)
        {
            AppFile = appfile;
            TbxAppFileId.Text = $"{AppFile.Id}";
            TbxFileName.Text = $"{AppFile.FileName}";
            TbxFileExtension.Text = $"{AppFile.FileExtension}";
            TbxFileCreated.Text = $"{AppFile.FileCreatedTime}";
            TbxFileModified.Text = $"{AppFile.FileLastModifiedTime}";
            Show();
            if (AppFile.UsersAssigned != null)
            {
                foreach (var user in AppFile.UsersAssigned.ToList())
                {
                    listbxUsersAssign.SelectedItems.Add(user);
                }
            }
            else
            {
                MessageBox.Show("File is currently not assigned to any user");
            }
        }

        private void BtnCancelFile_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void BtnSaveFile_Click(object sender, RoutedEventArgs e)
        {
            var context = new windowsAppContext();
            var selectedUsers = new List<User>();
            foreach (var selectedItem in listbxUsersAssign.SelectedItems)
            {
                if (selectedItem is User user)
                {
                    // update database entry for this recenetly updated user. 
                    var fileConcerned = context.AppFiles
                                    .Where(f => f.FileName == TbxAppFileId.Text)
                                    .FirstOrDefault();
                    // assign file to user
                    user.FileAssigned = fileConcerned;
                    context.SaveChanges();
                    Close();
                }
            }
        }
    }
}
