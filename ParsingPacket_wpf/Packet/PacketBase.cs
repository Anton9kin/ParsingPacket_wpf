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

        public PacketType Type { get; set; } = new PacketType();    //type of packet
        public UInt32 Seq { get; set; }            //seq of packet
        public UInt32 CRC32 { get; set; }          //CRC of packet

        public List<Parameter> List { get; set; } = new List<Parameter>();

        public bool Parsing(ref List<byte> data)
        {
            int length = data.Count;

            Type.Check(WorkBuffer.GetByte(ref data));

            if (Type.type == PacketType.TypePacket.Null)
                return false;

            Seq = WorkBuffer.GetUInt32(ref data);

            return true;
        }

        public void SetBaseParam()
        {
            Parameter p = new Parameter { Param = "Packet", Value = Type.type.ToString() };
            List.Add(p);

            p = new Parameter { Param = "SEQ", Value = Seq.ToString() };
            List.Add(p);

            p = new Parameter { Param = "CRC", Value = "0x" + CRC32.ToString("X") };
            List.Add(p);
        }

        public Parameter GetCCID(ref byte[] ccid)
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

        public Parameter TimestampToDate(UInt64 timeStamp) {
            DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dt = dt.AddSeconds(timeStamp);

            Parameter param = new Parameter { Param = "Time", Value = dt.ToString("dd/MM/yyyy HH:mm:ss") + dt.ToLocalTime().ToString(" (dd/MM/yyyy HH:mm:ss)") };

            return param;
        }
    }
}
