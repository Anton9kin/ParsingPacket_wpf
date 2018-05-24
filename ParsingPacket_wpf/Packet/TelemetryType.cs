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

        private TypeTel type; // Type telemetry
        private UInt32 data;	// Data of telemetry
        public List<Parameter> list = new List<Parameter>();

        public TelemetryType(List<byte> data)
        {
            PacketBase pack = new PacketBase();

            if (checkType(data[0]))
            {
                this.type = (TypeTel)data[0];
            }
            else
            {
                this.type = TypeTel.NONE;
            }
            data.RemoveAt(0);

            this.data = pack.GetUInt32(ref data);

            if (type != TypeTel.NONE)
                parsing();
        }

        private void parsing()
        {
            Parameter p;

            p = new Parameter { Param = "Type Telemetry", Value = type.ToString() };
            list.Add(p);

            switch (type)
            {
                case TypeTel.CSQ_Charge:
                    p = new Parameter { Param = "Signal quality", Value = (data & 0xFF).ToString() };
                    list.Add(p);

                    p = new Parameter { Param = "Charge", Value = ((data >> 8) & 0xFF).ToString() + " %" };
                    list.Add(p);

                    p = new Parameter { Param = "Voltage", Value = ((data >> 16) & 0xFFFF).ToString() + "0 mV" };
                    list.Add(p);

                    break;

                case TypeTel.DeviceState:
                    p = new Parameter { Param = "GeoFence", Value = (data & 0x01) == 0x01 ? "ON" : "OFF" };
                    list.Add(p);

                    p = new Parameter { Param = "PowerSave", Value = (data & (1 << 1)) == (1 << 1) ? "ON" : "OFF" };
                    list.Add(p);

                    p = new Parameter { Param = "Device", Value = (data & (1 << 2)) == (1 << 2) ? "Run" : "Sleep" };
                    list.Add(p);

                    p = new Parameter { Param = "Network", Value = (data & (1 << 3)) == (1 << 3) ? "3G" : "2G" };
                    list.Add(p);
                    
                    p = new Parameter { Param = "State", Value = (data & (3 << 8)) == (3 << 8) ? "Charging" : (data & (3 << 8)) == (2 << 8) ? "Active" : "Passive" };
                    list.Add(p);

                    p = new Parameter { Param = "BLE", Value = (data & (1 << 16)) == (1 << 16) ? "ON" : "OFF" };
                    list.Add(p);

                    GPS_Source gpsSource = (GPS_Source)((data >> 17) & 0x0F);
                    p = new Parameter { Param = "GPSSource", Value = gpsSource.ToString() };
                    list.Add(p);
                    break;

                case TypeTel.FixPos:
                    char dataChar = (char)((data) & 0xFF);
                    p = new Parameter { Param = "FIX", Value = dataChar.ToString() };
                    list.Add(p);

                    dataChar = (char)((data >> 8) & 0xFF);
                    p = new Parameter { Param = "Latitude", Value = dataChar.ToString() };
                    list.Add(p);

                    dataChar = (char)((data >> 16) & 0xFF);
                    p = new Parameter { Param = "Longitude", Value = dataChar.ToString() };
                    list.Add(p);

                    p = new Parameter { Param = "HDOP", Value = ((data >> 24) & 0xFF).ToString() };
                    list.Add(p);
                    break;

                case TypeTel.Latitude:
                    p = new Parameter { Param = "Latitude"};
                    p.Value = ((data >> 24) & 0xFF).ToString() + "° ";
                    p.Value += ((data >> 16) & 0xFF).ToString() + "' ";
                    p.Value += ((data >> 8) & 0xFF).ToString() + ".";
                    p.Value += ((data) & 0xFF).ToString() + '"';
                    list.Add(p);
                    break;

                case TypeTel.Longitude:
                    p = new Parameter { Param = "Longitude" };
                    p.Value = ((data >> 24) & 0xFF).ToString() + "° ";
                    p.Value += ((data >> 16) & 0xFF).ToString() + "' ";
                    p.Value += ((data >> 8) & 0xFF).ToString() + ".";
                    p.Value += ((data) & 0xFF).ToString() + '"';
                    list.Add(p);
                    break;
            }
        }

        private bool checkType(Byte type)
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
