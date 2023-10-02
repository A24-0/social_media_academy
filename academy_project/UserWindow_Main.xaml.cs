using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
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
using System.Net;
using System.Net.Sockets;
using System.Drawing;
using System.Drawing.Imaging;
//using DrawingColor = System.Drawing.Color;
using MediaColor = System.Windows.Media.Color;
using static System.Net.Mime.MediaTypeNames;


namespace academy_project
{
    public partial class UserWindow_Main : Window
    {
        private const int serverPort = 1648;
        private static string serverIp;

        private static TcpClient server;
        private static NetworkStream ns;
        private static BinaryReader br;
        private static BinaryWriter bw;

        private static Dictionary<int, Chat> chats;
        private static Dictionary<int, User> users;
        public UserWindow_Main(TcpClient server_, string serverIp_)
        {
            serverIp = serverIp_;
            server = server_;

            ns = server.GetStream();
            bw = new BinaryWriter(ns);
            br = new BinaryReader(ns);

            chats = new Dictionary<int, Chat>();

            InitializeComponent();

            //GenerateImage();

            //BitmapImage bmpImage = new BitmapImage();
            //bmpImage.BeginInit();
            //bmpImage.UriSource = new Uri("\\bin\\Debug\\net6.0-windows\\Avatar.png", UriKind.Relative);
            //bmpImage.EndInit();

            //Avatar.ImageSource = bmpImage;
            //Avatar.UpdateLayout();

            //ищем друга
            find_friend_btn.Click += (sender, e) =>
            {
                contentArea.Content = friendSearchPanel;
                friendSearchPanel.Visibility = Visibility.Visible;
            };

            chats_btn.Click += ShowChatList_Click;

            //здесь можно поменять имя, почту и пароль юзера
            profile_settings_btn.Click += (sender, e) =>
            {
                friendSearchPanel.Visibility = Visibility.Collapsed;
                contentArea.Content = new StackPanel
                {
                    Children =
                    {
                        new TextBox
                        {
                            Text = "Имя пользователя",
                            BorderThickness = new Thickness(1),
                            Margin = new Thickness(10),
                        },
                        new TextBox
                        {
                            Text = "Email",
                            BorderThickness = new Thickness(1),
                            Margin = new Thickness(10),
                        },
                        new PasswordBox
                        {
                            Password = "Пароль",
                            BorderThickness = new Thickness(1),
                            Margin = new Thickness(10),
                        },
                    },
                    Margin = new Thickness(10),
                    Orientation = Orientation.Vertical,
                };
            };
        }

        //private void GenerateImage()
        //{
        //    Bitmap bmp = new Bitmap(200, 200);

        //    Graphics g = Graphics.FromImage(bmp);

        //    Random rnd = new Random();
        //    DrawingColor bgColor = DrawingColor.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));

        //    g.FillRectangle(new SolidBrush(bgColor), new System.Drawing.Rectangle(0, 0, 200, 200));

        //    DrawingColor letterColor = DrawingColor.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
        //    Font font = new Font("Yanone Kaffeesatz", 100);
        //    StringFormat format = new StringFormat();
        //    format.Alignment = StringAlignment.Center;
        //    format.LineAlignment = StringAlignment.Center;


        //    //Здесь вставляется имя/фамилия/ник/символ на основе которого будет генериться ава, можно любой длины вставлять
        //    string penis = NameTB.Text.ToString();
        //    g.DrawString(penis, font, new SolidBrush(letterColor), new RectangleF(0, 0, 200, 200), format);

        //    bmp.Save("Avatar.png", ImageFormat.Png);
        //    g.Dispose();
        //    bmp.Dispose();
        //}

        private void Window_Closing(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }

        //показывает чаты
        private void ShowChatList_Click(object sender, RoutedEventArgs e)
        {
            friendSearchPanel.Visibility = Visibility.Collapsed;
            contentArea.Content = new ChatListControl(ref server, chats);
        }

        //открывает определенный чат
        private void OpenChat(Chat chat)
        {
            contentArea.Content = new ChatContentControl();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
        //ищем друга
        private async void FindFriendButton_Click(object sender, RoutedEventArgs e)
        {
            string friendUsername = friendNameTextBox.Text;
            bool friendExists = await CheckIfFriendExistsOnServer(friendUsername); //чекнуть, существует ли друг (запрос в бд)

            if (friendExists)
            {
                Chat newChat = CreateChatWithFriend(friendUsername);
                //chats.Add(newChat);

                OpenChat(newChat);
            }
            else
            {
                MessageBox.Show("такого пользователя не существует");
            }
        }
        private async Task<bool> CheckIfFriendExistsOnServer(string friendUsername)
        {
            //проверка существования друга на сервере
            //true, если друг существует, иначе false
            return true;
        }

        private Chat CreateChatWithFriend(string friendUsername)
        {
            //создать в бд связь юзера и его друга
            Chat newChat = new Chat(friendUsername);

            //добавить начальные сообщения в чат, если нужно

            return newChat;
        }
    }
}
