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

namespace shishicai
{
    /// <summary>
    /// rightlianxu.xaml 的交互逻辑
    /// </summary>
    public partial class rightlianxu : UserControl
    {
        public rightlianxu()
        {
            InitializeComponent();
        }
        public int[,] fill_lianxu;
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            fill_lianxu = new int[121, 7];
            for (int i = 0; i < 7; i++)
            {
                Line newline = new Line();
                newline.X1 = 0;
                newline.Y1 = i*20;
                newline.X2 = 2400;
                newline.Y2 = i*20;
                newline.Stroke = Brushes.DodgerBlue;
                newline.StrokeThickness = 1;
                newline.Opacity = 0.2;
                newline.SnapsToDevicePixels = true;
                main.Children.Add(newline);
            }
            for (int i = 0; i < 121; i++)
            {
                Line newline = new Line();
                newline.X1 = i*20;
                newline.Y1 = 0;
                newline.X2 = i*20;
                newline.Y2 = 120;
                newline.Stroke = Brushes.DodgerBlue;
                newline.StrokeThickness = 1;
                newline.Opacity = 0.2;
                newline.SnapsToDevicePixels = true;
                main.Children.Add(newline);
            }
        }

        public void create_lianxu_coor()//正向生成连续坐标
        {
            Dispatcher.Invoke(new Action(delegate
            {
                for (int i = 0; i < main.Children.Count; i++)
                {
                    smalllianxu dellab = main.Children[i] as smalllianxu;
                    if (dellab != null)
                    {
                        main.Children.Remove(dellab);
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

                            smalllianxu lianxu = new smalllianxu();
                            lianxu.create_ell(type);
                            lianxu.Margin = new Thickness(i * 20, j * 20, 0, 0);
                            main.Children.Add(lianxu);
                        }
                    }
                }
            }));

        }

    }
}
