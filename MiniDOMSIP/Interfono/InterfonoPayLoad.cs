using System;

namespace minidom.PBX
{
    public enum InterfonoPayLoadType : int
    {
        Handshake = 0,
        AudioData = 1,
        Disconnect = 255
    }

    [Serializable]
    public struct InterfonoPayLoad
    {
        public int id;
        public DateTime time;
        public InterfonoPayLoadType type;
        public int codec;
        public int bufferSize;
        public byte[] buffer;
        public int crc;
    }
}