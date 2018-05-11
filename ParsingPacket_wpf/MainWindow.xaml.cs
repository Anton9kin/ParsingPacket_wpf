using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ParsingPacket_wpf
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public void ParsePacket(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter) {
                Parameter param = new Parameter { Param = "Param", Value = "Value" };
                
                List<Parameter> list = new List<Parameter>();
                list.Add(param);

                param = new Parameter { Param="Hello", Value="World" };
                list.Add(param);

                param = new Parameter();

                param.Param = "Hi";
                param.Value = "WPF";
                list.Add(param);

                foreach (Parameter s in list) {
                    outData.Items.Add(s);
                }
            }
        }
        public void ClickData(object sender, MouseButtonEventArgs e) {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (Data.Text == "Data")
                    Data.Text = "";
            }
        }
    }
}
