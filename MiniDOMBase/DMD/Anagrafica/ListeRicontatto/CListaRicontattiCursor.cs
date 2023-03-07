using System;
using System.Collections.Generic;
using DMD;
using DMD.Databases;
using static minidom.Sistema;
using minidom.repositories;

namespace minidom
{
    public partial class Anagrafica
    {

        /// <summary>
        /// Cursore sulla tabella delle liste di ricontatti
        /// </summary>
        [Serializable]
        public class CListaRicontattiCursor 
            : minidom.Databases.DBObjectCursorPO<CListaRicontatti>
        {
            private DBCursorStringField m_Name = new DBCursorStringField("Name");
            private DBCursorStringField m_Descrizione = new DBCursorStringField("Descrizione");
            private DBCursorField<DateTime> m_DataInserimento = new DBCursorField<DateTime>("DataInserimento");
            private DBCursorField<int> m_IDProprietario = new DBCursorField<int>("IDProprietario");
            private DBCursorStringField m_NomeProprietario = new DBCursorStringField("NomeProprietario");
            private DBCursorField<int> m_Flags = new DBCursorField<int>("Flags");

            /// <summary>
            /// Costruttore
            /// </summary>
            public CListaRicontattiCursor()
            {
            }

            /// <summary>
            /// Campo IDProprietario
            /// </summary>
            public DBCursorField<int> IDProprietario
            {
                get
                {
                    return m_IDProprietario;
                }
            }

            /// <summary>
            /// Campo NomeProprietario
            /// </summary>
            public DBCursorStringField NomeProprietario
            {
                get
                {
                    return m_NomeProprietario;
                }
            }

            /// <summary>
            /// Campo Name
            /// </summary>
            public DBCursorStringField Name
            {
                get
                {
                    return m_Name;
                }
            }

            /// <summary>
            /// Campo Descrizione
            /// </summary>
            public DBCursorStringField Descrizione
            {
                get
                {
                    return m_Descrizione;
                }
            }

            /// <summary>
            /// Campo DataInserimento
            /// </summary>
            public DBCursorField<DateTime> DataInserimento
            {
                get
                {
                    return m_DataInserimento;
                }
            }

            /// <summary>
            /// Campo Flags
            /// </summary>
            public DBCursorField<int> Flags
            {
                get
                {
                    return m_Flags;
                }
            }

            //public override string GetTableName()
            //{
            //    return "tbl_ListeRicontatto";
            //}

            //protected internal override CDBConnection GetConnection()
            //{
            //    return ListeRicontatto.Database;
            //}

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Anagrafica.ListeRicontatto; //.Module;
            }

            /// <summary>
            /// Permessi
            /// </summary>
            /// <returns></returns>
            protected override DBCursorFieldBase GetWherePartLimit()
            {
                DBCursorFieldBase wherePart = DBCursorFieldBase.True;

                if (!this.Module.UserCanDoAction("list"))
                {
                    wherePart = DBCursorFieldBase.False;

                    var user = minidom.Sistema.Users.CurrentUser;
                    if (this.Module.UserCanDoAction("list_office"))
                    {
                        var arrPO = new List<int>();
                        foreach (var ufficio in user.Uffici)
                        {
                            arrPO.Add(DBUtils.GetID(ufficio, 0));
                        }

                        var officePart = new DBCursorField<int>("IDPuntoOperativo", OP.OP_IN, true);
                        officePart.ValueIn(arrPO.ToArray());
                        wherePart = wherePart.Or(officePart);
                    }

                    if (this.Module.UserCanDoAction("list_own"))
                    {
                        var userPart = new DBCursorField<int>("CreatoDa", OP.OP_EQ, true); userPart.Value = DBUtils.GetID(user, 0);
                        wherePart = wherePart.Or(userPart);

                        var proprietarioPart = new DBCursorField<int>("IDProprietario", OP.OP_EQ, true); proprietarioPart.Value = DBUtils.GetID(user, 0);
                        wherePart = wherePart.Or(proprietarioPart);
                    }


                }

                return wherePart;
            }


        }
    }
}