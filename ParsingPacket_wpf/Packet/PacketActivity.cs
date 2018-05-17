using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParsingPacket_wpf.Packet
{
    class PacketActivity : PacketBase
    {
        public Int64 Packet_Time { get; set; }	// Time create packet
        public int Step { get; set; }			// Steps of Pet
        public int Hi_Act_Time { get; set; }	// Time of High activity of Pet
        public int Low_Act_Time { get; set; }	// Time of Low activity of Pet
        public float CSQ { get; set; }			// Signal quality network
        public byte Charge { get; set; }        // Charge battery
        public int[] CCID { get; set; } = new int[20];       // CCID of device
        public List<Parameter> list { get; set; } = new List<Parameter>();

        public PacketActivity(string[] data) {
            Parameter param;

            string s = data[7] + data[6] + data[5] + data[4] + data[3] + data[2] + data[1] + data[0];
            Packet_Time = Convert.ToInt64(s, 16);
            param = TimestampToDate(Packet_Time);
            list.Add(param);

            s = data[9] + data[8];
            Step = Convert.ToInt16(s, 16);
            param = new Parameter { Param = "Step", Value = Step.ToString() + " steps" };
            list.Add(param);

            s = data[11] + data[10];
            Hi_Act_Time = Convert.ToInt16(s, 16);
            param = new Parameter { Param = "HiTime", Value = Hi_Act_Time.ToString() + " seconds" };
            list.Add(param);

            s = data[13] + data[12];
            Low_Act_Time = Convert.ToInt16(s, 16);
            param = new Parameter { Param = "Low_Act_Time", Value = Low_Act_Time.ToString() + " seconds" };
            list.Add(param);

            s = data[17] + data[16] + data[15] + data[14];

            Int32 d = Convert.ToInt32(s, 16);
            double f = Convert.ToDouble(d);

            byte[] bytes = BitConverter.GetBytes(d);
            float csq = BitConverter.ToSingle(bytes, 0);

            param = new Parameter { Param = "CSQ", Value = csq.ToString() };
            list.Add(param);

            Charge = Convert.ToByte(data[18], 16);
            param = new Parameter { Param = "Charge", Value = Charge.ToString() + " %" };
            list.Add(param);

            param = getCCID(data[19], 19);
            list.Add(param);
        }
    }
}
