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
        private Int64 Packet_Time { get; set; }	// Time create packet
        private UInt16 Step { get; set; }			// Steps of Pet
        private UInt16 Hi_Act_Time { get; set; }	// Time of High activity of Pet
        private UInt16 Low_Act_Time { get; set; }	// Time of Low activity of Pet
        private float CSQ { get; set; }			// Signal quality network
        private byte Charge { get; set; }        // Charge battery
        private int[] CCID { get; set; } = new int[20];       // CCID of device

        public PacketActivity(string[] dataPack) {
            Parameter param;
            int n = 0;
            int length = dataPack.Length;

            if (parsing(dataPack) == false) {
                MessageBox.Show("Not correct data", "Warning", MessageBoxButton.OK);
                return;
            }

            data = new string[length - 4 - 5];
            for (int i = 5; i < length - 4; i++)
                data[i - 5] = dataPack[i];

            //parse data of packet
            string s = getStr(ref n, sizeof(Int64));
            Packet_Time = Convert.ToInt64(s, 16);

            s = getStr(ref n, sizeof(UInt16));
            Step = Convert.ToUInt16(s, 16);

            s = getStr(ref n, sizeof(UInt16));
            Hi_Act_Time = Convert.ToUInt16(s, 16);

            s = getStr(ref n, sizeof(UInt16));
            Low_Act_Time = Convert.ToUInt16(s, 16);

            s = getStr(ref n, sizeof(float));
            CSQ = ConvertHexStr(s);

            s = getStr(ref n, sizeof(byte));
            Charge = Convert.ToByte(s, 16);

            //add Parameter
            param = getCCID(data, 19);
            list.Add(param);

            param = TimestampToDate(Packet_Time);
            list.Add(param);

            param = new Parameter { Param = "Step", Value = Step.ToString() + " steps" };
            list.Add(param);

            param = new Parameter { Param = "HiTime", Value = Hi_Act_Time.ToString() + " seconds" };
            list.Add(param);

            param = new Parameter { Param = "Low_Act_Time", Value = Low_Act_Time.ToString() + " seconds" };
            list.Add(param);

            param = new Parameter { Param = "CSQ", Value = CSQ.ToString() };
            list.Add(param);

            param = new Parameter { Param = "Charge", Value = Charge.ToString() + " %" };
            list.Add(param);
        }
    }
}
