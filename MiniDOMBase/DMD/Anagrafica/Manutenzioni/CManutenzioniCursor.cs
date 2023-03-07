using System;
using DMD;
using DMD.Databases;
using static minidom.Sistema;
using minidom.repositories;

namespace minidom
{
    public partial class Anagrafica
    {


        /// <summary>
        /// Cursore sulla tabella delle postazioni di lavoro
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class CManutenzioniCursor 
            : minidom.Databases.DBObjectCursorPO<CManutenzione>
        {

            private DBCursorField<int> m_IDPostazione = new DBCursorField<int>("IDPostazione");
            private DBCursorStringField m_NomePostazione = new DBCursorStringField("NomePostazione");
            private DBCursorField<DateTime> m_DataInizioIntervento = new DBCursorField<DateTime>("DataInizioIntervento");
            private DBCursorField<DateTime> m_DataFineIntervento = new DBCursorField<DateTime>("DataFineIntervento");
            private DBCursorField<decimal> m_ValoreImponibile = new DBCursorField<decimal>("ValoreImponibile");
            private DBCursorField<decimal> m_ValoreIvato = new DBCursorField<decimal>("ValoreIvato");
            private DBCursorField<decimal> m_CostoSpedizione = new DBCursorField<decimal>("CostoSpedizione");
            private DBCursorField<decimal> m_AltreSpese = new DBCursorField<decimal>("AltreSpese");
            private DBCursorStringField m_Descrizione = new DBCursorStringField("Descrizione");
            private DBCursorStringField m_Categoria1 = new DBCursorStringField("Categoria1");
            private DBCursorStringField m_Categoria2 = new DBCursorStringField("Categoria2");
            private DBCursorStringField m_Categoria3 = new DBCursorStringField("Categoria3");
            private DBCursorStringField m_Categoria4 = new DBCursorStringField("Categoria4");
            private DBCursorStringField m_Note = new DBCursorStringField("Note");
            private DBCursorField<int> m_IDAziendaFornitrice = new DBCursorField<int>("IDAziendaFornitrice");
            private DBCursorStringField m_NomeAziendaFornitrice = new DBCursorStringField("NomeAziendaFornitrice");
            private DBCursorField<int> m_IDRegistrataDa = new DBCursorField<int>("IDRegistrataDa");
            private DBCursorStringField m_NomeRegistrataDa = new DBCursorStringField("NomeRegistrataDa");
            private DBCursorField<int> m_Flags = new DBCursorField<int>("Flags");
            private DBCursorField<int> m_IDDocumento = new DBCursorField<int>("IDDocumento");
            private DBCursorStringField m_NumeroDocumento = new DBCursorStringField("NumeroDocumento");

            /// <summary>
            /// Costruttore
            /// </summary>
            public CManutenzioniCursor()
            {
            }

            /// <summary>
            /// Campo IDPostazione
            /// </summary>
            public DBCursorField<int> IDPostazione
            {
                get
                {
                    return m_IDPostazione;
                }
            }

            /// <summary>
            /// Campo NomePostazione
            /// </summary>
            public DBCursorStringField NomePostazione
            {
                get
                {
                    return m_NomePostazione;
                }
            }

            /// <summary>
            /// Campo DataInizioIntervento
            /// </summary>
            public DBCursorField<DateTime> DataInizioIntervento
            {
                get
                {
                    return m_DataInizioIntervento;
                }
            }

            /// <summary>
            /// Campo DataFineIntervento
            /// </summary>
            public DBCursorField<DateTime> DataFineIntervento
            {
                get
                {
                    return m_DataFineIntervento;
                }
            }

            /// <summary>
            /// Campo ValoreImponibile
            /// </summary>
            public DBCursorField<decimal> ValoreImponibile
            {
                get
                {
                    return m_ValoreImponibile;
                }
            }

            /// <summary>
            /// Campo ValoreIvato
            /// </summary>
            public DBCursorField<decimal> ValoreIvato
            {
                get
                {
                    return m_ValoreIvato;
                }
            }

            /// <summary>
            /// Campo CostoSpedizione
            /// </summary>
            public DBCursorField<decimal> CostoSpedizione
            {
                get
                {
                    return m_CostoSpedizione;
                }
            }

            /// <summary>
            /// Campo AltreSpese
            /// </summary>
            public DBCursorField<decimal> AltreSpese
            {
                get
                {
                    return m_AltreSpese;
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
            /// Campo Categoria1
            /// </summary>
            public DBCursorStringField Categoria1
            {
                get
                {
                    return m_Categoria1;
                }
            }

            /// <summary>
            /// Campo Categoria2
            /// </summary>
            public DBCursorStringField Categoria2
            {
                get
                {
                    return m_Categoria2;
                }
            }

            /// <summary>
            /// Campo Categoria3
            /// </summary>
            public DBCursorStringField Categoria3
            {
                get
                {
                    return m_Categoria3;
                }
            }

            /// <summary>
            /// Campo Categoria4
            /// </summary>
            public DBCursorStringField Categoria4
            {
                get
                {
                    return m_Categoria4;
                }
            }

            /// <summary>
            /// Campo Note
            /// </summary>
            public DBCursorStringField Note
            {
                get
                {
                    return m_Note;
                }
            }

            /// <summary>
            /// Campo IDAziendaFornitrice
            /// </summary>
            public DBCursorField<int> IDAziendaFornitrice
            {
                get
                {
                    return m_IDAziendaFornitrice;
                }
            }

            /// <summary>
            /// Campo NomeAziendaFornitrice
            /// </summary>
            public DBCursorStringField NomeAziendaFornitrice
            {
                get
                {
                    return m_NomeAziendaFornitrice;
                }
            }

            /// <summary>
            /// Campo IDRegistrataDa
            /// </summary>
            public DBCursorField<int> IDRegistrataDa
            {
                get
                {
                    return m_IDRegistrataDa;
                }
            }

            /// <summary>
            /// Campo NomeRegistrataDa
            /// </summary>
            public DBCursorStringField NomeRegistrataDa
            {
                get
                {
                    return m_NomeRegistrataDa;
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

            /// <summary>
            /// Campo IDDocumento
            /// </summary>
            public DBCursorField<int> IDDocumento
            {
                get
                {
                    return m_IDDocumento;
                }
            }

            /// <summary>
            /// Campo NumeroDocumento
            /// </summary>
            public DBCursorStringField NumeroDocumento
            {
                get
                {
                    return m_NumeroDocumento;
                }
            }

            //public override object InstantiateNew(DBReader dbRis)
            //{
            //    return new CManutenzione();
            //}

            //public override string GetTableName()
            //{
            //    return "tbl_Manutenzioni";
            //}

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Anagrafica.Manutenzioni; //.Module;
            }

            //protected internal override CDBConnection GetConnection()
            //{
            //    return APPConn;
            //}
        }
    }
}