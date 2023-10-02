using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace academy_project
{
    public partial class ChatListControl : UserControl
    {
        //private System.Action<Chat> openChatCallback;

        private static NetworkStream ns;
        private static BinaryReader br;
        private static BinaryWriter bw;
        public ChatListControl(ref TcpClient server, Dictionary<int, Chat> chats)
        {
            //, ref? Tuple<int, string> t
            InitializeComponent();
            ns = server.GetStream();
            bw = new BinaryWriter(ns);
            br = new BinaryReader(ns);

            bool tmp = false;
            try
            {
                Task.Run(() =>
                {
                    bw.Write(4);
                    tmp = br.ReadBoolean();
                    if (tmp)
                    {
                        int len_list = br.ReadInt32();
                        int chat_id;
                        string chat_name;
                        for (int i = 0; i < len_list; i++)
                        {
                            chat_id = br.ReadInt32();
                            chat_name = br.ReadString();
                            if (!chats.ContainsKey(chat_id))
                            {
                                chats.Add(chat_id, new Chat(chat_name));
                            }
                        }
                    }
                    chatListView.ItemsSource = chats;
                }).Wait();
            }
            catch (Exception)
            {
            }
        }

        private void ChatItem_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Chat selectedChat)
            {
                //openChatCallback.Invoke(selectedChat);
                //contentArea.Content = new ChatContentControl();
            }
        }
    }
}