using SQLitePCL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

//“空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409 上有介绍

namespace _2048
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            var viewTitleBar = Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().TitleBar;
            viewTitleBar.BackgroundColor = Windows.UI.Colors.CornflowerBlue;
            viewTitleBar.ButtonBackgroundColor = Windows.UI.Colors.CornflowerBlue;
        }

        private async void login_Click(object sender, RoutedEventArgs e)
        {
            var i = new MessageDialog("");
           
            //若没有输入账号密码就开始登录
            if(account.Text == "")
            {
                i.Content = "请输入账号";
                await i.ShowAsync();
            }
            else if(password.Text == "")
            {
                i.Content = "请输入密码";
                await i.ShowAsync();
            }
            else
            {
                var db = App.conn;
                using (var statement = db.Prepare("SELECT * FROM Players WHERE Account = ? AND Password = ?"))
                {
                    statement.Bind(1, account.Text);
                    statement.Bind(2, password.Text);
                    if (statement.Step() == SQLiteResult.ROW)
                    {
                        try
                        {
                            var username = (string)statement[1];
                            var password = (string)statement[2];
                            var account = (string)statement[3];
                            long highestScore = (long)statement[4];
                            App.Player = new Models.player(username, password, account, (string)statement[5], highestScore);
                            //若登录成功
                            Frame.Navigate(typeof(EnterPage), App.Player);
                        }
                        catch (Exception err)
                        {

                        }
                    }
                    else
                    {
                        var p = new MessageDialog("请输入正确的账号和密码").ShowAsync();
                    }
                }
            }

            //若登录不成功（账号密码不匹配之类）需添加代码（还有一个else if）

        }

        private void register_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(RegisterPage), "");
        }
    }
}
