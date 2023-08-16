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

namespace academy_project
{
    public partial class UserWindow_Main : Window
    {
        private List<Chat> chats; // Закиньте сюда чаты из бд

        public UserWindow_Main()
        {
            InitializeComponent();
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
        //показывает чаты
        private void ShowChatList_Click(object sender, RoutedEventArgs e)
        {
            friendSearchPanel.Visibility = Visibility.Collapsed;
            contentArea.Content = new ChatListControl(chats, OpenChat);
        }

        //открывает определенный чат
        private void OpenChat(Chat chat)
        {
            contentArea.Content = new ChatContentControl();
        }

        //ищем друга
        private async void FindFriendButton_Click(object sender, RoutedEventArgs e)
        {
            string friendUsername = friendNameTextBox.Text;
            bool friendExists = await CheckIfFriendExistsOnServer(friendUsername); //чекнуть, существует ли друг (запрос в бд)

            if (friendExists)
            {
                Chat newChat = CreateChatWithFriend(friendUsername);
                chats.Add(newChat);

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
            Message initialMessage = new Message("Система", $"Чат с пользователем {friendUsername} создан.");
            newChat.Messages.Add(initialMessage);

            return newChat;
        }
    }
}
