using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Windows;

namespace ParsingPacket_wpf.Packet
{
    class PacketResponseOptions : PacketBase
    {
        private UInt64 NewTime;	// Time create packet
        private Byte ActivityPeriod;// Period send Activity packet
        private Byte Indication;       // Enable/Disable indication in Activity mode
        private UInt16 TelemetryPeriod; // Telemetry period (bit 0-7: in activity mode bits:8-15 in passive)
        private CommandFromServer comServer;

        public PacketResponseOptions(List<byte> data)
        {
            if (Parsing(ref data) == false)
            {
                MessageBox.Show("Not correct data", "Warning", MessageBoxButton.OK);
                return;
            }

            NewTime = GetUInt64(ref data);

            ActivityPeriod = GetByte(ref data);
            Indication = GetByte(ref data);
            TelemetryPeriod = GetUInt16(ref data);
            comServer = new CommandFromServer(data);

            CRC32 = GetUInt32(ref data);
        }

        public List<Parameter> GetListParam()
        {
            SetBaseParam();

            Parameter p;

            p = new Parameter { Param = "", Value = "DATA:" };
            list.Add(p);

            p = TimestampToDate(NewTime);
            p.Param = "New time";
            list.Add(p);

            p = new Parameter { Param = "Period", Value = ActivityPeriod.ToString() };
            list.Add(p);

            p = new Parameter { Param = "Indication", Value = Indication == 1 ? "ON" : "OFF" };
            list.Add(p);

            p = new Parameter { Param = "Telemetry_Active", Value = (TelemetryPeriod & 0xFF).ToString() };
            list.Add(p);

            p = new Parameter { Param = "Telemetry_Passive", Value = (TelemetryPeriod >> 8).ToString() };
            list.Add(p);

            list.AddRange(comServer.list);

            return list;
        }
    }


}
