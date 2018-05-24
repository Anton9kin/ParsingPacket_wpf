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



        public PacketResultUpdate(string[] dataPack)
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
            Packet_Time = Convert.ToUInt64(s, 16);

            for (int i = 0; i < 4; i++)
            {
                s = getStr(ref n, sizeof(byte));
                Version[i] = Convert.ToByte(s, 16);
            }

            s = getStr(ref n, sizeof(UInt16));
            Update = (E_Status_Update)Convert.ToUInt16(s, 16);

            p = getCCID(data, n);
            list.Add(p);

            p = TimestampToDate(Packet_Time);
            list.Add(p);

            p = new Parameter { Param = "Version", Value = String.Format("{0}.{1}.{2}.{3}", Version[0], Version[1], Version[2], Version[3]) };
            list.Add(p);

            p = new Parameter { Param = "ResultUpdate", Value = Update.ToString() };
            list.Add(p);
        }
    }
}
