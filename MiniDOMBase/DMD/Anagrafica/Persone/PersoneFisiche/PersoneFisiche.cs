using System;
using DMD;
using DMD.Databases;
using minidom.repositories;
using static minidom.Sistema;
using static minidom.Anagrafica;

namespace minidom
{

  
    namespace repositories
    {



        /// <summary>
        /// Repository di oggetti di tipo <see cref="CPersonaFisica"/>
        /// </summary>
        [Serializable]
        public sealed class CPersoneFisicheRepository 
            : CModulesClass<Anagrafica.CPersonaFisica>
        {
            
            /// <summary>
            /// Costruttore
            /// </summary>
            public CPersoneFisicheRepository() 
                : base("modPersoneFisiche", typeof(Anagrafica.CPersonaFisicaCursor), 0)
            {
            }

               
        }
    }

    public partial class Anagrafica
    {

       

        private static CPersoneFisicheRepository m_PersoneFisiche = null;

        /// <summary>
        /// Repository di oggetti di tipo <see cref="CPersonaFisica"/>
        /// </summary>
        public static CPersoneFisicheRepository PersoneFisiche
        {
            get
            {
                if (m_PersoneFisiche is null)
                    m_PersoneFisiche = new CPersoneFisicheRepository();
                return m_PersoneFisiche;
            }
        }
    }
}