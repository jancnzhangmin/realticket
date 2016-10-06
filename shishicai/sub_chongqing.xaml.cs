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

namespace shishicai
{
    /// <summary>
    /// sub_chongqing.xaml 的交互逻辑
    /// </summary>
    public partial class sub_chongqing : UserControl
    {
        public sub_chongqing()
        {
            InitializeComponent();
        }
        Timer timer = new Timer();
        public DateTime last_update_time = new DateTime();
        int[,] fill_lianxu;
        bool first_create_flag = true;
        int cur_count = -1;
        int time_sum;

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //last_update_time = DateTime.Parse("1999/1/1");
            create_line();
            //cur_trem();
            timer.Interval = 1000;
            timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            timer.Start();
            //cur_count = PublicClass.ChongqingJson.Count;
            //init_trem();
            
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (first_create_flag && PublicClass.ChongqingJson.Count>0)
            {
                cur_trem();
                first_create_flag = false;
            }

            if (DateTime.Now > last_update_time && last_update_time > DateTime.Parse("2000/1/1") && cur_count!=PublicClass.ChongqingJson.Count)
            {
                cur_trem();
            }
            else
            {
                Dispatcher.Invoke(new Action(delegate
                {
                    if (last_update_time - DateTime.Now>=TimeSpan.FromSeconds(0))
                    {
                        show_update_time.Content = "距离下期开奖还剩： " + (last_update_time - DateTime.Now).ToString().Substring(0, (last_update_time - DateTime.Now).ToString().IndexOf('.'));
                    }
                    else
                    {
                        show_update_time.Content = "开奖中...";
                    }
                }));
            }

            if (PublicClass.ChongqingJson.Count > 0)
            {
                cur_count = PublicClass.ChongqingJson.Count;
            }
            if (time_sum == 20 && PublicClass.ChongqingJson.Count == 0)
            {
                PublicClass.init_trem("cqssc");
            }
            time_sum++;
            if (time_sum > 20)
            {
                time_sum = 0;
            }

        }

        private void create_line()
        {
            //////////////单双基线//////////////
            for (int i = 0; i < 7; i++)
            {
                Line newline = new Line();
                newline.X1 = 0;
                newline.Y1 = i * 30;
                newline.X2 = 3600;
                newline.Y2 = i * 30;
                newline.Stroke = Brushes.DodgerBlue;
                newline.StrokeThickness = 1;
                newline.SnapsToDevicePixels = true;
                newline.Opacity = 0.2;
                danshuangcanvas.Children.Add(newline);
            }

            for (int i = 0; i < 120; i++)
            {
                Line newline = new Line();
                newline.X1 = i * 30;
                newline.Y1 = 0;
                newline.X2 = i * 30;
                newline.Y2 = 180;
                newline.Stroke = Brushes.DodgerBlue;
                newline.StrokeThickness = 1;
                newline.SnapsToDevicePixels = true;
                newline.Opacity = 0.2;
                danshuangcanvas.Children.Add(newline);
            }

            /////////////龙虎基线////////////////
            for (int i = 0; i < 7; i++)
            {
                Line newline = new Line();
                newline.X1 = 0;
                newline.Y1 = i * 30;
                newline.X2 = 3600;
                newline.Y2 = i * 30;
                newline.Stroke = Brushes.DodgerBlue;
                newline.StrokeThickness = 1;
                newline.SnapsToDevicePixels = true;
                newline.Opacity = 0.2;
                longhucanvas.Children.Add(newline);
            }

            for (int i = 0; i < 120; i++)
            {
                Line newline = new Line();
                newline.X1 = i * 30;
                newline.Y1 = 0;
                newline.X2 = i * 30;
                newline.Y2 = 180;
                newline.Stroke = Brushes.DodgerBlue;
                newline.StrokeThickness = 1;
                newline.SnapsToDevicePixels = true;
                newline.Opacity = 0.2;
                longhucanvas.Children.Add(newline);
            }

            /////////////连续基线////////////////
            for (int i = 0; i < 7; i++)
            {
                Line newline = new Line();
                newline.X1 = 0;
                newline.Y1 = i * 30;
                newline.X2 = 3600;
                newline.Y2 = i * 30;
                newline.Stroke = Brushes.DodgerBlue;
                newline.StrokeThickness = 1;
                newline.SnapsToDevicePixels = true;
                newline.Opacity = 0.2;
                lianxucanvas.Children.Add(newline);
            }

            for (int i = 0; i < 121; i++)
            {
                Line newline = new Line();
                newline.X1 = i * 30;
                newline.Y1 = 0;
                newline.X2 = i * 30;
                newline.Y2 = 180;
                newline.Stroke = Brushes.DodgerBlue;
                newline.StrokeThickness = 1;
                newline.SnapsToDevicePixels = true;
                newline.Opacity = 0.2;
                lianxucanvas.Children.Add(newline);
            }


        }

        private void cur_trem()//调用当前开奖记录
        {
            Dispatcher.Invoke(new Action(delegate
            {
                tremlist newtrem = MainWindow.FindChild<tremlist>(Application.Current.MainWindow, "newtrem");
                if (newtrem != null)
                {
                    newtrem.cul_list(DateTime.Now.ToLongDateString(), "cq");
                }
            }));

            try
            {
                UI_process();
                create_danshuang_lab();
                create_longhu_lab();
                create_lianxu_coor();
            }
            catch
            {
            }
        }

        private void UI_process()//处理UI
        {
            try
            {
                Dispatcher.Invoke(new Action(delegate
                {
                    qi.Content = PublicClass.ChongqingJson[PublicClass.ChongqingJson.Count - 1]["expect"].ToString();
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


                    string[] opencode = PublicClass.ChongqingJson[PublicClass.ChongqingJson.Count - 1]["opencode"].ToString().Split(',');
                    foreach (string sopencode in opencode)
                    {
                        cur_circle newcircle = new cur_circle();
                        newcircle.Width = 65;
                        newcircle.Height = 60;
                        newcircle.num.Content = sopencode;
                        circle.Children.Add(newcircle);
                    }
                }));
            }
            catch { }

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
                            string[] opencode = PublicClass.ChongqingJson[i * 6 + j]["opencode"].ToString().Split(',');
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

                            newdanshuan.Margin = new Thickness(i * 30, j * 30, 0, 0);
                            danshuangcanvas.Children.Add(newdanshuan);
                        }
                        catch { }
                    }
                }




            }));
        }


        private void create_longhu_lab()//生成龙虎标志
        {
            Dispatcher.Invoke(new Action(delegate
            {

                for (int i = 0; i < longhucanvas.Children.Count; i++)
                {
                    longhu del_longhu = longhucanvas.Children[i] as longhu;
                    if (del_longhu != null)
                    {
                        longhucanvas.Children.Remove(del_longhu);
                        i--;
                    }
                }

                for (int i = 0; i < 20; i++)
                {
                    for (int j = 0; j < 6; j++)
                    {
                        try
                        {
                            string[] opencode = PublicClass.ChongqingJson[i * 6 + j]["opencode"].ToString().Split(',');
                            longhu newlonghu = new longhu();
                            if (int.Parse( opencode[1]) >int.Parse( opencode[4]))
                            {
                                newlonghu.createlab("龙");
                            }
                            else if (int.Parse(opencode[1]) < int.Parse(opencode[4]))
                            {
                                newlonghu.createlab("虎");
                            }
                            else
                            {
                                newlonghu.createlab("和");
                            }

                            newlonghu.Margin = new Thickness(i * 30, j * 30, 0, 0);
                            longhucanvas.Children.Add(newlonghu);
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
            Point cur = new Point(0, 0);
            fill_lianxu = new int[121, 7];
            for (int i = 0; i < PublicClass.ChongqingJson.Count; i++)
            {
                string opencode = PublicClass.ChongqingJson[i]["opencode"].ToString();
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
                            lianxu.Margin = new Thickness(i * 30, j * 30, 0, 0);
                            lianxucanvas.Children.Add(lianxu);
                        }
                    }
                }


                //rightlianxu rightcq = MainWindow.FindChild<rightlianxu>(Application.Current.MainWindow, "rightcq");

                //for (int i = 0; i < 120; i++)
                //{
                //    for (int j = 0; j < 6; j++)
                //    {
                //        rightcq.fill_lianxu[i, j] = fill_lianxu[i, j];
                //    }
                //}
                //rightcq.create_lianxu_coor();


            }));

        }



    }
}
