using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上提供

namespace _2048
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class RegisterPage : Page
    {
        public RegisterPage()
        {
            this.InitializeComponent();
            var viewTitleBar = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().TitleBar;
            viewTitleBar.BackgroundColor = Windows.UI.Colors.CornflowerBlue;
            viewTitleBar.ButtonBackgroundColor = Windows.UI.Colors.CornflowerBlue;
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
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

        private async void pictrue_select(object sender, RoutedEventArgs e)
        {
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".jpeg");
            openPicker.FileTypeFilter.Add(".png");
            StorageFile file = await openPicker.PickSingleFileAsync();
            if (file != null)
            {
                IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.Read);
                BitmapImage pic_ = new BitmapImage();
                pic_.SetSource(stream);
                this.image.Source = pic_;
            }
        }

        private async void register_click(object sender, RoutedEventArgs e)
        {
            var i = new MessageDialog("");
            if (name_re.Text == "")
            {
                i.Content = "请填写你的昵称";
                await i.ShowAsync();
            }
            else if(account_re.Text == "")
            {
                i.Content = "请填写你的账号";
                await i.ShowAsync();
            }
            else if(password_re.Text == "")
            {
                i.Content = "请填写你的密码";
                await i.ShowAsync();
            }
            else if (DateTime.Today < birthdate.Date)
            {
                var a = new MessageDialog("你的生日选择不正确，请重新选择").ShowAsync();
            }
            else
            {
                //若成功注册，则返回登录界面
                var db = App.conn;
                try
                {
                    using (var MyItem = db.Prepare("INSERT INTO Players (Username, Password, Account, Birthday, HighestScore) VALUES (?,?,?,?, ?)"))
                    {
                        MyItem.Bind(1, name_re.Text);
                        MyItem.Bind(2, password_re.Text);
                        MyItem.Bind(3, account_re.Text);
                        MyItem.Bind(4, birthdate.Date.ToString("s"));
                        MyItem.Bind(5, 0);
                        MyItem.Step();

                    }
                    Models.player play = new Models.player(name_re.Text, password_re.Text, account_re.Text, birthdate.Date.ToString("s"));
                    var mes = new MessageDialog("regist success").ShowAsync();
                    Frame.Navigate(typeof(EnterPage), play);
                }
                catch (Exception ex)
                {

                }
               
            }
            
            //若数据库中已经存在name，则注册不成功（需添加代码else if）
        }
    }
}
