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
using static minidom.Store;


namespace minidom
{
    public partial class Store
    {

        /// <summary>
        /// Cursore di oggetti di tipo <see cref="ArticoloInMagazzino"/>
        /// </summary>
        [Serializable]
        public class ArticoloInMagazzinoCursor 
            : minidom.Databases.DBObjectCursor<ArticoloInMagazzino>
        {
            private DBCursorField<double> m_Quantita = new DBCursorField<double>("Quantita");
            private DBCursorField<int> m_Flags = new DBCursorField<int>("Flags");
            private DBCursorField<int> m_IDArticolo = new DBCursorField<int>("IDArticolo");
            private DBCursorField<int> m_IDMagazzino = new DBCursorField<int>("IDMagazzino");

            /// <summary>
            /// Costruttore
            /// </summary>
            public ArticoloInMagazzinoCursor()
            {
            }

            /// <summary>
            /// Quantitia
            /// </summary>
            public DBCursorField<double> Quantitia
            {
                get
                {
                    return this.m_Quantita;
                }
            }

            /// <summary>
            /// Flags
            /// </summary>
            public DBCursorField<int> Flags
            {
                get
                {
                    return this.m_Flags;
                }
            }

            /// <summary>
            /// IDArticolo
            /// </summary>
            public DBCursorField<int> IDArticolo
            {
                get
                {
                    return this.m_IDArticolo;
                }
            }

            /// <summary>
            /// IDMagazzino
            /// </summary>
            public DBCursorField<int> IDMagazzino
            {
                get
                {
                    return this.m_IDMagazzino;
                }
            }

          
            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Store.Articoli.QuantitaInMagazzino;
            }
             
        }
    }
}