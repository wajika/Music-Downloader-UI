﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using System.Windows.Forms;
using MusicDownloader.Json;
using MusicDownloader.Library;
using Panuon.UI.Silver;
using Panuon.UI.Silver.Core;
using MessageBoxIcon = Panuon.UI.Silver.MessageBoxIcon;
using Application = System.Windows.Application;
using AduSkin.Controls.Metro;

namespace MusicDownloader.Pages
{
    /// <summary>
    /// SettingPage.xaml 的交互逻辑
    /// </summary>
    public partial class SettingPage : Page
    {
        public delegate void ChangeBlur(double value);
        public static event ChangeBlur ChangeBlurEvent;
        public delegate void SaveBlur(double value);
        public static event SaveBlur SaveBlurEvent;
        Setting setting;
        Music music = null;

        public SettingPage(Setting s, Music m)
        {
            setting = s;
            InitializeComponent();
            music = m;
        }

        private void browseButton_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                savePathTextBox.Text = fbd.SelectedPath;
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            savePathTextBox.Text = setting.SavePath;
            searchQuantityTextBox.Text = setting.SearchQuantity;
            switch (setting.DownloadQuality)
            {
                case "999000":
                    qualityComboBox.SelectedIndex = 0;
                    break;
                case "320000":
                    qualityComboBox.SelectedIndex = 1;
                    break;
                case "128000":
                    qualityComboBox.SelectedIndex = 2;
                    break;
            }
            nameStyleComboBox.SelectedIndex = setting.SaveNameStyle;
            pathStyleComboBox.SelectedIndex = setting.SavePathStyle;
            lrcCheckBox.IsChecked = setting.IfDownloadLrc;
            picCheckBox.IsChecked = setting.IfDownloadPic;
            TranslateLrcComboBox.SelectedIndex = setting.TranslateLrc;
            if (!string.IsNullOrEmpty(Tool.Config.Read("Close")))
            {
                CloseComboBox.SelectedIndex = int.Parse(Tool.Config.Read("Close"));
            }
            if (setting.Api1 != "")
            {
                Source1textBox.Text = setting.Api1;
            }
            if (setting.Api2 != "")
            {
                Source2textBox.Text = setting.Api2;
            }
            if (setting.Cookie1 != "")
            {
                cookietextbox1.Text = setting.Cookie1;
            }
            if (!string.IsNullOrEmpty(Tool.Config.Read("Blur")))
            {
                BlurSlider.Value = double.Parse(Tool.Config.Read("Blur"));
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (savePathTextBox.Text == "")
            {
                AduMessageBox.Show("路径不能为空", "提示", MessageBoxButton.OK);
                return;
            }
            if (searchQuantityTextBox.Text == "")
            {
                AduMessageBox.Show("搜索数量不能为空", "提示", MessageBoxButton.OK);
                return;
            }
            Tool.Config.Write("SavePath", savePathTextBox.Text);
            Tool.Config.Write("DownloadQuality", ((System.Windows.Controls.ContentControl)qualityComboBox.SelectedValue).Content.ToString().Substring(("无损(").Length, 6));
            Tool.Config.Write("IfDownloadLrc", lrcCheckBox.IsChecked.ToString());
            Tool.Config.Write("IfDownloadPic", picCheckBox.IsChecked.ToString());
            Tool.Config.Write("SaveNameStyle", nameStyleComboBox.SelectedIndex.ToString());
            Tool.Config.Write("SavePathStyle", pathStyleComboBox.SelectedIndex.ToString());
            Tool.Config.Write("SearchQuantity", searchQuantityTextBox.Text);
            Tool.Config.Write("TranslateLrc", TranslateLrcComboBox.SelectedIndex.ToString());
            Tool.Config.Write("Close", CloseComboBox.SelectedIndex.ToString());
            if (Source1textBox.Text != "" && Source1textBox.Text != null && Source1textBox.Text != "http://example:port/")
            {
                Tool.Config.Write("Source1", Source1textBox.Text);
                music.NeteaseApiUrl = Source1textBox.Text;
                setting.Api1 = Source1textBox.Text;
            }
            else
            {
                Tool.Config.Write("Source1", "");
                music.NeteaseApiUrl = music.api1;
            }
            if (Source2textBox.Text != "" && Source2textBox.Text != null && Source2textBox.Text != "http://example:port/")
            {
                Tool.Config.Write("Source2", Source2textBox.Text);
                music.QQApiUrl = Source2textBox.Text;
                setting.Api2 = Source2textBox.Text;
            }
            else
            {
                Tool.Config.Write("Source2", "");
                music.QQApiUrl = music.api2;
            }
            if (cookietextbox1.Text != "" && cookietextbox1.Text != null)
            {
                Tool.Config.Write("Cookie1", cookietextbox1.Text);
                music.cookie = cookietextbox1.Text;
            }
            else
            {
                Tool.Config.Write("Cookie1", "");
                music.cookie = music._cookie;
            }
            setting.SavePath = savePathTextBox.Text;
            setting.DownloadQuality = ((System.Windows.Controls.ContentControl)qualityComboBox.SelectedValue).Content.ToString().Substring(("无损(").Length, "999000".Length);
            setting.IfDownloadLrc = lrcCheckBox.IsChecked ?? false;
            setting.IfDownloadPic = picCheckBox.IsChecked ?? false;
            setting.SaveNameStyle = nameStyleComboBox.SelectedIndex;
            setting.SavePathStyle = pathStyleComboBox.SelectedIndex;
            setting.SearchQuantity = searchQuantityTextBox.Text;
            setting.TranslateLrc = TranslateLrcComboBox.SelectedIndex;
            AduMessageBox.Show("设置保存成功", "提示", MessageBoxButton.OK);
        }

        private void searchQuantityTextBox_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (!((74 <= (int)e.Key && (int)e.Key <= 83) || (34 <= (int)e.Key && (int)e.Key <= 43) || e.Key == Key.Back || e.Key == Key.Left || e.Key == Key.Right || e.Key == Key.PageDown || e.Key == Key.PageUp))
            {
                e.Handled = true;
            }
        }

        private void Source1textBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Source1textBox.Text == "http://example:port/")
            {
                Source1textBox.Text = "";
                Source1textBox.Foreground = new SolidColorBrush(Colors.White);
            }
        }

        private void Source1textBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Source1textBox.Text == "")
            {
                Source1textBox.Text = "http://example:port/";
                Source1textBox.Foreground = new SolidColorBrush(Colors.LightGray);
            }
        }

        private void Source2textBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Source2textBox.Text == "")
            {
                Source2textBox.Text = "http://example:port/";
                Source2textBox.Foreground = new SolidColorBrush(Colors.LightGray);
            }
        }

        private void Source2textBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (Source2textBox.Text == "http://example:port/")
            {
                Source2textBox.Text = "";
                Source2textBox.Foreground = new SolidColorBrush(Colors.White);
            }
        }

        private void BlurSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            ChangeBlurEvent(BlurSlider.Value);
        }

        private void BlurSlider_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            SaveBlurEvent(BlurSlider.Value);
        }

        private void ReBG_Click(object sender, RoutedEventArgs e)
        {
            Tool.Config.Write("Background", "");
            AduMessageBox.Show("恢复完成，重启后生效", "提示");
        }
    }
}
