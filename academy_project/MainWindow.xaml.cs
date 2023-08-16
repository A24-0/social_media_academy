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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace academy_project
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
        private void Window_Closing(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }

        private void registration_btn_Click(object sender, RoutedEventArgs e)
        {
            Registration reg_form = new Registration();
            this.Close();     
            reg_form.Show();
        }

        private void login_btn_Click(object sender, RoutedEventArgs e)
        {
            Login login_form = new Login();
            this.Close();
            login_form.Show();
        }
    }
}
