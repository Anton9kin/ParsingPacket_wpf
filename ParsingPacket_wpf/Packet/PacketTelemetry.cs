﻿using System;
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
        private byte[] CCID = new byte[20];       // CCID of device
        private List<TelemetryType> TMList = new List<TelemetryType>();

        public PacketTelemetry(List<byte> data)
        {
            if (Parsing(ref data) == false)
            {
                MessageBox.Show("Not correct data", "Warning", MessageBoxButton.OK);
                return;
            }
            Packet_Time = WorkBuffer.GetUInt64(ref data);
            Num = WorkBuffer.GetUInt16(ref data);

            for (int i = 0; i < CCID.Length; i++)
            {
                CCID[i] = WorkBuffer.GetByte(ref data);
            }

            for (int i = 0; i < Num; i++)
            {
                TMList.Add(new TelemetryType(data));
            }

            CRC32 = WorkBuffer.GetUInt32(ref data);
        }

        public List<Parameter> GetListParam()
        {
            SetBaseParam();

            Parameter p;

            p = TimestampToDate(Packet_Time);
            List.Add(p);

            p = GetCCID(ref CCID);
            List.Add(p);

            p = new Parameter { Param = "", Value = "DATA:" };
            List.Add(p);

            p = new Parameter { Param = "Num of struct", Value = Num.ToString() };
            List.Add(p);

            foreach (TelemetryType tm in TMList)
                List.AddRange(tm.List);

            return List;
        }
    }
}
