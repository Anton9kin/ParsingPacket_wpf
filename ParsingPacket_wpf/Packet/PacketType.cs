using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParsingPacket_wpf.Packet
{
    class PacketType
    {
        public enum TypePacket : byte
        {
            Null = 0x00,   //!< None type
            Error_Resp = 0xA0,   //!< Error response
            Marker = 0xA1,   //!< Common packet [out] (not used)
            Activity = 0xA2,   //!< Packet of Activity [out]
            Location = 0xA3,   //!< Packet of Location [out]
            Generic_Resp = 0xA4,   //!< Packet of Generic Response [in]
            Device_Error = 0xA5,   //!< Packet of Device Error [out]
            Location_2 = 0xA6,   //!< Packet of Location [out]

            Info_Update_Req = 0xB1, //!< Packet Update request [out]
            Info_Update_Resp = 0xB2, //!< Packet of response update [in]
            Info_Update_Result = 0xB3, //!< Packet update result [out]
            Info_Pet_Options_Req = 0xB4, //!< Packet of Pet options [out] (not used)
            Info_Pet_Options_Resp = 0xB5, //!< Packet of response Pet options [in] (not used)
            Info_Device_Options_Req = 0xB6, //!< Packet of Device options [out]
            Info_Device_Options_Resp = 0xB7, //!< Packet of response Device options [out]

            UDP_Statistic = 0xC1,   //!< Packet of statistic
            Telemetry = 0xC2,   //!< Packet of Telemetry

            SNTP_TIME1 = 0x1C,   //!< Header-1 of SNTP
            SNTP_TIME2 = 0xDC,   //!< Header-2 of SNTP

        };

        public TypePacket type { get; set; }

        public PacketType() {
            type = TypePacket.Null;
        }

        public void Check(byte data) {

            type = (TypePacket)data;

            if ((TypePacket.Activity == type) || (TypePacket.Device_Error == type) || (TypePacket.Error_Resp == type) ||
                (TypePacket.Generic_Resp == type) || (TypePacket.Info_Device_Options_Req == type) || (TypePacket.Info_Device_Options_Resp == type) ||
                (TypePacket.Info_Pet_Options_Req == type) || (TypePacket.Info_Pet_Options_Resp == type) || (TypePacket.Info_Update_Req == type) ||
                (TypePacket.Info_Update_Resp == type) || (TypePacket.Info_Update_Result == type) || (TypePacket.Location == type) ||
                (TypePacket.Location_2 == type) || (TypePacket.Marker == type) || (TypePacket.SNTP_TIME1 == type) || (TypePacket.SNTP_TIME2 == type) ||
                (TypePacket.Telemetry == type) || (TypePacket.UDP_Statistic == type))
            {
                this.type = type;
            }
            else {
                type = TypePacket.Null;
                this.type = type;
            }
        }
    }
}
