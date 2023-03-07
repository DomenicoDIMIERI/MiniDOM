using System;
using System.Collections;
using DMD;
using DMD.XML;
using DMD.Databases;
using DMD.Databases.Collections;
using static minidom.Sistema;
using static minidom.Anagrafica;
using minidom.repositories;
using System.Collections.Generic;


namespace minidom.repositories
{
    
    /// <summary>
    /// Repository di oggetti di tipo <see cref="CIntervalloCAP"/>
    /// </summary>
    [Serializable]
    public class CIntervalliCAPRepository
        : CModulesClass<CIntervalloCAP>
    {

        /// <summary>
        /// Costruttore
        /// </summary>
        public CIntervalliCAPRepository()
            : base("modLuoghiIntervalliCAP", typeof(CIntervalloCAPCursor), -1)
        {

        }

        

    }
}