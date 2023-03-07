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
using static minidom.Finanziaria;

namespace minidom
{
    namespace repositories
    {


        /// <summary>
        /// Repository di <see cref="CEstinzione"/>
        /// </summary>
        /// <remarks></remarks>
        public sealed class CEstinzioniClass
            : CModulesClass<CEstinzione>
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public CEstinzioniClass() 
                : base("modEstinzioni", typeof(CEstinzioniCursor))
            {
            }

            /// <summary>
            /// Restituisce un oggetto CEstinzioniPersona contenente tutte le estinzioni associate alla persona
            /// </summary>
            /// <param name="persona"></param>
            /// <returns></returns>
            /// <remarks></remarks>
            public Finanziaria.CEstinzioniPersona GetEstinzioniByPersona(Anagrafica.CPersona persona)
            {
                if (persona is null)
                    throw new ArgumentNullException("persona");
                return new Finanziaria.CEstinzioniPersona(persona);
            }

            /// <summary>
            /// Restituisce tutte le estinzioni per la pratica specificata
            /// </summary>
            /// <returns></returns>
            /// <remarks></remarks>
            public CCollection<Finanziaria.CEstinzione> GetEstinzioniByPratica(Finanziaria.CPraticaCQSPD pratica)
            {
                if (pratica is null)
                        throw new ArgumentNullException("pratica");
                var ret = new CCollection<CEstinzione>();
                if (DBUtils.GetID(pratica, 0) == 0)
                    return ret;
                using (var cursor = new Finanziaria.CEstinzioniCursor())
                {
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.IDPratica.Value = DBUtils.GetID(pratica, 0);
                    // cursor.Estinta.SortOrder = SortEnum.SORT_ASC
                    cursor.IgnoreRights = true;
                    cursor.DataInizio.SortOrder = SortEnum.SORT_DESC;
                    while (cursor.Read())
                    {
                        var est = cursor.Item;
                        est.SetPratica(pratica);
                        ret.Add(est);
                    }
                }

                return ret; 
            }

            /// <summary>
        /// Restituisce tutte le estinzioni per la pratica specificata
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
            public CCollection<Finanziaria.CEstinzione> GetEstinzioniByPratica(int idPratica)
            {
                return GetEstinzioniByPratica(Finanziaria.Pratiche.GetItemById(idPratica));
            }

            /// <summary>
        /// Restituisce un oggetto CEstinzioniPersona contenente tutte le estinzioni associate alla persona
        /// </summary>
        /// <param name="idPersona"></param>
        /// <returns></returns>
        /// <remarks></remarks>
            public Finanziaria.CEstinzioniPersona GetEstinzioniByPersona(int idPersona)
            {
                return GetEstinzioniByPersona(Anagrafica.Persone.GetItemById(idPersona));
            }

            // ''' <summary>
            // ''' Associa l'estinzione alla persona e restituisce l'ID dell'associazione
            // ''' </summary>
            // ''' <param name="estinzione"></param>
            // ''' <param name="persona"></param>
            // ''' <returns></returns>
            // ''' <remarks></remarks>
            // Public  Function addEstinzioneAPersona(ByVal estinzione As CEstinzione, ByVal persona As CPersona) As CEstinzioneXPersona
            // If (estinzione Is Nothing) Then Throw New ArgumentNullException("estinzione")
            // If (persona Is Nothing) Then Throw New ArgumentNullException("persona")
            // Dim estXpers As CEstinzioneXPersona
            // Dim cursor As New CEstinzioniXPersonaCursor
            // cursor.IgnoreRights = True
            // cursor.IDPersona.Value = GetID(persona)
            // cursor.IDEstinzione.Value = GetID(estinzione)
            // cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            // estXpers = cursor.Item
            // cursor.Dispose()
            // If (estXpers Is Nothing) Then
            // estXpers = New CEstinzioneXPersona
            // With estXpers
            // .Estinzione = estinzione
            // .Persona = persona
            // .Stato = ObjectStatus.OBJECT_VALID
            // End With
            // Finanziaria.Database.Save(estXpers)
            // End If
            // Return estXpers
            // End Function

            // Function IsEstinzioneLinkedWithPersona(ByVal idEstinzione As Integer, ByVal idPersona As Integer) As Boolean
            // If (idEstinzione = 0) Then Throw New ArgumentNullException("idEstinzione")
            // If (idPersona = 0) Then Throw New ArgumentNullException("idPersona")
            // Dim cursor As New CEstinzioniXPersonaCursor
            // Dim ret As Boolean
            // cursor.IgnoreRights = True
            // cursor.IDPersona.Value = idPersona
            // cursor.IDEstinzione.Value = idEstinzione
            // cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            // ret = Not cursor.EOF
            // cursor.Dispose()
            // Return ret
            // End Function

            // ''' <summary>
            // ''' Associa l'estinzione alla persona e restituisce l'ID dell'associazione
            // ''' </summary>
            // ''' <param name="idEstinzione"></param>
            // ''' <param name="idPersona"></param>
            // ''' <returns></returns>
            // ''' <remarks></remarks>
            // Public  Function addEstinzioneAPersona(ByVal idEstinzione As Integer, ByVal idPersona As Integer) As CEstinzioneXPersona
            // If (idEstinzione = 0) Then Throw New ArgumentNullException("idEstinzione")
            // If (idPersona = 0) Then Throw New ArgumentNullException("idPersona")
            // Dim estXpers As CEstinzioneXPersona
            // Dim cursor As New CEstinzioniXPersonaCursor
            // cursor.IgnoreRights = True
            // cursor.IDPersona.Value = idPersona
            // cursor.IDEstinzione.Value = idEstinzione
            // cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            // estXpers = cursor.Item
            // cursor.Dispose()
            // If (estXpers Is Nothing) Then
            // estXpers = New CEstinzioneXPersona
            // With estXpers
            // .IDEstinzione = idEstinzione
            // .IDPersona = idPersona
            // .Stato = ObjectStatus.OBJECT_VALID
            // End With
            // Finanziaria.Database.Save(estXpers)
            // End If
            // Return estXpers
            // End Function

            public string FormatTipo(Finanziaria.TipoEstinzione tipo)
            {
                switch (tipo)
                {
                    case Finanziaria.TipoEstinzione.ESTINZIONE_NO:
                        {
                            return "";
                        }

                    case Finanziaria.TipoEstinzione.ESTINZIONE_CESSIONEDELQUINTO:
                        {
                            return "CQS";
                        }

                    case Finanziaria.TipoEstinzione.ESTINZIONE_CQP:
                        {
                            return "CQP";
                        }

                    case Finanziaria.TipoEstinzione.ESTINZIONE_PRESTITODELEGA:
                        {
                            return "PD";
                        }

                    case Finanziaria.TipoEstinzione.ESTINZIONE_PRESTITOPERSONALE:
                        {
                            return "Prestito Personale";
                        }

                    case Finanziaria.TipoEstinzione.ESTINZIONE_PIGNORAMENTO:
                        {
                            return "Pignoramento";
                        }

                    case Finanziaria.TipoEstinzione.ESTINZIONE_MUTUO:
                        {
                            return "Mutuo";
                        }

                    case Finanziaria.TipoEstinzione.ESTINZIONE_PROTESTI:
                        {
                            return "Protesti";
                        }

                    case Finanziaria.TipoEstinzione.ESTINZIONE_ASSICURAZIONE:
                        {
                            return "Assicurazione";
                        }

                    case Finanziaria.TipoEstinzione.ESTINZIONE_ALIMENTI:
                        {
                            return "Alimenti";
                        }

                    case Finanziaria.TipoEstinzione.ESTINZIONE_PICCOLO_PRESTITO:
                        {
                            return "Piccolo Prestito";
                        }

                    default:
                        {
                            return "invalid";
                        }
                }
            }

            public CCollection<Finanziaria.EstinzioneXEstintore> GetEstinzioniXEstintore(Finanziaria.IEstintore estintore)
            {
                if (estintore is null)
                    throw new ArgumentNullException("estintore");
                return GetEstinzioniXEstintore(DBUtils.GetID((Databases.IDBObjectBase)estintore), DMD.RunTime.vbTypeName(estintore));
            }

            public CCollection<Finanziaria.EstinzioneXEstintore> GetEstinzioniXEstintore(int idEstintore, string tipoEstintore)
            {
                using (var cursor = new Finanziaria.EstinzioneXEstintoreCursor())
                {
                    if (string.IsNullOrEmpty(tipoEstintore))
                        throw new ArgumentNullException("tipoEstintore");
                    var ret = new CCollection<Finanziaria.EstinzioneXEstintore>();
                    if (idEstintore == 0)
                        return ret;
                    cursor.IDEstintore.Value = idEstintore;
                    cursor.TipoEstintore.Value = tipoEstintore;
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.IgnoreRights = true;
                    while (!cursor.EOF())
                    {
                        ret.Add(cursor.Item);
                        cursor.MoveNext();
                    }

                    return ret;
                }
            }

            /// <summary>
        /// Controlla tra tutti i prestiti in corso programmando le date di ricontatto secondo i parametri
        /// </summary>
        /// <remarks></remarks>
            public void RielaboraRinnovabili()
            {
                using (var cursor = new Finanziaria.CEstinzioniCursor())
                {
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.IgnoreRights = true;
                    cursor.IDEstintoDa.Value = 0;
                    cursor.IDEstintoDa.IncludeNulls = true;
                    cursor.Scadenza.Value = DMD.DateUtils.ToDay();
                    cursor.Scadenza.Operator = Databases.OP.OP_GE;
                    // cursor.Scadenza.IncludeNulls = True
                    while (!cursor.EOF())
                    {
                        cursor.Item.Save(true);
                        cursor.MoveNext();
                    }
                }
            }

            public string CanDelete(Finanziaria.CEstinzione p)
            {
                if (p is null)
                    throw new ArgumentNullException("p");
                return CanDelete(DBUtils.GetID(p));
            }

            public string CanDelete(int id)
            {
                Finanziaria.EstinzioneXEstintoreCursor cursor = null;
                try
                {
                    if (id == 0)
                        return DMD.Strings.vbNullString;
                    cursor = new Finanziaria.EstinzioneXEstintoreCursor();
                    cursor.IgnoreRights = true;
                    cursor.IDEstintore.Value = id;
                    while (!cursor.EOF())
                    {
                        var item = cursor.Item;
                        object estintore = item.Estintore;
                        if (estintore is Finanziaria.CPraticaCQSPD)
                        {
                            if (((Finanziaria.CPraticaCQSPD)estintore).Stato == ObjectStatus.OBJECT_VALID)
                            {
                                return "Pratica N°" + ((Finanziaria.CPraticaCQSPD)estintore).NumeroPratica;
                            }
                        }
                        else if (estintore is Finanziaria.CQSPDConsulenza)
                        {
                            if (((Finanziaria.CQSPDConsulenza)estintore).Stato == ObjectStatus.OBJECT_VALID)
                            {
                                return "Simulazione" + ((Finanziaria.CQSPDConsulenza)estintore).ID;
                            }
                        }
                        else if (estintore is Databases.IDBObject)
                        {
                            if (((Databases.IDBObject)estintore).Stato == ObjectStatus.OBJECT_VALID)
                            {
                                return DMD.RunTime.vbTypeName(estintore) + DBUtils.GetID((Databases.IDBObjectBase)estintore);
                            }
                        }
                        else
                        {
                            return DMD.RunTime.vbTypeName(estintore) + DBUtils.GetID((Databases.IDBObjectBase)estintore);
                        }

                        cursor.MoveNext();
                    }

                    return "";
                }
                catch (Exception ex)
                {
                    Sistema.Events.NotifyUnhandledException(ex);
                    throw;
                }
                finally
                {
                    if (cursor is object)
                    {
                        cursor.Dispose();
                        cursor = null;
                    }
                }
            }

            public CCollection<Finanziaria.CEstinzione> GetPrestitiAttivi(Anagrafica.CPersonaFisica persona, DateTime data)
            {
                CCollection<Finanziaria.CEstinzione> items = GetEstinzioniByPersona(persona);
                var ret = new CCollection<Finanziaria.CEstinzione>();
                foreach (Finanziaria.CEstinzione e in items)
                {
                    if (e.IsInCorso(data))
                    {
                        ret.Add(e);
                    }
                }

                return ret;
            }

            public CCollection<Finanziaria.CEstinzione> GetPrestitiAttivi(Anagrafica.CPersonaFisica persona)
            {
                return GetPrestitiAttivi(persona, DMD.DateUtils.Now());
            }

            protected internal new void doItemCreated(ItemEventArgs e)
            {
                base.doItemCreated(e);
            }

            protected internal new void doItemDeleted(ItemEventArgs e)
            {
                base.doItemDeleted(e);
            }

            protected internal new void doItemModified(ItemEventArgs e)
            {
                base.doItemModified(e);
            }

            public int? getMeseRinnovo(int? durata)
            {
                var m_Mesi = new[] { 24, 36, 48, 60, 72, 84, 96, 108, 120 };
                var m_Rinnovo = new[] { 10, 15, 20, 24, 29, 34, 39, 44, 48 };
                int i = DMD.Arrays.IndexOf(m_Mesi, (int)durata);
                if (i < 0)
                    return default; // throw "ArgumentException: La durata deve essere compresa tra 24 e 120 e multipla di 12";
                return m_Rinnovo[i];
            }

            //public override void Initialize()
            //{
            //    base.Initialize();
            //    Databases.CDBTable table;
            //    Databases.CDBEntityField col;
            //    table = Finanziaria.Database.Tables.GetItemByKey("tbl_Estinzioni");
            //    col = table.Fields.Alter("TAEG", typeof(double));
            //    col = table.Fields.Alter("StatoRichiestaConteggio", typeof(int));
            //    col = table.Fields.Alter("DataRichiestaConteggio", typeof(DateTime));
            //    col = table.Fields.Alter("ConteggioRichiestoDaID", typeof(int));
            //    col = table.Fields.Alter("DataEsito", typeof(DateTime));
            //    col = table.Fields.Alter("EsitoUserID", typeof(int));
            //    col = table.Fields.Alter("IDDocumentoEsito", typeof(int));
            //    col = table.Fields.Alter("NoteRichiestaConteggio", typeof(string), 0);
            //    col = table.Fields.Alter("NoteEsito", typeof(string), 0);
            //    table.Update();
            //}
        }
    }

    public partial class Finanziaria
    {
        private static CEstinzioniClass m_Estinzioni = null;

        public static CEstinzioniClass Estinzioni
        {
            get
            {
                if (m_Estinzioni is null)
                    m_Estinzioni = new CEstinzioniClass();
                return m_Estinzioni;
            }
        }
    }
}