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
        /// Cursore di <see cref="CProdTabFinConstraint"/>
        /// </summary>
        [Serializable]
        public class CProdTabFinConstraintCursor 
            : CTableConstraintCursor
        {
            private DBCursorField<int> m_OwnerID = new DBCursorField<int>("Owner");

            /// <summary>
            /// Costruttore
            /// </summary>
            public CProdTabFinConstraintCursor()
            {
            }

            /// <summary>
            /// OwnerID
            /// </summary>
            public DBCursorField<int> OwnerID
            {
                get
                {
                    return m_OwnerID;
                }
            }

            /// <summary>
            /// Accede all'oggetto corrente
            /// </summary>
            public new CProdTabFinConstraint Item
            {
                get
                {
                    return (CProdTabFinConstraint)base.Item;
                }

                set
                {
                    base.Item = value;
                }
            }

            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_FIN_ProdXTabFinConstr";
            }

            /// <summary>
            /// Istanzia un nuovo oggetto
            /// </summary>
            /// <param name="dbRis"></param>
            /// <returns></returns>
            public override object InstantiateNew(DBReader dbRis)
            {
                return new CProdTabFinConstraint();
            }
        }
    }
}