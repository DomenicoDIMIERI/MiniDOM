using System;
using DMD;
using DMD.XML;
using minidom;
using static minidom.Sistema;


namespace minidom
{
    public partial class Finanziaria
    {
        public enum StatoTeamManager : int
        {
            STATO_ATTIVO = Sistema.UserStatus.USER_ENABLED,
            STATO_DISABILITATO = Sistema.UserStatus.USER_DISABLED,
            STATO_ELIMINATO = Sistema.UserStatus.USER_DELETED,
            STATO_INATTIVAZIONE = Sistema.UserStatus.USER_NEW,
            STATO_SOSPESO = Sistema.UserStatus.USER_SUSPENDED
            // STATO_INVALID = UserStatus.USER_TEMP
        }

        [Serializable]
        public class CTeamManager 
            : Databases.DBObjectPO, IComparable, ICloneable
        {
            private string m_Nominativo;
            private StatoTeamManager m_StatoTeamManager;
            private int m_ListinoPredefinitoID;
            [NonSerialized] private CProfilo m_ListinoPredefinito;
            private int m_ReferenteID;
            [NonSerialized] private CAreaManager m_Referente;
            [NonSerialized] private Sistema.CUser m_User;
            private int m_UserID;
            private int m_PersonaID;
            [NonSerialized] private Anagrafica.CPersona m_Persona;
            private string m_Rapporto;
            private DateTime? m_DataInizioRapporto;
            private DateTime? m_DataFineRapporto;
            private bool m_SetPremiPersonalizzato;
            private int m_SetPremiID;
            [NonSerialized] private CSetPremi m_SetPremi;
            // Private m_Produttori As CTMProduttoriCollection
            // Private m_Pratiche As CTMPraticheCollection

            /// <summary>
            /// Costruttore
            /// </summary>
            public CTeamManager()
            {
                m_PersonaID = 0;
                m_Persona = null;
                m_UserID = 0;
                m_User = null;
                m_ReferenteID = 0;
                m_Referente = null;
                m_ListinoPredefinito = null;
                // Me.m_Produttori = Nothing
                // Me.m_Pratiche = Nothing
                m_SetPremi = null;
                m_SetPremiID = 0;
                m_SetPremiPersonalizzato = false;
                m_StatoTeamManager = StatoTeamManager.STATO_INATTIVAZIONE;
            }

            public override CModulesClass GetModule()
            {
                return TeamManagers.Module;
            }

            public string Nominativo
            {
                get
                {
                    return m_Nominativo;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_Nominativo;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Nominativo = value;
                    DoChanged("Nominativo", value, oldValue);
                }
            }

            public bool SetPremiPersonalizzato
            {
                get
                {
                    return m_SetPremiPersonalizzato;
                }

                set
                {
                    if (m_SetPremiPersonalizzato == value)
                        return;
                    m_SetPremiPersonalizzato = value;
                    DoChanged("SetPremiPersonalizzato", value, !value);
                }
            }

            public int IDSetPremiSpecificato
            {
                get
                {
                    return DBUtils.GetID(m_SetPremi, m_SetPremiID);
                }

                set
                {
                    int oldValue = IDSetPremiSpecificato;
                    if (oldValue == value)
                        return;
                    m_SetPremi = null;
                    m_SetPremiID = value;
                    DoChanged("IDSetPremiSpecificato", value, oldValue);
                }
            }

            public CSetPremi SetPremiSpecificato
            {
                get
                {
                    if (m_SetPremi is null)
                        m_SetPremi = TeamManagers.GetSetPremiById(m_SetPremiID);
                    return m_SetPremi;
                }

                set
                {
                    if (DBUtils.GetID(value) == IDSetPremiSpecificato)
                        return;
                    var oldValue = SetPremiSpecificato;
                    m_SetPremi = value;
                    m_SetPremiID = DBUtils.GetID(value);
                    DoChanged("SetPremiSpecificato", value, oldValue);
                }
            }

            public StatoTeamManager StatoTeamManager
            {
                get
                {
                    return m_StatoTeamManager;
                }

                set
                {
                    var oldValue = m_StatoTeamManager;
                    if (oldValue == value)
                        return;
                    m_StatoTeamManager = value;
                    DoChanged("StatoTeamManager", value, oldValue);
                }
            }

            public CProfilo ListinoPredefinito
            {
                get
                {
                    if (m_ListinoPredefinito is null)
                        m_ListinoPredefinito = Profili.GetItemById(m_ListinoPredefinitoID);
                    return m_ListinoPredefinito;
                }

                set
                {
                    var oldValue = ListinoPredefinito;
                    if (oldValue == value)
                        return;
                    m_ListinoPredefinito = value;
                    m_ListinoPredefinitoID = DBUtils.GetID(value);
                    DoChanged("ListinoPredefinito", value, oldValue);
                }
            }

            public int ListinoPredefinitoID
            {
                get
                {
                    return DBUtils.GetID(m_ListinoPredefinito, m_ListinoPredefinitoID);
                }

                set
                {
                    int oldValue = ListinoPredefinitoID;
                    if (oldValue == value)
                        return;
                    m_ListinoPredefinito = null;
                    m_ListinoPredefinitoID = value;
                    DoChanged("ListinoPredefinitoID", value, oldValue);
                }
            }

            public int PersonaID
            {
                get
                {
                    return DBUtils.GetID(m_Persona, m_PersonaID);
                }

                set
                {
                    int oldValue = PersonaID;
                    if (oldValue == value)
                        return;
                    m_PersonaID = value;
                    m_Persona = null;
                    DoChanged("PersonaID", value, oldValue);
                }
            }

            public Anagrafica.CPersona Persona
            {
                get
                {
                    if (m_Persona is null)
                        m_Persona = Anagrafica.Persone.GetItemById(m_PersonaID);
                    return m_Persona;
                }

                set
                {
                    var oldValue = Persona;
                    if (oldValue == value)
                        return;
                    m_Persona = value;
                    m_PersonaID = DBUtils.GetID(value);
                    DoChanged("Persona", value, oldValue);
                }
            }

            public CAreaManager Referente
            {
                get
                {
                    if (m_Referente is null)
                        m_Referente = AreaManagers.GetItemById(m_ReferenteID);
                    return m_Referente;
                }

                set
                {
                    var oldValue = Referente;
                    if (oldValue == value)
                        return;
                    m_Referente = value;
                    m_ReferenteID = DBUtils.GetID(value);
                    DoChanged("Referente", value, oldValue);
                }
            }

            public int ReferenteID
            {
                get
                {
                    return DBUtils.GetID(m_Referente, m_ReferenteID);
                }

                set
                {
                    int oldValue = ReferenteID;
                    if (oldValue == value)
                        return;
                    m_ReferenteID = value;
                    m_Referente = null;
                    DoChanged("ReferenteID", value, oldValue);
                }
            }

            public int UserID
            {
                get
                {
                    return DBUtils.GetID(m_User, m_UserID);
                }

                set
                {
                    int oldValue = UserID;
                    if (oldValue == value)
                        return;
                    m_UserID = value;
                    m_User = null;
                    DoChanged("UserID", value, oldValue);
                }
            }

            public Sistema.CUser User
            {
                get
                {
                    if (m_User is null)
                        m_User = Sistema.Users.GetItemById(m_UserID);
                    return m_User;
                }

                set
                {
                    var oldValue = User;
                    if (oldValue == value)
                        return;
                    m_User = value;
                    m_UserID = DBUtils.GetID(value);
                    DoChanged("User", value, oldValue);
                }
            }

            public string Rapporto
            {
                get
                {
                    return m_Rapporto;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_Rapporto;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Rapporto = value;
                    DoChanged("Rapporto", value, oldValue);
                }
            }

            public DateTime? DataInizioRapporto
            {
                get
                {
                    return m_DataInizioRapporto;
                }

                set
                {
                    var oldValue = m_DataInizioRapporto;
                    if (oldValue == value == true)
                        return;
                    m_DataInizioRapporto = value;
                    DoChanged("DataInizioRapporto", value, oldValue);
                }
            }

            public DateTime? DataFineRapporto
            {
                get
                {
                    return m_DataFineRapporto;
                }

                set
                {
                    var oldValue = m_DataFineRapporto;
                    if (oldValue == value == true)
                        return;
                    m_DataFineRapporto = value;
                    DoChanged("DataFineRapporto", value, oldValue);
                }
            }

            public bool IsValid()
            {
                return IsValidAt(DMD.DateUtils.Now());
            }

            public bool IsValidAt(DateTime data)
            {
                return m_StatoTeamManager == StatoTeamManager.STATO_ATTIVO && DMD.DateUtils.CheckBetween(m_DataInizioRapporto, m_DataFineRapporto, data);
            }


            /// <summary>
        /// Restituisce il set premi definito per l'utente o quello globale se si è scelto che l'utente debba utilizzare il set globale
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
            public CSetPremi GetSetPremi()
            {
                if (SetPremiPersonalizzato)
                {
                    return SetPremiSpecificato;
                }
                else
                {
                    return TeamManagers.DefaultSetPremi;
                }
            }

            public decimal CalcolaPremioInCorso()
            {
                var tmp = default(decimal);
                return CalcolaPremioAllaData(DMD.DateUtils.Now(), ref tmp);
            }

            public decimal CalcolaPremioPrecedente()
            {
                var tmp = default(decimal);
                CSetPremi sp;
                sp = GetSetPremi();
                switch (sp.TipoIntervallo)
                {
                    case 0: // Mensile
                        {
                            return CalcolaPremioAllaData(DMD.DateUtils.DateAdd("M", -1,DMD.DateUtils.Now()), ref tmp);
                        }

                    case (TipoIntervalloSetPremi)1: // Settimanale
                        {
                            return CalcolaPremioAllaData(DMD.DateUtils.DateAdd("d", -7, DMD.DateUtils.Now()), ref tmp);
                        }

                    case (TipoIntervalloSetPremi)2: // Annuale
                        {
                            return CalcolaPremioAllaData(DMD.DateUtils.DateAdd("y", -1, DateUtils.Now()), ref tmp);
                        }

                    default:
                        {
                            throw new NotSupportedException();
                            break;
                        }
                }
            }

            /// <summary>
        /// Calcola il premio in corso e la somma dei precedenti
        /// </summary>
        /// <param name="data"></param>
        /// <param name="sommaDeiPrecedenti"></param>
        /// <returns></returns>
        /// <remarks></remarks>
            public decimal CalcolaPremioAllaData(DateTime data, ref decimal sommaDeiPrecedenti)
            {
                decimal resto, termine, parziale;
                int i, j;
                CSetPremi sp;
                DateTime dataInizio = default, dataFine = default;
                CPraticaCQSPD pratica;
                decimal ml, provvAtt, netto;
                sp = GetSetPremi();
                switch (sp.TipoIntervallo)
                {
                    case TipoIntervalloSetPremi.Mensile:
                        {
                            dataInizio = DateUtils.DateSerial(DMD.DateUtils.Year(data), DateUtils.Month(data), 1);
                            dataFine = DateUtils.DateAdd("M", 1d, dataInizio);
                            break;
                        }

                    case TipoIntervalloSetPremi.Settimanale:
                        {
                            dataInizio = DateUtils.DateSerial(DMD.DateUtils.Year(data), DateUtils.Month(data), DateUtils.Day(data) - DateUtils.Weekday(data));
                            dataFine = DateUtils.DateAdd("d", 7d, dataInizio);
                            break;
                        }

                    case TipoIntervalloSetPremi.Annuale:
                        {
                            dataInizio = DateUtils.DateSerial(DMD.DateUtils.Year(data), 1, 1);
                            dataFine = DateUtils.DateSerial(DMD.DateUtils.Year(data) + 1, 1, 1);
                            break;
                        }
                }

                ml = 0m;
                provvAtt = 0m;
                netto = 0m;
                var pratiche = new CPraticheCollection(this);
                for (int i1 = 0, loopTo = pratiche.Count - 1; i1 <= loopTo; i1++)
                {
                    pratica = pratiche[i1];
                    var info = pratica.Info;
                    if (pratica.Stato == ObjectStatus.OBJECT_VALID)
                    {
                        if (pratica.Trasferita == true)
                        {
                            if ((info.DataTrasferimento >= dataInizio & info.DataTrasferimento < dataFine) == true)
                            {
                                ml = (decimal)(ml + pratica.MontanteLordo);
                                provvAtt = (decimal)(provvAtt + pratica.ValoreSpread);
                                netto = (decimal)(netto + pratica.NettoRicavo);
                            }
                        }
                        else if (pratica.StatoAttuale.MacroStato.HasValue)
                        {
                            switch (pratica.StatoAttuale.MacroStato)
                            {
                                case StatoPraticaEnum.STATO_LIQUIDATA:
                                    {
                                        if ((pratica.StatoLiquidata.Data >= dataInizio & pratica.StatoLiquidata.Data < dataFine) == true)
                                        {
                                            ml = (decimal)(ml + pratica.MontanteLordo);
                                            provvAtt = (decimal)(provvAtt + pratica.ValoreSpread);
                                            netto = (decimal)(netto + pratica.NettoRicavo);
                                        }

                                        break;
                                    }
                            }
                        }
                    }
                }

                parziale = 0m;
                if (sp.AScaglioni)
                {
                    switch (sp.TipoCalcolo)
                    {
                        case TipoCalcoloSetPremi.SU_PROVVIGIONEATTIVA:
                            {
                                resto = provvAtt;
                                i = 0;
                                while (resto > 0m & i < sp.Scaglioni.Count)
                                {
                                    i = i + 1;
                                    termine = Sistema.IIF(sp.Scaglioni[i].Soglia < resto, sp.Scaglioni[i].Soglia, resto);
                                    resto = resto - termine;
                                    parziale = (decimal)((double)(parziale + sp.Scaglioni[i].Fisso) + (double)termine * sp.Scaglioni[i].PercSuProvvAtt / 100d);
                                }

                                break;
                            }

                        case TipoCalcoloSetPremi.SU_MONTANTELORDO:
                            {
                                resto = ml;
                                i = 0;
                                while (resto > 0m & i < sp.Scaglioni.Count)
                                {
                                    i = i + 1;
                                    termine = Sistema.IIF(sp.Scaglioni[i].Soglia < resto, sp.Scaglioni[i].Soglia, resto);
                                    resto = resto - termine;
                                    parziale = (decimal)((double)(parziale + sp.Scaglioni[i].Fisso) + (double)termine * sp.Scaglioni[i].PercSuML / 100d);
                                }

                                break;
                            }

                        case TipoCalcoloSetPremi.SU_NETTORICAVO:
                            {
                                resto = netto;
                                i = 0;
                                while (resto > 0m & i < sp.Scaglioni.Count)
                                {
                                    i = i + 1;
                                    termine = Sistema.IIF(sp.Scaglioni[i].Soglia < resto, sp.Scaglioni[i].Soglia, resto);
                                    resto = resto - termine;
                                    parziale = (decimal)((double)(parziale + sp.Scaglioni[i].Fisso) + (double)termine * sp.Scaglioni[i].PercSuNetto / 100d);
                                }

                                break;
                            }
                    }
                }
                else
                {
                    switch (sp.TipoCalcolo)
                    {
                        case TipoCalcoloSetPremi.SU_PROVVIGIONEATTIVA:
                            {
                                resto = provvAtt;
                                j = -1;
                                var loopTo1 = sp.Scaglioni.Count;
                                for (i = 1; i <= loopTo1; i++)
                                {
                                    if (sp.Scaglioni[i].Soglia > resto)
                                    {
                                        j = i - 1;
                                        break;
                                    }
                                }

                                if (j > 0)
                                {
                                    parziale = (decimal)((double)sp.Scaglioni[j].Fisso + (double)resto * sp.Scaglioni[i].PercSuProvvAtt / 100d);
                                }

                                break;
                            }

                        case TipoCalcoloSetPremi.SU_MONTANTELORDO:
                            {
                                resto = ml;
                                j = -1;
                                var loopTo2 = sp.Scaglioni.Count;
                                for (i = 1; i <= loopTo2; i++)
                                {
                                    if (sp.Scaglioni[i].Soglia > resto)
                                    {
                                        j = i - 1;
                                        break;
                                    }
                                }

                                if (j > 0)
                                {
                                    parziale = (decimal)((double)sp.Scaglioni[j].Fisso + (double)resto * sp.Scaglioni[i].PercSuML / 100d);
                                }

                                break;
                            }

                        case TipoCalcoloSetPremi.SU_NETTORICAVO:
                            {
                                resto = netto;
                                j = -1;
                                var loopTo3 = sp.Scaglioni.Count;
                                for (i = 1; i <= loopTo3; i++)
                                {
                                    if (sp.Scaglioni[i].Soglia > resto)
                                    {
                                        j = i - 1;
                                        break;
                                    }
                                }

                                if (j > 0)
                                {
                                    parziale = (decimal)((double)sp.Scaglioni[j].Fisso + (double)resto * sp.Scaglioni[i].PercSuNetto / 100d);
                                }

                                break;
                            }
                    }
                }

                return parziale;
            }

            // Public ReadOnly Property Produttori As CTMProduttoriCollection
            // Get
            // If (Me.m_Produttori Is Nothing) Then
            // Me.m_Produttori = New CTMProduttoriCollection
            // Me.m_Produttori.Initialize(Me)
            // End If
            // Return Me.m_Produttori
            // End Get
            // End Property

            // Public ReadOnly Property Pratiche As CTMPraticheCollection
            // Get
            // If (Me.m_Pratiche Is Nothing) Then
            // Me.m_Pratiche = New CTMPraticheCollection
            // Me.m_Pratiche.Initialize(Me)
            // End If
            // Return Me.m_Pratiche
            // End Get
            // End Property


            protected override Databases.CDBConnection GetConnection()
            {
                return Database;
            }

            public override string GetTableName()
            {
                return "tbl_TeamManagers";
            }

            protected override bool SaveToDatabase(Databases.CDBConnection dbConn, bool force)
            {
                Sistema.CGroup grp;
                bool ret;
                ret = base.SaveToDatabase(dbConn, force);
                if (ret)
                {
                    if (m_User is object)
                        m_User.Save(force);
                    if (m_Persona is object)
                    {
                        if (Stato == ObjectStatus.OBJECT_VALID)
                            m_Persona.Stato = ObjectStatus.OBJECT_VALID;
                        m_Persona.Save(force);
                    }

                    if (m_ListinoPredefinito is object)
                        m_ListinoPredefinito.Save(force);
                    // If Not (m_Produttori Is Nothing) Then ret = dbConn.Save(Me.m_Produttori)
                    // If Not (m_Pratiche Is Nothing) Then ret = dbConn.Save(Me.m_Pratiche)
                    if (m_SetPremi is object)
                        m_SetPremi.Save(force);

                    // Assicuriamoci che l'oggetto appartenga al gruppo corretto
                    grp = Sistema.Groups.GetItemByName("Team Managers");
                    if (grp is null)
                    {
                        grp = new Sistema.CGroup("Team Managers");
                        grp.Stato = ObjectStatus.OBJECT_VALID;
                        grp.Save();
                    }

                    if (m_UserID != 0)
                    {
                        if (Stato == ObjectStatus.OBJECT_VALID)
                        {
                            if (!grp.Members.Contains(m_UserID))
                                grp.Members.Add(m_UserID);
                            grp = Sistema.Groups.GetItemByName("Users");
                            if (!grp.Members.Contains(m_UserID))
                                grp.Members.Add(m_UserID);
                        }
                        else if (grp.Members.Contains(m_UserID))
                            grp.Members.Remove(m_UserID);
                    }
                }

                return ret;
            }

            protected override bool LoadFromRecordset(DBReader reader)
            {
                this.m_UserID = reader.Read("Utente", this.m_UserID);
                this.m_PersonaID = reader.Read("Persona", this.m_PersonaID);
                this.m_ReferenteID = reader.Read("Referente", this.m_ReferenteID);
                this.m_ListinoPredefinitoID = reader.Read("ListinoPredefinito", this.m_ListinoPredefinitoID);
                this.m_SetPremiPersonalizzato = reader.Read("SetPremiPersonalizzato", this.m_SetPremiPersonalizzato);
                this.m_Rapporto = reader.Read("Rapporto", this.m_Rapporto);
                this.m_DataInizioRapporto = reader.Read("DataInizioRapporto", this.m_DataInizioRapporto);
                this.m_DataFineRapporto = reader.Read("DataFineRapporto", this.m_DataFineRapporto);
                this.m_SetPremiID = reader.Read("SetPremi", this.m_SetPremiID);
                this.m_Nominativo = reader.Read("Nominativo", this.m_Nominativo);
                this.m_StatoTeamManager = reader.Read("StatoTeamManager", this.m_StatoTeamManager);
                return base.LoadFromRecordset(reader);
            }

            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("Utente", UserID);
                writer.Write("Persona", PersonaID);
                writer.Write("Referente", ReferenteID);
                writer.Write("SetPremi", DBUtils.GetID(m_SetPremi, m_SetPremiID));
                writer.Write("SetPremiPersonalizzato", m_SetPremiPersonalizzato);
                writer.Write("ListinoPredefinito", DBUtils.GetID(m_ListinoPredefinito, m_ListinoPredefinitoID));
                writer.Write("Rapporto", m_Rapporto);
                writer.Write("DataInizioRapporto", m_DataInizioRapporto);
                writer.Write("DataFineRapporto", m_DataFineRapporto);
                writer.Write("Nominativo", m_Nominativo);
                writer.Write("StatoTeamManager", m_StatoTeamManager);
                return base.SaveToRecordset(writer);
            }

            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Utente", DBUtils.GetID(m_User, m_UserID));
                writer.WriteAttribute("Persona", DBUtils.GetID(m_Persona, m_PersonaID));
                writer.WriteAttribute("Referente", DBUtils.GetID(m_Referente, m_ReferenteID));
                writer.WriteAttribute("SetPremi", DBUtils.GetID(m_SetPremi, m_SetPremiID));
                writer.WriteAttribute("SetPremiPersonalizzato", m_SetPremiPersonalizzato);
                writer.WriteAttribute("ListinoPredefinito", DBUtils.GetID(m_ListinoPredefinito, m_ListinoPredefinitoID));
                writer.WriteAttribute("Rapporto", m_Rapporto);
                writer.WriteAttribute("DataInizioRapporto", m_DataInizioRapporto);
                writer.WriteAttribute("DataFineRapporto", m_DataFineRapporto);
                writer.WriteAttribute("Nominativo", m_Nominativo);
                writer.WriteAttribute("StatoTeamManager", (int?)m_StatoTeamManager);
                base.XMLSerialize(writer);
            }

            protected override void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "Utente":
                        {
                            m_UserID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Persona":
                        {
                            m_PersonaID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Referente":
                        {
                            m_ReferenteID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "SetPremi":
                        {
                            m_SetPremiID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "SetPremiPersonalizzato":
                        {
                            m_SetPremiPersonalizzato = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                            break;
                        }

                    case "ListinoPredefinito":
                        {
                            m_ListinoPredefinitoID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Rapporto":
                        {
                            m_Rapporto = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "DataInizioRapporto":
                        {
                            m_DataInizioRapporto = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "DataFineRapporto":
                        {
                            m_DataFineRapporto = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "Nominativo":
                        {
                            m_Nominativo = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "StatoTeamManager":
                        {
                            m_StatoTeamManager = (StatoTeamManager)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    default:
                        {
                            base.SetFieldInternal(fieldName, fieldValue);
                            break;
                        }
                }
            }

            public int CompareTo(CTeamManager obj)
            {
                return DMD.Strings.Compare(Nominativo, obj.Nominativo, true);
            }

            int IComparable.CompareTo(object obj)
            {
                return CompareTo((CTeamManager)obj);
            }

            protected override void OnAfterSave(SystemEvent e)
            {
                base.OnAfterSave(e);
                TeamManagers.UpdateCached(this);
            }

            public object Clone()
            {
                return MemberwiseClone();
            }
        }
    }
}