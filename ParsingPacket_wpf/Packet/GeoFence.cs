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
        public byte Index { get; set; }		/*!< Number of point*/

        public GeoFence(List<byte> data)
        {
            Latitude = WorkBuffer.GetFloat(ref data);
            Longitude = WorkBuffer.GetFloat(ref data);
            Index = WorkBuffer.GetByte(ref data);
        }
    }
}
