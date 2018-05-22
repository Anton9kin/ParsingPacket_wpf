﻿using System;
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

                string[] dataStr = Data.Text.Split(' ');
                

                List<Parameter> list = new List<Parameter>();

                PacketType typePacket = new PacketType();
                typePacket.Check( Byte.Parse(dataStr[0], System.Globalization.NumberStyles.HexNumber) );

                if (typePacket.type == PacketType.TypePacket.Null)
                {
                    MessageBox.Show("Not correct data", "Warning", MessageBoxButton.OK);
                    outData.Items.Clear();
                    return;
                }

                switch (typePacket.type) {
                    case PacketType.TypePacket.Activity:
                        PacketActivity activ = new PacketActivity(dataStr);
                        list = activ.list;
                        break;
                    case PacketType.TypePacket.Generic_Resp:
                        PacketGenericResponse genResp = new PacketGenericResponse(dataStr);
                        list = genResp.list;
                        break;
                    case PacketType.TypePacket.Info_Device_Options_Req:
                        PacketRequestOptions getOpt = new PacketRequestOptions(dataStr);
                        list = getOpt.list;
                        break;
                    case PacketType.TypePacket.Info_Device_Options_Resp:
                        PacketResponseOptions respOpt = new PacketResponseOptions(dataStr);
                        list = respOpt.list;
                        break;
                    case PacketType.TypePacket.Telemetry:
                        PacketTelemetry pt = new PacketTelemetry(dataStr);
                        list = pt.list;
                        break;
                    case PacketType.TypePacket.UDP_Statistic:
                        PacketStatistic ps = new PacketStatistic(dataStr);
                        list = ps.list;
                        break;
                    case PacketType.TypePacket.Device_Error:
                        PacketDeviceInfo di = new PacketDeviceInfo(dataStr);
                        list = di.list;
                        break;
                    case PacketType.TypePacket.Location:
                    case PacketType.TypePacket.Location_2:
                        PacketLocation pl = new PacketLocation(dataStr);
                        list = pl.list;
                        break;
                    case PacketType.TypePacket.Info_Update_Req:
                        PacketUpdateRequest ru = new PacketUpdateRequest(dataStr);
                        list = ru.list;
                        break;
                    case PacketType.TypePacket.Info_Update_Resp:
                        PacketUpdateResponse ur = new PacketUpdateResponse(dataStr);
                        list = ur.list;
                        break;
                }

                //PacketBase pack = new PacketBase();
                //parse data
                //if (pack.parsing(dataStr) == false)
                //{
                //    MessageBox.Show("Not correct data", "Warning", MessageBoxButton.OK);
                 //   outData.Items.Clear();
                 //   return;
                //}

                //add parsing data to list
                //list.AddRange(pack.list);

                //add list to grid
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
