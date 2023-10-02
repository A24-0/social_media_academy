using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace academy_project
{
    public partial class ChatContentControl : UserControl
    {
        public ChatContentControl()
        {
            InitializeComponent();
        }

        private void MessageTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            placeholderTextBlock.Visibility = string.IsNullOrWhiteSpace(messageTextBox.Text) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            string messageText = messageTextBox.Text;

            if (!string.IsNullOrWhiteSpace(messageText))
            {
                //Message newMessage = new Message("You", messageText);

                ////добавить сообщение в лист или закинуть на сервак
                ////тут добавляю в лист
                //messageListView.Items.Add(newMessage);

                //messageTextBox.Text = string.Empty;
                //placeholderTextBlock.Visibility = Visibility.Visible;
            }
        }
    }
}