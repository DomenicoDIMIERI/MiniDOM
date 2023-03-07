using System;
using DMD;
using DMD.XML;
using minidom;
using static minidom.Sistema;

namespace minidom
{
    public partial class Office
    {

        [Serializable]
        public sealed class DocumentiCaricati
        {
            private static Sistema.CModule m_Module;

            private DocumentiCaricati()
            {
                DMDObject.IncreaseCounter(this);
            }

            public static Sistema.CModule Module
            {
                get
                {
                    return null;
                }
            }

            

          
        }
    }
}