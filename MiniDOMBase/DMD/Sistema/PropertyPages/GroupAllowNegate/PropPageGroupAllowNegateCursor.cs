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
using System.Xml.Serialization;

namespace minidom
{
    public partial class Sistema
    {

        /// <summary>
        /// Cursore sulla tabella 
        /// </summary>
        [Serializable]
        public class PropPageGroupAllowNegateCursor 
            : GroupAllowNegateCursor<CRegisteredPropertyPage>
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public PropPageGroupAllowNegateCursor()
            {
            }

            /// <summary>
            /// ID della pagina di proprietà
            /// </summary>
            /// <returns></returns>
            protected override string GetItemFieldName()
            {
                return "IDPropPage";
            }

            /// <summary>
            /// ID del gruppo
            /// </summary>
            /// <returns></returns>
            protected override string GetGroupFieldName()
            {
                return "IDGroup";
            }

            /// <summary>
            /// Nome della tabella
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_PropPagesXGruppo";
            }

            // Protected Overrides Function GetConnection() As CDBConnection
            // Return Me.Application.Sistema.PropertyPages.Database
            // End Function

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Sistema.PropertyPages.GroupAllowNegateRepository;
            }

            // Public Overrides Function InstantiateNewT(dbRis As DBReader) As GroupAllowNegate(Of CRegisteredPropertyPage)
            // Return New PropPageGroupAllowNegate
            // End Function

            /// <summary>
            /// Istanzia un nuovo oggetto
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            public override GroupAllowNegate<CRegisteredPropertyPage> InstantiateNewT(DBReader reader)
            {
                return new PropPageGroupAllowNegate();
            }

            /// <summary>
            /// Restituisce o imposta l'elemento corrente
            /// </summary>
            [XmlIgnore]
            public new PropPageGroupAllowNegate Item
            {
                get
                {
                    return (PropPageGroupAllowNegate)base.Item;
                }

                set
                {
                    base.Item = value;
                }
            }

            //protected internal override Databases.CDBConnection GetConnection()
            //{
            //    return Databases.APPConn;
            //}

            /// <summary>
            /// Aggiunge un nuovo oggetto
            /// </summary>
            /// <returns></returns>
            public new PropPageGroupAllowNegate Add()
            {
                return (PropPageGroupAllowNegate)base.Add();
            }
        }
    }
}