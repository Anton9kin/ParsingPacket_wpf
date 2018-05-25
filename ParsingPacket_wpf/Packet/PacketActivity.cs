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
            if (Parsing(ref data) == false)
            {
                MessageBox.Show("Not correct data", "Warning", MessageBoxButton.OK);
                return;
            }
            Packet_Time = WorkBuffer.GetUInt64(ref data);
            Step = WorkBuffer.GetUInt16(ref data);
            Hi_Act_Time = WorkBuffer.GetUInt16(ref data);
            Low_Act_Time = WorkBuffer.GetUInt16(ref data);
            CSQ = WorkBuffer.GetFloat(ref data);
            Charge = WorkBuffer.GetByte(ref data);
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

            p = TimestampToDate(Packet_Time);
            List.Add(p);

            p = new Parameter { Param = "", Value = "DATA:" };
            List.Add(p);

            p = new Parameter { Param = "Step", Value = Step.ToString() + " steps" };
            List.Add(p);

            p = new Parameter { Param = "Hi_Time", Value = Hi_Act_Time.ToString() + " sec"};
            List.Add(p);

            p = new Parameter { Param = "Low_Time", Value = Low_Act_Time.ToString() + " sec" };
            List.Add(p);

            p = new Parameter { Param = "CSQ", Value = CSQ.ToString() };
            List.Add(p);

            p = new Parameter { Param = "Charge", Value = Charge.ToString() + "%" };
            List.Add(p);

            p = GetCCID(ref CCID);
            List.Add(p);

            return List;
        }
    }
}
