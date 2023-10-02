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
using System.Text.RegularExpressions;
using System.Net;
using System.Net.Sockets;

namespace academy_project
{
    public partial class Registration : Window
    {
        Regex validateEmailRegex = new Regex("^\\S+@\\S+\\.\\S+$");

        private const int serverPort = 1648; //порт подключения
        private static string serverIp = (IPAddress.Loopback.ToString());

        private static TcpClient server;
        private static NetworkStream ns;
        private static BinaryReader br;
        private static BinaryWriter bw;

        private static Dictionary<int, Chat> chats;
        private static Dictionary<int, User> users;
        private static void Initialization_TcpClient()
        {
            server = new TcpClient(serverIp, Convert.ToInt32(serverPort));
            ns = server.GetStream(); //
            bw = new BinaryWriter(ns); //
            br = new BinaryReader(ns); //
        }
        public Registration()
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
        static string EncryptString(string plainText, string key)
        {
            byte[] keyBytes = GetKeyBytes(key);
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = keyBytes;
                aesAlg.GenerateIV();

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        csEncrypt.Write(plainTextBytes, 0, plainTextBytes.Length);
                        csEncrypt.FlushFinalBlock();

                        byte[] encryptedBytes = msEncrypt.ToArray();

                        // Объединяем IV и зашифрованный текст в один массив байт
                        byte[] resultBytes = new byte[aesAlg.IV.Length + encryptedBytes.Length];
                        Buffer.BlockCopy(aesAlg.IV, 0, resultBytes, 0, aesAlg.IV.Length);
                        Buffer.BlockCopy(encryptedBytes, 0, resultBytes, aesAlg.IV.Length, encryptedBytes.Length);

                        return Convert.ToBase64String(resultBytes);
                    }
                }
            }
        }
        private void registration_btn_Click(object sender, RoutedEventArgs e)
        {
            //поля меньше либо равно 32 все, кроме почты
            validateEmailRegex.IsMatch(email_tb.Text.ToString());

            if (string.IsNullOrEmpty(name_tb.Text) != false && string.IsNullOrEmpty(login_tb.Text) != false
                && string.IsNullOrEmpty(email_tb.Text) != false
                && string.IsNullOrEmpty(password_tb.Text) != false)//проверка ввода                                                                                                                    ===========================
            {
                bool this_user_registered = false; //получилось зарегаться или нет //вметсто этого стринг но это когда коды создадим

                Task.Run(() =>
                {
                    try
                    {
                        bw.Write(1/*mes_for_ser_about_reg*/);//сообщение серверу что мы намерены зарегаться

                        bw.Write(login_tb.Text);//ввод логина с полей для пользователя                                                                         ===========================
                        bw.Write(name_tb.Text);//ввод имя                                                                                                      ===========================
                        bw.Write(email_tb.Text);//ввод почты                                                                                                   ===========================

                        //да свободен, нет занят
                        //массив 0 - свободен ли логин / 1 - свободен ли имя / 2 - свободен ли почта
                        bool[] this_free = { br.ReadBoolean(), br.ReadBoolean(), br.ReadBoolean() };
                        if ((this_free[0] && this_free[1] && this_free[2]) == true)//проверка что занято
                        {//если существует то сервак отправляет клиенту что нужно переделать
                            bw.Write(password_tb.Text);////ввод пароля с полей для пользователя                                                                       ===========================
                            this_user_registered = br.ReadBoolean();//обработка ответа
                        }
                        else if ((this_free[0] && this_free[1] && this_free[2]) == false)
                        {

                            //изменение галочек типа, что надо, что не надо менять                                                                             ===========================
                            //Console.WriteLine("login " + this_free[0]);
                            //Console.WriteLine("name " + this_free[1]);
                            //Console.WriteLine("email " + this_free[2]);
                        }
                    }
                    catch (Exception ex)
                    {
                        this_user_registered = false;
                    }
                }).Wait();

                if (this_user_registered/*message == answer_registration_is_ok*/)//код "ok" - все прошло хорошо
                {
                    MessageBox.Show("Вы зарегистрировались", "Успешная регистрация", MessageBoxButton.OK, MessageBoxImage.Information);
                    MainWindow mw_form = new MainWindow();
                    this.Close();
                    mw_form.Show();
                }
            }
        }
        //private void registration_btn_Click(object sender, RoutedEventArgs e)
        //{
        //    validateEmailRegex.IsMatch(email_tb.Text.ToString());

        //    //var db = new DB_PROJECTEntities();
        //    //var user_db = new Users();
        //    //List<Users> users = db.Users.ToList();
        //    //if (name_tb != null && password_tb != null && email_tb != null)
        //    //{
        //    //    foreach (var item in users)
        //    //    {
        //    //        user_db.Id = item.Id + 1;
        //    //    }
        //    //    user_db.Login = name_tb.Text;
        //    //    user_db.Password = password_tb.Text;
        //    //    user_db.Password = EncryptString(user_db.Password, user_db.Login);
        //    //    user_db.Email = email_tb.Text;

        //    //    db.Users.Add(user_db);
        //    //    db.SaveChanges();

        //    //    Login loginWindow = new Login();
        //    //    this.Close();
        //    //    loginWindow.Show();
        //    //}
        //    //else
        //    //{
        //    //    MessageBox.Show("Error data");
        //    //}
        //}       
    }
}
