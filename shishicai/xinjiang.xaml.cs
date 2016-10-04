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
using System.Timers;
using System.Net;
using Newtonsoft.Json.Linq;
using System.IO;

namespace shishicai
{
    /// <summary>
    /// xinjiang.xaml 的交互逻辑
    /// </summary>
    public partial class xinjiang : UserControl
    {
        public xinjiang()
        {
            InitializeComponent();
        }
        Timer timer = new Timer();
        DateTime nowdate = new DateTime();
        DateTime nowtime = new DateTime();
        DateTime last_update_time = new DateTime();
        int[,] fill_lianxu;
        string coordinate;
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //nowdate = DateTime.Now;
            PublicClass.XinjiangJson.Clear();
            DateTime cc = DateTime.Parse(DateTime.Today.ToLongDateString() + "10:00:00");

            timer.Interval = 1000;
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            init_trem();
            create_line();
        }

               private void modify_last_update_time()//修正下次更新时间
        {
            try
            {
                last_update_time = DateTime.Parse(PublicClass.XinjiangJson[PublicClass.XinjiangJson.Count - 1]["opentime"].ToString());
            }
            catch { }
            if (last_update_time >= DateTime.Parse(DateTime.Today.ToLongDateString() + "07:00:00") && last_update_time < DateTime.Parse(DateTime.Today.ToLongDateString() + "23:59:59"))
            {
                last_update_time = last_update_time.AddMinutes(10);
            }
            else if (last_update_time > DateTime.Parse(DateTime.Today.ToLongDateString() + "00:00:00") && last_update_time < DateTime.Parse(DateTime.Today.ToLongDateString() + "02:00:00"))
            {
                last_update_time = last_update_time.AddMinutes(5);
            }
            else
            {
                last_update_time = last_update_time.AddHours(5);
            }
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {

            modify_last_update_time();
            int last_str = int.Parse(DateTime.Now.ToString().Substring(DateTime.Now.ToString().Length - 1, 1));
            last_str = last_str % 1;
            if (Math.Abs(DateTime.Now.Day - last_update_time.Day) >= 1)
            {
                init_trem();
            }
            if (DateTime.Now > last_update_time && last_str == 0)
            {
                cur_trem();
            }
            else
            {
                Dispatcher.Invoke(new Action(delegate
                {
                    show_update_time.Content = "距离下期开奖还剩： "+ (last_update_time - DateTime.Now).ToString().Substring(0, (last_update_time - DateTime.Now).ToString().IndexOf('.'));
                }));
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
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://t.apiplus.cn/daily.do?code=xjssc&format=json"); ;
                request.Method = "GET";
                request.Timeout = 10000;
                response = (HttpWebResponse)request.GetResponse();
                reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.GetEncoding("UTF-8"));
                str = reader.ReadToEnd();
                str = str.Substring(str.IndexOf('['), str.Length - str.IndexOf('[') - 1);
                JArray jsonstr = JArray.Parse(str);

                //foreach (var c in jsonstr)
                //{
                //    int u;
                //}


                for (int i = 0; i < jsonstr.Count; i++)
                {
                    int curjson_count = (from c in PublicClass.XinjiangJson where c["expect"].ToString() == jsonstr[i]["expect"].ToString() select c).Count();
                    if (curjson_count==0)
                    {
                        PublicClass.XinjiangJson.Add(jsonstr[i]);

                        Dispatcher.Invoke(new Action(delegate
                        {
                            tremlist newtrem = MainWindow.FindChild<tremlist>(Application.Current.MainWindow, "newtrem");
                            if (newtrem != null)
                            {
                                //newtrem.tr
                                newtrem.cul_list(nowdate.ToLongDateString(),"xj");
                            }
                        }));

                        modify_last_update_time();
                        UI_process();
                        create_danshuang_lab();
                        create_lianxu_coor();
                    }
                }
            }
            catch
            {
            }
        }
            
            
        

        private void init_trem()//调用当日全部开奖记录
        {
            string str = null; 
            HttpWebResponse response = null;
            StreamReader reader = null;
            //DateTime nowdate = DateTime.Now;
            nowdate = DateTime.Now;

            for (int i = 0; i < 10; i++)
            {
                try
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://t.apiplus.cn/daily.do?code=xjssc&date=" + nowdate + "&format=json");
                    request.Method = "GET";
                    request.Timeout = 10000;
                    response = (HttpWebResponse)request.GetResponse();
                    i = 10;
                }
                catch { }

            }
                 reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.GetEncoding("UTF-8")); 
                 str = reader.ReadToEnd();
                 str = str.Substring(str.IndexOf('['), str.Length - str.IndexOf('[') - 1);
                 JArray jsonstr = JArray.Parse(str);

                 //foreach (var c in jsonstr)
                 //{
                 //    int u;
                 //}


                 for (int i = jsonstr.Count-1; i >= 0; i--)
                 {
                     PublicClass.XinjiangJson.Add(jsonstr[i]);
                 }
                 UI_process();
                 create_danshuang_lab();
                 create_lianxu_coor();



             tremlist newtrem = MainWindow.FindChild<tremlist>(Application.Current.MainWindow, "newtrem");
             if (newtrem != null)
             {
                 //newtrem.tr
                 newtrem.cul_list(nowdate.ToLongDateString(),"xj");
             }
             modify_last_update_time();
             timer.Start();
             
        }

        private void UI_process()//处理UI
        {
            Dispatcher.Invoke(new Action(delegate
            {
                try
                {
                    qi.Content = PublicClass.XinjiangJson[PublicClass.XinjiangJson.Count - 1]["expect"].ToString();
                    //cur_circle newcircle = new cur_circle();
                    //newcircle.Width = 60;
                    //newcircle.Height = 60;

                    //circle.Children.Add(newcircle);
                    for (int i = 0; i < circle.Children.Count; i++)
                    {
                        cur_circle delcircle = circle.Children[i] as cur_circle;
                        if (delcircle != null)
                        {
                            circle.Children.Remove(delcircle);
                            i--;
                        }
                    }


                    string[] opencode = PublicClass.XinjiangJson[PublicClass.XinjiangJson.Count - 1]["opencode"].ToString().Split(',');
                    foreach (string sopencode in opencode)
                    {
                        cur_circle newcircle = new cur_circle();
                        newcircle.Width = 65;
                        newcircle.Height = 60;
                        newcircle.num.Content = sopencode;
                        circle.Children.Add(newcircle);
                    }
                }
                catch { }
            }));

        }

        private void create_line()
        {
            //////////////单双基线//////////////
            for (int i = 0; i < 7; i++)
            {
                Line newline = new Line();
                newline.X1 = 0;
                newline.Y1 = i*40;
                newline.X2 = 4800;
                newline.Y2 = i*40;
                newline.Stroke = Brushes.DodgerBlue;
                newline.StrokeThickness = 1;
                newline.SnapsToDevicePixels = true;
                newline.Opacity = 0.2;
                danshuangcanvas.Children.Add(newline);
            }

            for (int i = 0; i < 120; i++)
            {
                Line newline = new Line();
                newline.X1 = i * 40;
                newline.Y1 = 0;
                newline.X2 = i*40;
                newline.Y2 = 240;
                newline.Stroke = Brushes.DodgerBlue;
                newline.StrokeThickness = 1;
                newline.SnapsToDevicePixels = true;
                newline.Opacity = 0.2;
                danshuangcanvas.Children.Add(newline);
            }

            /////////////连续基线////////////////
            for (int i = 0; i < 7; i++)
            {
                Line newline = new Line();
                newline.X1 = 0;
                newline.Y1 = i * 40;
                newline.X2 = 4800;
                newline.Y2 = i * 40;
                newline.Stroke = Brushes.DodgerBlue;
                newline.StrokeThickness = 1;
                newline.SnapsToDevicePixels = true;
                newline.Opacity = 0.2;
                lianxucanvas.Children.Add(newline);
            }

            for (int i = 0; i < 120; i++)
            {
                Line newline = new Line();
                newline.X1 = i * 40;
                newline.Y1 = 0;
                newline.X2 = i * 40;
                newline.Y2 = 240;
                newline.Stroke = Brushes.DodgerBlue;
                newline.StrokeThickness = 1;
                newline.SnapsToDevicePixels = true;
                newline.Opacity = 0.2;
                lianxucanvas.Children.Add(newline);
            }


        }
        private void create_danshuang_lab()//生成单双标志
        {
            Dispatcher.Invoke(new Action(delegate
            {

                for (int i = 0; i < danshuangcanvas.Children.Count; i++)
                {
                    danshuanglab del_danshuang = danshuangcanvas.Children[i] as danshuanglab;
                    if (del_danshuang != null)
                    {
                        danshuangcanvas.Children.Remove(del_danshuang);
                        i--;
                    }
                }

                for (int i = 0; i < 20; i++)
                {
                    for (int j = 0; j < 6; j++)
                    {
                        try
                        {
                            string[] opencode = PublicClass.XinjiangJson[i * 6 + j]["opencode"].ToString().Split(',');
                            danshuanglab newdanshuan = new danshuanglab();
                            int ds = -1;
                            int dou = -1;
                            //int ccc = int.Parse(opencode[4].ToString()) % 2;
                            if (int.Parse(opencode[4].ToString()) % 2 == 0)
                            {
                                ds = 0;
                            }
                            else
                            {
                                ds = 1;
                            }

                            if (opencode[3] == opencode[4])
                            {
                                dou = int.Parse(opencode[4].ToString());
                            }

                            newdanshuan.createlab(ds, dou);

                            newdanshuan.Margin = new Thickness(i*40, j*40, 0, 0);
                            danshuangcanvas.Children.Add(newdanshuan);
                        }
                        catch { }
                    }
                }




            }));
        }

        

        private void create_lianxulab()//生成连续标志
        {
            Dispatcher.Invoke(new Action(delegate
            {
                
                fill_lianxu = new int[7, 121];
                for (int i = 0; i < 7; i++)
                {
                    for (int j = 0; j < 121; j++)
                    {
                        fill_lianxu[i, j] = 0;
                    }
                }

                for (int i = 0; i < 20; i++)
                {
                    for (int j = 0; j < 6; j++)
                    {
                        try
                        {
                            coordinate = "";
                            recur_create(0, 0, i * 6 + j);
                        }
                        catch { }
                    }
                }



            }));
        }


        private void create_lianxu_coor()//正向生成连续坐标
        {
            Point next_d = new Point(0, 0);
            Point next_s = new Point(0, 0);
            Point cur = new Point(0,0);
            fill_lianxu = new int[121, 7];
            for (int i = 0; i < PublicClass.XinjiangJson.Count; i++)
            {
                string opencode = PublicClass.XinjiangJson[i]["opencode"].ToString();
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
                        fill_lianxu[0, 0] = 2+dou;
                        next_d = new Point(1, 0);
                        next_s = new Point(0, 1);
                    }
                    else
                    {
                        fill_lianxu[0, 0] = 1+dou;
                        next_d = new Point(0, 1);
                        next_s = new Point(1, 0);
                    }
                }
                else if (mod == 0)
                {
                    fill_lianxu[(int)next_s.X, (int)next_s.Y] = 2+dou;
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
                    fill_lianxu[(int)next_d.X, (int)next_d.Y] = 1+dou;
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
            for (int i = 0; i < lianxucanvas.Children.Count; i++)
            {
                lianxulab dellab = lianxucanvas.Children[i] as lianxulab;
                if (dellab != null)
                {
                    lianxucanvas.Children.Remove(dellab);
                    i--;
                }
            }

                for (int i = 0; i < 120; i++)
                {
                    for (int j = 0; j < 6; j++)
                    {
                        if (fill_lianxu[i, j] != 0)
                        {
                            int type = 0;
                            if (fill_lianxu[i, j] == 1)
                            {
                                type = 1;
                            }
                            else if (fill_lianxu[i, j] > 9)
                            {
                                type = fill_lianxu[i, j];
                            }

                            lianxulab lianxu = new lianxulab();
                            lianxu.create_ell(type);
                            lianxu.Margin = new Thickness(i * 40, j * 40, 0, 0);
                            lianxucanvas.Children.Add(lianxu);
                        }
                    }
                }


                rightlianxu rightxj = MainWindow.FindChild<rightlianxu>(Application.Current.MainWindow, "rightxj");

                for (int i = 0; i < 120; i++)
                {
                    for (int j = 0; j < 6; j++)
                    {
                        rightxj.fill_lianxu[i, j] = fill_lianxu[i, j];
                    }
                }
                rightxj.create_lianxu_coor();



            }));

        }


        private void create_sumlab()//生成统计
        {

        }


        private void recur_create(int x,int y,int index)//递归处理图
        {

            if (coordinate == "")
            {
                if (y == 6)
                {
                    recur_create(x+1, 0, index);
                }
                if (fill_lianxu[x, y] == 0)
                {
                    int mod = int.Parse(PublicClass.XinjiangJson[index].ToString()) % 2;
                    if (mod == 0)
                    {
                        if (y == 0 && x == 0)
                        {
                            coordinate = x + "," + y;
                            fill_lianxu[x, y] = 2;
                        }
                        else if (fill_lianxu[x, y + 1] == 1)
                        {
                            coordinate = x + "," + y;
                            fill_lianxu[x, y] = 2;
                        }
                        else if (fill_lianxu[x-1, y] != 2)
                        {
                            coordinate = x + "," + y;
                            fill_lianxu[x, y] = 2;
                        }
 
                    }
                }
            }

            


 
        }

    }
}
