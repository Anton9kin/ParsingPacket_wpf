using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ParsingPacket_wpf.Packet
{
    class PacketTelemetry : PacketBase
    {
        private UInt64 Packet_Time;    // Time in packet
        private UInt16 Num;     // Number of struct #ST_Type_Telemetry
        private TelemetryType TM;
        private byte[] CCID = new byte[20];       // CCID of device

        public PacketTelemetry(List<byte> data)
        {
            if (Parsing(ref data) == false)
            {
                MessageBox.Show("Not correct data", "Warning", MessageBoxButton.OK);
                return;
            }
            Packet_Time = GetUint64(ref data);
            Num = GetUInt16(ref data);

            for (int i = 0; i < CCID.Length; i++)
            {
                CCID[i] = GetByte(ref data);
            }

            TM = new TelemetryType(data, Num);

            CRC32 = GetUInt32(ref data);
        }

        public List<Parameter> GetListParam()
        {
            SetBaseParam();

            Parameter p;

            p = TimestampToDate(Packet_Time);
            list.Add(p);

            p = GetCCID_Byte(ref CCID);
            list.Add(p);

            p = new Parameter { Param = "", Value = "DATA:" };
            list.Add(p);

            p = new Parameter { Param = "Num of struct", Value = Num.ToString() };
            list.Add(p);

            list.AddRange(TM.list);

            return list;
        }
    }
}
