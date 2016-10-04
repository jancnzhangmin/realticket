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
            chongqing newchongqing = new chongqing();
            LayoutDocument cq = new LayoutDocument();
            cq.Content = newchongqing;
            cq.Title = "重庆时时彩";
            avapane.Children.Add(cq);

            xinjiang newxinjiang = new xinjiang();
            LayoutDocument xj = new LayoutDocument();
            xj.Content = newxinjiang;
            xj.Title = "新疆时时彩";
            avapane.Children.Add(xj);

            tremlist newtrem = new tremlist();
            newtrem.Name = "newtrem";
            leftpanel.Content = newtrem;


            rightlianxu rightcq = new rightlianxu();
            rightcq.Name = "rightcq";
            rightcq.titlelable.Content = "重庆时时彩";
            //rightpanel.Content = rightcq;
            rightstack.Children.Add(rightcq);

            rightlianxu rightxj = new rightlianxu();
            rightxj.Name = "rightxj";
            rightxj.titlelable.Content = "新疆时时彩";
            //rightpanel.Content = rightxj;
            rightstack.Children.Add(rightxj);



        }


    }
}
