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
    /// lianxulab.xaml 的交互逻辑
    /// </summary>
    public partial class lianxulab : UserControl
    {
        public lianxulab()
        {
            InitializeComponent();
        }

        public void create_ell(int type)//生成图
        {
            Ellipse ell = new Ellipse();
            ell.Width = 36;
            ell.Height = 36;
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
                    duilab.Foreground = Brushes.OrangeRed;
                    duilab.Margin = new Thickness(10, 8, 0, 0);
                    lianxucanvas.Children.Add(duilab);
                }
                else
                {
                    ell.Stroke = Brushes.DodgerBlue;
                    Label duilab = new Label();
                    duilab.Content = "对";
                    duilab.Foreground = Brushes.DodgerBlue;
                    duilab.Margin = new Thickness(10, 8, 0, 0);
                    lianxucanvas.Children.Add(duilab);
                }



                
            }
            ell.StrokeThickness = 6;
            ell.SnapsToDevicePixels = true;
            ell.Margin = new Thickness(3, 3, 0, 0);
            lianxucanvas.Children.Add(ell);
        }
    }
}
