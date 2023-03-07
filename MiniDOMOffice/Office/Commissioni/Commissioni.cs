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

    namespace repositories
    {

        /// <summary>
        /// Firma dell'evento StatoCommissioneChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void StatoCommissioneChangedEventHandler(object sender, ItemEventArgs<Commissione> e);


        /// <summary>
        /// Repository di <see cref="Commissione"/>
        /// </summary>
        /// <remarks></remarks>
        public sealed partial class CCommissioniClass
            : CModulesClass<Commissione>
        {

            /// <summary>
            /// Evento generato quando viene modificato lo stato di una commissione
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            /// <remarks></remarks>
            public event StatoCommissioneChangedEventHandler StatoCommissioneChanged;

            

            /// <summary>
            /// Costruttore
            /// </summary>
            public CCommissioniClass() 
                : base("modOfficeCommissioni", typeof(CommissioneCursor))
            {
                
            }

            

            /// <summary>
            /// Inizializza le risorse
            /// </summary>
            public override void Initialize()
            {
                base.Initialize();
                minidom.Anagrafica.PersonaMerged += this.HandlePersonaMerged;
                minidom.Anagrafica.PersonaUnMerged += this.HandlePersonaUnMerged;
            }

            /// <summary>
            /// Rilascia le risorse
            /// </summary>
            public override void Terminate()
            {
                minidom.Anagrafica.PersonaMerged -= this.HandlePersonaMerged;
                minidom.Anagrafica.PersonaUnMerged -= this.HandlePersonaUnMerged;

                base.Terminate();
            }

            /// <summary>
            /// Gestisce il merge delle persone
            /// </summary>
            /// <param name="e"></param>
            /// <param name="sender"></param>
            private void HandlePersonaMerged(object sender, MergePersonaEventArgs e)
            {
                lock (Anagrafica.@lock)
                {
                    var mi = e.MI;
                    var persona1 = mi.Persona1;
                    var persona2 = mi.Persona2;
                    CMergePersonaRecord rec;


                    if (persona1 is Anagrafica.CPersonaFisica)
                    {
                        // Tabella tbl_OfficeCommissioni 
                        using (var cursor = new CommissioneCursor())
                        {
                            cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                            cursor.IDPersonaIncontrata.Value = mi.IDPersona2;
                            cursor.IgnoreRights = true;
                            while (cursor.Read())
                            {
                                rec = new CMergePersonaRecord();
                                rec.NomeTabella = "tbl_OfficeCommissioni";
                                rec.RecordID = DBUtils.GetID(cursor.Item, 0);
                                rec.FieldName = "IDPersona";
                                mi.TabelleModificate.Add(rec);

                                cursor.Item.PersonaIncontrata = (CPersonaFisica)mi.Persona1;
                                cursor.Item.Save();
                            }
                        }
                    }
                    else
                    {
                        // Tabella tbl_OfficeCommissioni 
                        using (var cursor = new CommissioneCursor())
                        {
                            cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                            cursor.IDAzienda.Value = mi.IDPersona2;
                            cursor.IgnoreRights = true;
                            while (cursor.Read())
                            {
                                rec = new CMergePersonaRecord();
                                rec.NomeTabella = "tbl_OfficeCommissioni";
                                rec.RecordID = DBUtils.GetID(cursor.Item, 0);
                                rec.FieldName = "IDAzienda";
                                mi.TabelleModificate.Add(rec);

                                cursor.Item.Azienda = (CAzienda)mi.Persona1;
                                cursor.Item.Save();
                            }
                        }

                    }
                } //lock
            }

            /// <summary>
            /// Gestisce l'evento Unmerged di due persone
            /// </summary>
            /// <param name="e"></param>
            /// <param name="sender"></param>
            private void HandlePersonaUnMerged(object sender, MergePersonaEventArgs e)
            {
                lock (Anagrafica.@lock)
                {
                    var mi = e.MI;
                    if (mi.Persona1 is CPersonaFisica)
                    {
                        var items = mi.GetAffectedRecorsIDs("tbl_OfficeCommissioni", "IDPersona");
                        using (var cursor = new CommissioneCursor())
                        {
                            cursor.ID.ValueIn(items);
                            cursor.IgnoreRights = true;
                            while (cursor.Read())
                            {
                                cursor.Item.PersonaIncontrata = (CPersonaFisica)mi.Persona2;
                                cursor.Item.Save();
                            }
                        }
                    }
                    else
                    {
                        var items = mi.GetAffectedRecorsIDs("tbl_OfficeCommissioni", "IDAzienda");
                        using (var cursor = new CommissioneCursor())
                        {
                            cursor.ID.ValueIn(items);
                            cursor.IgnoreRights = true;
                            while (cursor.Read())
                            {
                                cursor.Item.Azienda = (CAzienda)mi.Persona2;
                                cursor.Item.Save();
                            }
                        }
                    }
                
                } //lock
            }


            //private Sistema.CModule InitModule()
            //{
            //    var ret = Sistema.Modules.GetItemByName("modOfficeCommissioni");
            //    if (ret is null)
            //    {
            //        ret = new Sistema.CModule("modOfficeCommissioni");
            //        ret.Description = "Commissioni";
            //        ret.DisplayName = "Commissioni";
            //        ret.Parent = Office.Module;
            //        ret.Stato = ObjectStatus.OBJECT_VALID;
            //        ret.Save();
            //        ret.InitializeStandardActions();
            //    }

            //    if (!Database.Tables.ContainsKey("tbl_OfficeCommissioni"))
            //    {
            //        var table = Database.Tables.Add("tbl_OfficeCommissioni");
            //        Databases.CDBEntityField field;
            //        field = table.Fields.Add("ID", TypeCode.Int32);
            //        field.AutoIncrement = true;
            //        field = table.Fields.Add("IDOperatore", TypeCode.Int32);
            //        field = table.Fields.Add("NomeOperatore", TypeCode.String);
            //        field.MaxLength = 255;
            //        field = table.Fields.Add("OraUscita", TypeCode.DateTime);
            //        field = table.Fields.Add("OraRientro", TypeCode.DateTime);
            //        field = table.Fields.Add("Motivo", TypeCode.String);
            //        field.MaxLength = 255;
            //        field = table.Fields.Add("Luogo", TypeCode.String);
            //        field = table.Fields.Add("IDAzienda", TypeCode.Int32);
            //        field = table.Fields.Add("NomeAzienda", TypeCode.String);
            //        field.MaxLength = 255;
            //        field = table.Fields.Add("IDPersona", TypeCode.Int32);
            //        field = table.Fields.Add("NomePersona", TypeCode.String);
            //        field.MaxLength = 255;
            //        field = table.Fields.Add("Esito", TypeCode.String);
            //        field = table.Fields.Add("Scadenzario", TypeCode.DateTime);
            //        field = table.Fields.Add("NoteScadenzario", TypeCode.String);
            //        field.MaxLength = 255;
            //        field = table.Fields.Add("IDPuntoOperativo", TypeCode.Int32);
            //        field = table.Fields.Add("NomePuntoOperativo", TypeCode.String);
            //        field.MaxLength = 255;
            //        field = table.Fields.Add("CreatoDa", TypeCode.Int32);
            //        field = table.Fields.Add("CreatoIl", TypeCode.DateTime);
            //        field = table.Fields.Add("ModificatoDa", TypeCode.Int32);
            //        field = table.Fields.Add("ModificatoIl", TypeCode.DateTime);
            //        field = table.Fields.Add("Stato", TypeCode.Int32);
            //        table.Create();
            //    }

            //    return ret;
            //}

            /// <summary>
            /// Genera l'evento 
            /// </summary>
            /// <param name="e"></param>
            internal void OnStatoCommissioneChanged(ItemEventArgs<Commissione> e)
            {
                var commissione = e.Item;
                var cliente = commissione.PersonaIncontrata;
                
                if (cliente is object)
                {
                    var info = CustomerCalls.CRM.GetContattoInfo(cliente);
                    info.AggiornaOperazione(commissione, "Commissione " + FormatStatoCommissione(commissione.StatoCommissione) + " " + commissione.Motivo);
                }

                this.StatoCommissioneChanged?.Invoke(null, e);
                this.DispatchEvent(
                        new Sistema.EventDescription(
                                    "stato_commissione", 
                                    "L'utente [" + Sistema.Users.CurrentUser.UserName 
                                    + "] ha messo la commissione [" + e.Item.ToString() 
                                    + " (" + DBUtils.GetID(e.Item, 0) + ")] in stato " 
                                    + Commissioni.FormatStatoCommissione(e.Item.StatoCommissione), e));
            }

            /// <summary>
            /// Restituisce una stringa che descrive lo stato
            /// </summary>
            /// <param name="statocommissione"></param>
            /// <returns></returns>
            public string FormatStatoCommissione(StatoCommissione statocommissione)
            {
                switch (statocommissione)
                {
                    case StatoCommissione.Annullata: return "Annullata";
                    case StatoCommissione.Completata: return "Completata";
                    case StatoCommissione.Iniziata: return "Iniziata";
                    case StatoCommissione.NonIniziata: return "Salvata";
                    case StatoCommissione.Rimandata: return "Rimandata";
                    default: throw new NotSupportedException("Stato commissione non supportato: " + (int)statocommissione);  
                }
            }

            /// <summary>
            /// Restituisce la collezione delle commissioni da fare
            /// </summary>
            /// <param name="user"></param>
            /// <param name="finoA"></param>
            /// <returns></returns>
            public CCollection<Commissione> GetCommissioniDaFare(
                                        Sistema.CUser user, 
                                        DateTime? finoA = default
                                        )
            {
                // If (user Is Nothing) Then Throw New ArgumentNullException("user")
                return GetCommissioniDaFare(DBUtils.GetID(user, 0), finoA);
            }

            /// <summary>
            /// Restituisce la collezione delle commissioni da fare
            /// </summary>
            /// <param name="userID"></param>
            /// <param name="finoA"></param>
            /// <returns></returns>
            public CCollection<Commissione> GetCommissioniDaFare(int userID, DateTime? finoA = default)
            {
                var ret = new CCollection<Commissione>();

                // If (userID <> 0 AndAlso Not Office.Commissioni.Module.UserCanDoAction("list") AndAlso Not Office.Commissioni.Module.UserCanDoAction("list_office")) Then
                // Dim dbSQL As String
                // dbSQL = "SELECT * FROM [tbl_OfficeCommissioni] WHERE [Stato]=" & ObjectStatus.OBJECT_VALID & " AND "
                // dbSQL &= "[StatoCommissione] In (" & StatoCommissione.NonIniziata & "," & StatoCommissione.Rimandata & ") AND "
                // dbSQL &= "(([IDAssegnataA]=" & userID & ") OR [IDPuntoOperativo] In ("
                // Dim items As CCollection(Of CUfficio) = Anagrafica.Uffici.GetPuntiOperativiConsentiti



                // Else
                using (var cursor = new CommissioneCursor())
                {
                    if (userID != 0)
                        cursor.IDAssegnataA.ValueIn(new[] { userID, 0 });
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.StatoCommissione.ValueIn(new[] { StatoCommissione.NonIniziata, StatoCommissione.Rimandata });
                    // cursor.IgnoreRights = True
                    if (finoA.HasValue)
                    {
                        cursor.DataPrevista.Value = finoA.Value;
                        cursor.DataPrevista.Operator = OP.OP_LE;
                        cursor.DataPrevista.IncludeNulls = true;
                    }

                    while (cursor.Read())
                    {
                        ret.Add(cursor.Item);
                    }
                }

                return ret;
            }

         
            /// <summary>
            /// Restituisce la collezione delle commissioni in corso per l'utente
            /// </summary>
            /// <param name="user"></param>
            /// <returns></returns>
            public CCollection<Commissione> GetCommissioniInCorso(Sistema.CUser user)
            {
                if (user is null)
                    throw new ArgumentNullException("user");

                var ret = new CCollection<Commissione>();
             
                using (var cursor = new CommissioneCursor())
                {
                    cursor.IDOperatore.Value = DBUtils.GetID(user, 0);
                    cursor.StatoCommissione.Value = StatoCommissione.Iniziata;
                    cursor.IgnoreRights = true;
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    while (cursor.Read())
                    {
                        ret.Add(cursor.Item);
                    }

                    return ret;
                }
                    
            }

            /// <summary>
            /// Restituisce le commissioni programmate per la persona
            /// </summary>
            /// <param name="persona"></param>
            /// <returns></returns>
            public CCollection<Commissione> GetCommissioniByPersona(CPersona persona)
            {
                if (persona is null)
                    throw new ArgumentNullException("persona");
                var ret = new CCollection<Commissione>();
                using (var cursor = new CommissioneCursor())
                {
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.IDPersonaIncontrata.Value = DBUtils .GetID(persona, 0);
                    cursor.DataPrevista.SortOrder = SortEnum.SORT_DESC;
                    cursor.IgnoreRights = true;
                    while (cursor.Read())
                    {
                        ret.Add(cursor.Item);
                    }
                }

                return ret;
            }

            /// <summary>
            /// Restituisce una collezione di tutte le commissioni (programate per qualsiasi ufficio e operatore) che si svolgono presso i luoghi specificati o presso le persone specificate
            /// </summary>
            /// <param name="personeVisitate">Array contenente gli ID delle aziende o delle persone visitate</param>
            /// <param name="luoghi">Collezione contenente gli indirizzi visitati durante la commissione</param>
            /// <returns></returns>
            /// <remarks></remarks>
            public CCollection<Commissione> GetCommissioniSuggerite(
                                        int[] personeVisitate, 
                                        CCollection<LuogoDaVisitare> luoghi, 
                                        bool strictMode
                                        )
            {
                var ret = new CCollection<Commissione>();

                using (var cursor = new CommissioneCursor())
                {
                    int[] aziende = null;  // Array ordinate degli ID delle aziende visitate
                    Commissione c;
                    Anagrafica.CIndirizzo addr;
                    if (personeVisitate is object)
                    {
                        aziende = (int[])personeVisitate.Clone();
                        DMD.Arrays.Sort(aziende);
                    }

                    cursor.IgnoreRights = true;
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.StatoCommissione.ValueIn(new[] { StatoCommissione.NonIniziata, StatoCommissione.Rimandata });

                    while (cursor.Read())
                    {
                        bool added = false;
                        c = cursor.Item;

                        // Se la commissione è relativa ad una azienda visitata nelle commissioni prese in carico
                        if (aziende is object)
                        {
                            if (
                                   (c.IDAzienda != 0 && DMD.Arrays.BinarySearch(aziende, c.IDAzienda) >= 0)
                                || (c.IDPersonaIncontrata != 0 && DMD.Arrays.BinarySearch(aziende, c.IDPersonaIncontrata) >= 0)
                                )
                            {
                                ret.Add(c);
                                added = true;
                            }
                        }

                        if (added == false && luoghi is object)
                        {
                            foreach (var ldv in c.Luoghi)
                            {
                                

                                if (ldv.IDPersona != 0 && DMD.Arrays.BinarySearch(aziende, ldv.IDPersona) >= 0)
                                {
                                    ret.Add(c);
                                    added = true;
                                }

                                if (added == false)
                                {
                                    foreach (var ldv1 in luoghi)
                                    {
                                        addr = ldv1.Indirizzo;
                                        if (strictMode)
                                        {
                                            if (addr.Equals(ldv.Indirizzo))
                                            {
                                                ret.Add(c);
                                                added = true;
                                                break;
                                            }
                                        }
                                        else if (addr.equalsIgnoreCivico(ldv.Indirizzo))
                                        {
                                            ret.Add(c);
                                            added = true;
                                            break;
                                        }
                                    }
                                }

                                if (added)
                                    break;
                            }
                        } 
                    }

                }

                return ret;
            }
             
        }


    }

    public partial class Office
    {


     
        private static CCommissioniClass m_Commissioni = null;

        /// <summary>
        /// Repository di <see cref="Commissione"/>
        /// </summary>
        public static CCommissioniClass Commissioni
        {
            get
            {
                if (m_Commissioni is null)
                    m_Commissioni = new CCommissioniClass();
                return m_Commissioni;
            }
        }
    }
}