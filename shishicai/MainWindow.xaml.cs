using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Xceed.Wpf.AvalonDock.Layout;
using System.Timers;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;

namespace shishicai
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        Timer timer = new Timer();
        DateTime nowdate = new DateTime();
        DateTime chongqing_last_update_time = new DateTime();

        public static T FindChild<T>(DependencyObject parent, string childName)//查找控件
where T : DependencyObject
        {
            if (parent == null) return null;
            T foundChild = null;
            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                // 如果子控件不是需查找的控件类型 
                T childType = child as T;
                if (childType == null)
                {
                    // 在下一级控件中递归查找 
                    foundChild = FindChild<T>(child, childName);
                    // 找到控件就可以中断递归操作  
                    if (foundChild != null) break;
                }
                else if (!string.IsNullOrEmpty(childName))
                {
                    var frameworkElement = child as FrameworkElement;
                    // 如果控件名称符合参数条件 
                    if (frameworkElement != null && frameworkElement.Name == childName)
                    {
                        foundChild = (T)child;
                        break;
                    }
                }
                else
                {
                    // 查找到了控件 
                    foundChild = (T)child;
                    break;
                }
            }
            return foundChild;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //chongqing newchongqing = new chongqing();
            //LayoutDocument cq = new LayoutDocument();
            //cq.Content = newchongqing;
            //cq.Title = "重庆时时彩";
            //avapane.Children.Add(cq);

            //xinjiang newxinjiang = new xinjiang();
            //LayoutDocument xj = new LayoutDocument();
            //xj.Content = newxinjiang;
            //xj.Title = "新疆时时彩";
            //avapane.Children.Add(xj);

            tremlist newtrem = new tremlist();
            newtrem.Name = "newtrem";
            leftpanel.Content = newtrem;


            rightlianxu rightcq = new rightlianxu();
            rightcq.Name = "rightcq";
            rightcq.titlelable.Content = "重庆时时彩";
            //rightpanel.Content = rightcq;
            rightstack.Children.Add(rightcq);

            //rightlianxu rightxj = new rightlianxu();
            //rightxj.Name = "rightxj";
            //rightxj.titlelable.Content = "新疆时时彩";
            ////rightpanel.Content = rightxj;
            //rightstack.Children.Add(rightxj);

            sub_chongqing chongqing = new sub_chongqing();
            chongqing.Name = "chongqing";
            LayoutDocument cq = new LayoutDocument();
            cq.Content = chongqing;
            cq.Title = "重庆时时彩";
            avapane.Children.Add(cq);
            





            timer.Interval = 1000;
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            //init_trem();
            PublicClass.init_trem("cqssc");
            create_lianxu_coor("cqssc");
            timer.Start();

        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            modify_chongqing_last_update_time();
            Dispatcher.Invoke(new Action(delegate
            {
                sub_chongqing chongqing = MainWindow.FindChild<sub_chongqing>(Application.Current.MainWindow, "chongqing");
                if (chongqing != null)
                {
                    chongqing.last_update_time = chongqing_last_update_time;
                }
            }));
            if (PublicClass.ChongqingJson.Count > 120  || PublicClass.ChongqingJson.Count == 0)
            {
                Dispatcher.Invoke(new Action(delegate
                {
                PublicClass.init_trem("cqssc");
                create_lianxu_coor("cqssc");
                }));
            }
            if (DateTime.Now > chongqing_last_update_time)
            {
                cur_trem();
                create_lianxu_coor("cqssc");
            }

        }

        private void cur_trem()//调用当前开奖记录
        {
            string str = null;
            HttpWebResponse response = null;
            StreamReader reader = null;
            //nowdate = DateTime.Now;
            //if (nowdate.Hour < 2)
            //{
            //    nowdate = nowdate.Subtract(TimeSpan.FromDays(1));
            //}

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://t.apiplus.cn/daily.do?code=cqssc&format=json"); ;
                request.Method = "GET";
                request.Timeout = 10000;
                response = (HttpWebResponse)request.GetResponse();
                reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.GetEncoding("UTF-8"));
                str = reader.ReadToEnd();
                str = str.Substring(str.IndexOf('['), str.Length - str.IndexOf('[') - 1);
                JArray jsonstr = JArray.Parse(str);
                for (int i = 0; i < jsonstr.Count; i++)
                {
                    int curjson_count = (from c in PublicClass.ChongqingJson where c["expect"].ToString() == jsonstr[i]["expect"].ToString() select c).Count();
                    if (curjson_count == 0)
                    {
                        PublicClass.ChongqingJson.Add(jsonstr[i]);
                        create_lianxu_coor("cqssc");
                    }
                }
            }
            catch
            {
            }
        }

        private void modify_chongqing_last_update_time()//修正下次更新时间
        {
            try
            {
                chongqing_last_update_time = DateTime.Parse(PublicClass.ChongqingJson[PublicClass.ChongqingJson.Count - 1]["opentime"].ToString());
            }
            catch { }
            if (chongqing_last_update_time >= DateTime.Parse(DateTime.Today.ToLongDateString() + "10:00:00") && chongqing_last_update_time < DateTime.Parse(DateTime.Today.ToLongDateString() + "22:00:00"))
            {
                chongqing_last_update_time = chongqing_last_update_time.AddMinutes(10);
            }
            else if (chongqing_last_update_time > DateTime.Parse(DateTime.Today.ToLongDateString() + "22:00:00") && chongqing_last_update_time < DateTime.Parse(DateTime.Today.ToLongDateString() + "23:59:59"))
            {
                chongqing_last_update_time = chongqing_last_update_time.AddMinutes(5);
            }
            else if (chongqing_last_update_time > DateTime.Parse(DateTime.Today.ToLongDateString() + "00:00:00") && chongqing_last_update_time < DateTime.Parse(DateTime.Today.ToLongDateString() + "2:00:00"))
            {
                chongqing_last_update_time = chongqing_last_update_time.AddMinutes(5);
            }
            else
            {
                chongqing_last_update_time = chongqing_last_update_time.AddHours(7);
            }
        }

        private void create_lianxu_coor(string code)
        {
            List<JToken> myJson = new List<JToken>();
            if (code == "cqssc")
            {
                myJson = PublicClass.ChongqingJson;
            }
            Point next_d = new Point(0, 0);
            Point next_s = new Point(0, 0);
            Point cur = new Point(0, 0);
            int[,] fill_lianxu = new int[121, 7];
            for (int i = 0; i < myJson.Count; i++)
            {
                string opencode = myJson[i]["opencode"].ToString();
                string[] sopencode = opencode.Split(',');
                int mod = int.Parse(sopencode[4]);
                mod = mod % 2;

                int dou = 0;
                if (sopencode[3] == sopencode[4])
                {
                    dou = 10;
                }


                if (i == 0)
                {
                    if (mod == 0)
                    {
                        fill_lianxu[0, 0] = 2 + dou;
                        next_d = new Point(1, 0);
                        next_s = new Point(0, 1);
                    }
                    else
                    {
                        fill_lianxu[0, 0] = 1 + dou;
                        next_d = new Point(0, 1);
                        next_s = new Point(1, 0);
                    }
                }
                else if (mod == 0)
                {
                    fill_lianxu[(int)next_s.X, (int)next_s.Y] = 2 + dou;
                    next_d = new Point(next_s.X + 1, 0);
                    if (fill_lianxu[(int)next_s.X, (int)next_s.Y + 1] == 0)
                    {
                        if (next_s.Y + 1 == 6 || fill_lianxu[(int)next_s.X, (int)next_s.Y + 1] == 1)
                        {
                            next_s = new Point(next_s.X + 1, next_s.Y);
                            for (int b = (int)next_d.X; b > 0; b--)
                            {
                                if (fill_lianxu[b, 0] == 0)
                                {
                                    next_d = new Point(b, 0);
                                }
                            }
                        }
                        else
                        {
                            next_s = new Point(next_s.X, next_s.Y + 1);
                        }
                    }
                    else
                    {
                        next_s = new Point(next_s.X, next_s.Y + 1);
                    }
                }
                else
                {
                    fill_lianxu[(int)next_d.X, (int)next_d.Y] = 1 + dou;
                    next_s = new Point(next_d.X + 1, 0);
                    if (fill_lianxu[(int)next_d.X, (int)next_d.Y + 1] == 0)
                    {
                        if (next_d.Y + 1 == 6 || fill_lianxu[(int)next_d.X, (int)next_d.Y + 1] == 2)
                        {
                            next_d = new Point(next_d.X + 1, next_d.Y);
                            for (int b = (int)next_s.X; b > 0; b--)
                            {
                                if (fill_lianxu[b, 0] == 0)
                                {
                                    next_s = new Point(b, 0);
                                }
                            }
                        }
                        else
                        {
                            next_d = new Point(next_d.X, next_d.Y + 1);
                        }
                    }
                    else
                    {
                        next_d = new Point(next_d.X, next_d.Y + 1);
                    }
                }
            }


            Dispatcher.Invoke(new Action(delegate
            {
                rightlianxu rightcq = MainWindow.FindChild<rightlianxu>(Application.Current.MainWindow, "rightcq");

                for (int i = 0; i < 120; i++)
                {
                    for (int j = 0; j < 6; j++)
                    {
                        rightcq.fill_lianxu[i, j] = fill_lianxu[i, j];
                    }
                }
                rightcq.create_lianxu_coor();


            }));
        }






    }
}
