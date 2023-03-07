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
using static minidom.Finanziaria;


namespace minidom
{
    public partial class Finanziaria
    {

        /// <summary>
        /// Area manager
        /// </summary>
        [Serializable]
        public class CAreaManager
            : CTeamManager
        {
            //[NonSerialized] private CAMPraticheCollection m_Pratiche;
            //[NonSerialized] private CAMTeamManagersCollection m_TeamManagers;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CAreaManager()
            {
                m_Pratiche = null;
                m_TeamManagers = null;
            }

            public override bool Equals(Databases.DBObject obj)
            {
                return (obj is CAreaManager) && base.Equals(obj);
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Finanziaria.AreaManagers;
            }

            /// <summary>
            /// Restituisce la collezione delle pratiche caricate sotto il team-manager
            /// </summary>
            /// <returns></returns>
            public CAMPraticheCollection GetPratiche()
            {
                return new CAMPraticheCollection(this);
            }

            /// <summary>
            /// Restituisce la collezione dei team-manager gestiti dall'area manager
            /// </summary>
            public CAMTeamManagersCollection GetTeamManagers()
            {
                return new CAMTeamManagersCollection(this);
            }

            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_AreaManagers";
            }

             
        }
    }
}