using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Streams;
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
    public sealed partial class GradePage : Page
    {
        public GradePage()
        {
            this.InitializeComponent();
            var viewTitleBar = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().TitleBar;
            viewTitleBar.BackgroundColor = Windows.UI.Colors.CornflowerBlue;
            viewTitleBar.ButtonBackgroundColor = Windows.UI.Colors.CornflowerBlue;
            dtm.DataRequested += DataTransferManager_DataRequested;
        }

        ~GradePage()
        {
            dtm.DataRequested -= DataTransferManager_DataRequested;
        }

        private Models.player Player;
        DataTransferManager dtm = DataTransferManager.GetForCurrentView();

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            try
            {
                Player = (Models.player)e.Parameter;
            }
            catch
            {
            }
            Frame rootFrame = Window.Current.Content as Frame;
             SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
            //if (rootFrame.CanGoBack)
            //{
            //    SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
            //        AppViewBackButtonVisibility.Visible;
            //}
            //else
            //{
            //    SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
            //        AppViewBackButtonVisibility.Collapsed;
            //}
        }

        private void share_button_Click(object sender, RoutedEventArgs e)
        {
            //分享得分
            DataTransferManager.ShowShareUI();
        }

        async private void DataTransferManager_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            DataPackage data = args.Request.Data;
            data.Properties.Title = "快来加入我们的2048吧~";
            data.Properties.Description = "我在2048上的最高分是：" + Player.HighestScore;
            DataRequestDeferral GetFiles = args.Request.GetDeferral();
            try
            {
                string ImagePath = Directory.GetCurrentDirectory() + "\\Assets\\";
                StorageFolder folder = await StorageFolder.GetFolderFromPathAsync(ImagePath);
                StorageFile imageFile = await folder.GetFileAsync("1.jpg");
                data.Properties.Thumbnail = RandomAccessStreamReference.CreateFromFile(imageFile);
                data.SetBitmap(RandomAccessStreamReference.CreateFromFile(imageFile));
            }
            finally
            {
                GetFiles.Complete();
            }
        }

        private void restart_button_Click(object sender, RoutedEventArgs e)
        {
            //重新开始
            Frame.Navigate(typeof(GamePage), Player);
        }
    }
}
