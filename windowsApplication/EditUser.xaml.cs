using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using windowsApp.Models;
using windowsApplication.Data;

namespace windowsApplication
{
    /// <summary>
    /// Interaction logic for EditUser.xaml
    /// </summary>
    public partial class EditUser : Window
    {
        public EditUser()
        {
            InitializeComponent();
        }

        public User User { get; set; }

        public void ShowUser(User user)
        {
            User = user;
            TbxUserId.Text = $"{User.Id}";
            TbxUserName.Text = $"{User.Name}";
            TbxEmailAddress.Text = $"{User.EmailAddress}";
            TbxPhone.Text = $"{User.Phone}";
            Show();
        }

        private void BtnCancelUser_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void BtnSaveUser_Click(object sender, RoutedEventArgs e)
        {
            var context = new windowsAppContext();
            // update database entry for this recenetly updated user. 
            var updateUserConcerned = context.Users
                            .Where(p => p.Id.ToString() == TbxUserId.Text.ToString())
                            .FirstOrDefault();

            if (updateUserConcerned is User)
            {
                updateUserConcerned.Name = TbxUserName.Text;
                updateUserConcerned.EmailAddress = TbxEmailAddress.Text;
                updateUserConcerned.Phone = TbxPhone.Text;
            }
            context.SaveChanges();
            Close();
        }
    }
}
