using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParsingPacket_wpf.Packet
{
    class TelemetryType
    {
        private  enum TypeTel : byte
        {
            NONE = 0,       // None type
            CSQ_Charge = 1, // CSQ/Charge/Voltage type
            Latitude = 2,   // Latitude type
            Longitude = 3,  // Longitude type
            FixPos = 4,     // Fix/Pos/HDOP
            DeviceState = 5, 
                            /**
		                        0 byte - 0x01 geofence (0 - off, 1 - on)
			                             0x02 power save mode (0 - off, 1 - on)
			                             0x04 walk (0 sleep, 1 - walk)
			                             0x08 type network (0 - 2G, 1 - 3G)
		                        1 byte - state device (passive, active,  charging)
		                        2, 3 byte - reserved
	                        */
            MaxType,         // Max number of type
        };

        private enum GPS_Source { UNKNOWN, INTERNAL, BLE};

        private TypeTel Type; // Type telemetry
        private UInt32 Data;	// Data of telemetry
        public List<Parameter> List = new List<Parameter>();

        public TelemetryType(List<byte> data)
        {
            if (CheckType(data[0]))
            {
                this.Type = (TypeTel)data[0];
            }
            else
            {
                this.Type = TypeTel.NONE;
            }
            data.RemoveAt(0);

            this.Data = WorkBuffer.GetUInt32(ref data);

            if (Type != TypeTel.NONE)
                Parsing();
        }

        private void Parsing()
        {
            Parameter p;

            p = new Parameter { Param = "Type Telemetry", Value = Type.ToString() };
            List.Add(p);

            switch (Type)
            {
                case TypeTel.CSQ_Charge:
                    p = new Parameter { Param = "Signal quality", Value = (Data & 0xFF).ToString() };
                    List.Add(p);

                    p = new Parameter { Param = "Charge", Value = ((Data >> 8) & 0xFF).ToString() + " %" };
                    List.Add(p);

                    p = new Parameter { Param = "Voltage", Value = ((Data >> 16) & 0xFFFF).ToString() + "0 mV" };
                    List.Add(p);

                    break;

                case TypeTel.DeviceState:
                    p = new Parameter { Param = "GeoFence", Value = (Data & 0x01) == 0x01 ? "ON" : "OFF" };
                    List.Add(p);

                    p = new Parameter { Param = "PowerSave", Value = (Data & (1 << 1)) == (1 << 1) ? "ON" : "OFF" };
                    List.Add(p);

                    p = new Parameter { Param = "Device", Value = (Data & (1 << 2)) == (1 << 2) ? "Run" : "Sleep" };
                    List.Add(p);

                    p = new Parameter { Param = "Network", Value = (Data & (1 << 3)) == (1 << 3) ? "3G" : "2G" };
                    List.Add(p);
                    
                    p = new Parameter { Param = "State", Value = (Data & (3 << 8)) == (3 << 8) ? "Charging" : (Data & (3 << 8)) == (2 << 8) ? "Active" : "Passive" };
                    List.Add(p);

                    p = new Parameter { Param = "BLE", Value = (Data & (1 << 16)) == (1 << 16) ? "ON" : "OFF" };
                    List.Add(p);

                    GPS_Source gpsSource = (GPS_Source)((Data >> 17) & 0x0F);
                    p = new Parameter { Param = "GPSSource", Value = gpsSource.ToString() };
                    List.Add(p);
                    break;

                case TypeTel.FixPos:
                    char dataChar = (char)((Data) & 0xFF);
                    p = new Parameter { Param = "FIX", Value = dataChar.ToString() };
                    List.Add(p);

                    dataChar = (char)((Data >> 8) & 0xFF);
                    p = new Parameter { Param = "Latitude", Value = dataChar.ToString() };
                    List.Add(p);

                    dataChar = (char)((Data >> 16) & 0xFF);
                    p = new Parameter { Param = "Longitude", Value = dataChar.ToString() };
                    List.Add(p);

                    p = new Parameter { Param = "HDOP", Value = ((Data >> 24) & 0xFF).ToString() };
                    List.Add(p);
                    break;

                case TypeTel.Latitude:
                    p = new Parameter { Param = "Latitude"};
                    p.Value = ((Data >> 24) & 0xFF).ToString() + "° ";
                    p.Value += ((Data >> 16) & 0xFF).ToString() + "' ";
                    p.Value += ((Data >> 8) & 0xFF).ToString() + ".";
                    p.Value += ((Data) & 0xFF).ToString() + '"';
                    List.Add(p);
                    break;

                case TypeTel.Longitude:
                    p = new Parameter { Param = "Longitude" };
                    p.Value = ((Data >> 24) & 0xFF).ToString() + "° ";
                    p.Value += ((Data >> 16) & 0xFF).ToString() + "' ";
                    p.Value += ((Data >> 8) & 0xFF).ToString() + ".";
                    p.Value += ((Data) & 0xFF).ToString() + '"';
                    List.Add(p);
                    break;
            }
        }

        private bool CheckType(Byte type)
        {
            foreach (TypeTel tt in Enum.GetValues(typeof(TypeTel)))
            {
                if ((TypeTel)type == tt)
                    return true;
            }

            return false;
        }
    }
}
