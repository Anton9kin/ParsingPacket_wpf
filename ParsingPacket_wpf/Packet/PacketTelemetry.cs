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
            int length = dataPack.Length;

            if (parsing(dataPack) == false)
            {
                MessageBox.Show("Not correct data", "Warning", MessageBoxButton.OK);
                return;
            }

            data = new string[length - 4 - 5];
            for (int i = 5; i < length - 4; i++)
                data[i - 5] = dataPack[i];

            string s = data[7] + data[6] + data[5] + data[4] + data[3] + data[2] + data[1] + data[0];
            Packet_Time = Convert.ToInt64(s, 16);
            param = TimestampToDate(Packet_Time);
            list.Add(param);

            Num = Convert.ToUInt16(data[9] + data[8], 16);
            param = new Parameter { Param = "Num of struct", Value = Num.ToString() };
            list.Add(param);

            param = getCCID(data, 10);
            list.Add(param);


            Byte type = 0;
            UInt32 dataType = 0;

            for (UInt16 i = 0; i < Num; i++)
            {
                type = Convert.ToByte(data[5*i + 30], 16);
                dataType = Convert.ToUInt32(data[5 * i + 34] + data[5 * i + 33] + data[5 * i + 32] + data[5 * i + 31], 16);

                TelemetryType tt = new TelemetryType(type, dataType);
                list.AddRange(tt.list);
            }
        }
    }
}
