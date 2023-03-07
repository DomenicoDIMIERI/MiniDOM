using System;
using DMD;
using DMD.Databases;
using DMD.Databases.Collections;
using static minidom.Sistema;
using static minidom.Anagrafica;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using minidom.repositories;

namespace minidom
{
    public partial class Anagrafica
    {

        /// <summary>
        /// Cursore sulla tabella delle persone fisiche
        /// </summary>
        [Serializable]
        public class CPersonaFisicaCursor 
            : CPersonaCursor
        {
            private DBCursorField<int> m_Impiego_IDAzienda = new DBCursorField<int>("IMP_IDAzienda");
            private DBCursorStringField m_Impiego_NomeAzienda = new DBCursorStringField("IMP_NomeAzienda");
            private DBCursorField<int> m_Impiego_IDEntePagante = new DBCursorField<int>("IMP_IDEntePagante");
            private DBCursorStringField m_Impiego_NomeEntePagante = new DBCursorStringField("IMP_NomeEntePagante");
            private DBCursorStringField m_Impiego_Posizione = new DBCursorStringField("IMP_Posizione");
            private DBCursorField<DateTime> m_Impiego_DataAssunzione = new DBCursorField<DateTime>("IMP_DataAssunzione");
            private DBCursorField<DateTime> m_Impiego_DataLicenziamento = new DBCursorField<DateTime>("IMP_DataLicenziamento");
            private DBCursorField<decimal> m_Impiego_StipendioNetto = new DBCursorField<decimal>("IMP_StipendioNetto");
            private DBCursorField<decimal> m_Impiego_StipendioLordo = new DBCursorField<decimal>("IMP_StipendioLordo");
            private DBCursorStringField m_Impiego_TipoContratto = new DBCursorStringField("IMP_TipoContratto");
            private DBCursorStringField m_Impiego_TipoRapporto = new DBCursorStringField("IMP_TipoRapporto");
            private DBCursorField<decimal> m_Impiego_TFR = new DBCursorField<decimal>("IMP_TFR");
            private DBCursorField<int> m_Impiego_MensilitaPercepite = new DBCursorField<int>("IMP_MensilitaPercepite");
            private DBCursorField<float> m_Impiego_PercTFRAzienda = new DBCursorField<float>("IMP_PercTFRAzienda");
            private DBCursorStringField m_Impiego_NomeFPC = new DBCursorStringField("IMP_NomeFPC");
            private DBCursorStringField m_Impiego_CategoriaAzienda = new DBCursorStringField("CategoriaAzienda");
            private DBCursorStringField m_Impiego_TipologiaAzienda = new DBCursorStringField("TipologiaAzienda");
            // Private m_TipoRapportoDiLavoro As New CCursorFieldObj(Of String)("TipologiaRapportoDiLavoro")

            private DBCursorStringField m_StatoCivile = new DBCursorStringField("StatoCivile");
            private DBCursorStringField m_Disabilita = new DBCursorStringField("Categoria");

            /// <summary>
            /// Costruttore
            /// </summary>
            public CPersonaFisicaCursor()
            {
                TipoPersona.Value = Anagrafica.TipoPersona.PERSONA_FISICA;
            }

            /// <summary>
            /// Disabilita
            /// </summary>
            public DBCursorStringField Disabilita
            {
                get
                {
                    return m_Disabilita;
                }
            }


            /// <summary>
            /// Restituisce o imposta l'elemento corrente
            /// </summary>
            [XmlIgnore]
            public new CPersonaFisica Item
            {
                get
                {
                    return (CPersonaFisica)base.Item;
                }

                set
                {
                    base.Item = value;
                }
            }

            /// <summary>
            /// Impiego_CategoriaAzienda
            /// </summary>
            public DBCursorStringField Impiego_CategoriaAzienda
            {
                get
                {
                    return m_Impiego_CategoriaAzienda;
                }
            }

            /// <summary>
            /// Impiego_TipologiaAzienda
            /// </summary>
            public DBCursorStringField Impiego_TipologiaAzienda
            {
                get
                {
                    return m_Impiego_TipologiaAzienda;
                }
            }

            // Public ReadOnly Property TipoRapportoDiLavoro As CCursorFieldObj(Of String)
            // Get
            // Return Me.m_TipoRapportoDiLavoro
            // End Get
            // End Property


            /// <summary>
            /// StatoCivile
            /// </summary>
            public DBCursorStringField StatoCivile
            {
                get
                {
                    return m_StatoCivile;
                }
            }

            /// <summary>
            /// Impiego_IDAzienda
            /// </summary>
            public DBCursorField<int> Impiego_IDAzienda
            {
                get
                {
                    return m_Impiego_IDAzienda;
                }
            }

            /// <summary>
            /// Impiego_NomeAzienda
            /// </summary>
            public DBCursorStringField Impiego_NomeAzienda
            {
                get
                {
                    return m_Impiego_NomeAzienda;
                }
            }

            /// <summary>
            /// Impiego_IDEntePagante
            /// </summary>
            public DBCursorField<int> Impiego_IDEntePagante
            {
                get
                {
                    return m_Impiego_IDEntePagante;
                }
            }

            /// <summary>
            /// Impiego_NomeEntePagante
            /// </summary>
            public DBCursorStringField Impiego_NomeEntePagante
            {
                get
                {
                    return m_Impiego_NomeEntePagante;
                }
            }

            /// <summary>
            /// Impiego_Posizione
            /// </summary>
            public DBCursorStringField Impiego_Posizione
            {
                get
                {
                    return m_Impiego_Posizione;
                }
            }

            /// <summary>
            /// Impiego_DataAssunzione
            /// </summary>
            public DBCursorField<DateTime> Impiego_DataAssunzione
            {
                get
                {
                    return m_Impiego_DataAssunzione;
                }
            }

            /// <summary>
            /// Impiego_DataLicenziamento
            /// </summary>
            public DBCursorField<DateTime> Impiego_DataLicenziamento
            {
                get
                {
                    return m_Impiego_DataLicenziamento;
                }
            }

            /// <summary>
            /// Impiego_StipendioNetto
            /// </summary>
            public DBCursorField<decimal> Impiego_StipendioNetto
            {
                get
                {
                    return m_Impiego_StipendioNetto;
                }
            }

            /// <summary>
            /// Impiego_StipendioLordo
            /// </summary>
            public DBCursorField<decimal> Impiego_StipendioLordo
            {
                get
                {
                    return m_Impiego_StipendioLordo;
                }
            }

            /// <summary>
            /// Impiego_TipoContratto
            /// </summary>
            public DBCursorStringField Impiego_TipoContratto
            {
                get
                {
                    return m_Impiego_TipoContratto;
                }
            }

            /// <summary>
            /// Impiego_TipoRapporto
            /// </summary>
            public DBCursorStringField Impiego_TipoRapporto
            {
                get
                {
                    return m_Impiego_TipoRapporto;
                }
            }

            /// <summary>
            /// Impiego_TFR
            /// </summary>
            public DBCursorField<decimal> Impiego_TFR
            {
                get
                {
                    return m_Impiego_TFR;
                }
            }

            /// <summary>
            /// Impiego_MensilitaPercepite
            /// </summary>
            public DBCursorField<int> Impiego_MensilitaPercepite
            {
                get
                {
                    return m_Impiego_MensilitaPercepite;
                }
            }

            /// <summary>
            /// Impiego_PercTFRAzienda
            /// </summary>
            public DBCursorField<float> Impiego_PercTFRAzienda
            {
                get
                {
                    return m_Impiego_PercTFRAzienda;
                }
            }

            /// <summary>
            /// Impiego_NomeFPC
            /// </summary>
            public DBCursorStringField Impiego_NomeFPC
            {
                get
                {
                    return m_Impiego_NomeFPC;
                }
            }

            /// <summary>
            /// Istanzia un nuovo oggetto
            /// </summary>
            /// <param name="dbRis"></param>
            /// <returns></returns>
            public override CPersona InstantiateNewT(DBReader dbRis)
            {
                return new CPersonaFisica();
            }

            /// <summary>
            /// Respository
            /// </summary>
            /// <returns></returns>
            protected override CModulesClass GetModule()
            {
                return minidom.Anagrafica.PersoneFisiche;
            }

            /// <summary>
            /// Rimuove dal cursore i campi che vengono trattati in maniera diversa
            /// </summary>
            /// <returns></returns>
            protected override DBCursorFieldsCollection GetCursorFields()
            {
                var ret = base.GetCursorFields();
                ret.Remove(this.m_Impiego_CategoriaAzienda);
                ret.Remove(this.m_Impiego_TipologiaAzienda);
                ret.Remove(this.m_Impiego_IDAzienda);
                ret.Remove(this.m_Impiego_NomeAzienda);
                ret.Remove(this.m_Impiego_IDEntePagante);
                ret.Remove(this.m_Impiego_NomeEntePagante);
                ret.Remove(this.m_Impiego_Posizione);
                ret.Remove(this.m_Impiego_DataAssunzione);
                ret.Remove(this.m_Impiego_DataLicenziamento);
                ret.Remove(this.m_Impiego_StipendioNetto);
                ret.Remove(this.m_Impiego_StipendioLordo);
                ret.Remove(this.m_Impiego_TipoContratto);
                ret.Remove(this.m_Impiego_TipoRapporto);
                ret.Remove(this.m_Impiego_TFR);
                ret.Remove(this.m_Impiego_MensilitaPercepite);
                ret.Remove(this.m_Impiego_PercTFRAzienda);
                ret.Remove(this.m_Impiego_NomeFPC);
                return ret;
            }

            /// <summary>
            /// Rimuove dalla lista dei campi ordinamento quelli che vengono trattati in maniera diversa
            /// </summary>
            /// <returns></returns>
            public override List<DBCursorField> GetSortFields()
            {
                var ret = base.GetSortFields();
                ret.Remove(m_Impiego_CategoriaAzienda);
                ret.Remove(m_Impiego_TipologiaAzienda);
                ret.Remove(m_Impiego_IDAzienda);
                ret.Remove(m_Impiego_NomeAzienda);
                ret.Remove(m_Impiego_IDEntePagante);
                ret.Remove(m_Impiego_NomeEntePagante);
                ret.Remove(m_Impiego_Posizione);
                ret.Remove(m_Impiego_DataAssunzione);
                ret.Remove(m_Impiego_DataLicenziamento);
                ret.Remove(m_Impiego_StipendioNetto);
                ret.Remove(m_Impiego_StipendioLordo);
                ret.Remove(m_Impiego_TipoContratto);
                ret.Remove(m_Impiego_TipoRapporto);
                ret.Remove(m_Impiego_TFR);
                ret.Remove(m_Impiego_MensilitaPercepite);
                ret.Remove(m_Impiego_PercTFRAzienda);
                ret.Remove(m_Impiego_NomeFPC);
                return ret;
            }

            // Private Function changeName(ByVal field As CCursorFieldObj(Of String), ByVal newName As String) As String
            // Dim f As New CCursorFieldObj(Of String)(newName, field.Operator, field.IncludeNulls)
            // f.Value = field.Value
            // f.Value1 = field.Value1
            // f.SortOrder = field.SortOrder
            // f.SortPriority = field.SortPriority
            // Return f.GetSQL
            // End Function

            //private string GetSQLImp()
            //{
            //    string wherePart = "[Stato]=" + ((int)ObjectStatus.OBJECT_VALID).ToString();
            //    if (m_Impiego_IDAzienda.IsSet())
            //        wherePart = DMD.Strings.Combine(wherePart, m_Impiego_IDAzienda.GetSQL("Azienda"), " AND ");
            //    if (m_Impiego_NomeAzienda.IsSet())
            //        wherePart = DMD.Strings.Combine(wherePart, m_Impiego_NomeAzienda.GetSQL("NomeAzienda"), " AND ");
            //    if (m_Impiego_IDEntePagante.IsSet())
            //        wherePart = DMD.Strings.Combine(wherePart, m_Impiego_IDEntePagante.GetSQL("IDEntePagante"), " AND ");
            //    if (m_Impiego_NomeEntePagante.IsSet())
            //        wherePart = DMD.Strings.Combine(wherePart, m_Impiego_NomeEntePagante.GetSQL("NomeEntePagante"), " AND ");
            //    if (m_Impiego_Posizione.IsSet())
            //        wherePart = DMD.Strings.Combine(wherePart, m_Impiego_Posizione.GetSQL("Posizione"), " AND ");
            //    if (m_Impiego_DataAssunzione.IsSet())
            //        wherePart = DMD.Strings.Combine(wherePart, m_Impiego_DataAssunzione.GetSQL("DataAssunzione"), " AND ");
            //    if (m_Impiego_DataLicenziamento.IsSet())
            //        wherePart = DMD.Strings.Combine(wherePart, m_Impiego_DataLicenziamento.GetSQL("DataLicenziamento"), " AND ");
            //    if (m_Impiego_StipendioNetto.IsSet())
            //        wherePart = DMD.Strings.Combine(wherePart, m_Impiego_StipendioNetto.GetSQL("StipendioNetto"), " AND ");
            //    if (m_Impiego_StipendioLordo.IsSet())
            //        wherePart = DMD.Strings.Combine(wherePart, m_Impiego_StipendioLordo.GetSQL("StipendioLordo"), " AND ");
            //    if (m_Impiego_TipoContratto.IsSet())
            //        wherePart = DMD.Strings.Combine(wherePart, m_Impiego_TipoContratto.GetSQL("TipoContratto"), " AND ");
            //    if (m_Impiego_TipoRapporto.IsSet())
            //        wherePart = DMD.Strings.Combine(wherePart, m_Impiego_TipoRapporto.GetSQL("TipoRapporto"), " AND ");
            //    if (m_Impiego_TFR.IsSet())
            //        wherePart = DMD.Strings.Combine(wherePart, m_Impiego_TFR.GetSQL("TFR"), " AND ");
            //    if (m_Impiego_MensilitaPercepite.IsSet())
            //        wherePart = DMD.Strings.Combine(wherePart, m_Impiego_MensilitaPercepite.GetSQL("MensilitaPercepite"), " AND ");
            //    if (m_Impiego_PercTFRAzienda.IsSet())
            //        wherePart = DMD.Strings.Combine(wherePart, m_Impiego_PercTFRAzienda.GetSQL("PercTFRAzienda"), " AND ");
            //    if (m_Impiego_NomeFPC.IsSet())
            //        wherePart = DMD.Strings.Combine(wherePart, m_Impiego_NomeFPC.GetSQL("NomeFPC"), " AND ");

            //    // Dim timpSQL As String = "SELECT * FROM [tbl_Impiegati] WHERE " & wherePart
            //    var dbSQL = new System.Text.StringBuilder();
            //    dbSQL.Append("SELECT [TPI1].* FROM (SELECT * FROM [tbl_Impiegati] WHERE ");
            //    dbSQL.Append(wherePart);
            //    dbSQL.Append(") AS [TPI2] INNER JOIN (");
            //    dbSQL.Append(base.GetSQL());
            //    dbSQL.Append(") AS [TPI1] ON [TPI1].[IDImpiego] = [TPI2].[ID]");
            //    return dbSQL.ToString();
            //}

            
            private bool IsImpiegoSet()
            {
                return m_Impiego_IDAzienda.IsSet() 
                    || m_Impiego_NomeAzienda.IsSet() 
                    || m_Impiego_IDEntePagante.IsSet() 
                    || m_Impiego_NomeEntePagante.IsSet() 
                    || m_Impiego_Posizione.IsSet() 
                    || m_Impiego_DataAssunzione.IsSet() 
                    || m_Impiego_DataLicenziamento.IsSet() 
                    || m_Impiego_StipendioNetto.IsSet() 
                    || m_Impiego_StipendioLordo.IsSet() 
                    || m_Impiego_TipoContratto.IsSet() 
                    || m_Impiego_TipoRapporto.IsSet() 
                    || m_Impiego_TFR.IsSet() 
                    || m_Impiego_MensilitaPercepite.IsSet() 
                    || m_Impiego_PercTFRAzienda.IsSet() 
                    || m_Impiego_NomeFPC.IsSet();
            }

            /// <summary>
            /// Clausole aggiuntive
            /// </summary>
            /// <returns></returns>
            public override DBCursorFieldBase GetWherePart()
            {
                var ret = base.GetWherePart();
                if (this.IsImpiegoSet())
                {
                    var lst = new List<int>();
                    using(var cursor = new CImpiegatiCursor())
                    {
                        cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                        cursor.IgnoreRights = true;
                        if (this.m_Impiego_IDAzienda.IsSet())
                        {
                            cursor.AziendaID.Operator = this.m_Impiego_IDAzienda.Operator;
                            cursor.AziendaID.IncludeNulls = this.m_Impiego_IDAzienda.IncludeNulls;
                            cursor.AziendaID.Values = this.m_Impiego_IDAzienda.Values;
                        }

                        if (this.m_Impiego_NomeAzienda.IsSet())
                        {
                            cursor.NomeAzienda.Operator = this.m_Impiego_NomeAzienda.Operator;
                            cursor.NomeAzienda.IncludeNulls = this.m_Impiego_NomeAzienda.IncludeNulls;
                            cursor.NomeAzienda.Values = this.m_Impiego_NomeAzienda.Values;
                        }

                        if (this.m_Impiego_IDEntePagante.IsSet())
                        {
                            cursor.IDEntePagante.Operator = this.m_Impiego_IDEntePagante.Operator;
                            cursor.IDEntePagante.IncludeNulls = this.m_Impiego_IDEntePagante.IncludeNulls;
                            cursor.IDEntePagante.Values = this.m_Impiego_IDEntePagante.Values;
                        }

                        if (this.m_Impiego_NomeEntePagante.IsSet())
                        {
                            cursor.NomeEntePagante.Operator = this.m_Impiego_NomeEntePagante.Operator;
                            cursor.NomeEntePagante.IncludeNulls = this.m_Impiego_NomeEntePagante.IncludeNulls;
                            cursor.NomeEntePagante.Values = this.m_Impiego_NomeEntePagante.Values;
                        }

                        if (this.m_Impiego_Posizione.IsSet())
                        {
                            cursor.Posizione.Operator = this.m_Impiego_Posizione.Operator;
                            cursor.Posizione.IncludeNulls = this.m_Impiego_Posizione.IncludeNulls;
                            cursor.Posizione.Values = this.m_Impiego_Posizione.Values;
                        }

                        if (this.m_Impiego_Posizione.IsSet())
                        {
                            cursor.Posizione.Operator = this.m_Impiego_Posizione.Operator;
                            cursor.Posizione.IncludeNulls = this.m_Impiego_Posizione.IncludeNulls;
                            cursor.Posizione.Values = this.m_Impiego_Posizione.Values;
                        }

                        if (this.m_Impiego_DataAssunzione.IsSet())
                        {
                            cursor.DataAssunzione.Operator = this.m_Impiego_DataAssunzione.Operator;
                            cursor.DataAssunzione.IncludeNulls = this.m_Impiego_DataAssunzione.IncludeNulls;
                            cursor.DataAssunzione.Values = this.m_Impiego_DataAssunzione.Values;
                        }

                        if (this.m_Impiego_DataLicenziamento.IsSet())
                        {
                            cursor.DataLicenziamento.Operator = this.m_Impiego_DataLicenziamento.Operator;
                            cursor.DataLicenziamento.IncludeNulls = this.m_Impiego_DataLicenziamento.IncludeNulls;
                            cursor.DataLicenziamento.Values = this.m_Impiego_DataLicenziamento.Values;
                        }

                        if (this.m_Impiego_StipendioNetto.IsSet())
                        {
                            cursor.StipendioNetto.Operator = this.m_Impiego_StipendioNetto.Operator;
                            cursor.StipendioNetto.IncludeNulls = this.m_Impiego_StipendioNetto.IncludeNulls;
                            cursor.StipendioNetto.Values = this.m_Impiego_StipendioNetto.Values;
                        }

                        if (this.m_Impiego_StipendioLordo.IsSet())
                        {
                            cursor.StipendioLordo.Operator = this.m_Impiego_StipendioLordo.Operator;
                            cursor.StipendioLordo.IncludeNulls = this.m_Impiego_StipendioLordo.IncludeNulls;
                            cursor.StipendioLordo.Values = this.m_Impiego_StipendioLordo.Values;
                        }

                        if (this.m_Impiego_TipoContratto.IsSet())
                        {
                            cursor.TipoContratto.Operator = this.m_Impiego_TipoContratto.Operator;
                            cursor.TipoContratto.IncludeNulls = this.m_Impiego_TipoContratto.IncludeNulls;
                            cursor.TipoContratto.Values = this.m_Impiego_TipoContratto.Values;
                        }

                        if (this.m_Impiego_TipoRapporto.IsSet())
                        {
                            cursor.TipoRapporto.Operator = this.m_Impiego_TipoRapporto.Operator;
                            cursor.TipoRapporto.IncludeNulls = this.m_Impiego_TipoRapporto.IncludeNulls;
                            cursor.TipoRapporto.Values = this.m_Impiego_TipoRapporto.Values;
                        }

                        if (this.m_Impiego_TFR.IsSet())
                        {
                            cursor.TFR.Operator = this.m_Impiego_TFR.Operator;
                            cursor.TFR.IncludeNulls = this.m_Impiego_TFR.IncludeNulls;
                            cursor.TFR.Values = this.m_Impiego_TFR.Values;
                        }

                        if (this.m_Impiego_MensilitaPercepite.IsSet())
                        {
                            cursor.MensilitaPercepite.Operator = this.m_Impiego_MensilitaPercepite.Operator;
                            cursor.MensilitaPercepite.IncludeNulls = this.m_Impiego_MensilitaPercepite.IncludeNulls;
                            cursor.MensilitaPercepite.Values = this.m_Impiego_MensilitaPercepite.Values;
                        }

                        if (this.m_Impiego_PercTFRAzienda.IsSet())
                        {
                            cursor.PercTFRAzienda.Operator = this.m_Impiego_PercTFRAzienda.Operator;
                            cursor.PercTFRAzienda.IncludeNulls = this.m_Impiego_PercTFRAzienda.IncludeNulls;
                            cursor.PercTFRAzienda.Values = this.m_Impiego_PercTFRAzienda.Values;
                        }

                        if (this.m_Impiego_NomeFPC.IsSet())
                        {
                            cursor.NomeFPC.Operator = this.m_Impiego_NomeFPC.Operator;
                            cursor.NomeFPC.IncludeNulls = this.m_Impiego_NomeFPC.IncludeNulls;
                            cursor.NomeFPC.Values = this.m_Impiego_NomeFPC.Values;
                        }
                     
                        while (cursor.Read())
                        {
                            lst.Add(cursor.Item.IDPersona);
                        }

                        lst.Sort();
                        if (lst.Count == 0)
                            return DBCursorField.False;

                        ret *= this.Field("ID").In(lst);
                    }
                }
                if (m_Impiego_CategoriaAzienda.IsSet())
                    throw new NotImplementedException(); //wherePart = DMD.Strings.Combine(wherePart, m_Impiego_CategoriaAzienda.GetSQL("Categoria"), " AND ");
                if (m_Impiego_TipologiaAzienda.IsSet())
                    throw new NotImplementedException(); //wherePart = DMD.Strings.Combine(wherePart, m_Impiego_TipologiaAzienda.GetSQL("Tipologia"), " AND ");
                
                return ret;
            }

             
        }
    }
}