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
        private UInt64 Packet_Time;    ///< Time create packet
        private byte[] Version = new byte[4];       ///< Name current version of device
        private byte[] CCID = new byte[20];       // CCID of device

        public PacketUpdateRequest(List<byte> data)
        {
            if (Parsing(ref data) == false)
            {
                MessageBox.Show("Not correct data", "Warning", MessageBoxButton.OK);
                return;
            }
            Packet_Time = GetUInt64(ref data);

            for (int i = 0; i < Version.Length; i++)
            {
                Version[i] = GetByte(ref data);
            }

            for (int i = 0; i < CCID.Length; i++)
            {
                CCID[i] = GetByte(ref data);
            }

            CRC32 = GetUInt32(ref data);
        }

        public List<Parameter> GetListParam()
        {
            SetBaseParam();

            Parameter p;

            p = TimestampToDate(Packet_Time);
            list.Add(p);

            p = new Parameter { Param = "", Value = "DATA:" };
            list.Add(p);

            p = new Parameter { Param = "Version", Value = String.Format("{0}.{1}.{2}.{3}", Version[0], Version[1], Version[2], Version[3]) };
            list.Add(p);

            p = GetCCID_Byte(ref CCID);
            list.Add(p);

            return list;
        }
    }
}
