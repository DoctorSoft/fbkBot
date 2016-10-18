using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Libs;

namespace FacebookBot
{
    public partial class Settings : Window
    {
        MainWindow main;
        Properties.Settings config = Properties.Settings.Default;

        public Settings()
        {
            InitializeComponent();
        }

        private void tree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            TreeViewItem node = e.NewValue as TreeViewItem;
            tab.SelectedIndex = node.TabIndex;
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            applyButton_Click(this, null);
            this.Close();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            main = this.Owner as MainWindow;

            //Общие
            connString.Text = config.connString;
            period.Text = config.period.ToString();
            rucaptcha.Text = config.rucaptcha;

            //Сеть
            proxyCheck.IsChecked = config.proxyBool;
            proxyText.Text = config.proxyText;

            //Интерфейс
            minimCheck.IsChecked = config.minimTray;
            closeCheck.IsChecked = config.closeTray;
        }

        private void applyButton_Click(object sender, RoutedEventArgs e)
        {
            //Общие
            config.connString = connString.Text;
            double d = Helper.DoubleParse(period.Text); if (d != 0) config.period = d;
            config.rucaptcha = rucaptcha.Text;

            //Сеть
            config.proxyBool = (bool)proxyCheck.IsChecked;
            if ((bool)proxyCheck.IsChecked) { config.proxyText = proxyText.Text; }

            //Интерфейс
            config.minimTray = (bool)minimCheck.IsChecked;
            config.closeTray = (bool)closeCheck.IsChecked;
            main.CanClose = config.closeTray;

            config.Save();
        }
    }
}
