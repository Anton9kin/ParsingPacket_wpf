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

        public PacketType type = new PacketType();    //type of packet
        public UInt32 seq { get; set; }            //seq of packet
        public UInt32 CRC32 { get; set; }          //CRC of packet

        public string[] data { get; set; }      //data of packet
        public List<Parameter> list { get; set; } = new List<Parameter>();

        public bool Parsing(ref List<byte> data)
        {
            int length = data.Count;

            type.Check(GetByte(ref data));

            if (type.type == PacketType.TypePacket.Null)
                return false;

            seq = GetUInt32(ref data);

            return true;
        }

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
            seq = UInt32.Parse(tmpData, NumberStyles.HexNumber);

            param = new Parameter { Param = "SEQ", Value = seq.ToString() };
            list.Add(param);

            //add CRC
            tmpData = data[length - 1] + data[length - 2] + data[length - 3] + data[length - 4];
            CRC32 = UInt32.Parse(tmpData, NumberStyles.HexNumber);

            param = new Parameter { Param = "CRC", Value = "0x" + CRC32.ToString("X") };
            list.Add(param);

            //add Separator
            param = new Parameter { Param = "", Value = "" };
            list.Add(param);

            return true;
        }

        public Parameter GetCCID_Byte(ref byte[] ccid)
        {
            string s = "";
            for (int i = 0; i < ccid.Length; i++)
            {
                if (ccid[i] >= 48)
                {
                    ccid[i] -= 48;
                    s += ccid[i].ToString();
                }
                else
                    s += "_";
            }
            return new Parameter { Param = "CCID", Value = s };
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

        public Parameter TimestampToDate(UInt64 timeStamp) {
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

        public float GetFloat(ref List<byte> list)
        {
            float data = 0;
            byte[] bytes = new byte[sizeof(UInt32)];

            for (int i = 0; i < sizeof(UInt32); i++)
                bytes[i] = GetByte(ref list);

            data = BitConverter.ToSingle(bytes, 0);

            return data;
        }

        public byte GetByte(ref List<byte> list)
        {
            byte data = list[0];
            list.RemoveAt(0);
            return data;
        }

        public UInt16 GetUInt16(ref List<byte> list)
        {
            UInt16 data = 0;
            byte[] bytes = new byte [sizeof(UInt16)];

            for (int i = 0; i < sizeof(UInt16); i++)
                bytes[i] = GetByte(ref list);

            data = BitConverter.ToUInt16(bytes, 0);

            return data;
        }

        public UInt32 GetUInt32(ref List<byte> list)
        {
            UInt32 data = 0;
            byte[] bytes = new byte[sizeof(UInt32)];

            for (int i = 0; i < sizeof(UInt32); i++)
                bytes[i] = GetByte(ref list);

            data = BitConverter.ToUInt32(bytes, 0);
            return data;
        }

        public UInt64 GetUint64(ref List<byte> list)
        {
            UInt64 data = 0;
            byte[] bytes = new byte[sizeof(UInt64)];

            for (int i = 0; i < sizeof(UInt64); i++)
                bytes[i] = GetByte(ref list);

            data = BitConverter.ToUInt64(bytes, 0);
            return data;
        }
        public string getStr(ref int start, int size)
        {
            string s = "";
            for (int i = 0; i < size; i++)
            {
                s = s.Insert(0, this.data[start + i]);
            }
            start += size;
            return s;
        }
    }
}
