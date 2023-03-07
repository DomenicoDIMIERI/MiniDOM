using DMD.Databases;
using minidom.repositories;
using System;
using System.Xml.Serialization;

namespace minidom
{
    public partial class Sistema
    {

        /// <summary>
        /// Cursore sulla tabella degli oggetti di tipo <see cref="PropPageUserAllowNegate"/>
        /// </summary>
        [Serializable]
        public class PropPageUserAllowNegateCursor
            : UserAllowNegateCursor<CRegisteredPropertyPage>
        {
            /// <summary>
            /// Costruttore
            /// </summary>
            public PropPageUserAllowNegateCursor()
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
            /// Nome della tabella
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_PropPageXUtente";
            }

            //protected internal override Databases.CDBConnection GetConnection()
            //{
            //    return Databases.APPConn;
            //}

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Sistema.PropertyPages.UserAllowNegateRepository;
            }

            /// <summary>
            /// Istanzia un un nuovo oggetto
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            public override UserAllowNegate<CRegisteredPropertyPage> InstantiateNewT(DBReader reader)
            {
                return new PropPageUserAllowNegate();
            }

            /// <summary>
            /// Restituisce o imposta l'elemento corrente
            /// </summary>
            [XmlIgnore]
            public new PropPageUserAllowNegate Item
            {
                get
                {
                    return (PropPageUserAllowNegate)base.Item;
                }

                set
                {
                    base.Item = value;
                }
            }

            /// <summary>
            /// Aggiunge un nuovo oggetto
            /// </summary>
            /// <returns></returns>
            public new PropPageUserAllowNegate Add()
            {
                return (PropPageUserAllowNegate)base.Add();
            }
        }
    }
}