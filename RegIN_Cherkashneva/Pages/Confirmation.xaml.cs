﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RegIN_Cherkashneva.Pages
{
    public partial class Confirmation : Page
    {
        public enum TypeConfirmation
        {
            Login,
            Regin
        }
        TypeConfirmation ThisTypeConfirmation;
        public int Code = 0;
        public Confirmation()
        {
            InitializeComponent();
        }
        public void SendMailCode()
        {
            Code = new Random().Next(100000, 999999);
            Classes.SendMail.SendMessage($"Login code: {Code}", MainWindow.mainWindow.UserLogIn.Login);
            Thread TSendMailCode = new Thread(TimerSendMailCode);
            TSendMailCode.Start();
        }
        public Confirmation(TypeConfirmation TypeConfirmation)
        {
            InitializeComponent();
            ThisTypeConfirmation = TypeConfirmation;
            SendMailCode();
        }
        public void TimerSendMailCode()
        {
            for (int i = 0; i < 60; i++)
            {
                Dispatcher.Invoke(() =>
                {
                    LTimer.Content = $"A second message can be sent after {(60 - i)} seconds";
                });
                Thread.Sleep(1000);
            }
            Dispatcher.Invoke(() =>
            {
                BSendMessage.IsEnabled = true;
                LTimer.Content = "";
            });
        }
        private void SendMail(object sender, RoutedEventArgs e)
        {
            SendMailCode();
        }
        private void SetCode(object sender, KeyEventArgs e)
        {
            if (TbCode.Text.Length == 6)
                SetCode();
        }
        private void SetCode(object sender, RoutedEventArgs e) =>
            SetCode();
        void SetCode()
        {
            if (TbCode.Text == Code.ToString() && TbCode.IsEnabled)
            {
                TbCode.IsEnabled = false;
                if (ThisTypeConfirmation == TypeConfirmation.Login)
                {
                    MessageBox.Show("Авторизация пользователя успешно подтверждена");
                }
                else
                {
                    MainWindow.mainWindow.UserLogIn.SetUser();
                    MessageBox.Show("Регистрация пользователя успешно подтверждена");
                }
            }
        }
        private void OpenLogin(object sender, MouseButtonEventArgs e)
        {
            MainWindow.mainWindow.OpenPage(new Login());
        }
    }
}
