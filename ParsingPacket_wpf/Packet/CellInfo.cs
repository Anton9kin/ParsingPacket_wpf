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

        public int Size()
        {
            return sizeof(UInt16) + sizeof(UInt32) + sizeof(byte);
        }

        public int SizeLac()
        {
            return sizeof(UInt16);
        }
        public int SizeCell()
        {
            return sizeof(UInt32);
        }
        public int SizeRSSI()
        {
            return sizeof(byte);
        }

        public void SetLAC(string s)
        {
            LAC = Convert.ToUInt16(s, 16);
        }

        public void SetCell(string s)
        {
            CELL_ID = Convert.ToUInt32(s, 16);
        }

        public void SetRssi(string s)
        {
            RSSI = Convert.ToByte(s, 16);
        }
    }
}
