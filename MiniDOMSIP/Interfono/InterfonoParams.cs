using System;

namespace minidom.PBX
{
    [Serializable]
    public class InterfonoParams
    {
        public int srcID;
        public int codec;
        public string srcIP;
        public int srcPort;
        public string targetIP;
        public int targetPort;
        public string Result;

        public InterfonoParams()
        {
            codec = 0;
        }
    }
}