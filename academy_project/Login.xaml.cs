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

namespace academy_project
{
    /// <summary>
    /// Логика взаимодействия для Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
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
            if (login_tb.Text != null && password_tb.Text != null) 
            {
                var db = new DB_PROJECTEntities();

                List<Users> users = db.Users.ToList();


                bool flag = false;
                CheckedUser checkedUser = new CheckedUser();
                foreach (var item in users)
                {
                    string tmpPassword = item.Password;
                    var NewtmpPassword = DecryptString(item.Password, item.Login);
                    if (password_tb.Text != null && login_tb.Text != null) 
                    {
                        if (NewtmpPassword == password_tb.Text && login_tb.Text == item.Login)
                        {
                            checkedUser.Username = item.Login;
                            checkedUser.Email = item.Email;
                            checkedUser.Id = item.Id;
                            checkedUser.Password = item.Password;
                            UserWindow userWindow = new UserWindow();
                            this.Close();
                            userWindow.Show();
                        }
                        else 
                        {
                            flag = true;
                        }
                    }
                }
                if (flag == true)
                {
                    login_tb.Text = null;
                    login_tb.Text = "Wrong data";

                    password_tb.Text = null;
                    password_tb.Text = "Wrong data";

                }
            }
        }
    }
}
