using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using ParsingPacket_wpf.Packet;

namespace ParsingPacket_wpf
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Parameter> list;

        public MainWindow()
        {
            InitializeComponent();
        }

        public void ParsePacket(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter) {

                outData.Items.Clear();

                string str = GetText();

                str = CheckStringData(str);

                Data.Document.Blocks.Clear();

                SetText(str);
                
                string[] dataStr = str.Split(' ');

                List<byte> dataByte = new List<byte>();
                foreach (string s in dataStr)
                {
                    dataByte.Add(Byte.Parse(s, System.Globalization.NumberStyles.HexNumber));
                }

                list = new List<Parameter>();
                int size = 0;

                while (dataByte.Count > 0)
                {
                    if (list.Count > 0)
                    {
                        list.Add(new Parameter { Param = "", Value = "" });
                        list.Add(new Parameter { Param = "", Value = "" });
                    }
                    PacketType typePacket = new PacketType();

                    typePacket.Check(dataByte[0]);

                    if (typePacket.type == PacketType.TypePacket.Null)
                    {
                        MessageBox.Show("Not correct data", "Warning", MessageBoxButton.OK);
                        outData.Items.Clear();
                        return;
                    }

                    switch (typePacket.type)
                    {
                        case PacketType.TypePacket.Activity:
                            PacketActivity ra = new PacketActivity(dataByte);
                            list.AddRange(ra.GetListParam());
                            break;
                        case PacketType.TypePacket.Generic_Resp:
                            PacketGenericResponse gr = new PacketGenericResponse(dataByte);
                            list.AddRange(gr.GetListParam());
                            break;
                        case PacketType.TypePacket.Info_Device_Options_Req:
                            PacketRequestOptions getOpt = new PacketRequestOptions(dataByte);
                            list.AddRange(getOpt.GetListParam());
                            break;
                        case PacketType.TypePacket.Info_Device_Options_Resp:
                            PacketResponseOptions respOpt = new PacketResponseOptions(dataByte);
                            list.AddRange(respOpt.GetListParam());
                            break;
                        case PacketType.TypePacket.Telemetry:
                            PacketTelemetry pt = new PacketTelemetry(dataByte);
                            list.AddRange(pt.GetListParam());
                            break;
                        case PacketType.TypePacket.UDP_Statistic:
                            PacketStatistic ps = new PacketStatistic(dataByte);
                            list.AddRange(ps.GetListParam());
                            break;
                        case PacketType.TypePacket.Device_Error:
                            PacketDeviceInfo di = new PacketDeviceInfo(dataByte);
                            list.AddRange(di.GetListParam());
                            break;
                        case PacketType.TypePacket.Location:
                        case PacketType.TypePacket.Location_2:
                            PacketLocation rl = new PacketLocation(dataByte);
                            list.AddRange(rl.GetListParam());
                            break;
                        case PacketType.TypePacket.Info_Update_Req:
                            PacketUpdateRequest ru = new PacketUpdateRequest(dataStr);
                            list = ru.list;
                            break;
                        case PacketType.TypePacket.Info_Update_Resp:
                            PacketUpdateResponse ur = new PacketUpdateResponse(dataStr);
                            list = ur.list;
                            break;
                        case PacketType.TypePacket.Info_Update_Result:
                            PacketResultUpdate rr = new PacketResultUpdate(dataStr);
                            list = rr.list;
                            break;
                    }
                }
                
                //add list to grid
                foreach (Parameter s in list)
                {
                    outData.Items.Add(s);
                }
            }
        }
        public void ClickData(object sender, MouseButtonEventArgs e) {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (GetText() == "Data")
                    SetText ("");
            }
        }

        private string CheckStringData(string str)
        {
            string newS = "";

            var re = new Regex("\r\n");

            newS = re.Replace(str, "");

            return newS;
        }

        private string GetText()
        {
            return new TextRange(Data.Document.ContentStart, Data.Document.ContentEnd).Text;
        }

        private void SetText(string s)
        {
            FlowDocument document = new FlowDocument();
            Paragraph paragraph = new Paragraph();
            paragraph.Inlines.Add(s);
            document.Blocks.Add(paragraph);
            Data.Document = document;
        }
    }
}
