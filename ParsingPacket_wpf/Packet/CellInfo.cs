using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParsingPacket_wpf.Packet
{
    class CellInfo
    {
        public UInt16 LAC { get; set; }       // Local area code
        public UInt32 CELL_ID { get; set; }   // Cell ID base station
        public byte RSSI { get; set; }		   // RSSI base station

        public CellInfo(List<byte> data)
        {
            PacketBase pack = new PacketBase();

            LAC = pack.GetUInt16(ref data);
            CELL_ID = pack.GetUInt32(ref data);
            RSSI = pack.GetByte(ref data);
        }
    }
}
