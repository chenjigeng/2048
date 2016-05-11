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
    public sealed partial class EnterPage : Page
    {
        public EnterPage()
        {
            this.InitializeComponent();
            var viewTitleBar = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().TitleBar;
            viewTitleBar.BackgroundColor = Windows.UI.Colors.CornflowerBlue;
            viewTitleBar.ButtonBackgroundColor = Windows.UI.Colors.CornflowerBlue;
        }
        private Models.player Player;
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            Player = (Models.player)(e.Parameter);
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
        private void Game_Click(object sender, RoutedEventArgs e)
        {
            //转到游戏开始界面
            Frame.Navigate(typeof(GamePage), Player);
        }

        private void Rule_Click(object sender, RoutedEventArgs e)
        {
            //转到游戏说明界面
            Frame.Navigate(typeof(RulePage), Player );
        }

        private void Grade_Click(object sender, RoutedEventArgs e)
        {
            //转到排行榜界面（测试游戏结束界面）
            Frame.Navigate(typeof(RankPage), Player);
            //Frame.Navigate(typeof(GradePage), "");
        }
    }
}
