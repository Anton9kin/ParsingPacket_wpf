using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ParsingPacket_wpf.Packet
{
    class PacketTelemetry : PacketBase
    {
        private Int64 Packet_Time { get; set; }    // Time in packet
        private UInt16 Num { get; set; }     // Number of struct #ST_Type_Telemetry

        public PacketTelemetry(string[] dataPack)
        {
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

            param = TimestampToDate(Packet_Time);
            list.Add(param);

            Num = Convert.ToUInt16(getStr(ref n, sizeof(UInt16)), 16);
            param = new Parameter { Param = "Num of struct", Value = Num.ToString() };
            list.Add(param);

            param = getCCID(data, 10);
            list.Add(param);
            n += 20;

            Byte type = 0;
            UInt32 dataType = 0;

            for (UInt16 i = 0; i < Num; i++)
            {
                type = Convert.ToByte(getStr(ref n, sizeof(byte)), 16);
                dataType = Convert.ToUInt32(getStr(ref n, sizeof(UInt32)), 16);

                TelemetryType tt = new TelemetryType(type, dataType);
                list.AddRange(tt.list);
            }
        }
    }
}
