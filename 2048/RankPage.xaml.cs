using SQLitePCL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace _2048
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class RankPage : Page
    {
        public RankPage()
        {
            this.InitializeComponent();
            var viewTitleBar = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().TitleBar;
            viewTitleBar.BackgroundColor = Windows.UI.Colors.CornflowerBlue;
            viewTitleBar.ButtonBackgroundColor = Windows.UI.Colors.CornflowerBlue;
            updateRanking();


        }
        private Models.player Player;
        string[] rankname = new string[5];
        string[] rankscore = new string[5];
        string[] ranknum = new string[5];
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Player = (Models.player)e.Parameter;
            Frame rootFrame = Window.Current.Content as Frame;
            if (rootFrame.CanGoBack)
            {
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                    AppViewBackButtonVisibility.Visible;
            }
            else
            {
                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                    AppViewBackButtonVisibility.Collapsed;
            }
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(GamePage), Player);
        }
        private void updateRanking()
        {
            int i = 0;
            var db = App.conn;
            try
            {
                using (var statement = db.Prepare("SELECT * FROM Players ORDER BY HighestScore DESC LIMIT 5"))
                {
                    while (statement.Step() == SQLiteResult.ROW)
                    {
                        rankname[i] = ((string)statement[1]);
                        rankscore[i] = ((long)statement[4]) + "";
                        ranknum[i] = i + 1 + "";
                        i++;
                    }
                }
            }
            catch (Exception exe)
            {

            }
        }
    }
}
