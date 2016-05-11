using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.ServiceModel.Channels;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Input;
using Windows.UI.Popups;
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
    public sealed partial class GamePage : Page, INotifyPropertyChanged
    {
        public GamePage()
        {
            this.InitializeComponent();
            for (int i = 1; i < 5; i++)
                for (int j = 1; j < 5; j++)
                {
                    remains.Insert(remains.Count, i * 10 + j);
                }
            randomProduce();
            randomProduce();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        enum DIRECTION { LEFT, RIGHT, UP, DOWN };
        DIRECTION direction;
        enum TYPE { t_0000, t_0001, t_0010, t_0011, t_0100, t_0101, t_0110, t_0111, t_1000, t_1001, t_1010, t_1011, t_1100, t_1101, t_1110, t_1111, t_null };
        TYPE[] types = new TYPE[4];
        private int _scores = -4;
        public int scores
        {
            get
            {
                return _scores;
            }
            set
            {
                _scores = value;
                NotifyPropertyChanged();
            }
        }
        private Models.player Player;
        public List<int> remains = new List<int>();
        public List<int> occupies = new List<int>();
        PointerPoint press;
        PointerPoint release;
        bool isMoved = true;

        private TYPE str2type(string t)
        {
            switch (t)
            {
                case "0000":
                    return TYPE.t_0000;
                case "0001":
                    return TYPE.t_0001;
                case "0010":
                    return TYPE.t_0010;
                case "0011":
                    return TYPE.t_0011;
                case "0100":
                    return TYPE.t_0100;
                case "0101":
                    return TYPE.t_0101;
                case "0110":
                    return TYPE.t_0110;
                case "0111":
                    return TYPE.t_0111;
                case "1000":
                    return TYPE.t_1000;
                case "1001":
                    return TYPE.t_1001;
                case "1010":
                    return TYPE.t_1010;
                case "1011":
                    return TYPE.t_1011;
                case "1100":
                    return TYPE.t_1100;
                case "1101":
                    return TYPE.t_1101;
                case "1110":
                    return TYPE.t_1110;
                case "1111":
                    return TYPE.t_1111;
            }
            return TYPE.t_null;
        }

        public void randomProduce()
        {
            if (!isMoved)
            {
                isMoved = true;
                return;
            }
            int len = remains.Count;
            var ran = new Random();
            int ran_num = ran.Next(0, len);
            int pos = remains[ran_num];
            remains.Remove(pos);
            occupies.Add(pos);
            Button obj = (Button)FindName("b" + pos);
            obj.Content = 2;
            scores += (int)obj.Content;
        }

        public void setButtonStyle(Button button, int buttonContent)
        {
            switch (buttonContent)
            {
                case 2:
                    {
                        break;
                    }
            }
        }

        private void detectTypes()
        {
            switch (direction)
            {
                case DIRECTION.LEFT:
                    {
                        for (int i = 1; i < 5; i++)
                        {
                            string temp = "";
                            temp += occupies.Contains(1 + i * 10) ? '1' : '0';
                            temp += occupies.Contains(2 + i * 10) ? '1' : '0';
                            temp += occupies.Contains(3 + i * 10) ? '1' : '0';
                            temp += occupies.Contains(4 + i * 10) ? '1' : '0';
                            types[i - 1] = str2type(temp);
                        }
                        break;
                    }
                case DIRECTION.RIGHT:
                    {
                        for (int i = 1; i < 5; i++)
                        {
                            string temp = "";
                            temp += occupies.Contains(4 + i * 10) ? '1' : '0';
                            temp += occupies.Contains(3 + i * 10) ? '1' : '0';
                            temp += occupies.Contains(2 + i * 10) ? '1' : '0';
                            temp += occupies.Contains(1 + i * 10) ? '1' : '0';
                            types[i - 1] = str2type(temp);
                        }
                        break;
                    }
                case DIRECTION.UP:
                    {
                        for (int i = 1; i < 5; i++)
                        {
                            string temp = "";
                            temp += occupies.Contains(10 + i) ? '1' : '0';
                            temp += occupies.Contains(20 + i) ? '1' : '0';
                            temp += occupies.Contains(30 + i) ? '1' : '0';
                            temp += occupies.Contains(40 + i) ? '1' : '0';
                            types[i - 1] = str2type(temp);
                        }
                        break;
                    }
                case DIRECTION.DOWN:
                    {
                        for (int i = 1; i < 5; i++)
                        {
                            string temp = "";
                            temp += occupies.Contains(40 + i) ? '1' : '0';
                            temp += occupies.Contains(30 + i) ? '1' : '0';
                            temp += occupies.Contains(20 + i) ? '1' : '0';
                            temp += occupies.Contains(10 + i) ? '1' : '0';
                            types[i - 1] = str2type(temp);
                        }
                        break;
                    }
            }
        }

        private void updateRO()
        {
            // 新一轮记录之前先清空两个 List
            occupies.Clear();
            remains.Clear();

            // 依次判断每个 Button 的占用情况
            // 分别 Add 到两个 List 中
            if (b11.Content == null)
                remains.Add(11);
            else
                occupies.Add(11);

            if (b12.Content == null)
                remains.Add(12);
            else
                occupies.Add(12);

            if (b13.Content == null)
                remains.Add(13);
            else
                occupies.Add(13);

            if (b14.Content == null)
                remains.Add(14);
            else
                occupies.Add(14);

            if (b21.Content == null)
                remains.Add(21);
            else
                occupies.Add(21);

            if (b22.Content == null)
                remains.Add(22);
            else
                occupies.Add(22);

            if (b23.Content == null)
                remains.Add(23);
            else
                occupies.Add(23);

            if (b24.Content == null)
                remains.Add(24);
            else
                occupies.Add(24);

            if (b31.Content == null)
                remains.Add(31);
            else
                occupies.Add(31);

            if (b32.Content == null)
                remains.Add(32);
            else
                occupies.Add(32);

            if (b33.Content == null)
                remains.Add(33);
            else
                occupies.Add(33);

            if (b34.Content == null)
                remains.Add(34);
            else
                occupies.Add(34);

            if (b41.Content == null)
                remains.Add(41);
            else
                occupies.Add(41);

            if (b42.Content == null)
                remains.Add(42);
            else
                occupies.Add(42);

            if (b43.Content == null)
                remains.Add(43);
            else
                occupies.Add(43);

            if (b44.Content == null)
                remains.Add(44);
            else
                occupies.Add(44);

        }

        private void updateOnce()
        {
            Button b_1 = null, b_2 = null, b_3 = null, b_4 = null;
            int movedCount = 0;
            for (int i = 1; i < 5; i++)
            {
                if (direction == DIRECTION.LEFT)
                {
                    b_1 = (Button)FindName("b" + (i * 10 + 1));
                    b_2 = (Button)FindName("b" + (i * 10 + 2));
                    b_3 = (Button)FindName("b" + (i * 10 + 3));
                    b_4 = (Button)FindName("b" + (i * 10 + 4));
                }
                else if (direction == DIRECTION.RIGHT)
                {
                    b_1 = (Button)FindName("b" + (i * 10 + 4));
                    b_2 = (Button)FindName("b" + (i * 10 + 3));
                    b_3 = (Button)FindName("b" + (i * 10 + 2));
                    b_4 = (Button)FindName("b" + (i * 10 + 1));
                }
                else if (direction == DIRECTION.UP)
                {
                    b_1 = (Button)FindName("b" + (i + 10));
                    b_2 = (Button)FindName("b" + (i + 20));
                    b_3 = (Button)FindName("b" + (i + 30));
                    b_4 = (Button)FindName("b" + (i + 40));
                }
                else
                {
                    b_1 = (Button)FindName("b" + (i + 40));
                    b_2 = (Button)FindName("b" + (i + 30));
                    b_3 = (Button)FindName("b" + (i + 20));
                    b_4 = (Button)FindName("b" + (i + 10));
                }
                #region
                switch (types[i - 1])
                {
                    case TYPE.t_0000:
                    case TYPE.t_1000:
                    default:
                        {
                            movedCount++;
                            break;
                        }
                    case TYPE.t_0001:
                        {
                            b_1.Content = b_4.Content;
                            b_4.Content = null;
                            break;
                        }
                    case TYPE.t_0010:
                        {
                            b_1.Content = b_3.Content;
                            b_3.Content = null;
                            break;
                        }
                    case TYPE.t_0011:
                        {
                            if ((int)b_3.Content == (int)b_4.Content)
                            {
                                b_1.Content = (int)b_3.Content * 2;
                                scores += (int)b_1.Content;
                                b_4.Content = b_3.Content = null;
                            }
                            else
                            {
                                b_1.Content = b_3.Content;
                                b_2.Content = b_4.Content;
                                b_4.Content = b_3.Content = null;
                            }
                            break;
                        }
                    case TYPE.t_0100:
                        {
                            b_1.Content = b_2.Content;
                            b_2.Content = null;
                            break;
                        }
                    case TYPE.t_0101:
                        {
                            if ((int)b_2.Content == (int)b_4.Content)
                            {
                                b_1.Content = (int)b_2.Content * 2;
                                scores += (int)b_1.Content;
                                b_2.Content = b_4.Content = null;
                            }
                            else
                            {
                                b_1.Content = b_2.Content;
                                b_2.Content = b_4.Content;
                                b_4.Content = null;
                            }
                            break;
                        }
                    case TYPE.t_0110:
                        {
                            if ((int)b_2.Content == (int)b_3.Content)
                            {
                                b_1.Content = (int)b_2.Content * 2;
                                scores += (int)b_1.Content;
                                b_2.Content = b_3.Content = null;
                            }
                            else
                            {
                                b_1.Content = b_2.Content;
                                b_2.Content = b_3.Content;
                                b_3.Content = null;
                            }
                            break;
                        }
                    case TYPE.t_0111:
                        {
                            if ((int)b_2.Content == (int)b_3.Content)
                            {
                                b_1.Content = (int)b_2.Content * 2;
                                scores += (int)b_1.Content;
                                b_2.Content = b_4.Content;
                                b_3.Content = b_4.Content = null;
                            }
                            else if ((int)b_3.Content == (int)b_4.Content)
                            {
                                b_1.Content = b_2.Content;
                                b_2.Content = (int)b_3.Content * 2;
                                scores += (int)b_2.Content;
                                b_3.Content = b_4.Content = null;
                            }
                            else
                            {
                                b_1.Content = b_2.Content;
                                b_2.Content = b_3.Content;
                                b_3.Content = b_4.Content;
                                b_4.Content = null;
                            }
                            break;
                        }
                    case TYPE.t_1001:
                        {
                            if ((int)b_1.Content == (int)b_4.Content)
                            {
                                b_1.Content = (int)b_1.Content * 2;
                                scores += (int)b_1.Content;
                                b_4.Content = null;
                            }
                            else
                            {
                                b_2.Content = b_4.Content;
                                b_4.Content = null;
                            }
                            break;
                        }
                    case TYPE.t_1010:
                        {
                            if ((int)b_1.Content == (int)b_3.Content)
                            {
                                b_1.Content = (int)b_1.Content * 2;
                                scores += (int)b_1.Content;
                                b_3.Content = null;
                            }
                            else
                            {
                                b_2.Content = b_3.Content;
                                b_3.Content = null;
                            }
                            break;
                        }
                    case TYPE.t_1011:
                        {
                            if ((int)b_1.Content == (int)b_3.Content)
                            {
                                b_1.Content = (int)b_1.Content * 2;
                                scores += (int)b_1.Content;
                                b_2.Content = b_4.Content;
                                b_3.Content = b_4.Content = null;
                            }
                            else if ((int)b_3.Content == (int)b_4.Content)
                            {
                                b_2.Content = (int)b_3.Content * 2;
                                scores += (int)b_2.Content;
                                b_3.Content = b_4.Content = null;
                            }
                            else
                            {
                                b_2.Content = b_3.Content;
                                b_3.Content = b_4.Content;
                                b_4.Content = null;
                            }
                            break;
                        }
                    case TYPE.t_1100:
                        {
                            if ((int)b_1.Content == (int)b_2.Content)
                            {
                                b_1.Content = (int)b_1.Content * 2;
                                scores += (int)b_1.Content;
                                b_2.Content = null;
                            }
                            else
                            {
                                movedCount++;
                            }
                            break;
                        }
                    case TYPE.t_1101:
                        {
                            if ((int)b_1.Content == (int)b_2.Content)
                            {
                                b_1.Content = (int)b_1.Content * 2;
                                scores += (int)b_1.Content;
                                b_2.Content = b_4.Content;
                                b_4.Content = null;
                            }
                            else if ((int)b_2.Content == (int)b_4.Content)
                            {
                                b_2.Content = (int)b_2.Content * 2;
                                scores += (int)b_2.Content;
                                b_4.Content = null;
                            }
                            else
                            {
                                b_3.Content = b_4.Content;
                                b_4.Content = null;
                            }
                            break;
                        }
                    case TYPE.t_1110:
                        {
                            if ((int)b_1.Content == (int)b_2.Content)
                            {
                                b_1.Content = (int)b_1.Content * 2;
                                scores += (int)b_1.Content;
                                b_2.Content = b_3.Content;
                                b_3.Content = null;
                            }
                            else if ((int)b_2.Content == (int)b_3.Content)
                            {
                                b_2.Content = (int)b_2.Content * 2;
                                scores += (int)b_2.Content;
                                b_3.Content = null;
                            }
                            else
                            {
                                movedCount++;
                            }
                            break;
                        }
                    case TYPE.t_1111:
                        {
                            if ((int)b_1.Content == (int)b_2.Content)
                            {
                                b_1.Content = (int)b_1.Content * 2;
                                scores += (int)b_1.Content;
                                if ((int)b_3.Content == (int)b_4.Content)
                                {
                                    b_2.Content = 2 * (int)b_3.Content;
                                    scores += (int)b_2.Content;
                                    b_3.Content = b_4.Content = null;
                                }
                                else
                                {
                                    b_2.Content = b_3.Content;
                                    b_3.Content = b_4.Content;
                                    b_4.Content = null;
                                }
                            }
                            else if ((int)b_2.Content == (int)b_3.Content)
                            {
                                b_2.Content = (int)b_2.Content * 2;
                                scores += (int)b_2.Content;
                                b_3.Content = b_4.Content;
                                b_4.Content = null;
                            }
                            else if ((int)b_3.Content == (int)b_4.Content)
                            {
                                b_3.Content = (int)b_3.Content * 2;
                                scores += (int)b_3.Content;
                                b_4.Content = null;
                            }
                            else
                            {
                                movedCount++;
                            }
                            break;
                        }
                }
                #endregion
            }
            if (movedCount == 4)
                isMoved = false;
        }

        private bool gameOver()
        {
            if (remains.Count != 0)
                return false;
            if ((int)b11.Content == (int)b12.Content || (int)b11.Content == (int)b21.Content)
                return false;
            if ((int)b12.Content == (int)b13.Content || (int)b12.Content == (int)b22.Content)
                return false;
            if ((int)b13.Content == (int)b14.Content || (int)b13.Content == (int)b23.Content)
                return false;
            if ((int)b14.Content == (int)b24.Content)
                return false;
            if ((int)b21.Content == (int)b22.Content || (int)b21.Content == (int)b31.Content)
                return false;
            if ((int)b22.Content == (int)b23.Content || (int)b22.Content == (int)b32.Content)
                return false;
            if ((int)b23.Content == (int)b24.Content || (int)b23.Content == (int)b33.Content)
                return false;
            if ((int)b24.Content == (int)b34.Content)
                return false;
            if ((int)b31.Content == (int)b32.Content || (int)b31.Content == (int)b41.Content)
                return false;
            if ((int)b32.Content == (int)b33.Content || (int)b32.Content == (int)b42.Content)
                return false;
            if ((int)b33.Content == (int)b34.Content || (int)b33.Content == (int)b43.Content)
                return false;
            if ((int)b34.Content == (int)b44.Content)
                return false;
            if ((int)b41.Content == (int)b42.Content)
                return false;
            if ((int)b42.Content == (int)b43.Content)
                return false;
            if ((int)b43.Content == (int)b44.Content)
                return false;
            return true;
        }

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
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            Player.CurrentScore = scores;
            if (Player.HighestScore < scores)
            {
                Player.HighestScore = scores;
                Player.update();
            }
        }
        //private void mousePressed(object sender, PointerRoutedEventArgs e)
        //{
        //    Pointer currentPoint = e.Pointer;
        //    press = PointerPoint.GetCurrentPoint(currentPoint.PointerId);

        //    //Windows.UI.Input.PointerPoint ptrPt = e.GetCurrentPoint(currentPoint.PointerId);

        //}

        //private void mouseReleased(object sender, PointerRoutedEventArgs e)
        //{
        //    Pointer currentPoint = e.Pointer;
        //    release = PointerPoint.GetCurrentPoint(currentPoint.PointerId);
        //    double dx = release.Position.X - press.Position.X;
        //    double dy = release.Position.Y - press.Position.Y;
        //    if (Math.Abs(dx) > Math.Abs(dy))
        //    {
        //        if (dx > 0)
        //            direction = DIRECTION.RIGHT;
        //        else
        //            direction = DIRECTION.LEFT;
        //    }
        //    else
        //    {
        //        if (dy < 0)
        //            direction = DIRECTION.UP;
        //        else
        //            direction = DIRECTION.DOWN;
        //    }
        //    update();
        //}

        private void update()
        {
            detectTypes();
            updateOnce();
            updateRO();
            randomProduce();
            if (gameOver())
                Frame.Navigate(typeof(GradePage), Player);
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            Window.Current.CoreWindow.KeyDown += CoreWindow_KeyDown;
            Window.Current.CoreWindow.PointerPressed += CoreWindow_PointerPressed;
            Window.Current.CoreWindow.PointerReleased += CoreWindow_PointerReleased;
        }

        private void CoreWindow_KeyDown(CoreWindow coreWindow, KeyEventArgs e)
        {
            if (e.VirtualKey == Windows.System.VirtualKey.Up)
                direction = DIRECTION.UP;
            else if (e.VirtualKey == Windows.System.VirtualKey.Down)
                direction = DIRECTION.DOWN;
            else if (e.VirtualKey == Windows.System.VirtualKey.Left)
                direction = DIRECTION.LEFT;
            else if (e.VirtualKey == Windows.System.VirtualKey.Right)
                direction = DIRECTION.RIGHT;
            update();
        }

        private void CoreWindow_PointerPressed(CoreWindow sender, PointerEventArgs args)
        {
            PointerPoint currentPoint = args.CurrentPoint;
            press = PointerPoint.GetCurrentPoint(currentPoint.PointerId);

            //Windows.UI.Input.PointerPoint ptrPt = e.GetCurrentPoint(currentPoint.PointerId);
        }

        private void CoreWindow_PointerReleased(CoreWindow sender, PointerEventArgs args)
        {
            var currentPoint = args.CurrentPoint;
            release = PointerPoint.GetCurrentPoint(currentPoint.PointerId);
            double dx = release.Position.X - press.Position.X;
            double dy = release.Position.Y - press.Position.Y;
            if (Math.Abs(dx) > Math.Abs(dy))
            {
                if (dx > 0)
                    direction = DIRECTION.RIGHT;
                else
                    direction = DIRECTION.LEFT;
            }
            else
            {
                if (dy < 0)
                    direction = DIRECTION.UP;
                else
                    direction = DIRECTION.DOWN;
            }
            update();
        }


    }
}