using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParsingPacket_wpf.Packet
{
    class GeoFence
    {
        public float Latitude { get; set; } /*!< GPS coordinate latitude */
        public float Longitude { get; set; }    /*!< GPS coordinate longitude */
        public byte index { get; set; }		/*!< Number of point*/

        public GeoFence(List<byte> data)
        {
            PacketBase pack = new PacketBase();

            Latitude = pack.GetFloat(ref data);
            Longitude = pack.GetFloat(ref data);
            index = pack.GetByte(ref data);
        }
    }
}
