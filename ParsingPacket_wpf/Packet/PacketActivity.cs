using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ParsingPacket_wpf.Packet
{
    class PacketActivity : PacketBase
    {
        private UInt64 Packet_Time;	// Time create packet
        private UInt16 Step;			// Steps of Pet
        private UInt16 Hi_Act_Time;	// Time of High activity of Pet
        private UInt16 Low_Act_Time;	// Time of Low activity of Pet
        private float CSQ;			// Signal quality network
        private byte Charge;        // Charge battery
        private byte[] CCID = new byte[20];       // CCID of device

        public PacketActivity(List<byte> data)
        {
            Parameter p;
            if (Parsing(ref data) == false)
            {
                MessageBox.Show("Not correct data", "Warning", MessageBoxButton.OK);
                return;
            }
            Packet_Time = GetUint64(ref data);
            Step = GetUInt16(ref data);
            Hi_Act_Time = GetUInt16(ref data);
            Low_Act_Time = GetUInt16(ref data);
            CSQ = GetFloat(ref data);
            Charge = GetByte(ref data);
            for (int i = 0; i < CCID.Length; i++)
            {
                CCID[i] = GetByte(ref data);
            }
            CRC32 = GetUInt32(ref data);

        }

        public List<Parameter> GetListParam()
        {
            Parameter p = new Parameter { Param = "Packet", Value = type.type.ToString() };
            list.Add(p);

            p = new Parameter { Param = "SEQ", Value = seq.ToString() };
            list.Add(p);

            p = new Parameter { Param = "CRC", Value = "0x" + CRC32.ToString("X") };
            list.Add(p);

            p = TimestampToDate(Packet_Time);
            list.Add(p);

            p = new Parameter { Param = "", Value = "DATA:" };
            list.Add(p);

            p = new Parameter { Param = "Step", Value = Step.ToString() + " steps" };
            list.Add(p);

            p = new Parameter { Param = "Hi_Time", Value = Hi_Act_Time.ToString() + " sec"};
            list.Add(p);

            p = new Parameter { Param = "Low_Time", Value = Low_Act_Time.ToString() + " sec" };
            list.Add(p);

            p = new Parameter { Param = "CSQ", Value = CSQ.ToString() };
            list.Add(p);

            p = new Parameter { Param = "Charge", Value = Charge.ToString() + "%" };
            list.Add(p);

            p = GetCCID_Byte(ref CCID);
            list.Add(p);

            return list;
        }
    }
}
