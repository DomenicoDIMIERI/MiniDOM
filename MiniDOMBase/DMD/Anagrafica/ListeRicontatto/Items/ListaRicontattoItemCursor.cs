using System;
using System.Xml.Serialization;
using DMD;
using DMD.Databases;
using static minidom.Sistema;
using minidom.repositories;

namespace minidom
{
    public partial class Anagrafica
    {


        /// <summary>
        /// Cursore sulla tabella degli elementi delle liste di ricontatto
        /// </summary>
        [Serializable]
        public class ListaRicontattoItemCursor 
            : CRicontattiCursor
        {
            private DBCursorStringField m_NomeLista = new DBCursorStringField("NomeLista");
            private DBCursorField<int> m_IDListaRicontatti = new DBCursorField<int>("IDLista");

            /// <summary>
            /// Costruttore
            /// </summary>
            public ListaRicontattoItemCursor()
            {

            }

            /// <summary>
            /// Campo NomeLista
            /// </summary>
            public DBCursorStringField NomeLista
            {
                get
                {
                    return m_NomeLista;
                }
            }

            /// <summary>
            /// Campo IDListaRicontatti
            /// </summary>
            public DBCursorField<int> IDListaRicontatti
            {
                get
                {
                    return this.m_IDListaRicontatti;
                }
            }

            //public override object InstantiateNew(Databases.DBReader dbRis)
            //{
            //    return new ListaRicontattoItem();
            //}

            //public override string GetTableName()
            //{
            //    return "tbl_ListeRicontattoItems";
            //}

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Anagrafica.ListeRicontatto.Items; //.Module;
            }

            /// <summary>
            /// Restituisce o imposta l'elemento corrente
            /// </summary>
            [XmlIgnore]
            public new ListaRicontattoItem Item
            {
                get
                {
                    return (ListaRicontattoItem)base.Item;
                }
                set
                {
                    base.Item = value;
                }
            }

            /// <summary>
            /// Aggiunge un nuovo elemento
            /// </summary>
            /// <returns></returns>
            public new ListaRicontattoItem Add()
            {
                return (ListaRicontattoItem)base.Add();
            }

            //protected internal override Databases.CDBConnection GetConnection()
            //{
            //    return ListeRicontatto.Database;
            //}

            // Public Overrides Function GetWherePartLimit() As String
            // Dim tmpSQL As String = MyBase.GetWherePartLimit()
            // If Not Me.Module.UserCanDoAction("list") Then
            // If Me.Module.UserCanDoAction("list_own") Then
            // tmpSQL = DMD.Strings.Combine(tmpSQL, "([IDAssegnatoA]=" & GetID(Users.CurrentUser) & ")", " OR ")
            // End If
            // End If
            // Return tmpSQL
            // End Function


        }
    }
}