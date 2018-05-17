using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace ParsingPacket_wpf.Packet
{
    class PacketResponseOptions : PacketBase
    {
        private Int64 Packet_Time { get; set; }	// Time create packet
        private Byte ActivityPeriod { get; set; }// Period send Activity packet
        private Byte Indication { get; set; }       // Enable/Disable indication in Activity mode
        private UInt16 TelemetryPeriod { get; set; } // Telemetry period (bit 0-7: in activity mode bits:8-15 in passive)
        private Byte Code { get; set; }     // Command from UDP
        private UInt32 Data;        ///< Data from UDP

        public PacketResponseOptions(string[] data) {
            Parameter param;

            string s = data[7] + data[6] + data[5] + data[4] + data[3] + data[2] + data[1] + data[0];
            Packet_Time = Convert.ToInt64(s, 16);
            param = TimestampToDate(Packet_Time);
            list.Add(param);

            ActivityPeriod = Byte.Parse(data[8], NumberStyles.HexNumber);
            param = new Parameter { Param = "Period", Value = ActivityPeriod.ToString() };
            list.Add(param);

            Indication = Byte.Parse(data[9], NumberStyles.HexNumber);
            param = new Parameter { Param = "Indication", Value = Indication == 1 ? "ON" : "OFF" };
            list.Add(param);

            TelemetryPeriod = UInt16.Parse(data[11] + data[10], NumberStyles.HexNumber);
            param = new Parameter { Param = "Telemetry_Active", Value = (TelemetryPeriod & 0xFF).ToString() };
            list.Add(param);

            param = new Parameter { Param = "Telemetry_Passive", Value = (TelemetryPeriod >> 8).ToString() };
            list.Add(param);

            Code = Byte.Parse(data[12], NumberStyles.HexNumber);
            param = new Parameter { Param = "Command from server", Value = Code.ToString() };
            list.Add(param);

            Data = UInt32.Parse(data[16] + data[15] + data[14] + data[13], NumberStyles.HexNumber);
            param = new Parameter { Param = "Parameter command", Value = Data.ToString() };
            list.Add(param);
        }
    }


}
