using System;
using System.Collections;
using System.Collections.Generic;
using DMD;
using DMD.XML;
using DMD.Databases;
using DMD.Databases.Collections;
using minidom;
using minidom.repositories;
using static minidom.Sistema;
using static minidom.Anagrafica;

namespace minidom.repositories
{
    /// <summary>
    /// Repository di oggetti di tipo <see cref="PropPageUserAllowNegate"  />
    /// </summary>
    [Serializable]
    public class PropPageUserAllowNegateRepository
        : CModulesClass<PropPageUserAllowNegate>
    {

        /// <summary>
        /// Costruttore
        /// </summary>
        public PropPageUserAllowNegateRepository()
            : base("modPropPageUsrAllowNegate", typeof(PropPageUserAllowNegateCursor), -1)
        {
        }

    }
}