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
        private CommandFromServer ComServer;

        public PacketResponseOptions(List<byte> data)
        {
            if (Parsing(ref data) == false)
            {
                MessageBox.Show("Not correct data", "Warning", MessageBoxButton.OK);
                return;
            }

            NewTime = WorkBuffer.GetUInt64(ref data);

            ActivityPeriod = WorkBuffer.GetByte(ref data);
            Indication = WorkBuffer.GetByte(ref data);
            TelemetryPeriod = WorkBuffer.GetUInt16(ref data);
            ComServer = new CommandFromServer(data);

            CRC32 = WorkBuffer.GetUInt32(ref data);
        }

        public List<Parameter> GetListParam()
        {
            SetBaseParam();

            Parameter p;

            p = new Parameter { Param = "", Value = "DATA:" };
            List.Add(p);

            p = TimestampToDate(NewTime);
            p.Param = "New time";
            List.Add(p);

            p = new Parameter { Param = "Period", Value = ActivityPeriod.ToString() };
            List.Add(p);

            p = new Parameter { Param = "Indication", Value = Indication == 1 ? "ON" : "OFF" };
            List.Add(p);

            p = new Parameter { Param = "Telemetry_Active", Value = (TelemetryPeriod & 0xFF).ToString() };
            List.Add(p);

            p = new Parameter { Param = "Telemetry_Passive", Value = (TelemetryPeriod >> 8).ToString() };
            List.Add(p);

            List.AddRange(ComServer.List);

            return List;
        }
    }


}
