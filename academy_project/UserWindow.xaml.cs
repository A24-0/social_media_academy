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

namespace academy_project
{
    /// <summary>
    /// Логика взаимодействия для UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        public UserWindow()
        {
            InitializeComponent();
            CheckedUser checkedUser = new CheckedUser();
            lb_username.Content = checkedUser.Username;
            lb_email.Content = checkedUser.Email;
        }

        private void ChangePassword_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
