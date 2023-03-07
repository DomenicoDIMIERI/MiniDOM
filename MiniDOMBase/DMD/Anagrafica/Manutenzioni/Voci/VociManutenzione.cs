using System;
using DMD;
using DMD.Databases;
using DMD.Databases.Collections;
using static minidom.Sistema;
using static minidom.Anagrafica;
using minidom.repositories;

namespace minidom.repositories
{

    /// <summary>
    /// Repository di oggetti di tipo <see cref="VoceManutenzione"/>
    /// </summary>
    [Serializable]
    public class CVociManutenzioneClass 
        : CModulesClass<Anagrafica.VoceManutenzione>
    {

        /// <summary>
        /// Costruttore
        /// </summary>
        public CVociManutenzioneClass() 
            : base("modManutenziniVoci", typeof(Anagrafica.VociManutenzioneCursor), 0)
        {
        }
    }
}