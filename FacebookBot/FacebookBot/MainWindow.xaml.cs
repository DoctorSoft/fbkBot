using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Threading;
using System.Windows.Input;
using Libs;
using System.Collections.Specialized;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using HtmlAgilityPack;

namespace FacebookBot
{
    public partial class MainWindow : Window
    {
        Properties.Settings config = Properties.Settings.Default;

        bool stop = false;
        Timer timer, timerLog;
        object lockObject = new object();
        object lockObjectLog = new object();

        public List<Account> accounts = new List<Account>();
        Random random = new Random((int)DateTime.Now.Ticks);
        RandomMessage randomMessages;
        List<string> groups = new List<string>();
        List<string> community = new List<string>();
        List<string> blacklist = new List<string>();

        public MainWindow()
        {
            InitializeComponent();
        }

        #region Иконка в трее
        private System.Windows.Forms.NotifyIcon trayIcon = null;
        private System.Windows.Controls.ContextMenu trayMenu = null;
        private WindowState fCurrentWindowState = WindowState.Normal;
        public WindowState CurrentWindowState
        {
            get { return fCurrentWindowState; }
            set { fCurrentWindowState = value; }
        }
        private bool fCanClose = false;
        public bool CanClose
        {
            get { return fCanClose; }
            set { fCanClose = value; }
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            CreateTrayIcon();
        }

        private bool CreateTrayIcon()
        {
            bool result = false;
            if (trayIcon == null)
            {
                trayIcon = new System.Windows.Forms.NotifyIcon();
                trayIcon.Icon = FacebookBot.Properties.Resources.icon;
                trayIcon.Text = this.Title;
                trayMenu = Resources["trayMenu"] as System.Windows.Controls.ContextMenu;
                trayIcon.Click += delegate(object sender, EventArgs e)
                {
                    if ((e as System.Windows.Forms.MouseEventArgs).Button == System.Windows.Forms.MouseButtons.Left)
                    {
                        ShowHideMainWindow(sender, null);
                    }
                    else
                    {
                        trayMenu.IsOpen = true;
                        Activate();
                    }
                };
                result = true;
            }
            else
            {
                result = true;
            }
            trayIcon.Visible = true;
            return result;
        }

        private void ShowHideMainWindow(object sender, RoutedEventArgs e)
        {
            trayMenu.IsOpen = false;
            if (IsVisible)
            {
                Hide();
                (trayMenu.Items[0] as System.Windows.Controls.MenuItem).Header = "Показать";
            }
            else
            {
                Show();
                (trayMenu.Items[0] as System.Windows.Controls.MenuItem).Header = "Скрыть";
                WindowState = CurrentWindowState;
                Activate();
            }
        }

        protected override void OnStateChanged(EventArgs e)
        {
            base.OnStateChanged(e);
            if (this.WindowState == System.Windows.WindowState.Minimized && config.minimTray)
            {
                Hide();
                (trayMenu.Items[0] as System.Windows.Controls.MenuItem).Header = "Показать";
            }
            else
            {
                CurrentWindowState = WindowState;
            }
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
            if (!CanClose)
            {
                e.Cancel = true;
                CurrentWindowState = this.WindowState;
                (trayMenu.Items[0] as System.Windows.Controls.MenuItem).Header = "Показать";
                Hide();
            }
            else
            {
                trayIcon.Visible = false;
            }
        }

        private void MenuExitClick(object sender, RoutedEventArgs e)
        {
            CanClose = true;
            this.Close();
        }
        #endregion

        #region Отображение окон
        public void ShowSettings()
        {
            var window = new Settings();
            window.Owner = this;
            var windowObject = window as Settings;
            window.ShowDialog();
        }

        public void ShowNewAccount()
        {
            var window = new NewAccount();
            window.Owner = this;
            var windowObject = window as NewAccount;
            window.ShowDialog();
        }
        #endregion

        #region События окна
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CanClose = !config.closeTray;
            LoadConfig();

            timerLog = new Timer(DoWorkLog, null, 0, 500);
        }

        void Window_Closing(object sender, EventArgs e)
        {
            SaveConfig();
            stop = true;
        }
        #endregion

        #region Панель инструментов
        private void playButton_Click(object sender, RoutedEventArgs e)
        {
            if (!SaveConfig()) return;

            playButton.Visibility = System.Windows.Visibility.Collapsed;
            pauseButton.Visibility = System.Windows.Visibility.Visible;
            stop = false;

            Start();
        }

        private void pauseButton_Click(object sender, RoutedEventArgs e)
        {
            SaveConfig();
            playButton.Visibility = System.Windows.Visibility.Visible;
            pauseButton.Visibility = System.Windows.Visibility.Collapsed;
            stop = true;

            Stop();
        }

        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            CanClose = true;
            this.Close();
        }

        private void settingsButton_Click(object sender, RoutedEventArgs e)
        {
            ShowSettings();
        }

        private void clearLogButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DirectoryInfo di = new DirectoryInfo(Helper.PathCommonApp + "logs");
                foreach (FileInfo file in di.GetFiles()) file.Delete();
            }
            catch { }
        }

        private void logblacklistButton_Click(object sender, RoutedEventArgs e)
        {
            string path = string.Format("{0}logblacklist.csv", Helper.PathCommonApp);
            if (File.Exists(path)) Process.Start(path);
            else MessageBox.Show("Пока отсутствует файл", "", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        #endregion

        #region События элементов
        private void log_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Delete || e.Key == Key.Back)
            {
                log.Clear();
            }
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            SaveConfig();
            MessageBox.Show("Успешно сохранено", "", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void addAccount_Click(object sender, RoutedEventArgs e)
        {
            ShowNewAccount();
        }

        private void accountsList_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            Serialize(accounts, "accounts");
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                var el = sender as CheckBox;
                var binding = el.GetBindingExpression(CheckBox.IsCheckedProperty).ParentBinding.Path.Path;
                var acc = accounts.First(x => x.Login == el.Tag.ToString());
                var prop = acc.GetType().GetProperty(binding);
                prop.SetValue(acc, el.IsChecked.Value, null);
                if (binding == "Disabled" && el.IsChecked.Value == true) { acc.DateDisabled = DateTime.Now; acc.Hours = 1000000; }
                Serialize(accounts, "accounts");
            }
            catch { }
        }

        private void remove_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Вы уверены?", "Удаление", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                var el = sender as Button;
                var acc = accounts.First(x => x.Login == el.Tag.ToString());
                accounts.Remove(acc);
                Serialize(accounts, "accounts");
                accountsList.Items.Refresh();
            }
        }

        private void disableAll_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                var el = sender as CheckBox;
                var name = el.Tag.ToString();
                var value = el.IsChecked.Value;
                foreach (var acc in accounts)
                {
                    var prop = acc.GetType().GetProperty(name);
                    prop.SetValue(acc, value, null);
                    if (name == "Disabled" && value) { acc.DateDisabled = DateTime.Now; acc.Hours = 1000000; }
                }
                Serialize(accounts, "accounts");
                accountsList.Items.Refresh();
            }
            catch { }
        }

        private void minFriendsAll_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                var el = sender as TextBox;
                foreach (var acc in accounts)
                {
                    acc.LimitFriends = Helper.IntParse(el.Text);
                }
                Serialize(accounts, "accounts");
                accountsList.Items.Refresh();
            }
            catch { }
        }

        private void export_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "accounts";
            dlg.DefaultExt = ".txt";
            dlg.Filter = "Text documents (.txt)|*.txt";
            if (dlg.ShowDialog() == true)
            {
                var list = new List<string>();
                foreach (var acc in accounts) list.Add(string.Format("{0}:{1}:{2}:{3}{4}", acc.Login, acc.Pass, acc.IP, acc.Port, !string.IsNullOrEmpty(acc.LoginProxy) ? ":" + acc.LoginProxy + ":" + acc.PassProxy : ""));
                File.WriteAllLines(dlg.FileName, list);
            }
        }

        private void import_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".txt";
            dlg.Filter = "Text documents (.txt)|*.txt";
            if (dlg.ShowDialog() == true)
            {
                var list = File.ReadAllLines(dlg.FileName);
                accounts.Clear();
                foreach (var l in list)
                {
                    var split = l.Split(':');
                    accounts.Add(new Account
                    {
                        Login = split[0],
                        Pass = split[1],
                        IP = split[2],
                        Port = Helper.IntParse(split[3]),
                        LoginProxy = split.Length > 4 ? split[4] : "",
                        PassProxy = split.Length > 4 ? split[5] : "",
                        IsRequest = true,
                        IsGroup = true,
                        IsMessage = true,
                        LimitFriends = 10000
                    });
                    accountsList.Items.Refresh();
                    Serialize(accounts, "accounts");
                }
            }
        }
        #endregion

        #region Таймеры
        public void Start()
        {
            try
            {
                timer = new Timer(DoWork, null, 0, (long)TimeSpan.FromSeconds(config.period).TotalMilliseconds);
            }
            catch (Exception ex) { Helper.Log(ex); }
        }

        public void Stop()
        {
            try
            {
                timer.Dispose();
            }
            catch (Exception ex) { Helper.Log(ex); }
        }

        void DoWork(object state)
        {
            if (Monitor.TryEnter(lockObject))
            {
                try
                {
                    Go();
                }
                finally
                {
                    Monitor.Exit(lockObject);
                }
            }
        }

        void DoWorkLog(object state)
        {
            if (Monitor.TryEnter(lockObjectLog))
            {
                try
                {
                    string path = Helper.PathCommonApp + "logs";
                    if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                    var logs = new List<string>();
                    var files = Directory.GetFiles(path).Reverse().Take(2);
                    foreach (var file in files)
                    {
                        while (true)
                        {
                            try
                            {
                                logs.AddRange(Regex.Split(File.ReadAllText(file), @"\|\|").Reverse());
                                break;
                            }
                            catch { Thread.Sleep(10); }
                        }
                    }
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        int len = log.Text.Length;
                        int start = log.SelectionStart;
                        int lensel = log.SelectionLength;
                        log.Text = string.Join("", logs);
                        if (start > -1)
                        {
                            try
                            {
                                int delta = log.Text.Length - len;
                                log.SelectionStart = start + delta;
                                log.SelectionLength = lensel;
                            }
                            catch { }
                        }
                    }));
                }
                catch (Exception ex) { Helper.Log(ex.ToString()); }
                finally
                {
                    Monitor.Exit(lockObjectLog);
                }
            }
        }
        #endregion

        #region Разное
        void LoadConfig()
        {
            try
            {
                accounts = Deserialize<List<Account>>("accounts");
                groups = Deserialize<List<string>>("groups");
                community = Deserialize<List<string>>("community");
                blacklist = Deserialize<List<string>>("blacklist");
                if (accounts == null) accounts = new List<Account>();
                if (groups == null) groups = new List<string>();
                if (community == null) community = new List<string>();
                if (blacklist == null) blacklist = new List<string>();
                accountsList.Items.Clear();
                accountsList.ItemsSource = accounts;
                accountsList.Items.Refresh();
                groupsText.Text = string.Join("\r\n", groups);
                communityText.Text = string.Join("\r\n", community);

                confirm1.Text = config.confirm1.ToString();
                confirm2.Text = config.confirm2.ToString();
                msgsReq1.Text = config.msgsReq1.ToString();
                msgsReq2.Text = config.msgsReq2.ToString();
                confirmCount1.Text = config.confirmCount1.ToString();
                confirmCount2.Text = config.confirmCount2.ToString();
                msgsCount1.Text = config.msgsCount1.ToString();
                msgsCount2.Text = config.msgsCount2.ToString();
                send1.Text = config.send1.ToString();
                send2.Text = config.send2.ToString();
                groupBefore1.Text = config.groupBefore1.ToString();
                groupBefore2.Text = config.groupBefore2.ToString();
                groupBetween1.Text = config.groupBetween1.ToString();
                groupBetween2.Text = config.groupBetween2.ToString();
                remove1.Text = config.remove1.ToString();
                remove2.Text = config.remove2.ToString();
                block1.Text = config.block1.ToString();
                block2.Text = config.block2.ToString();
                like1.Text = config.like1.ToString();
                like2.Text = config.like2.ToString();
                comment1.Text = config.comment1.ToString();
                comment2.Text = config.comment2.ToString();
                likeCount1.Text = config.likeCount1.ToString();
                likeCount2.Text = config.likeCount2.ToString();
                commentCount1.Text = config.commentCount1.ToString();
                commentCount2.Text = config.commentCount2.ToString();
                passiveGroup1.Text = config.passiveGroup1.ToString();
                passiveGroup2.Text = config.passiveGroup2.ToString();
                passiveAdd1.Text = config.passiveAdd1.ToString();
                passiveAdd2.Text = config.passiveAdd2.ToString();
                sendPhotoCount1.Text = config.sendPhotoCount1.ToString();
                sendPhotoCount2.Text = config.sendPhotoCount2.ToString();
                groupOneTime1.Text = config.groupOneTime1.ToString();
                groupOneTime2.Text = config.groupOneTime2.ToString();
                timerGroup1.Text = config.timerGroup1.ToString();
                timerGroup2.Text = config.timerGroup2.ToString();

                limitConfirm1.Text = config.limitConfirm1.ToString();
                limitConfirm2.Text = config.limitConfirm2.ToString();
                limitConfirmDay1.Text = config.limitConfirmDay1.ToString();
                limitConfirmDay2.Text = config.limitConfirmDay2.ToString();
                limitGroup1.Text = config.limitGroup1.ToString();
                limitGroup2.Text = config.limitGroup2.ToString();
                limitGroupDay1.Text = config.limitGroupDay1.ToString();
                limitGroupDay2.Text = config.limitGroupDay2.ToString();
                limitCommunity1.Text = config.limitCommunity1.ToString();
                limitCommunity2.Text = config.limitCommunity2.ToString();
                limitCommunityDay1.Text = config.limitCommunityDay1.ToString();
                limitCommunityDay2.Text = config.limitCommunityDay2.ToString();
                limitDialog1.Text = config.limitDialog1.ToString();
                limitDialog2.Text = config.limitDialog2.ToString();
                limitDialogDay1.Text = config.limitDialogDay1.ToString();
                limitDialogDay2.Text = config.limitDialogDay2.ToString();

                randomMessages = Deserialize<RandomMessage>("randomMessages");
                if (randomMessages == null)
                {
                    randomMessages = new RandomMessage();
                    randomMessages.AnswerPause1 = new int[7] { 10, 10, 10, 10, 10, 10, 10 };
                    randomMessages.AnswerPause2 = new int[7] { 20, 20, 20, 20, 20, 20, 20 };
                    randomMessages.AnswerNoPause1 = new int[7] { 30, 30, 30, 30, 30, 30, 30 };
                    randomMessages.AnswerNoPause2 = new int[7] { 50, 50, 50, 50, 50, 50, 50 };
                    string[][] answers =
                    {
                        new string[] { "11", "12", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                        new string[] { "21", "22", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                        new string[] { "31", "32", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                        new string[] { "", "42", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                        new string[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                        new string[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                        new string[] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" },
                        new string[] { "comment1", "comment666", "", "", "", "", "", "", "", "", "", "", "", "", "" }
                    };
                    randomMessages.Answers = answers;
                }
                if (randomMessages.Answers.Length < 8)
                {
                    var ans = randomMessages.Answers.ToList();
                    ans.Add(new string[] { "comment1", "comment2", "", "", "", "", "", "", "", "", "", "", "", "", "" });
                    randomMessages.Answers = ans.ToArray();
                }
                for (int i = 0; i < 8; i++)
                {
                    if (i < 7)
                    {
                        var sp = new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = System.Windows.HorizontalAlignment.Center };
                        sp.Children.Add(new TextBox { Width = 50, Height = 30, Text = randomMessages.AnswerPause1[i].ToString() });
                        sp.Children.Add(new TextBlock { Text = "--" });
                        sp.Children.Add(new TextBox { Width = 50, Height = 30, Text = randomMessages.AnswerPause2[i].ToString() });
                        Grid.SetColumn(sp, i + 1);
                        Grid.SetRow(sp, 1);
                        messagesGrid.Children.Add(sp);

                        var sp1 = new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = System.Windows.HorizontalAlignment.Center };
                        sp1.Children.Add(new TextBox { Width = 50, Height = 30, Text = randomMessages.AnswerNoPause1[i].ToString() });
                        sp1.Children.Add(new TextBlock { Text = "--" });
                        sp1.Children.Add(new TextBox { Width = 50, Height = 30, Text = randomMessages.AnswerNoPause2[i].ToString() });
                        Grid.SetColumn(sp1, i + 1);
                        Grid.SetRow(sp1, 2);
                        messagesGrid.Children.Add(sp1);
                    }

                    for (int j = 0; j < randomMessages.Answers[i].Length; j++)
                    {
                        if (messagesGrid.RowDefinitions.Count < j + 3) messagesGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(70) });
                        var text = new TextBox { Text = randomMessages.Answers[i][j], TextWrapping = TextWrapping.Wrap, AcceptsReturn = true, FontSize = 12 };
                        Grid.SetColumn(text, i + 1);
                        Grid.SetRow(text, j + 3);
                        messagesGrid.Children.Add(text);
                    }
                }
            }
            catch (Exception ex) { Helper.Log(ex); }
        }

        bool SaveConfig()
        {
            try
            {
                Serialize(accounts, "accounts");
                groups = Regex.Split(groupsText.Text, "\r\n").ToList();
                community = Regex.Split(communityText.Text, "\r\n").ToList();
                Serialize(groups, "groups");
                Serialize(community, "community");

                config.confirm1 = Helper.IntParse(confirm1.Text);
                config.confirm2 = Helper.IntParse(confirm2.Text);
                config.msgsReq1 = Helper.IntParse(msgsReq1.Text);
                config.msgsReq2 = Helper.IntParse(msgsReq2.Text);
                config.confirmCount1 = Helper.IntParse(confirmCount1.Text);
                config.confirmCount2 = Helper.IntParse(confirmCount2.Text);
                config.msgsCount1 = Helper.IntParse(msgsCount1.Text);
                config.msgsCount2 = Helper.IntParse(msgsCount2.Text);
                config.send1 = Helper.IntParse(send1.Text);
                config.send2 = Helper.IntParse(send2.Text);
                config.groupBefore1 = Helper.IntParse(groupBefore1.Text);
                config.groupBefore2 = Helper.IntParse(groupBefore2.Text);
                config.groupBetween1 = Helper.IntParse(groupBetween1.Text);
                config.groupBetween2 = Helper.IntParse(groupBetween2.Text);
                config.remove1 = Helper.IntParse(remove1.Text);
                config.remove2 = Helper.IntParse(remove2.Text);
                config.block1 = Helper.IntParse(block1.Text);
                config.block2 = Helper.IntParse(block2.Text);
                config.like1 = Helper.IntParse(like1.Text);
                config.like2 = Helper.IntParse(like2.Text);
                config.comment1 = Helper.IntParse(comment1.Text);
                config.comment2 = Helper.IntParse(comment2.Text);
                config.likeCount1 = Helper.IntParse(likeCount1.Text);
                config.likeCount2 = Helper.IntParse(likeCount2.Text);
                config.commentCount1 = Helper.IntParse(commentCount1.Text);
                config.commentCount2 = Helper.IntParse(commentCount2.Text);
                config.passiveGroup1 = Helper.IntParse(passiveGroup1.Text);
                config.passiveGroup2 = Helper.IntParse(passiveGroup2.Text);
                config.passiveAdd1 = Helper.IntParse(passiveAdd1.Text);
                config.passiveAdd2 = Helper.IntParse(passiveAdd2.Text);
                config.sendPhotoCount1 = Helper.IntParse(sendPhotoCount1.Text);
                config.sendPhotoCount2 = Helper.IntParse(sendPhotoCount2.Text);
                config.groupOneTime1 = Helper.IntParse(groupOneTime1.Text);
                config.groupOneTime2 = Helper.IntParse(groupOneTime2.Text);
                config.timerGroup1 = Helper.IntParse(timerGroup1.Text);
                config.timerGroup2 = Helper.IntParse(timerGroup2.Text);

                config.limitConfirm1 = Helper.IntParse(limitConfirm1.Text);
                config.limitConfirm2 = Helper.IntParse(limitConfirm2.Text);
                config.limitConfirmDay1 = Helper.IntParse(limitConfirmDay1.Text);
                config.limitConfirmDay2 = Helper.IntParse(limitConfirmDay2.Text);
                config.limitGroup1 = Helper.IntParse(limitGroup1.Text);
                config.limitGroup2 = Helper.IntParse(limitGroup2.Text);
                config.limitGroupDay1 = Helper.IntParse(limitGroupDay1.Text);
                config.limitGroupDay2 = Helper.IntParse(limitGroupDay2.Text);
                config.limitCommunity1 = Helper.IntParse(limitCommunity1.Text);
                config.limitCommunity2 = Helper.IntParse(limitCommunity2.Text);
                config.limitCommunityDay1 = Helper.IntParse(limitCommunityDay1.Text);
                config.limitCommunityDay2 = Helper.IntParse(limitCommunityDay2.Text);
                config.limitDialog1 = Helper.IntParse(limitDialog1.Text);
                config.limitDialog2 = Helper.IntParse(limitDialog2.Text);
                config.limitDialogDay1 = Helper.IntParse(limitDialogDay1.Text);
                config.limitDialogDay2 = Helper.IntParse(limitDialogDay2.Text);

                for (int i = 1; i < messagesGrid.ColumnDefinitions.Count; i++)
                {
                    for (int j = 1; j < messagesGrid.RowDefinitions.Count; j++)
                    {
                        var el = messagesGrid.Children.Cast<UIElement>().FirstOrDefault(e => Grid.GetRow(e) == j && Grid.GetColumn(e) == i);
                        if (el != null)
                        {
                            if (j == 1)
                            {
                                var sp = el as StackPanel;
                                var text1 = sp.Children[0] as TextBox;
                                var text2 = sp.Children[2] as TextBox;
                                randomMessages.AnswerPause1[i - 1] = Helper.IntParse(text1.Text);
                                randomMessages.AnswerPause2[i - 1] = Helper.IntParse(text2.Text);
                            }
                            else if (j == 2)
                            {
                                var sp = el as StackPanel;
                                var text1 = sp.Children[0] as TextBox;
                                var text2 = sp.Children[2] as TextBox;
                                randomMessages.AnswerNoPause1[i - 1] = Helper.IntParse(text1.Text);
                                randomMessages.AnswerNoPause2[i - 1] = Helper.IntParse(text2.Text);
                            }
                            else
                            {
                                var text = el as TextBox;
                                randomMessages.Answers[i - 1][j - 3] = text.Text;
                            }
                        }
                    }
                }
                Serialize(randomMessages, "randomMessages");
            }
            catch (Exception ex) { Helper.Log(ex); }

            config.Save();
            return true;
        }

        ParserWc NewParser(Account acc)
        {
            var p = new ParserWc("", 500, acc.Login + "_cookie");
            if (!string.IsNullOrEmpty(acc.IP)) p.Proxy = acc.IP + ":" + acc.Port;
            if (!string.IsNullOrEmpty(acc.LoginProxy)) p.Proxy += "|" + acc.LoginProxy + ":" + acc.PassProxy;
            return p;
        }

        void StopUi()
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                pauseButton_Click(this, null);
            }));
        }

        void Render()
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                lastUpdate.Text = DateTime.Now.ToString("G");
                accountsList.Items.Refresh();
            }));
        }

        void Status(Account acc, string status, params object[] args)
        {
            if (args.Length > 0) status = string.Format(status, args);
            acc.Status = status;
            Dispatcher.BeginInvoke(new Action(() =>
            {
                accountsList.Items.Refresh();
            }));
            LogAccount(status, acc);
        }

        public void Serialize(object o, string fileName)
        {
            while (true)
            {
                try
                {
                    string s = JsonConvert.SerializeObject(o);
                    File.WriteAllText(string.Format("{0}{1}.json", Helper.PathCommonApp, fileName), s, Encoding.UTF8);
                    break;
                }
                catch { Thread.Sleep(100); }
            }
        }

        T Deserialize<T>(string fileName)
        {
            T r = default(T);

            string path = string.Format("{0}{1}.json", Helper.PathCommonApp, fileName);
            if (File.Exists(path))
            {
                string s = File.ReadAllText(path, Encoding.UTF8);
                r = JsonConvert.DeserializeObject<T>(s);
            }

            return r;
        }

        void RandomSleep(int t1, int t2, Account acc, string status, params object[] args)
        {
            if (args.Length > 0) status = string.Format(status, args);
            int ms1 = (int)TimeSpan.FromSeconds(t1).TotalMilliseconds;
            int ms2 = (int)TimeSpan.FromSeconds(t2).TotalMilliseconds;
            int ms = random.Next(ms1, ms2);
            Status(acc, "Ожидание {0} {1} сек", status, Math.Round(ms / 1000d, 2));
            Thread.Sleep(ms);
        }

        JObject Json(Parser p)
        {
            return Parser.Json(p.Content.Replace("for (;;);", ""));
        }

        void Log(Account acc, object s, params object[] args)
        {
            if (args.Length > 0) s = string.Format(s.ToString(), args);
            Helper.Log("{0} -> {1}", acc.Login, s);
            LogAccount(s.ToString(), acc);
        }

        void LogBlacklist(Friend fr, Account acc, string accountId)
        {
            while (true)
            {
                try
                {
                    string s = string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10}\r\n", "https://www.facebook.com/" + accountId, acc.Login, acc.Pass, "https://www.facebook.com/" + fr.FriendId,
                        fr.FriendName, fr.DateAdded, fr.DateStartedDialog, fr.DateEndedDialog, fr.DateStartedGroups, fr.DateEndedGroups, fr.DateRemoved);
                    string path = string.Format("{0}logblacklist.csv", Helper.PathCommonApp);
                    if (!File.Exists(path)) s = "Наш акаунт ссылка на него;Логин;Пароль;Ссылка на акаунт который мы добавили в друзья;Добавили в друзья название ака для черного списка;" +
                        "Добавление в друзья дата;Добавление в друзья дата;Старт диалога;Финал диалога;Добавление в группы старт;Добавление в группы финиш;Удаление из друзей\r\n" + s;
                    File.AppendAllText(path, s, Encoding.UTF8);
                    break;
                }
                catch { Thread.Sleep(100); }
            }
        }

        void LogAccount(string s, Account acc)
        {
            while (true)
            {
                try
                {
                    s = string.Format("||{0} => {1} -> {2}\r\n", DateTime.Now, acc.Login, s);
                    string path = Helper.PathCommonApp + "logs\\" + acc.Login + "\\";
                    if (!Directory.Exists(path)) Directory.CreateDirectory(path);
                    path += string.Format("log_{0}.log", DateTime.Today.ToString("d"));
                    File.AppendAllText(path, s, Encoding.UTF8);
                    break;
                }
                catch { Thread.Sleep(100); }
            }
        }
        #endregion

        #region Парсинг
        void Go()
        {
            try
            {
                if (accounts.Count == 0) { Helper.Log("Необходимо добавить аккаунт"); StopUi(); return; }
                if (string.IsNullOrEmpty(config.rucaptcha)) { Helper.Log("Необходимо указать ключ Rucaptcha в настройках"); StopUi(); return; }
                WaitHandle[] waitHandles = new WaitHandle[accounts.Count];

                for (int i = 0; i < accounts.Count; i++)
                {
                    int j = i;
                    var handle = new EventWaitHandle(false, EventResetMode.ManualReset);
                    var t = new Thread(() => GoThread(handle, accounts[j]));
                    waitHandles[i] = handle;
                    t.Start();
                }

                WaitHandle.WaitAll(waitHandles);

                Render();
            }
            catch (Exception ex) { Helper.Log(ex); }
        }

        void GoThread(EventWaitHandle handle, Account acc)
        {
            try
            {
                if (acc.LimitFriends == 0) acc.LimitFriends = 10000;
                var p = NewParser(acc);
                NameValueCollection data;
                while (true)
                {
                    try
                    {
                        bool b = false;
                        if (stop) break;

                        if (acc.Hour != DateTime.Now.Hour)
                        {
                            acc.Hour = DateTime.Now.Hour;
                            acc.CurrentLimitConfirm = 0;
                            acc.LimitConfirm = random.Next(config.limitConfirm1, config.limitConfirm2);
                            acc.CurrentLimitGroup = 0;
                            acc.LimitGroup = random.Next(config.limitGroup1, config.limitGroup2);
                            acc.CurrentLimitCommunity = 0;
                            acc.LimitCommunity = random.Next(config.limitCommunity1, config.limitCommunity2);
                            acc.CurrentLimitDialog = 0;
                            acc.LimitDialog = random.Next(config.limitDialog1, config.limitDialog2);
                        }
                        if (acc.Day != DateTime.Now.Day)
                        {
                            acc.Day = DateTime.Now.Day;
                            acc.CurrentLimitConfirmDay = 0;
                            acc.LimitConfirmDay = random.Next(config.limitConfirmDay1, config.limitConfirmDay2);
                            acc.CurrentLimitGroupDay = 0;
                            acc.LimitGroupDay = random.Next(config.limitGroupDay1, config.limitGroupDay2);
                            acc.CurrentLimitCommunityDay = 0;
                            acc.LimitCommunityDay = random.Next(config.limitCommunityDay1, config.limitCommunityDay2);
                            acc.CurrentLimitDialogDay = 0;
                            acc.LimitDialogDay = random.Next(config.limitDialogDay1, config.limitDialogDay2);
                        }

                        if (acc.Disabled)
                        {
                            var time = DateTime.Now - acc.DateDisabled;
                            if (time.TotalHours > acc.Hours)
                            {
                                acc.Disabled = false;
                                Serialize(accounts, "accounts");
                            }
                            else
                            {
                                Status(acc, "Отключен");
                                Thread.Sleep(10000);
                                continue;
                            }
                        }
                        
                        Status(acc, "Авторизация");
                        if (FacebookAuth(p, acc))
                        {
                            FacebookStat(p, acc);
                            var friends = Deserialize<List<Friend>>(acc.Login + "_friends");
                            if (friends == null) friends = new List<Friend>();
                            string fb_dtsg;
                            string accountId = p.RegexMatch(@"ACCOUNT_ID"":""(\d+)").Groups[1].Value;
                            fb_dtsg = p.RegexMatch(@"fb_dtsg"" value=""([^""]+)").Groups[1].Value;

                            #region Запрос новых сообщений
                            if (acc.IsMessage)
                            {
                                var blacklistChat = Deserialize<List<string>>(acc.Login + "_blacklistChat");
                                if (blacklistChat == null) blacklistChat = new List<string>();
                                var list = new List<string> { "inbox", "pending" };
                                foreach (var l in list)
                                {
                                    int newCount = random.Next(config.msgsCount1, config.msgsCount2);
                                    int newCount1 = newCount;
                                    Status(acc, "Запрос новых сообщений {0} шт", newCount);
                                    data = new NameValueCollection();
                                    data.Add("client", "web_messenger");
                                    data.Add(l + "[offset]", "0");
                                    data.Add(l + "[limit]", "1000");
                                    data.Add(l + "[filter]", "unread");
                                    data.Add("__user", accountId);
                                    data.Add("__a", "1");
                                    data.Add("__be", "-1");
                                    data.Add("__pc", "PHASED:DEFAULT");
                                    data.Add("fb_dtsg", fb_dtsg);
                                    p.Post("https://www.facebook.com/ajax/mercury/threadlist_info.php?dpr=1", data);

                                    var j = Json(p);
                                    if (j != null && j["payload"] != null && j["payload"]["threads"] != null)
                                    {
                                        var threads = j["payload"]["threads"];
                                        var ids = threads.Select(x => x["thread_fbid"].ToString()).ToList();
                                        for (int i = 0; i < ids.Count; i++)
                                        {
                                            try
                                            {
                                                if (newCount <= 0) break;
                                                if (stop || b) break;
                                                string id = ids[i];
                                                if (blacklistChat.Contains(id)) continue;
                                                var timestamp = Helper.DateTimeToJavaTimeStamp(DateTime.UtcNow).ToString();
                                                RandomSleep(config.msgsReq1, config.msgsReq2, acc, "перед пометкой сообщений для потока {0}", id);

                                                Status(acc, "Пометка сообщений для потока {0}", id);
                                                data = new NameValueCollection();
                                                data.Add("ids[" + id + "]", "true");
                                                data.Add("watermarkTimestamp", timestamp);
                                                data.Add("shouldSendReadReceipt", "true");
                                                data.Add("commerce_last_message_type", "non_ad");
                                                data.Add("__user", accountId);
                                                data.Add("__a", "1");
                                                data.Add("__be", "-1");
                                                data.Add("__pc", "PHASED:DEFAULT");
                                                data.Add("fb_dtsg", fb_dtsg);
                                                p.Post("https://www.facebook.com/ajax/mercury/change_read_status.php?dpr=1", data);
                                                var j2 = Json(p);
                                                if (j2 == null || j2["errorSummary"] != null)
                                                {
                                                    string error = j2["errorSummary"].ToString();
                                                    blacklistChat.Add(id);
                                                    Serialize(blacklistChat, acc.Login + "_blacklistChat");
                                                }
                                                else newCount--;
                                            }
                                            catch (Exception ex) { Log(acc, ex); }
                                        }
                                    }
                                    if (j != null && j["payload"] != null && j["payload"]["participants"] != null)
                                    {
                                        var participants = j["payload"]["participants"];
                                        var ids = participants.Select(x => x["fbid"].ToString()).ToList();
                                        var names = participants.Select(x => x["name"].ToString()).ToList();
                                        for (int i = 0; i < ids.Count; i++)
                                        {
                                            try
                                            {
                                                if (newCount1 <= 0) break;
                                                if (stop || b) break;
                                                string id = ids[i];
                                                if (!blacklist.Contains(id))
                                                {
                                                    string name = names[i];
                                                    RandomSleep(config.msgsReq1, config.msgsReq2, acc, "перед запросом сообщений для {0}", name);
                                                    var friend = friends.FirstOrDefault(x => x.FriendId == id);
                                                    if (friend == null)
                                                    {
                                                        friend = new Friend { AccountLogin = acc.Login, FriendId = id, FriendName = name };
                                                        friends.Add(friend);
                                                    }

                                                    Status(acc, "Запрос сообщений для {0}", name);
                                                    data = new NameValueCollection();
                                                    data.Add("messages[user_ids][" + id + "][offset]", "0");
                                                    data.Add("messages[user_ids][" + id + "][timestamp]", "");
                                                    data.Add("messages[user_ids][" + id + "][limit]", "20");
                                                    data.Add("client", "web_messenger");
                                                    data.Add("__user", accountId);
                                                    data.Add("__a", "1");
                                                    data.Add("__be", "-1");
                                                    data.Add("__pc", "PHASED:DEFAULT");
                                                    data.Add("fb_dtsg", fb_dtsg);
                                                    p.Post("https://www.facebook.com/ajax/mercury/thread_info.php?dpr=1", data);

                                                    friend.Messages.Clear();
                                                    var j1 = Json(p);
                                                    if (j1 == null) Log(acc, "Неизвестная ошибка при пометки сообщений для {0}", name);
                                                    else if (j1["errorSummary"] != null)
                                                    {
                                                        string error = j1["errorSummary"].ToString();
                                                        if (error != "Сообщения недоступны")
                                                            Log(acc, "Ошибка при пометки сообщений для {0}: {1}", friend.FriendName, error);
                                                    }
                                                    else
                                                    {
                                                        var actions = j1["payload"]["actions"];
                                                        if (actions != null)
                                                        {
                                                            foreach (var action in actions)
                                                            {
                                                                string text = action["body"] != null ? action["body"].ToString() : "";
                                                                var message = new Message { Text = text };
                                                                message.Kind = action["author"].ToString().Contains(accountId) ? MessageKind.Bot : MessageKind.Friend;
                                                                message.Date = Helper.JavaTimeStampToDateTime((double)action["timestamp"]).ToLocalTime();
                                                                message.OfflineThreadingId = action["offline_threading_id"].ToString();
                                                                friend.Messages.Add(message);
                                                            }
                                                        }
                                                        friend.ChangeDate(randomMessages, config.groupBefore1, config.groupBefore2, config.remove1, config.remove2, config.passiveGroup1, config.passiveGroup2, config.passiveAdd1, config.passiveAdd2);
                                                        Serialize(friends, acc.Login + "_friends");
                                                        newCount1--;
                                                    }
                                                }
                                            }
                                            catch (Exception ex) { Log(acc, ex); }
                                        }
                                    }
                                }
                            }
                            #endregion

                            #region Отправка сообщений
                            if (acc.IsMessage)
                            {
                                FacebookStat(p, acc);
                                var frs = friends.Where(x => !x.Completed).OrderByDescending(x => x.HasNewMessage).ToList();
                                foreach (var friend in frs)
                                {
                                    try
                                    {
                                        if (stop || b) break;
                                        bool send = false, first = false;
                                        friend.CheckDate(randomMessages, config.groupBefore1, config.groupBefore2, config.remove1, config.remove2, config.passiveGroup1, config.passiveGroup2, config.passiveAdd1, config.passiveAdd2);
                                        var message = friend.Messages.LastOrDefault();
                                        if (message != null)
                                        {
                                            var time = DateTime.Now - message.Date;
                                            if (message.Kind == MessageKind.Friend && time.TotalSeconds > friend.Secs) send = true;
                                            if (message.Kind == MessageKind.Bot && time.TotalMinutes > friend.Mins && !acc.Passive) send = true;
                                        }
                                        else
                                        {
                                            if (!acc.Passive)
                                            {
                                                var time = DateTime.Now - friend.Date;
                                                if (time.TotalMinutes > friend.Mins) { send = true; first = true; }
                                                message = new Message { OfflineThreadingId = "6168370707694663921" };
                                            }
                                        }
                                        if (send)
                                        {
                                            if (first)
                                            {
                                                if (acc.CurrentLimitDialog >= acc.LimitDialog) { Status(acc, "Достигнут лимит начинаемых диалогов {0} в час", acc.LimitDialog); continue; }
                                                if (acc.CurrentLimitDialogDay >= acc.LimitDialogDay) { Status(acc, "Достигнут лимит начинаемых диалогов {0} в день", acc.LimitDialogDay); continue; }
                                            }
                                            if (friend.Step == 0) friend.DateStartedDialog = DateTime.Now;
                                            if (friend.Step > 7) friend.Step = friend.Messages.Where(x => x.Kind == MessageKind.Bot).Count() - 1;
                                            if (friend.Step > 6) friend.Step = 6;
                                            var messages = randomMessages.Answers[friend.Step].Where(x => !string.IsNullOrEmpty(x)).ToList();
                                            int index = random.Next(0, messages.Count);
                                            string body = messages[index];

                                            //Отправка файла
                                            var image_ids = new List<string>();
                                            var m = Regex.Match(body, @"\[([^\]]+)\]");
                                            if (m.Success)
                                            {
                                                string path = m.Groups[1].Value;
                                                int photoCount = random.Next(config.sendPhotoCount1, config.sendPhotoCount2);
                                                var di = new DirectoryInfo(path);
                                                var fis = Helper.Shuffle<FileInfo>(di.GetFiles().ToList());
                                                foreach (var fi in fis)
                                                {
                                                    if (photoCount == 0) break;
                                                    Status(acc, "Отправляется файл для {0}", friend.FriendName);
                                                    p.Referer = "https://www.facebook.com/";
                                                    p.Accept = "*/*";

                                                    p.AddHeader("Accept-Encoding", "gzip, deflate, sdch");
                                                    p.AddHeader("Access-Control-Request-Headers", "content-type, x-msgr-region");
                                                    p.AddHeader("Access-Control-Request-Method", "POST");
                                                    p.AddHeader("Origin", "https://www.facebook.com");
                                                    p.GoOptions(string.Format("https://upload.facebook.com/ajax/mercury/upload.php?dpr=1&__user={0}&__a=1&__be=-1&__pc=PHASED%3ADEFAULT&fb_dtsg={1}", accountId, fb_dtsg));

                                                    p.ClearHeaders();
                                                    p.AddHeader("X-MSGR-Region", "ASH");
                                                    p.AddHeader("Origin", "https://www.facebook.com");
                                                    p.UploadFile(string.Format("https://upload.facebook.com/ajax/mercury/upload.php?dpr=1&__user={0}&__a=1&__be=-1&__pc=PHASED%3ADEFAULT&fb_dtsg={1}", accountId, fb_dtsg),
                                                        fi.FullName, "upload_1024", "image/jpeg");
                                                    var j1 = Json(p);
                                                    if (j1 == null) Log(acc, "Неизвестная ошибка при отправке файла для {0}", friend.FriendName);
                                                    else if (j1["errorSummary"] != null)
                                                    {
                                                        string error = j1["errorSummary"].ToString();
                                                        Log(acc, "Ошибка при отправке файла для {0}: {1}", friend.FriendName, error);
                                                    }
                                                    else
                                                    {
                                                        string image_id = j1["payload"]["metadata"][0]["fbid"].ToString();
                                                        image_ids.Add(image_id);

                                                        /*data = new NameValueCollection();
                                                        data.Add("client", "mercury");
                                                        data.Add("action_type", "ma-type:user-generated-message");
                                                        data.Add("body", "");
                                                        data.Add("ephemeral_ttl_mode", "0");
                                                        data.Add("has_attachment", "true");
                                                        data.Add("image_ids[0]", image_id);
                                                        data.Add("message_id", message.OfflineThreadingId);
                                                        data.Add("offline_threading_id", message.OfflineThreadingId);
                                                        data.Add("other_user_fbid", friend.FriendId);
                                                        data.Add("source", "source:chat:web");
                                                        data.Add("specific_to_list[0]", "fbid:" + friend.FriendId);
                                                        data.Add("specific_to_list[1]", "fbid:" + accountId);
                                                        data.Add("timestamp", Helper.DateTimeToJavaTimeStamp(DateTime.UtcNow).ToString());
                                                        data.Add("ui_push_phase", "V3");
                                                        data.Add("__user", accountId);
                                                        data.Add("__a", "1");
                                                        data.Add("__be", "-1");
                                                        data.Add("__pc", "PHASED:DEFAULT");
                                                        data.Add("fb_dtsg", fb_dtsg);
                                                        p.Post("https://www.facebook.com/messaging/send/?dpr=1", data);*/
                                                    }
                                                    photoCount--;
                                                }
                                            }

                                            body = Regex.Replace(body, @"\[([^\]]+)\]", "");
                                            RandomSleep(config.confirm1, config.confirm2, acc, "перед отправкой сообщения для {0}", friend.FriendName);
                                            Status(acc, "Отправка сообщения для {0}", friend.FriendName);
                                            data = new NameValueCollection();
                                            data.Add("client", "web_messenger");
                                            data.Add("action_type", "ma-type:user-generated-message");
                                            data.Add("body", body);
                                            data.Add("ephemeral_ttl_mode", "0");
                                            data.Add("force_sms", "true");
                                            data.Add("has_attachment", (image_ids.Count > 0).ToString());
                                            for (int i = 0; i < image_ids.Count; i++) data.Add("image_ids[" + i + "]", image_ids[i]);
                                            data.Add("message_id", message.OfflineThreadingId);
                                            data.Add("offline_threading_id", message.OfflineThreadingId);
                                            data.Add("other_user_fbid", friend.FriendId);
                                            data.Add("source", "source:titan:web");
                                            data.Add("specific_to_list[0]", "fbid:" + friend.FriendId);
                                            data.Add("specific_to_list[1]", "fbid:" + accountId);
                                            data.Add("timestamp", Helper.DateTimeToJavaTimeStamp(DateTime.UtcNow).ToString());
                                            data.Add("ui_push_phase", "V3");
                                            data.Add("__user", accountId);
                                            data.Add("__a", "1");
                                            data.Add("__be", "-1");
                                            data.Add("__pc", "PHASED:DEFAULT");
                                            data.Add("fb_dtsg", fb_dtsg);
                                            p.Post("https://www.facebook.com/messaging/send/?dpr=1", data);

                                            while (true)
                                            {
                                                var j1 = Json(p);
                                                if (j1 == null) Log(acc, "Неизвестная ошибка при отправке сообщения для {0}", friend.FriendName);
                                                else if (j1["errorSummary"] != null)
                                                {
                                                    string error = j1["errorSummary"].ToString();
                                                    Log(acc, "Ошибка при отправке сообщения для {0}: {1}", friend.FriendName, error);
                                                    if (error == "Сообщение не отправлено") RemoveFriend(friend, friends, acc, accountId);
                                                    else if (error.Contains("езопасност") || error.Contains("Security Check Required")) { if (Captcha(p, data, acc, "https://www.facebook.com/messaging/send/?dpr=1", accountId)) continue; }
                                                }
                                                else
                                                {
                                                    friend.Step++;
                                                    friend.Messages.Add(new Message { Date = DateTime.Now, Kind = MessageKind.Bot, OfflineThreadingId = message.OfflineThreadingId, Text = body });
                                                    friend.ChangeDate(randomMessages, config.groupBefore1, config.groupBefore2, config.remove1, config.remove2, config.passiveGroup1, config.passiveGroup2, config.passiveAdd1, config.passiveAdd2);
                                                    Serialize(friends, acc.Login + "_friends");
                                                    if (first)
                                                    {
                                                        acc.CurrentLimitDialog++;
                                                        acc.CurrentLimitDialogDay++;
                                                    }
                                                }
                                                break;
                                            }
                                        }
                                    }
                                    catch (Exception ex) { Log(acc, ex); }
                                }
                            }
                            #endregion

                            #region Добавление в группы
                            if (acc.IsGroup)
                            {
                                var frs = friends.Where(x => x.Completed && !x.Added && (DateTime.Now - x.Date).TotalSeconds > x.Secs).ToList();
                                if (acc.Passive) frs.AddRange(friends.Where(x => x.Messages.Count == 0 && (DateTime.Now - x.Date).TotalMinutes > x.PassiveGroupMins));
                                foreach (var fr in frs)
                                {
                                    try
                                    {
                                        if (stop || b) break;
                                        if (acc.Passive && !fr.Completed)
                                        {
                                            int friendsCount = Helper.IntParse(acc.Friends);
                                            if (friendsCount > acc.LimitFriends)
                                            {
                                                RemoveFriendFacebook(fr, friends, acc, accountId, fb_dtsg, p);
                                                continue;
                                            }
                                        }
                                        bool added = false;

                                        foreach (var cm in community)
                                        {
                                            if (acc.CurrentLimitCommunity >= acc.LimitCommunity) { Status(acc, "Достигнут лимит добавления в сообщества {0} в час", acc.LimitCommunity); break; }
                                            if (acc.CurrentLimitCommunityDay >= acc.LimitCommunityDay) { Status(acc, "Достигнут лимит добавления в сообщества {0} в день", acc.LimitCommunityDay); break; }
                                            if (stop || b) break;
                                            RandomSleep(config.groupBetween1, config.groupBetween2, acc, "перед добавлением друга {0} в сообщество {1}", fr.FriendName, cm);
                                            Status(acc, "Добавление друга {0} в сообщество {1}", fr.FriendName, cm);
                                            p.Go(cm);
                                            string cmId = p.RegexMatch(@"targetID:""(\d+)").Groups[1].Value;

                                            data = new NameValueCollection();
                                            data.Add("page_id", cmId);
                                            data.Add("invitee", fr.FriendId);
                                            data.Add("elem_id", "");
                                            data.Add("action", "send");
                                            data.Add("ref", "context_row_dialog");
                                            data.Add("__user", accountId);
                                            data.Add("__a", "1");
                                            data.Add("__be", "-1");
                                            data.Add("__pc", "PHASED:DEFAULT");
                                            data.Add("fb_dtsg", fb_dtsg);
                                            p.Post("https://www.facebook.com/ajax/pages/invite/send_single/?dpr=1", data);

                                            while (true)
                                            {
                                                var j1 = Json(p);
                                                if (j1 == null) Log(acc, "Неизвестная ошибка при добавлении друга {0} в сообщество {1}", fr.FriendName, cm);
                                                else if (j1["errorSummary"] != null)
                                                {
                                                    string error = j1["errorSummary"].ToString();
                                                    Log(acc, "Ошибка при добавлении друга {0} в сообщество {1}: {2}", fr.FriendName, cm, error);
                                                    if (error.Contains("езопасност") || error.Contains("Security Check Required")) { if (Captcha(p, data, acc, "https://www.facebook.com/ajax/pages/invite/send_single/?dpr=1", accountId)) continue; }
                                                }
                                                else
                                                {
                                                    added = true;
                                                    acc.CurrentLimitCommunity++;
                                                    acc.CurrentLimitCommunityDay++;
                                                }
                                                break;
                                            }
                                        }

                                        if (added)
                                        {
                                            fr.Added = true;
                                            fr.ChangeDate(randomMessages, config.groupBefore1, config.groupBefore2, config.remove1, config.remove2, config.passiveGroup1, config.passiveGroup2, config.passiveAdd1, config.passiveAdd2);
                                            fr.DateEndedGroups = DateTime.Now;
                                            Serialize(friends, acc.Login + "_friends");
                                        }
                                    }
                                    catch (Exception ex) { Log(acc, ex); }
                                }
                                Serialize(friends, acc.Login + "_friends");

                                try
                                {
                                    if (acc.TimerGroup == 0)
                                    {
                                        acc.TimerGroup = random.Next(config.timerGroup1, config.timerGroup2);
                                        acc.TimerGroupDate = DateTime.Now;
                                        Serialize(accounts, "accounts");
                                    }
                                    if ((DateTime.Now - acc.TimerGroupDate).TotalHours > acc.TimerGroup)
                                    {
                                        frs = friends.Where(x => x.Completed && !x.AddedGroup && (DateTime.Now - x.Date).TotalSeconds > x.Secs).ToList();
                                        if (acc.Passive) frs.AddRange(friends.Where(x => x.Messages.Count == 0 && (DateTime.Now - x.Date).TotalMinutes > x.PassiveGroupMins));
                                        int frsCount = random.Next(config.groupOneTime1, config.groupOneTime2);
                                        frs = frs.Take(frsCount).ToList();
                                        if (frs.Count > 0)
                                        {
                                            foreach (var gr in groups)
                                            {
                                                if (acc.CurrentLimitGroup >= acc.LimitGroup) { Status(acc, "Достигнут лимит добавления в группы {0} в час", acc.LimitGroup); break; }
                                                if (acc.CurrentLimitGroupDay >= acc.LimitGroupDay) { Status(acc, "Достигнут лимит добавления в группы {0} в день", acc.LimitGroupDay); break; }
                                                if (stop || b) break;
                                                RandomSleep(config.groupBetween1, config.groupBetween2, acc, "перед добавлением в группу {0}", gr);
                                                Status(acc, "Добавление в группу {0}", gr);
                                                string groupId = Regex.Match(gr, @"\d+").Value;

                                                data = new NameValueCollection();
                                                data.Add("fb_dtsg", fb_dtsg);
                                                for (int i = 0; i < frs.Count; i++)
                                                {
                                                    var fr = frs[i];
                                                    data.Add("members[" + i + "]", fr.FriendId);
                                                    data.Add("text_members[" + i + "]", fr.FriendName);
                                                    fr.DateStartedGroups = DateTime.Now;
                                                    fr.AddedGroup = true;
                                                    fr.ChangeDate(randomMessages, config.groupBefore1, config.groupBefore2, config.remove1, config.remove2, config.passiveGroup1, config.passiveGroup2, config.passiveAdd1, config.passiveAdd2);
                                                    fr.DateEndedGroups = DateTime.Now;
                                                }
                                                data.Add("__user", accountId);
                                                data.Add("__a", "1");
                                                data.Add("__be", "-1");
                                                data.Add("__pc", "PHASED:DEFAULT");
                                                p.Post("https://www.facebook.com/ajax/groups/members/add_post.php?source=dialog_typeahead&group_id=" + groupId + "&refresh=1&dpr=1", data);

                                                while (true)
                                                {
                                                    var j1 = Json(p);
                                                    if (j1 == null) Log(acc, "Неизвестная ошибка при добавлении в группу {0}", gr);
                                                    else if (j1["errorSummary"] != null)
                                                    {
                                                        string error = j1["errorSummary"].ToString();
                                                        if (error == "Уже является участником") { }
                                                        else if (error == "Невозможно добавить участника" || error == "К сожалению, эта функция сейчас не доступна") { }
                                                        else if (error.Contains("езопасност") || error.Contains("Security Check Required")) { if (Captcha(p, data, acc, "https://www.facebook.com/ajax/groups/members/add_post.php?dpr=1", accountId)) continue; }
                                                        else Log(acc, "Ошибка при добавлении в группу {0}: {1}", gr, error);
                                                    }
                                                    else
                                                    {
                                                        acc.CurrentLimitGroup++;
                                                        acc.CurrentLimitGroupDay++;
                                                        Serialize(friends, acc.Login + "_friends");
                                                    }
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex) { Log(acc, ex); }
                            }
                            #endregion

                            var frs1 = friends.Where(x => !x.Liked && x.Messages.Count == 0 && (DateTime.Now - x.Date).TotalMinutes > x.PassiveGroupMins);
                            #region Лайки
                            if (acc.IsLike)
                            {
                                foreach (var fr in frs1)
                                {
                                    //Лайки фото
                                    int likeCount = random.Next(config.likeCount1, config.likeCount2);
                                    Status(acc, "Лайки фото {0} {1} шт", fr.FriendName, likeCount);
                                    p.Go("https://www.facebook.com/{0}/photos", fr.FriendId);
                                    var ms = p.RegexMatches(@"<div data-fbid=""(\d+)");
                                    foreach (Match m in ms)
                                    {
                                        if (likeCount == 0) break;
                                        string fbid = m.Groups[1].Value;
                                        //string nctr = photo.ParentNode.ParentNode.GetAttributeValue("id", "");

                                        RandomSleep(config.like1, config.like2, acc, "перед лайком фото {0}", fr.FriendName);
                                        Status(acc, "Лайк фото {0}", fr.FriendName);
                                        data = new NameValueCollection();
                                        data.Add("fbid", fbid);
                                        //data.Add("relatedid", "u_jsonp_18_6");
                                        data.Add("hovercardendpoint", "/ajax/photos/hovercard.php?fbid=" + fbid);
                                        //data.Add("related", "u_jsonp_18_6");
                                        data.Add("avoid_showing_message", "0");
                                        data.Add("includecommentlink", "true");
                                        data.Add("hovercardposition", "");
                                        data.Add("includeleadingseparator", "false");
                                        data.Add("dialogClass", "");
                                        data.Add("fetchOnHover", "false");
                                        data.Add("includeSocialContext", "false");
                                        data.Add("tagURI", "");
                                        data.Add("nctr[_mod]", "pagelet_timeline_recent");
                                        data.Add("__user", accountId);
                                        data.Add("__a", "1");
                                        data.Add("__be", "-1");
                                        data.Add("__pc", "PHASED:DEFAULT");
                                        data.Add("fb_dtsg", fb_dtsg);
                                        p.Post("https://www.facebook.com/ajax/photos/photo/like.php?dpr=1", data);

                                        var j1 = Json(p);
                                        if (j1 == null) Log(acc, "Неизвестная ошибка при лайке фото {0}", fr.FriendName);
                                        else if (j1["errorSummary"] != null)
                                        {
                                            string error = j1["errorSummary"].ToString();
                                            //Log(acc, "Ошибка при лайке фото {0}: {1}", fr.FriendName, error);
                                        }

                                        likeCount--;
                                    }
                                    fr.Liked = true;
                                    Serialize(friends, acc.Login + "_friends");
                                }
                            }
                            #endregion

                            #region Комменты
                            if (acc.IsComment)
                            {
                                foreach (var fr in frs1)
                                {
                                    //Комментарии к постам
                                    int commentCount = random.Next(config.commentCount1, config.commentCount2);
                                    Status(acc, "Комментарии к постам {0} {1} шт", fr.FriendName, commentCount);
                                    p.Go("https://www.facebook.com/profile.php?id={0}", fr.FriendId);
                                    var ms = p.RegexMatches(@"name=""ft_ent_identifier"" value=""(\d+)");
                                    foreach (Match m in ms)
                                    {
                                        if (commentCount == 0) break;
                                        string postId = m.Groups[1].Value;
                                        //string rootid = post.SelectSingleNode(".//div[contains(@class, 'uiUfi UFIContainer')]").GetAttributeValue("id", "");
                                        var messages = randomMessages.Answers[7].Where(x => !string.IsNullOrEmpty(x)).ToList();
                                        int index = random.Next(0, messages.Count);
                                        string body = messages[index];

                                        RandomSleep(config.comment1, config.comment2, acc, "перед комментарием к посту {0}", fr.FriendName);
                                        Status(acc, "Комментарий поста {0}", fr.FriendName);
                                        data = new NameValueCollection();
                                        data.Add("ft_ent_identifier", postId);
                                        data.Add("comment_text", body);
                                        data.Add("source", "21");
                                        data.Add("client_id", "1473400976203:4250690853");
                                        data.Add("session_id", "7d546a6e");
                                        data.Add("reply_fbid", "");
                                        data.Add("parent_comment_id", "");
                                        //data.Add("rootid", rootid);
                                        data.Add("attached_sticker_fbid", "0");
                                        data.Add("attached_photo_fbid", "0");
                                        data.Add("attached_video_fbid", "0");
                                        //data.Add("feedback_referrer", "/mely.ferty/friends");
                                        //data.Add("feed_context", "");
                                        data.Add("video_time_offset", "");
                                        data.Add("is_live_streaming", "false");
                                        data.Add("ft[tn]", "[]");
                                        data.Add("ft[top_level_post_id]", postId);
                                        data.Add("ft[tl_objid]", postId);
                                        data.Add("ft[fbfeed_location]", "10");
                                        //data.Add("ft[thid]", "100012856742671:306061129499414:2:0:1475305199:3311410959610735747");
                                        data.Add("nctr[_mod]", "pagelet_timeline_recent");
                                        data.Add("av", accountId);
                                        data.Add("__user", accountId);
                                        data.Add("__a", "1");
                                        data.Add("__be", "-1");
                                        data.Add("__pc", "PHASED:DEFAULT");
                                        data.Add("fb_dtsg", fb_dtsg);
                                        p.Post("https://www.facebook.com/ufi/add/comment/?dpr=1", data);

                                        var j1 = Json(p);
                                        if (j1 == null) Log(acc, "Неизвестная ошибка при комментарии поста {0}", fr.FriendName);
                                        else if (j1["errorSummary"] != null)
                                        {
                                            string error = j1["errorSummary"].ToString();
                                            //Log(acc, "Ошибка при комментарии поста {0}: {1}", fr.FriendName, error);
                                        }

                                        commentCount--;
                                    }
                                }
                            }
                            #endregion

                            #region Удаление из друзей
                            if (acc.IsRemove)
                            {
                                var friendsCount = Helper.IntParse(acc.Friends) - acc.LimitFriends;
                                if (friendsCount > 0)
                                {
                                    var frs = friends.Where(x => x.Added/* || (x.Liked && (DateTime.Now - x.Date).TotalMinutes > x.PassiveAddMins)*/).Take(friendsCount).ToList();
                                    foreach (var friend in frs)
                                    {
                                        try
                                        {
                                            if (stop || b) break;
                                            var time = DateTime.Now - friend.Date;
                                            if (time.TotalSeconds > friend.Secs)
                                            {
                                                if (Helper.IntParse(acc.Friends) > acc.LimitFriends)
                                                    RemoveFriendFacebook(friend, friends, acc, accountId, fb_dtsg, p);
                                            }
                                        }
                                        catch (Exception ex) { Log(acc, ex); }
                                    }
                                }
                            }
                            #endregion

                            #region Заявки в друзья
                            if (acc.IsRequest)
                            {
                                int count = 0;
                                int confirmCount = random.Next(config.confirmCount1, config.confirmCount2);
                                Status(acc, "Запрос заявок {0} шт", confirmCount);
                                while (true)
                                {
                                    if (acc.CurrentLimitConfirm >= acc.LimitConfirm) { Status(acc, "Достигнут лимит принятия заявок {0} в час", acc.LimitConfirm); break; }
                                    if (acc.CurrentLimitConfirmDay >= acc.LimitConfirmDay) { Status(acc, "Достигнут лимит принятия заявок {0} в день", acc.LimitConfirmDay); break; }
                                    if (stop || b) break;
                                    p.Go("https://www.facebook.com/friends/requests/");
                                    fb_dtsg = p.RegexMatch(@"fb_dtsg"" value=""([^""]+)").Groups[1].Value;
                                    var divs = p.SelectNodes("//div[contains(@class, 'friendRequestItem')]");
                                    if (divs != null)
                                    {
                                        if (divs.Count == 0) break;
                                        foreach (var div in divs)
                                        {
                                            try
                                            {
                                                if (count >= confirmCount) break;
                                                if (stop || b) break;
                                                string id = div.GetAttributeValue("data-id", "");
                                                string name = id;
                                                var aName = div.SelectSingleNode(string.Format(".//a[@data-hovercard='/ajax/hovercard/user.php?id={0}']", id));
                                                if (aName != null) name = aName.InnerHtml;
                                                if (blacklist.Contains(id))
                                                {
                                                    Status(acc, "Есть в черном списке. Удаление заявки для {0}", name);
                                                    data = new NameValueCollection();
                                                    data.Add("action", "reject");
                                                    data.Add("id", id);
                                                    data.Add("ref", "/reqs.php");
                                                    data.Add("floc", "friend_center_requests");
                                                    data.Add("frefs[0]", "jwl");
                                                    data.Add("viewer_id", accountId);
                                                    data.Add("__user", accountId);
                                                    data.Add("__a", "1");
                                                    data.Add("__be", "-1");
                                                    data.Add("__pc", "PHASED:DEFAULT");
                                                    data.Add("fb_dtsg", fb_dtsg);
                                                    p.Post("https://www.facebook.com/requests/friends/ajax/?dpr=1", data);
                                                }
                                                else
                                                {
                                                    RandomSleep(config.confirm1, config.confirm2, acc, "перед принятием заявки для {0}", name);

                                                    Status(acc, "Прием заявки для {0}", name);
                                                    data = new NameValueCollection();
                                                    data.Add("action", "confirm");
                                                    data.Add("id", id);
                                                    data.Add("ref", "/reqs.php");
                                                    data.Add("floc", "friend_center_requests");
                                                    data.Add("frefs[0]", "none");
                                                    data.Add("viewer_id", accountId);
                                                    data.Add("__user", accountId);
                                                    data.Add("__a", "1");
                                                    data.Add("__be", "-1");
                                                    data.Add("__pc", "PHASED:DEFAULT");
                                                    data.Add("fb_dtsg", fb_dtsg);
                                                    p.Post("https://www.facebook.com/requests/friends/ajax/?dpr=1", data);

                                                    var j1 = Json(p);
                                                    if (j1 == null) Log(acc, "Неизвестная ошибка при приеме заявки от {0}", name);
                                                    else
                                                    {
                                                        var payload = j1["payload"];
                                                        if ((bool)payload["success"] == true)
                                                        {
                                                            var friend = new Friend { AccountLogin = acc.Login, FriendId = id, FriendName = name, ReqDate = DateTime.Now, DateAdded = DateTime.Now };
                                                            friend.ChangeDate(randomMessages, config.groupBefore1, config.groupBefore2, config.remove1, config.remove2, config.passiveGroup1, config.passiveGroup2, config.passiveAdd1, config.passiveAdd2);
                                                            friends.Add(friend);
                                                            Serialize(friends, acc.Login + "_friends");
                                                            acc.CurrentLimitConfirm++;
                                                            acc.CurrentLimitConfirmDay++;
                                                            if (acc.CurrentLimitConfirm >= acc.LimitConfirm) { Status(acc, "Достигнут лимит принятия заявок {0} в час", acc.LimitConfirm); break; }
                                                            if (acc.CurrentLimitConfirmDay >= acc.LimitConfirmDay) { Status(acc, "Достигнут лимит принятия заявок {0} в день", acc.LimitConfirmDay); break; }
                                                        }
                                                        else
                                                        {
                                                            string err = payload["err"].ToString();
                                                            Log(acc, "Ошибка при приеме заявки от {0}: {1}", name, err);
                                                            if (err.Contains("Вы отправляете запросы на дружбу, которые могут посчитать оскорбительными"))
                                                            {
                                                                acc.Disable(config.block1, config.block2);
                                                                Serialize(accounts, "accounts");
                                                                b = true;
                                                                break;
                                                            }
                                                            else if (err == "Невозможно добавить этого друга")
                                                            {
                                                                data = new NameValueCollection();
                                                                data.Add("action", "reject");
                                                                data.Add("id", id);
                                                                data.Add("ref", "/reqs.php");
                                                                data.Add("floc", "friend_center_requests");
                                                                data.Add("frefs[0]", "jwl");
                                                                data.Add("viewer_id", accountId);
                                                                data.Add("__user", accountId);
                                                                data.Add("__a", "1");
                                                                data.Add("__be", "-1");
                                                                data.Add("__pc", "PHASED:DEFAULT");
                                                                data.Add("fb_dtsg", fb_dtsg);
                                                                p.Post("https://www.facebook.com/requests/friends/ajax/?dpr=1", data);
                                                            }
                                                        }
                                                    }
                                                }
                                                count++;
                                            }
                                            catch (Exception ex) { Log(acc, ex); }
                                        }
                                    }
                                    else break;
                                    if (count >= confirmCount) break;
                                }
                            }
                            #endregion
                        }
                        else Log(acc, "Не удалось авторизоваться");
                        if (stop) break;
                        Thread.Sleep(TimeSpan.FromSeconds(config.period));
                    }
                    catch (Exception ex) { Log(acc, ex); }
                }

                handle.Set();
            }
            catch (Exception ex) { Log(acc, ex); }
        }

        bool FacebookAuth(Parser p, Account acc)
        {
            try
            {
                p.Go("https://www.facebook.com/");
                string s = "blue_bar_profile_link";
                if (!string.IsNullOrEmpty(p.Content) && !p.Contains(s))
                {
                    var data = new NameValueCollection();
                    data.Add("lsd", p.SelectSingleNode("//input[@name='lsd']").GetAttributeValue("value", ""));
                    data.Add("persistent", p.SelectSingleNode("//input[@name='persistent']").GetAttributeValue("value", ""));
                    data.Add("default_persistent", p.SelectSingleNode("//input[@name='default_persistent']").GetAttributeValue("value", ""));
                    data.Add("timezone", p.SelectSingleNode("//input[@name='timezone']").GetAttributeValue("value", ""));
                    data.Add("lgndim", p.SelectSingleNode("//input[@name='lgndim']").GetAttributeValue("value", ""));
                    data.Add("lgnrnd", p.SelectSingleNode("//input[@name='lgnrnd']").GetAttributeValue("value", ""));
                    data.Add("lgnjs", p.SelectSingleNode("//input[@name='lgnjs']").GetAttributeValue("value", ""));
                    data.Add("ab_test_data", p.SelectSingleNode("//input[@name='ab_test_data']").GetAttributeValue("value", ""));
                    data.Add("locale", p.SelectSingleNode("//input[@name='locale']").GetAttributeValue("value", ""));
                    data.Add("next", p.SelectSingleNode("//input[@name='next']").GetAttributeValue("value", ""));
                    data.Add("email", acc.Login);
                    data.Add("pass", acc.Pass);
                    p.Post("https://www.facebook.com/login.php?login_attempt=1&lwv=110", data);
                }
                if (p.Contains(s)) return true;
            }
            catch (Exception ex) { Log(acc, ex); }
            return false;
        }

        void FacebookStat(Parser p, Account acc)
        {
            try
            {
                Status(acc, "Запрос статистики");
                p.Go("https://www.facebook.com/profile.php?sk=friends");
                var m = p.RegexMatch(@"span class=""_gs6"">([^<]+)");
                acc.Friends = m.Groups[1].Value.Replace("Â", "");
                m = p.RegexMatch(@"_5ugh _3-99"" id=""[^""]+"">([^<]+)");
                acc.Requests = m.Groups[1].Value;
                var span = p.SelectSingleNode("//span[@id='mercurymessagesCountValue']");
                if (span != null) acc.NewMessages = span.InnerHtml;
            }
            catch (Exception ex) { Log(acc, ex); }
        }

        void RemoveFriend(Friend fr, List<Friend> friends, Account acc, string accountId)
        {
            blacklist.Add(fr.FriendId);
            friends.Remove(fr);
            Serialize(friends, acc.Login + "_friends");
            Serialize(blacklist, "blacklist");
            fr.DateRemoved = DateTime.Now;
            LogBlacklist(fr, acc, accountId);
        }

        void RemoveFriendFacebook(Friend fr, List<Friend> friends, Account acc, string accountId, string fb_dtsg, Parser p)
        {
            RandomSleep(config.remove1, config.remove2, acc, "перед удалением друга {0}", fr.FriendName);
            Status(acc, "Удаление друга {0}", fr.FriendName);
            var data = new NameValueCollection();
            data.Add("uid", fr.FriendId);
            data.Add("unref", "bd_friends_tab");
            data.Add("floc", "friends_tab");
            //data.Add("nctr[_mod]", "pagelet_timeline_app_collection_100013042811170:2356318349:2");
            data.Add("__user", accountId);
            data.Add("__a", "1");
            data.Add("__be", "-1");
            data.Add("__pc", "PHASED:DEFAULT");
            data.Add("fb_dtsg", fb_dtsg);
            p.Post("https://www.facebook.com/ajax/profile/removefriendconfirm.php?dpr=1", data);

            var j1 = Json(p);
            if (j1 == null) Log(acc, "Неизвестная ошибка при удалении друга {0}", fr.FriendName);
            else if (j1["errorSummary"] != null)
            {
                string error = j1["errorSummary"].ToString();
                Log(acc, "Ошибка при удалении друга {0}: {1}", fr.FriendName, error);
                if (error == "Материалы больше не доступны") RemoveFriend(fr, friends, acc, accountId);
            }
            else RemoveFriend(fr, friends, acc, accountId);
        }

        bool Captcha(ParserWc p, NameValueCollection data, Account acc, string uri, string accountId)
        {
            try
            {
                Log(acc, "Распознавание капчи...");
                string pattern = @"img class=\\""img\\"" src=\\""([^""]+)";
                var m = p.RegexMatch(pattern);
                if (!m.Success)
                {
                    string captcha_persist_data = p.RegexMatch(@"=\\""captcha_persist_data\\"" value=\\""([^""]+)").Groups[1].Value.Replace("\\", "");
                    p.Go("https://www.facebook.com/captcha/refresh_ajax.php?dpr=1&new_captcha_type=TFBCaptcha&skipped_captcha_data={0}&__user={1}&__a=1&__req=1v&__be=-1&__pc=PHASED%3ADEFAULT",
                        captcha_persist_data, accountId);
                    m = p.RegexMatch(pattern);
                }
                if (m.Success)
                {
                    string captcha_persist_data = p.RegexMatch(@"=\\""captcha_persist_data\\"" value=\\""([^""]+)").Groups[1].Value.Replace("\\", "");
                    Log(acc, "Капча отправлена на распознавание");
                    p.RucaptchaKey = config.rucaptcha;
                    p.AntigateKey = config.rucaptcha;
                    string cap = p.AntiCaptcha(m.Groups[1].Value.Replace("\\", ""), acc.Login);
                    Log(acc, "Результат: {0}", cap);

                    data.Add("captcha_persist_data", captcha_persist_data);
                    data.Add("captcha_response", cap);
                    p.Post(uri, data);
                    return true;
                }
                else Log(acc, "Не удалось спарсить картиинку капчи");

                /*var ms = p.RegexMatches(@"id=\\""VisualCaptcha\d\\"" src=\\""([^""]+)");
                if (ms.Count == 9)
                {
                    Log(acc, "Найдены url картинок капчи");
                    var paths = new List<string>();
                    for (int i = 0; i < ms.Count; i++)
                    {
                        string uriImg = ms[i].Groups[1].Value.Replace("\\", "");
                        string path = string.Format("{0}{1}_captcha_{2}.jpg", Helper.PathCommonApp, acc.Login, i);
                        paths.Add(path);
                        p.DownloadFile(uriImg, path);
                    }
                    string textinstructions = p.DecodeJavascriptUnicode(p.RegexMatch(@"<div class=\\""fsl fwb fcb\\"">([^<]+)").Groups[1].Value);
                    Log(acc, "Загружены картинки капчи");

                    System.Drawing.Image img = new System.Drawing.Bitmap(495, 495);
                    var g = System.Drawing.Graphics.FromImage(img);
                    var fillRect = new System.Drawing.Rectangle(0, 0, 495, 495);
                    var blueBrush = new System.Drawing.SolidBrush(System.Drawing.Color.White);
                    var fillRegion = new System.Drawing.Region(fillRect);
                    g.FillRegion(blueBrush, fillRegion);
                    g.DrawImage(System.Drawing.Image.FromFile(paths[0]), new System.Drawing.Point(0, 0));
                    g.DrawImage(System.Drawing.Image.FromFile(paths[1]), new System.Drawing.Point(165, 0));
                    g.DrawImage(System.Drawing.Image.FromFile(paths[2]), new System.Drawing.Point(330, 0));
                    g.DrawImage(System.Drawing.Image.FromFile(paths[3]), new System.Drawing.Point(0, 165));
                    g.DrawImage(System.Drawing.Image.FromFile(paths[4]), new System.Drawing.Point(165, 165));
                    g.DrawImage(System.Drawing.Image.FromFile(paths[5]), new System.Drawing.Point(330, 165));
                    g.DrawImage(System.Drawing.Image.FromFile(paths[6]), new System.Drawing.Point(0, 330));
                    g.DrawImage(System.Drawing.Image.FromFile(paths[7]), new System.Drawing.Point(165, 330));
                    g.DrawImage(System.Drawing.Image.FromFile(paths[8]), new System.Drawing.Point(330, 330));
                    string pathEnd = string.Format("{0}{1}_captcha.jpg", Helper.PathCommonApp, acc.Login);
                    img.Save(pathEnd, System.Drawing.Imaging.ImageFormat.Jpeg);
                    Log(acc, "Картинки капчи склеены в одну {0}", pathEnd);

                    Log(acc, "Капча отправлена на распознавание");
                    p.RucaptchaKey = config.rucaptcha;
                    string cap = p.RuCaptcha(pathEnd, "", textinstructions);
                    Log(acc, "Результат: {0}", cap);

                    cap = cap.Replace("coordinate:", "");
                    var split = cap.Split(';');
                    for (int i = 0; i < split.Length; i++)
                    {
                        var m = Regex.Match(split[i], @"x=(\d+),y=(\d+)");
                        int x = Helper.IntParse(m.Groups[1].Value);
                        int y = Helper.IntParse(m.Groups[2].Value);
                        int result = 0;
                        if (x > 165 && x < 330 && y > 0 && y < 165) result = 1;
                        else if (x > 330 && x < 495 && y > 0 && y < 165) result = 2;
                        else if (x > 0 && x < 165 && y > 165 && y < 330) result = 3;
                        else if (x > 165 && x < 330 && y > 165 && y < 330) result = 4;
                        else if (x > 330 && x < 495 && y > 165 && y < 330) result = 5;
                        else if (x > 0 && x < 165 && y > 330 && y < 495) result = 6;
                        else if (x > 165 && x < 330 && y > 330 && y < 495) result = 7;
                        else if (x > 330 && x < 495 && y > 330 && y < 495) result = 8;
                        data.Add("answers[" + i + "]", "VisualCaptcha" + result);
                    }
                    data.Add("captcha_persist_data", captcha_persist_data);

                    p.Post(uri, data);
                    return true;
                }
                else
                {
                }*/
            }
            catch (Exception ex) { Log(acc, ex); }
            return false;
        }
        #endregion
    }
}
