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
    /// longhu.xaml 的交互逻辑
    /// </summary>
    public partial class longhu : UserControl
    {
        public longhu()
        {
            InitializeComponent();
        }

        public void createlab(string longhu)
        {
            Label longhulab = new Label();
            if (longhu == "龙")
            {
                longhulab.Background = Brushes.OrangeRed;
            }
            else if (longhu == "虎")
            {
                longhulab.Background = Brushes.DodgerBlue;
            }
            else
            {
                longhulab.Background = Brushes.ForestGreen;
            }
            longhulab.Foreground = Brushes.White;
            longhulab.Width = 27;
            longhulab.Height = 27;
            longhulab.Content = longhu;
            //longhulab.FontWeight = FontWeights.Bold;
            //dslab.FontWeight = FontWeights.Bold;
            longhulab.FontSize = 14;

            //maincanvas.Children.Add(longhulab);


            longhulab.Margin = new Thickness(1, 0, 0, 0);
            maincanvas.Children.Add(longhulab);
            
        }
    }
}
