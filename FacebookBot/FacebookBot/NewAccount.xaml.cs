using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Libs;

namespace FacebookBot
{
    /// <summary>
    /// Логика взаимодействия для NewAccount.xaml
    /// </summary>
    public partial class NewAccount : Window
    {
        MainWindow main;

        public NewAccount()
        {
            InitializeComponent();
        }

        private void saveAccount_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(loginText.Text) || string.IsNullOrEmpty(passText.Text)) { MessageBox.Show("Необходимо заполнить все поля, помеченные звездочкой", "", MessageBoxButton.OK, MessageBoxImage.Error); return; }
            if (loginText.Text.Contains(":") || passText.Text.Contains(":")) { MessageBox.Show("В полях содержится запрещенный символ \":\"", "", MessageBoxButton.OK, MessageBoxImage.Error); return; }
            var acc = main.accounts.FirstOrDefault(x => x.Login == loginText.Text);
            if (acc != null) { MessageBox.Show("Такой аккаунт уже существует", "", MessageBoxButton.OK, MessageBoxImage.Error); return; }
            else
            {
                main.accounts.Add(new Account
                {
                    Login = loginText.Text,
                    Pass = passText.Text,
                    IP = ipText.Text,
                    Port = Helper.IntParse(portText.Text),
                    LoginProxy = loginProxyText.Text,
                    PassProxy = passProxyText.Text,
                    IsRequest = true,
                    IsGroup = true,
                    IsMessage = true,
                    LimitFriends = 10000
                });
                main.accountsList.Items.Refresh();
                main.Serialize(main.accounts, "accounts");
                this.Close();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            main = this.Owner as MainWindow;
        }
    }
}
