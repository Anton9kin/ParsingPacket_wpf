using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace ParsingPacket_wpf.Packet
{
    class PacketBase
    {

        private PacketType type = new PacketType();    //type of packet
        public int seq { get; set; }            //seq of packet
        public int CRC32 { get; set; }          //CRC of packet
        public Int64 time;           //time of packet

        public string[] data { get; set; }      //data of packet
        public List<Parameter> list { get; set; } = new List<Parameter>();

        public bool parsing(string[] data) {
            int length = data.Length;
            string tmpData = "";

            //check that data split have format "XX",lengh <= 2 bytes
            foreach (string s in data)
                if (s.Length > 2)
                    return false;
            
            //check that header from PacketType.TypePacket
            type.Check(Byte.Parse(data[0], NumberStyles.HexNumber));
            if (type.type == PacketType.TypePacket.Null)
                return false;

            //add Packet type
            Parameter param = new Parameter { Param = "Packet", Value = type.type.ToString() };
            list.Add(param);

            //add SEQ
            tmpData = data[4] + data[3] + data[2] + data[1];
            seq = Int32.Parse(tmpData, NumberStyles.HexNumber);

            param = new Parameter { Param = "SEQ", Value = seq.ToString() };
            list.Add(param);

            //add CRC
            tmpData = data[length - 1] + data[length - 2] + data[length - 3] + data[length - 4];
            CRC32 = Int32.Parse(tmpData, NumberStyles.HexNumber);

            param = new Parameter { Param = "CRC", Value = "0x" + CRC32.ToString("X") };
            list.Add(param);

            //add Separator
            param = new Parameter { Param = "", Value = "" };
            list.Add(param);
            /*
            this.data = new string[length - 4 - 5];
            for (int i = 5; i < length - 4; i++)
                this.data[i - 5] = data[i];


            //parse data to according with type of packet
            switch (type.type) {
                case PacketType.TypePacket.Null: break;
                case PacketType.TypePacket.Activity:
                    PacketActivity active = new PacketActivity(this.data);
                    list.AddRange(active.list);
                    break;
                case PacketType.TypePacket.Info_Device_Options_Req:
                    PacketRequestOptions getOptions = new PacketRequestOptions(this.data);
                    list.AddRange(getOptions.list);
                    break;
                case PacketType.TypePacket.Info_Device_Options_Resp:
                    PacketResponseOptions respOptions = new PacketResponseOptions(this.data);
                    list.AddRange(respOptions.list);
                    break;

            }
            */
            return true;
        }

        public Parameter getCCID(string[] ccid, int index) {
            string s = "";
            Int16[] CCID = new Int16[20];

            for (int i = 0; i < 20; i++)
            {
                CCID[i] = Convert.ToInt16(ccid[index + i], 16);
                if (CCID[i] >= 48)
                {
                    CCID[i] -= 48;

                    s += CCID[i].ToString();
                }
            }

            Parameter pCCID = new Parameter { Param = "CCID", Value = s };
            return pCCID;
        }

        public Parameter TimestampToDate(Int64 timeStamp) {
            DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dt = dt.AddSeconds(timeStamp);

            Parameter param = new Parameter { Param = "Time", Value = dt.ToString("dd/MM/yyyy HH:mm:ss") + dt.ToLocalTime().ToString(" (dd/MM/yyyy HH:mm:ss)") };

            return param;
        }

        public float ConvertHexStr(string s)
        {
            Int32 d = Convert.ToInt32(s, 16);
            double f = Convert.ToDouble(d);

            byte[] bytes = BitConverter.GetBytes(d);
            return BitConverter.ToSingle(bytes, 0);
        }

    }
}
