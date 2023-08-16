using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace academy_project
{
    public partial class ChatListControl : UserControl
    {
        private List<Chat> chats;
        private System.Action<Chat> openChatCallback;

        public ChatListControl(List<Chat> chats, System.Action<Chat> openChatCallback)
        {
            InitializeComponent();
            this.chats = chats;
            this.openChatCallback = openChatCallback;
            chatListView.ItemsSource = chats;
        }

        private void ChatItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Chat selectedChat)
            {
                openChatCallback.Invoke(selectedChat);
            }
        }
    }
}