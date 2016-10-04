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
    /// danshuanglab.xaml 的交互逻辑
    /// </summary>
    public partial class danshuanglab : UserControl
    {
        public danshuanglab()
        {
            InitializeComponent();
        }

        public void createlab(int dan_shuang,int dou)
        {
            Rectangle rect = new Rectangle();
            rect.Width = 37;
            rect.Height = 37;
            if (dan_shuang == 1)
            {
                rect.Fill = Brushes.DodgerBlue;
            }
            else
            {
                rect.Fill = Brushes.OrangeRed;
            }
            rect.SnapsToDevicePixels = true;
            rect.Margin = new Thickness(2, 2, 0, 0);
            maincanvas.Children.Add(rect);
            Label dslab = new Label();
            dslab.Foreground = Brushes.White;
            //dslab.FontWeight = FontWeights.Bold;
            dslab.FontSize = 18;
            if (dan_shuang == 1)
            {
                dslab.Content = "单";
            }
            else
            {
                dslab.Content = "双";
            }
            maincanvas.Children.Add(dslab);

            if (dou > -1)
            {
                Label doulab = new Label();
                doulab.Foreground = Brushes.White;
                doulab.Content = dou+"对";
                doulab.FontSize = 14;
                doulab.Margin = new Thickness(12, 18, 0, 0);
                maincanvas.Children.Add(doulab);
            }
        }
    }
}
