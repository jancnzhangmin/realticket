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
    /// tremlist.xaml 的交互逻辑
    /// </summary>
    public partial class tremlist : UserControl
    {
        public tremlist()
        {
            InitializeComponent();
        }

        public void cul_list(string nowtime,string local)//列出开奖列表
        {
            try
            {
                for (int i = 0; i < trem_list.Items.Count;i++ )
                {
                    StackPanel temstack = trem_list.Items[i] as StackPanel;
                    if (temstack != null)
                    {
                        trem_list.Items.Remove(temstack);
                        i--;
                    }
                }
            }
            catch { }

            StackPanel titlestack = new StackPanel();
            Label titlelable = new Label();
            titlelable.Foreground = Brushes.Red;
            titlelable.FontWeight = FontWeights.Bold;
            titlelable.Content = nowtime + " 开奖号码";
            titlestack.Children.Add(titlelable);
            trem_list.Items.Add(titlestack);

            int json_count=0;
            if (local == "cq")
            {
                json_count = PublicClass.ChongqingJson.Count;
            }
            else if (local == "xj")
            {
                json_count = PublicClass.XinjiangJson.Count;
            }

            for (int i = 0; i < json_count ; i++)
            {
                StackPanel contentstack = new StackPanel();
                contentstack.Orientation = Orientation.Horizontal;
                Label smtitle = new Label();
                smtitle.Foreground = Brushes.DimGray;
                if (local == "cq")
                {
                    smtitle.Content = PublicClass.ChongqingJson[i]["expect"].ToString().Substring(8, 3);
                }
                else if (local == "xj")
                {
                    smtitle.Content = PublicClass.XinjiangJson[i]["expect"].ToString().Substring(8, 3);
                }
                contentstack.Children.Add(smtitle);

                Label contentlab = new Label();
                contentlab.Foreground = Brushes.Red;
                contentlab.FontWeight = FontWeights.Bold;

                string[] Sopencode=new string[5];
                if (local == "cq")
                {
                    Sopencode = PublicClass.ChongqingJson[i]["opencode"].ToString().Split(',');
                }
                else if (local == "xj")
                {
                    Sopencode = PublicClass.XinjiangJson[i]["opencode"].ToString().Split(',');
                }



                foreach (string s in Sopencode)
                {
                    contentlab.Content += " " + s;
                }

                //contentstack.Children.Add(smtitle);
                contentstack.Children.Add(contentlab);
                trem_list.Items.Add(contentstack);
            }


        }
    }
}
