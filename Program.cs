using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows;

namespace MesClient
{
    class Program
    {
        private const int serverPort = 1648; //порт подключения
        private static string serverIp =(IPAddress.Loopback.ToString());

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

        static void Main(string[] args)
        {
            Initialization_TcpClient();
            Console.ReadKey();
        }
        private void bt_sing_up_Click()//тригер на кнопку send
        {
            if (true)//проверка ввода                                                                                                                    ===========================
            {
                bool this_user_registered = false;//получилось зарегаться или нет //вметсто этого стринг но это когда коды создадим

                //гденибудь для юзера прописать что идет процесс отправки                                                                               ===========================
                Task.Run(() =>
                {
                    try
                    {
                        bw.Write(1/*mes_for_ser_about_reg*/);//сообщение серверу что мы намерены зарегаться

                        bw.Write("test");//ввод логина с полей для пользователя                                                                         ===========================
                        bw.Write("test");//ввод имя                                                                                                     ===========================
                        bw.Write("test");//ввод почта                                                                                                   ===========================

                        //да свободен, нет занят
                        //массив 0 - свободен ли логин / 1 - свободен ли имя / 2 - свободен ли почта
                        bool[] this_free = { br.ReadBoolean(), br.ReadBoolean(), br.ReadBoolean() };
                        if (this_free[0] && this_free[1] && this_free[2])//проверка что занято
                        {//если существует то сервак отправляет клиенту что нужно переделать
                            bw.Write("test");////ввод пароля с полей для пользователя                                                                       ===========================
                            this_user_registered = br.ReadBoolean();//обработка ответа
                        }
                        else
                        {
                            //изменение галочек типа, что надо, что не надо менять                                                                             ===========================
                            Console.WriteLine("login " + this_free[0]);
                            Console.WriteLine("name " + this_free[1]);
                            Console.WriteLine("email " + this_free[2]);
                        }
                    }
                    catch (Exception ex)
                    {
                        this_user_registered = false;
                    }
                }).Wait();

                //гденибудь для юзера прописать что процесс отправки окончен                                                                                   ===========================

                if (this_user_registered/*message == answer_registration_is_ok*/)//код "ok" - все прошло хорошо
                {
                    MessageBox.Show("Вы зарегистрировались");
                    //т.к. мы зарегались теперь нужно войти или от сюда переход на главный экран или каким-то иным путём по типа возврата праметра...          =============================
                }
                else if (this_user_registered/*message == answer_login_busy*/)//код "busy" - логин занят
                {
                    //от сюда можно напомнить чтоб проверил поменял логин почту и имя                                                                           ===================
                    //если не залогинился просто дождаться следующего нажатия тоесть окно пока не скрываем                                                      ===================
                }
                else //если вылетела какая-то ошибка
                {
                    // т.к. сейчас тест и пока что передается только bool значение, вывода ошибок пока нет                                                      +++++++++++++++++++
                }
            }
            else
            {
                //MessageBox.Show("Проверте поля ввода");                                                                                                        ==============================
            }
        }
        private void bt_sing_in_Click()
        {
            if (true)//проверка ввода                                                                                                                             ===========================
            {
                bool this_user_logged = false;//получилось войти или нет

                //гденибудь для юзера прописать что идёт процесс  отправки                                                                                       ===========================
                Task.Run(() =>
                {
                    try
                    {
                        bw.Write(2/*mes_for_ser_about_reg*/);//сообщение серверу что мы намерены зарегаться

                        bw.Write("test");//ввод логина с полей для пользователя                                                                                     ============================


                        if (br.ReadBoolean())//проверка существует ли логин
                        {
                            bw.Write("test");//ввод пароля с полей для пользователя                                                                                 ===========================

                            this_user_logged = br.ReadBoolean();//обработка ответа

                            if (!this_user_logged)
                            {
                                Console.WriteLine("не правильный пароль или ошибка");
                                //сообщение что пароль не подходит                                                                                                       ============================
                            }
                        }
                        else
                        {
                            Console.WriteLine("такого логина не существует");
                            //если существует то сервак отправляет клиенту что нужно переделать
                            //изменение галочек типа, что надо, что не надо менять                                                                                  ===========================
                        }
                    }
                    catch (Exception ex)
                    {
                        this_user_logged = false;
                    }
                }).Wait();

                //гденибудь для юзера прописать что процесс отправки окончен                                                                                            ===========================

                if (this_user_logged/*message == answer_registration_is_ok*/)//код "ok" - все прошло хорошо
                {
                    MessageBox.Show("Вы успешно зарегистрировались, теперь нужно залогиниться"); //сообщение для пользователя что все прошло хорошо                     ===========================
                    //любым способо переход на окно с чатами                                                                                                            =======================
                }
                else if (this_user_logged/*message == answer_login_busy*/)//код "busy" - логин занят
                {
                    //от сюда можно вынести что пароль не верный                                                                                                ===================
                }
                else //если вылетела какая-то ошибка
                {
                    // т.к. сейчас тест и пока что передается только bool значение, вывода ошибок пока нет                                                      ++++++++++++++++
                }
            }
            else
            {
                //MessageBox.Show("Проверте поля ввода");                                                                                         ==============================
            }
        }
    }
}
