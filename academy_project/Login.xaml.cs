using System;
using System.Collections.Generic;
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

namespace academy_project
{
    public partial class Login : Window
    {
        private const int serverPort = 1648; //порт подключения
        private static string serverIp = (IPAddress.Loopback.ToString());

        private static TcpClient server;
        private static NetworkStream ns;
        private static BinaryReader br;
        private static BinaryWriter bw;

        private static void Initialization_TcpClient()
        {
            server = new TcpClient(serverIp, Convert.ToInt32(serverPort));
            ns = server.GetStream(); //создать заново
            bw = new BinaryWriter(ns); //
            br = new BinaryReader(ns); //
        }
        public Login()
        {
            Initialization_TcpClient();
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
        static byte[] GetKeyBytes(string login)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] loginBytes = Encoding.UTF8.GetBytes(login);
                byte[] hashedBytes = sha256.ComputeHash(loginBytes);

                // Усекаем хеш до 16 байт для AES-128, 24 байта для AES-192 или 32 байта для AES-256
                byte[] truncatedBytes = new byte[16];
                Buffer.BlockCopy(hashedBytes, 0, truncatedBytes, 0, truncatedBytes.Length);

                return truncatedBytes;
            }
        }
        static string DecryptString(string cipherText, string key)
        {
            byte[] keyBytes = GetKeyBytes(key);
            byte[] cipherTextBytes = Convert.FromBase64String(cipherText);

            using (Aes aesAlg = Aes.Create())
            {
                // Извлекаем IV из массива байт
                byte[] ivBytes = new byte[aesAlg.IV.Length];
                Buffer.BlockCopy(cipherTextBytes, 0, ivBytes, 0, aesAlg.IV.Length);

                aesAlg.Key = keyBytes;
                aesAlg.IV = ivBytes;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream())
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Write))
                    {
                        // Зашифрованный текст начинается после IV
                        csDecrypt.Write(cipherTextBytes, aesAlg.IV.Length, cipherTextBytes.Length - aesAlg.IV.Length);
                        csDecrypt.FlushFinalBlock();

                        byte[] decryptedBytes = msDecrypt.ToArray();
                        return Encoding.UTF8.GetString(decryptedBytes);
                    }
                }
            }
        }

        private void login_btn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(login_tb.Text) != false
                && string.IsNullOrEmpty(email_tb.Text) != false
                && string.IsNullOrEmpty(password_tb.Text) != false)
            {
                bool this_user_logged = false;//получилось войти или нет

                //где-нибудь для юзера прописать что идёт процесс отправки                                                                                       ===========================
                Task.Run(() =>
                {
                    try
                    {
                        bw.Write(2/*mes_for_ser_about_reg*/);//сообщение серверу что мы намерены зарегаться

                        bw.Write(login_tb.Text);//ввод логина с полей для пользователя                                                                                     ============================


                        if (br.ReadBoolean())//проверка существует ли логин
                        {
                            bw.Write(password_tb.Text);//ввод пароля с полей для пользователя                                                                                 ===========================

                            this_user_logged = br.ReadBoolean();//обработка ответа

                            if (!this_user_logged)
                            {
                                //Console.WriteLine("не правильный пароль или ошибка");
                                //сообщение что пароль не подходит                                                                                                       ============================
                            }
                        }
                        else
                        {
                            //Console.WriteLine("такого логина не существует");
                            //если существует то сервак отправляет клиенту что нужно переделать
                            //изменение галочек типа, что надо, что не надо менять                                                                                  ===========================
                        }
                    }
                    catch (Exception ex)
                    {
                        this_user_logged = false;
                    }
                }).Wait();

                if (this_user_logged/*message == answer_registration_is_ok*/)//код "ok" - все прошло хорошо
                {
                    UserWindow_Main uw_mform = new UserWindow_Main(server, serverIp);
                    this.Close();
                    uw_mform.Show();
                }

            }
        }
    }
}
