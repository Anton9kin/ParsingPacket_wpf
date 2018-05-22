using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ParsingPacket_wpf.Packet
{
    class PacketUpdateResponse : PacketBase
    {
        private enum E_Event_Result
        {
            EER_TRUE = 0x01,//!< If update is needed
            EER_FALSE = 0x02,//!< If not update
        };

        private UInt32 FileSize;  ///< Size of file update
        private UInt32 FileCRC;   ///< CRC of file update
        private byte[] Version = new byte[4]; ///< Name of file update
        private E_Event_Result Update; ///< State update (#EER_TRUE if update is need, else #EER_FALSE)


        public PacketUpdateResponse(string[] dataPack)
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

            string s = getStr(ref n, sizeof(UInt32));
            FileSize = Convert.ToUInt32(s, 16);

            s = getStr(ref n, sizeof(UInt32));
            FileCRC = Convert.ToUInt32(s, 16);

            for (int i = 0; i < 4; i++)
            {
                s = getStr(ref n, sizeof(byte));
                Version[i] = Convert.ToByte(s, 16);
            }

            s = getStr(ref n, sizeof(byte));
            Update = (E_Event_Result)Convert.ToByte(s, 16);

            p = new Parameter { Param = "Update", Value = (Update == E_Event_Result.EER_TRUE) ? "Enable" : "Disable" };
            list.Add(p);

            if (Update == E_Event_Result.EER_TRUE)
            {
                p = new Parameter { Param = "Version", Value = String.Format("{0}.{1}.{2}.{3}", Version[0], Version[1], Version[2], Version[3]) };
                list.Add(p);

                p = new Parameter { Param = "Size", Value = FileSize.ToString() + " bytes" };
                list.Add(p);

                p = new Parameter { Param = "CRC", Value = "0x" + FileCRC.ToString("X") };
                list.Add(p);
            }
            
        }
    }
}
