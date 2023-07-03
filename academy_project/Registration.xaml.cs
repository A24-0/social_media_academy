
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
    /// Логика взаимодействия для Registration.xaml
    /// </summary>
    /// 

   
    public partial class Registration : Window
    {
        public Registration()
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

            var db = new DB_PROJECTEntities();
            var user_db = new Users();
            List<Users> users = db.Users.ToList();
            if (login_tb != null && password_tb != null && email_tb != null)
            {
                foreach (var item in users)
                {
                    user_db.Id = item.Id + 1;
                }
                user_db.Login = login_tb.Text;
                user_db.Password = password_tb.Text;
                user_db.Password = EncryptString(user_db.Password, user_db.Login);
                user_db.Email = email_tb.Text;

                db.Users.Add(user_db);
                db.SaveChanges();

                Login loginWindow = new Login();
                this.Close();
                loginWindow.Show();
            }
            else 
            {
                MessageBox.Show("Error data");
            }
        }
    }
}
