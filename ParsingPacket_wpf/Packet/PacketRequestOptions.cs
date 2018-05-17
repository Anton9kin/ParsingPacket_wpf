using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParsingPacket_wpf.Packet
{
    class PacketRequestOptions : PacketBase
    {
        private Int64 Packet_Time { get; set; }	// Time create packet
        private int[] CCID { get; set; } = new int[20];       // CCID of device

        public PacketRequestOptions(string[] data) {
            Parameter param;

            string s = data[7] + data[6] + data[5] + data[4] + data[3] + data[2] + data[1] + data[0];
            Packet_Time = Convert.ToInt64(s, 16);
            param = TimestampToDate(Packet_Time);
            list.Add(param);

            param = getCCID(data, 8);
            list.Add(param);
        }
    }
}
