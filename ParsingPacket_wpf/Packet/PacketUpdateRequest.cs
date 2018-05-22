using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ParsingPacket_wpf.Packet
{
    class PacketUpdateRequest : PacketBase
    {
        private Int64 Packet_Time;    ///< Time create packet
        private byte[] Version = new byte[4];   	///< Name current version of device

        public PacketUpdateRequest(string[] dataPack)
        {
            Parameter p;
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

            for (int i = 0; i < 4; i++)
            {
                s = getStr(ref n, sizeof(byte));
                Version[i] = Convert.ToByte(s, 16);
            }

            p = getCCID(data, n);
            n += 20;

            list.Add(p);

            p = TimestampToDate(Packet_Time);
            list.Add(p);

            p = new Parameter { Param = "Version", Value = String.Format("{0}.{1}.{2}.{3}", Version[0], Version[1], Version[2], Version[3]) };
            list.Add(p);
        }
    }
}
