using DMD.Databases;
using System;

namespace minidom
{
    public partial class Anagrafica
    {

        /// <summary>
        /// Azienda principale
        /// </summary>
        [Serializable]
        public class CAziendaPrincipale 
            : CAzienda
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public CAziendaPrincipale()
            {
            }


            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="azienda"></param>
            public CAziendaPrincipale(CAzienda azienda) : this()
            {
                if (azienda is null)
                    throw new ArgumentNullException("azienda");
                this.InitializeFrom(azienda);
                DBUtils.SetID(this, DBUtils.GetID(azienda));
            }

            
        }
    }
}