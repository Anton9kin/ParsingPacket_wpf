using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ParsingPacket_wpf.Packet
{
    class PacketStatistic : PacketBase
    {
        private enum E_Boot_State
        {
            FirstBoot = 0,//!< EBS_FirstBoot
            SwitchOff = 1,//!< EBS_SwitchOff
            SOS = 2,    //!< EBS_SOS
            Active = 3,  //!< EBS_Active
            Passive = 4, //!< EBS_Passive
            ReBoot = 5,  //!< EBS_ReBoot
            GPRS = 6,   //!< EBS_GPRS
            REBOOT_SMS = 7, //!< EBS_REBOOT_SMS
            UnknownBoot,  //!< EBS_UnknownBoot
        };

        private CommandFromServer.Type mode { get; set; }   // Type statistic (#ECC_Get_UDP_Statistic, #ECC_Get_UDP_Error, #ECC_GETStatusDevice, #ECC_DeviceVers)
        private UInt32 RX_packets { get; set; }         // Data statistic
        private UInt32 TX_packets { get; set; }         // Data statistic
        private UInt32 out_bytes { get; set; }          // Data statistic
        private UInt32 in_bytes  { get; set; }	        // Data statistic

        public PacketStatistic (string[] dataPack)
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

            mode = (CommandFromServer.Type) Convert.ToByte(data[0], 16);
            RX_packets = Convert.ToUInt32(data[4] + data[3] + data[2] + data[1], 16);
            TX_packets = Convert.ToUInt32(data[8] + data[7] + data[6] + data[5], 16);
            out_bytes = Convert.ToUInt32(data[12] + data[11] + data[10] + data[9], 16);
            in_bytes = Convert.ToUInt32(data[16] + data[15] + data[14] + data[13], 16);

            param = getCCID(data, 17);
            list.Add(param);

            Parameter p;

            switch (mode)
            {
                case CommandFromServer.Type.Get_TRANSPORT_Statistic:
                    p = new Parameter { Param = "Num of Received Packets", Value = String.Format("{0} ({1} bytes)", RX_packets, in_bytes) };
                    list.Add(p);

                    p = new Parameter { Param = "Num of Sended Packets", Value = String.Format("{0} ({1} bytes)", TX_packets, out_bytes) };
                    list.Add(p);

                    break;

                case CommandFromServer.Type.Get_TRANSPORT_Error:
                    p = new Parameter { Param = "Num of Open Error", Value = RX_packets.ToString()};
                    list.Add(p);

                    p = new Parameter { Param = "Num of Close Error", Value = TX_packets.ToString() };
                    list.Add(p);

                    p = new Parameter { Param = "Num of Send Error", Value = out_bytes.ToString() };
                    list.Add(p);

                    p = new Parameter { Param = "Num of Receive Error", Value = in_bytes.ToString() };
                    list.Add(p);
                    break;

                case CommandFromServer.Type.GETStatusDevice:
                    E_Boot_State boot = (E_Boot_State)(RX_packets & 0xFF);
                    p = new Parameter { Param = "Num of Open Error", Value = boot.ToString() };
                    list.Add(p);
                    break;

                case CommandFromServer.Type.DeviceVers:
                    UInt32 algorithm = RX_packets & 0xFF;
                    UInt32 release = TX_packets & 0xFF;
                    UInt32 rev = (TX_packets >> 16) & 0xFF;
                    UInt32 modemVer = (RX_packets >> 16) & 0xFF;

                    p = new Parameter { Param = "Version FirmWare", Value = String.Format("v%d_%d_rev_%d", algorithm, release, rev) };
                    list.Add(p);

                    p = new Parameter { Param = "Version Modem", Value = modemVer.ToString() };
                    list.Add(p);

                    break;
            }
        }
    }
}
