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
        private int Step { get; set; }			// Steps of Pet
        private int Hi_Act_Time { get; set; }	// Time of High activity of Pet
        private int Low_Act_Time { get; set; }	// Time of Low activity of Pet
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
            CSQ = ConvertHexStr(s);

            param = new Parameter { Param = "CSQ", Value = CSQ.ToString() };
            list.Add(param);

            Charge = Convert.ToByte(data[18], 16);
            param = new Parameter { Param = "Charge", Value = Charge.ToString() + " %" };
            list.Add(param);

            param = getCCID(data, 19);
            list.Add(param);
        }
    }
}
