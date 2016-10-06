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
            rect.Width = 27;
            rect.Height = 27;
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
            dslab.FontSize = 14;
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
                doulab.Content = dou;
                doulab.FontSize = 12;
                doulab.Margin = new Thickness(14, 8, 0, 0);
                maincanvas.Children.Add(doulab);
            }
        }
    }
}
