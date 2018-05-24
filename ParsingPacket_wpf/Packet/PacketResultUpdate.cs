using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ParsingPacket_wpf.Packet
{
    class PacketResultUpdate : PacketBase
    {
        private enum E_Status_Update {
			NONE = 0,
			SUCCESS = (1 << 0),
			ERROR_CONNECT_MOBILE = (1 << 1),
            ERROR_CONNECT_WEB = (1 << 2),
            ERROR_CRC = (1 << 3),
            ERROR_VOLTAGE = (1 << 4),
            ERROR_MODEM = (1 << 5),
            ERROR_FLASH = (1 << 6),
            ERROR_RECOVERY = (1 << 7),

            MAX
        };
        private UInt64 Packet_Time;   // Time create packet
        private byte[] Version = new byte[4];     // Name version update
        private E_Status_Update Update;			// Status updating (#UPDATE_SUCCESS, #UPDATE_ERROR_CONNECT_MOBILE)
        private byte[] CCID = new byte[20];       // CCID of device

        public PacketResultUpdate(List<byte> data)
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
            Update = (E_Status_Update)GetByte(ref data);

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

            p = new Parameter { Param = "", Value = "DATA:" };
            list.Add(p);

            p = new Parameter { Param = "Version", Value = String.Format("{0}.{1}.{2}.{3}", Version[0], Version[1], Version[2], Version[3]) };
            list.Add(p);

            p = new Parameter { Param = "ResultUpdate", Value = Update.ToString() };
            list.Add(p);

            p = GetCCID_Byte(ref CCID);
            list.Add(p);

            return list;
        }
    }
}
