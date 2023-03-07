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

namespace minidom
{
    
    namespace repositories
    {

        /// <summary>
        /// Repository di oggetti di tipo <see cref="CModuleAction"/>
        /// </summary>
        /// <remarks></remarks>
        public sealed class CDefinedActionsRepository 
            : CModulesClass<CModuleAction>
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public CDefinedActionsRepository()
                : base("modModuleDefinedActions", typeof(CModuleActionsCursor), -1)
            {
                // Me.Sorted = True
            }

               
        }
         
        public partial class CModulesModeClass
        {
            /// <summary>
            /// Lock sulle azioni
            /// </summary>
            public readonly object actionsLock = new object();


            

            private CDefinedActionsRepository m_DefinedActions = null;

            /// <summary>
            /// Repository di oggetti di tipo <see cref="CModuleAction"/>
            /// </summary>
            public CDefinedActionsRepository DefinedActions
            {
                get
                {
                    lock (actionsLock)
                    {
                        if (m_DefinedActions is null) m_DefinedActions = new CDefinedActionsRepository();
                        return m_DefinedActions;
                    }
                }
            }
             
        }

    }


}