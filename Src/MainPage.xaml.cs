using System;
using System.Collections.Generic;
using System.Linq;
using Tools.Utils;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using static Tools.Utils.Conditions;

namespace Tools
{
    public sealed partial class MainPage : Page
    {
        private StorageFolder Folder { get; set; }

        public MainPage()
        {
            this.InitializeComponent();
            WindowUtils.CustomTitleBar(TitleBarArea);
            Loaded += SettingsPageLoaded;
        }

        private async void OpenFolderButton_Click(object sender, RoutedEventArgs e)
        {
            Folder = await IOutils.OpenFolderAsync();

            if (Folder == null) return;

            OpenFolderTextBox.Text = Folder.Path;
        }

        private async void StartButton_ClickAsync(object sender, RoutedEventArgs e)
        {
            if (Folder == null)
            {
                TeachingTipShow("错误提示", "请先选择文件夹！");
                return;
            }

            OutputDataTextBox.Text = string.Empty;
            OutputFolderTreeTextBox.Text = string.Empty;
            OutputFileListTextBox.Text = string.Empty;
            OutputErrorFileTextBox.Text = string.Empty;
            LoadingDataProgressBar.Visibility = Visibility.Visible;

            string title = "读取完成";
            string subtitle = "所有文件已读取完成！";
            try
            {
                DataResult result = await new ReadDataUtils().StartCollectAsync(Folder, Conditions);
                OutputDataTextBox.Text = result.FileContent;
                OutputFolderTreeTextBox.Text = result.FolderStructure;
                OutputFileListTextBox.Text = result.FileList;
                OutputErrorFileTextBox.Text = result.ErrorFileName;

                if (!string.IsNullOrEmpty(result.ErrorFileName))
                {
                    subtitle = "部分文件不能读取！";
                }

            }
            catch (Exception exception)
            {
                if (exception is ArgumentNullException)
                {
                    title = "错误提示";
                    subtitle = "文件夹为空！";
                }

            }
            finally
            {
                TeachingTipShow(title, subtitle);
                LoadingDataProgressBar.Visibility = Visibility.Collapsed;
            }
        }

        private void CopyFileContentButton_Click(object sender, RoutedEventArgs e)
        {
            ToClipboard(OutputDataTextBox.Text);
        }

        private void CopyFileNameButton_Click(object sender, RoutedEventArgs e)
        {
            ToClipboard(OutputFileListTextBox.Text);
        }

        private void CopyErrorFileNameButton_Click(object sender, RoutedEventArgs e)
        {
            ToClipboard(OutputErrorFileTextBox.Text);
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            OutputDataTextBox.Text = string.Empty;
            OutputFolderTreeTextBox.Text = string.Empty;
            OutputFileListTextBox.Text = string.Empty;
            OutputErrorFileTextBox.Text = string.Empty;
        }

        private void ThemeRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            var selectedTheme = ((RadioButton)sender)?.Tag?.ToString();

            if (selectedTheme != null)
            {
                SettingsUtils.Theme = App.GetEnum<ElementTheme>(selectedTheme);
            }
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            SettingsPage("设置");
        }

        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            SettingsPage("关于");
        }

        private ConditionsResult Conditions
        {
            get
            {
                ConditionsResult conditionsResult = new ConditionsResult
                {
                    IncludeFile = new Dictionary<Conditions, string[]>
                    {
                        { FileFullName, IncludeFileFullNameTextBox.Text.Split(",") },
                        { FilePartName, IncludeFilePartNameTextBox.Text.Split(",") },
                        { FileNamePrefix, IncludeFileNamePrefixTextBox.Text.Split(",") },
                        { FileNameSuffix, IncludeFileNameSuffixTextBox.Text.Split(",") }
                    },
                    IncludeFolder = new Dictionary<Conditions, string[]>
                    {
                        { FolderFullName, IncludeFolderFullNameTextBox.Text.Split(",") },
                        { FolderPartName, IncludeFolderPartNameTextBox.Text.Split(",") },
                        { FolderNamePrefix, IncludeFolderNamePrefixTextBox.Text.Split(",") },
                        { FolderNameSuffix, IncludeFolderNameSuffixTextBox.Text.Split(",") }
                    },
                    ExcludeFile = new Dictionary<Conditions, string[]>
                    {
                        { FileFullName, ExcludeFileFullNameTextBox.Text.Split(",") },
                        { FilePartName, ExcludeFilePartNameTextBox.Text.Split(",") },
                        { FileNamePrefix, ExcludeFileNamePrefixTextBox.Text.Split(",") },
                        { FileNameSuffix, ExcludeFileNameSuffixTextBox.Text.Split(",") }

                    },
                    ExcludeFolder = new Dictionary<Conditions, string[]>
                    {
                        { FolderFullName, ExcludeFolderFullNameTextBox.Text.Split(",") },
                        { FolderPartName, ExcludeFolderPartNameTextBox.Text.Split(",") },
                        { FolderNamePrefix, ExcludeFolderNamePrefixTextBox.Text.Split(",") },
                        { FolderNameSuffix, ExcludeFolderNameSuffixTextBox.Text.Split(",") }
                    }
                };

                return ConditionUitls.CondotionsIsNull(conditionsResult);
            }
        }

        private void ToClipboard(string text)
        {
            DataPackage data = new DataPackage();
            data.SetText(text);

            Clipboard.SetContent(data);
        }

        private void SettingsPage(string title)
        {
            MenueSplitView.IsPaneOpen = !MenueSplitView.IsPaneOpen;
            MenueTitleTextBlock.Text = title;

            switch (title)
            {
                case "设置":
                    AboutStackPanel.Visibility = Visibility.Collapsed;
                    SettingsStackPannel.Visibility = Visibility.Visible;
                    break;
                case "关于":
                    SettingsStackPannel.Visibility = Visibility.Collapsed;
                    AboutStackPanel.Visibility = Visibility.Visible;
                    break;
            }
        }

        private void SettingsPageLoaded(object sender, RoutedEventArgs e)
        {
            var currentTheme = SettingsUtils.Theme.ToString();
            ThemeStackPanel.Children.Cast<RadioButton>().FirstOrDefault(c => c?.Tag?.ToString() == currentTheme).IsChecked = true;
        }

        private void TeachingTipShow(string title, string subtitle)
        {
            MsgTeachingTip.IsOpen = true;
            MsgTeachingTip.Title = title;
            MsgTeachingTip.Subtitle = subtitle;
        }
    }
}
