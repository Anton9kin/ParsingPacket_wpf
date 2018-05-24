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
        private enum E_Boot_State : byte
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

        private CommandFromServer.Type mode;   // Type statistic (#ECC_Get_UDP_Statistic, #ECC_Get_UDP_Error, #ECC_GETStatusDevice, #ECC_DeviceVers)
        private UInt32 RX_packets;         // Data statistic
        private UInt32 TX_packets;         // Data statistic
        private UInt32 out_bytes;          // Data statistic
        private UInt32 in_bytes;	        // Data statistic
        private byte[] CCID = new byte[20];       // CCID of device

        public PacketStatistic(List<byte> data)
        {
            if (Parsing(ref data) == false)
            {
                MessageBox.Show("Not correct data", "Warning", MessageBoxButton.OK);
                return;
            }

            mode = (CommandFromServer.Type)GetByte(ref data);
            RX_packets = GetUInt32(ref data);
            TX_packets = GetUInt32(ref data);
            out_bytes = GetUInt32(ref data);
            in_bytes = GetUInt32(ref data);
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

            p = GetCCID_Byte(ref CCID);
            list.Add(p);

            p = new Parameter { Param = "", Value = "DATA:" };
            list.Add(p);

            switch (mode)
            {
                case CommandFromServer.Type.Get_TRANSPORT_Statistic:
                    p = new Parameter { Param = "Num of Received Packets", Value = String.Format("{0} ({1} bytes)", RX_packets, in_bytes) };
                    list.Add(p);

                    p = new Parameter { Param = "Num of Sended Packets", Value = String.Format("{0} ({1} bytes)", TX_packets, out_bytes) };
                    list.Add(p);

                    break;

                case CommandFromServer.Type.Get_TRANSPORT_Error:
                    p = new Parameter { Param = "Num of Open Error", Value = RX_packets.ToString() };
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

            return list;
        }
    }
}
