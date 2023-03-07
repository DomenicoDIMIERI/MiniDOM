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
using static minidom.Office;

namespace minidom
{
    public partial class Office
    {

        /// <summary>
        /// Cursore di <see cref="Uscita"/>
        /// </summary>
        /// <remarks></remarks>
        public class UsciteCursor 
            : minidom.Databases.DBObjectCursorPO<Uscita>
        {
            private DBCursorField<int> m_IDOperatore = new DBCursorField<int>("IDOperatore");
            private DBCursorStringField m_NomeOperatore = new DBCursorStringField("NomeOperatore");
            private DBCursorField<DateTime> m_OraUscita = new DBCursorField<DateTime>("OraUscita");
            private DBCursorField<DateTime> m_OraRientro = new DBCursorField<DateTime>("OraRientro");
            private DBCursorField<double> m_DistanzaPercorsa = new DBCursorField<double>("DistanzaPercorsa");
            private DBCursorField<int> m_IDVeicoloUsato = new DBCursorField<int>("IDVeicoloUsato");
            private DBCursorStringField m_NomeVeicoloUsato = new DBCursorStringField("NomeVeicoloUsato");
            private DBCursorField<double> m_LitriCarburante = new DBCursorField<double>("LitriCarburante");
            private DBCursorField<decimal> m_Rimborso = new DBCursorField<decimal>("Rimborso");
            private DBCursorStringField m_Indirizzo_Via = new DBCursorStringField("Indirizzo_Via");
            private DBCursorStringField m_Indirizzo_Civico = new DBCursorStringField("Indirizzo_Civico");
            private DBCursorStringField m_Indirizzo_Citta = new DBCursorStringField("Indirizzo_Citta");
            private DBCursorStringField m_Indirizzo_Provincia = new DBCursorStringField("Indirizzo_Provincia");
            private DBCursorStringField m_Indirizzo_CAP = new DBCursorStringField("Indirizzo_CAP");
            private DBCursorField<double> m_Lat = new DBCursorField<double>("Lat");
            private DBCursorField<double> m_Lng = new DBCursorField<double>("Lng");
            private DBCursorField<double> m_Alt = new DBCursorField<double>("Alt");

            /// <summary>
            /// Costruttore
            /// </summary>
            public UsciteCursor()
            {

            }

            /// <summary>
            /// Indirizzo_Via
            /// </summary>
            public DBCursorStringField Indirizzo_Via
            {
                get
                {
                    return m_Indirizzo_Via;
                }
            }

            /// <summary>
            /// Indirizzo_Civico
            /// </summary>
            public DBCursorStringField Indirizzo_Civico
            {
                get
                {
                    return m_Indirizzo_Civico;
                }
            }

            /// <summary>
            /// Indirizzo_Citta
            /// </summary>
            public DBCursorStringField Indirizzo_Citta
            {
                get
                {
                    return m_Indirizzo_Citta;
                }
            }

            /// <summary>
            /// Indirizzo_Provincia
            /// </summary>
            public DBCursorStringField Indirizzo_Provincia
            {
                get
                {
                    return m_Indirizzo_Provincia;
                }
            }

            /// <summary>
            /// Indirizzo_CAP
            /// </summary>
            public DBCursorStringField Indirizzo_CAP
            {
                get
                {
                    return m_Indirizzo_CAP;
                }
            }

            /// <summary>
            /// Lat
            /// </summary>
            public DBCursorField<double> Lat
            {
                get
                {
                    return m_Lat;
                }
            }

            /// <summary>
            /// Lng
            /// </summary>
            public DBCursorField<double> Lng
            {
                get
                {
                    return m_Lng;
                }
            }

            /// <summary>
            /// Alt
            /// </summary>
            public DBCursorField<double> Alt
            {
                get
                {
                    return m_Alt;
                }
            }

            /// <summary>
            /// IDVeicoloUsato
            /// </summary>
            public DBCursorField<int> IDVeicoloUsato
            {
                get
                {
                    return m_IDVeicoloUsato;
                }
            }

            /// <summary>
            /// NomeVeicoloUsato
            /// </summary>
            public DBCursorStringField NomeVeicoloUsato
            {
                get
                {
                    return m_NomeVeicoloUsato;
                }
            }

            /// <summary>
            /// LitriCarburante
            /// </summary>
            public DBCursorField<double> LitriCarburante
            {
                get
                {
                    return m_LitriCarburante;
                }
            }

            /// <summary>
            /// Rimborso
            /// </summary>
            public DBCursorField<decimal> Rimborso
            {
                get
                {
                    return m_Rimborso;
                }
            }

            /// <summary>
            /// IDOperatore
            /// </summary>
            public DBCursorField<int> IDOperatore
            {
                get
                {
                    return m_IDOperatore;
                }
            }

            /// <summary>
            /// NomeOperatore
            /// </summary>
            public DBCursorStringField NomeOperatore
            {
                get
                {
                    return m_NomeOperatore;
                }
            }

            /// <summary>
            /// OraUscita
            /// </summary>
            public DBCursorField<DateTime> OraUscita
            {
                get
                {
                    return m_OraUscita;
                }
            }

            /// <summary>
            /// OraRientro
            /// </summary>
            public DBCursorField<DateTime> OraRientro
            {
                get
                {
                    return m_OraRientro;
                }
            }

            /// <summary>
            /// DistanzaPercorsa
            /// </summary>
            public DBCursorField<double> DistanzaPercorsa
            {
                get
                {
                    return m_DistanzaPercorsa;
                }
            }

            /// <summary>
            /// Gestisce i Permessi
            /// </summary>
            /// <returns></returns>
            protected override DBCursorFieldBase GetWherePartLimit()
            {
                var ret = base.GetWherePartLimit();
                if (!Module.UserCanDoAction("list"))
                {
                    if (Module.UserCanDoAction("list_assigned"))
                    {
                        ret += this.Field("IDOperatore").EQ(DBUtils.GetID(Sistema.Users.CurrentUser, 0));
                    }
                    else
                    {
                        ret = DBCursorField.False;
                    }
                }
                return ret;
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Office.Uscite;
            }

            /// <summary>
            /// Inizializza i parametri
            /// </summary>
            /// <param name="item"></param>
            protected override void OnInitialize(Uscita item)
            {
                base.OnInitialize(item);
                ret.Operatore = Sistema.Users.CurrentUser;
            }
        }
    }
}