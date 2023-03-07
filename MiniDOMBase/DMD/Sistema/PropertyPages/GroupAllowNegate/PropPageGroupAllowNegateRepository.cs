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
    /// Repository di oggetti di tipo <see cref="PropPageGroupAllowNegate"  />
    /// </summary>
    [Serializable]
    public class PropPageGroupAllowNegateRepository
        : CModulesClass<PropPageGroupAllowNegate>
    {

        /// <summary>
        /// Costruttore
        /// </summary>
        public PropPageGroupAllowNegateRepository()
            : base("modPropPageGrpAllowNegate", typeof(PropPageGroupAllowNegateCursor), -1)
        {
        }

    }
}