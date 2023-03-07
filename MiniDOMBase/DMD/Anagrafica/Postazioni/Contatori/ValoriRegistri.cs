using System;
using DMD.XML;
using DMD;
using DMD.Databases;
using DMD.Databases.Collections;
using static minidom.Sistema;
using static minidom.Anagrafica;

namespace minidom.repositories
{

    /// <summary>
    /// Repository di oggetti di tipo <see cref="ValoreRegistroContatore"/>
    /// </summary>
    [Serializable]
    public class CValoriRegistriClass 
        : CModulesClass<Anagrafica.ValoreRegistroContatore>
    {

        /// <summary>
        /// Costruttore
        /// </summary>
        public CValoriRegistriClass() 
            : base("modPostazioniRegistri", typeof(Anagrafica.ValoreRegistroCursor), 0)
        {
        }
    }
}