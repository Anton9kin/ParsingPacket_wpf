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

        private CommandFromServer.Type Mode;   // Type statistic (#ECC_Get_UDP_Statistic, #ECC_Get_UDP_Error, #ECC_GETStatusDevice, #ECC_DeviceVers)
        private UInt32 RX_packets;         // Data statistic
        private UInt32 TX_packets;         // Data statistic
        private UInt32 Out_bytes;          // Data statistic
        private UInt32 In_bytes;	        // Data statistic
        private byte[] CCID = new byte[20];       // CCID of device

        public PacketStatistic(List<byte> data)
        {
            if (Parsing(ref data) == false)
            {
                MessageBox.Show("Not correct data", "Warning", MessageBoxButton.OK);
                return;
            }

            Mode = (CommandFromServer.Type)WorkBuffer.GetByte(ref data);
            RX_packets = WorkBuffer.GetUInt32(ref data);
            TX_packets = WorkBuffer.GetUInt32(ref data);
            Out_bytes = WorkBuffer.GetUInt32(ref data);
            In_bytes = WorkBuffer.GetUInt32(ref data);
            for (int i = 0; i < CCID.Length; i++)
            {
                CCID[i] = WorkBuffer.GetByte(ref data);
            }
            CRC32 = WorkBuffer.GetUInt32(ref data);
        }

        public List<Parameter> GetListParam()
        {
            SetBaseParam();

            Parameter p;

            p = GetCCID(ref CCID);
            List.Add(p);

            p = new Parameter { Param = "", Value = "DATA:" };
            List.Add(p);

            switch (Mode)
            {
                case CommandFromServer.Type.Get_TRANSPORT_Statistic:
                    p = new Parameter { Param = "Num of Received Packets", Value = String.Format("{0} ({1} bytes)", RX_packets, In_bytes) };
                    List.Add(p);

                    p = new Parameter { Param = "Num of Sended Packets", Value = String.Format("{0} ({1} bytes)", TX_packets, Out_bytes) };
                    List.Add(p);

                    break;

                case CommandFromServer.Type.Get_TRANSPORT_Error:
                    p = new Parameter { Param = "Num of Open Error", Value = RX_packets.ToString() };
                    List.Add(p);

                    p = new Parameter { Param = "Num of Close Error", Value = TX_packets.ToString() };
                    List.Add(p);

                    p = new Parameter { Param = "Num of Send Error", Value = Out_bytes.ToString() };
                    List.Add(p);

                    p = new Parameter { Param = "Num of Receive Error", Value = In_bytes.ToString() };
                    List.Add(p);
                    break;

                case CommandFromServer.Type.GETStatusDevice:
                    E_Boot_State boot = (E_Boot_State)(RX_packets & 0xFF);
                    p = new Parameter { Param = "Num of Open Error", Value = boot.ToString() };
                    List.Add(p);
                    break;

                case CommandFromServer.Type.DeviceVers:
                    UInt32 algorithm = RX_packets & 0xFF;
                    UInt32 release = TX_packets & 0xFF;
                    UInt32 rev = (TX_packets >> 16) & 0xFF;
                    UInt32 modemVer = (RX_packets >> 16) & 0xFF;

                    p = new Parameter { Param = "Version FirmWare", Value = String.Format("v%d_%d_rev_%d", algorithm, release, rev) };
                    List.Add(p);

                    p = new Parameter { Param = "Version Modem", Value = modemVer.ToString() };
                    List.Add(p);

                    break;
            }

            return List;
        }
    }
}
