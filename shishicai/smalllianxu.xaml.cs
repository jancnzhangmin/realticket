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
    /// smalllianxu.xaml 的交互逻辑
    /// </summary>
    public partial class smalllianxu : UserControl
    {
        public smalllianxu()
        {
            InitializeComponent();
        }

        public void create_ell(int type)//生成图
        {

            Ellipse ell = new Ellipse();
            ell.Width = 18;
            ell.Height = 18;
            if (type == 0)
            {
                ell.Stroke = Brushes.OrangeRed;
            }
            else if (type == 1)
            {
                ell.Stroke = Brushes.DodgerBlue;
            }
            else
            {
                int gewei = int.Parse(type.ToString().Substring(type.ToString().Length - 1, 1));
                if (gewei % 2 == 0)
                {
                    ell.Stroke = Brushes.OrangeRed;
                    Label duilab = new Label();
                    duilab.Content = "对";
                    duilab.FontSize = 8;
                    duilab.Foreground = Brushes.OrangeRed;
                    duilab.Margin = new Thickness(2, 0, 0, 0);
                    lianxucanvas.Children.Add(duilab);
                }
                else
                {
                    ell.Stroke = Brushes.DodgerBlue;
                    Label duilab = new Label();
                    duilab.Content = "对";
                    duilab.FontSize = 8;
                    duilab.Foreground = Brushes.DodgerBlue;
                    duilab.Margin = new Thickness(2, 0, 0, 0);
                    lianxucanvas.Children.Add(duilab);
                }




            }
            ell.StrokeThickness = 3;
            ell.SnapsToDevicePixels = true;
            ell.Margin = new Thickness(2, 2, 0, 0);
            lianxucanvas.Children.Add(ell);
        }

    }
}
