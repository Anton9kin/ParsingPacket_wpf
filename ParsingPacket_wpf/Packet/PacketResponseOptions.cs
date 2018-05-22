using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Windows;

namespace ParsingPacket_wpf.Packet
{
    class PacketResponseOptions : PacketBase
    {
        private Int64 Packet_Time { get; set; }	// Time create packet
        private Byte ActivityPeriod { get; set; }// Period send Activity packet
        private Byte Indication { get; set; }       // Enable/Disable indication in Activity mode
        private UInt16 TelemetryPeriod { get; set; } // Telemetry period (bit 0-7: in activity mode bits:8-15 in passive)
        private CommandFromServer comServer;

        public PacketResponseOptions(string[] dataPack) {
            Parameter param;
            int n = 0;
            int length = dataPack.Length;

            if (parsing(dataPack) == false)
            {
                MessageBox.Show("Not correct data", "Warning", MessageBoxButton.OK);
                return;
            }

            data = new string[length - 4 - 5];
            for (int i = 5; i < length - 4; i++)
                data[i - 5] = dataPack[i];


            string s = getStr(ref n, sizeof(Int64));
            Packet_Time = Convert.ToInt64(s, 16);

            s = getStr(ref n, sizeof(byte));
            ActivityPeriod = Convert.ToByte(s, 16);

            s = getStr(ref n, sizeof(byte));
            Indication = Convert.ToByte(s, 16);

            s = getStr(ref n, sizeof(UInt16));
            TelemetryPeriod = Convert.ToUInt16(s, 16);

            param = TimestampToDate(Packet_Time);
            list.Add(param);

            param = new Parameter { Param = "Period", Value = ActivityPeriod.ToString() };
            list.Add(param);

            param = new Parameter { Param = "Indication", Value = Indication == 1 ? "ON" : "OFF" };
            list.Add(param);

            param = new Parameter { Param = "Telemetry_Active", Value = (TelemetryPeriod & 0xFF).ToString() };
            list.Add(param);

            param = new Parameter { Param = "Telemetry_Passive", Value = (TelemetryPeriod >> 8).ToString() };
            list.Add(param);

            comServer = new CommandFromServer(data, 12);
            list.AddRange(comServer.list);
        }
    }


}
