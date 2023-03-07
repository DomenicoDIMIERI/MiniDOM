using System;
using System.Collections;
using System.Diagnostics;
using DMD;
using DMD.XML;
using minidom;
using static minidom.Sistema;

namespace minidom
{
    public partial class Finanziaria
    {
        [Serializable]
        public class CProfilo : Databases.DBObject, ISupportInitializeFrom, IComparable, ICloneable
        {
            private int m_CessionarioID;
            [NonSerialized]
            private CCQSPDCessionarioClass m_Cessionario;
            private string m_NomeCessionario;
            private string m_Nome;
            private string m_ProfiloVisibile;
            private string m_Profilo;
            private string m_Path;
            private bool m_Visibile;
            private string m_IconPath;
            private string m_UserName;
            private string m_Password;
            private int m_ParentID;
            private CProfilo m_Parent;
            private bool m_EreditaProdotti;
            private double m_ProvvigioneFissa;
            private bool m_ConsentiProvvigione;
            private DateTime? m_DataInizio;
            private DateTime? m_DataFine;
            [NonSerialized]
            private CProdottiXProfiloRelations m_ProdottiXProfiloRelations;
            [NonSerialized]
            private CProfiloXUserAllowNegateCollection m_UsersAuth;
            [NonSerialized]
            private CProfiloXGroupAllowNegateCollection m_GroupAuth;

            public CProfilo()
            {
            }

            public override CModulesClass GetModule()
            {
                return Profili.Module;
            }

            /// <summary>
        /// Restituisce un oggetto che consente di accedere e modificare la relazione diretta (eredita, includi, escludi) e gli spread per ogni singolo prodotto.
        /// Se il prodotto non è definito in questa collezione e se la proprietà EreditaProdotti è impostata su True esso verrà ereditato direttamente dal preventivatore genitore (se esiste).
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public CProdottiXProfiloRelations ProdottiXProfiloRelations
            {
                get
                {
                    if (m_ProdottiXProfiloRelations is null)
                        m_ProdottiXProfiloRelations = new CProdottiXProfiloRelations(this);
                    return m_ProdottiXProfiloRelations;
                }
            }

            /// <summary>
        /// Restituisce l'insieme delle associazioni (consenti/nega) del profilo corrente per gli specifici utenti
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public CProfiloXUserAllowNegateCollection UserAuth
            {
                get
                {
                    if (m_UsersAuth is null)
                        m_UsersAuth = new CProfiloXUserAllowNegateCollection(this);
                    return m_UsersAuth;
                }
            }

            /// <summary>
        /// Restituisce l'insieme delle associazioni (consenti/nega) del profilo corrente per i gruppi utente
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public CProfiloXGroupAllowNegateCollection GroupAuth
            {
                get
                {
                    if (m_GroupAuth is null)
                        m_GroupAuth = new CProfiloXGroupAllowNegateCollection(this);
                    return m_GroupAuth;
                }
            }

            /// <summary>
        /// Restituisce o imposta la data di inizio validità del profilo
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public DateTime? DataInizio
            {
                get
                {
                    return m_DataInizio;
                }

                set
                {
                    var oldValue = m_DataInizio;
                    if (oldValue == value == true)
                        return;
                    m_DataInizio = value;
                    DoChanged("DataInizio", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la data di fine validità del profilo
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public DateTime? DataFine
            {
                get
                {
                    return m_DataFine;
                }

                set
                {
                    var oldValue = m_DataFine;
                    if (oldValue == value == true)
                        return;
                    m_DataFine = value;
                    DoChanged("DataFine", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce vero se il profilo è valido alla data corrente
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
            public bool IsValid()
            {
                return IsValid(DMD.DateUtils.Now());
            }

            /// <summary>
        /// Restituisce vero se il profilo è valido alla data corrente
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
            public bool IsValid(DateTime allaData)
            {
                return DMD.DateUtils.CheckBetween(allaData, m_DataInizio, m_DataFine);
            }

            /// <summary>
        /// Restituisce il valore della provvigione fissa imposta al collaboratore
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public double ProvvigioneFissa
            {
                get
                {
                    return m_ProvvigioneFissa;
                }

                set
                {
                    double oldValue = m_ProvvigioneFissa;
                    if (oldValue == value)
                        return;
                    m_ProvvigioneFissa = value;
                    DoChanged("ProvvigioneFissa", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta un valore che indica se consentire al collaboratore di modificare la provvigione.
        /// Se questo valore è impostato su False allora il collaboratore potrà caricare solo la ProvvigioneFissa
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public bool ConsentiProvvigione
            {
                get
                {
                    return m_ConsentiProvvigione;
                }

                set
                {
                    if (m_ConsentiProvvigione == value)
                        return;
                    m_ConsentiProvvigione = value;
                    DoChanged("ConsentiProvvigione", value, !value);
                }
            }

            /// <summary>
        /// Restituisce o imposta il nome del cessionario a cui appartiene il profilo
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string NomeCessionario
            {
                get
                {
                    return m_NomeCessionario;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_NomeCessionario;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeCessionario = value;
                    DoChanged("NomeCessionario", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'ID del cessionario a cui appartiene il profilo
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int IDCessionario
            {
                get
                {
                    return DBUtils.GetID(m_Cessionario, m_CessionarioID);
                }

                set
                {
                    int oldValue = IDCessionario;
                    if (oldValue == value)
                        return;
                    m_CessionarioID = value;
                    m_Cessionario = null;
                    DoChanged("IDCessionario", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'oggetto Cessionario a cui appartiene il profilo
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public CCQSPDCessionarioClass Cessionario
            {
                get
                {
                    if (m_Cessionario is null)
                        m_Cessionario = Cessionari.GetItemById(m_CessionarioID);
                    return m_Cessionario;
                }

                set
                {
                    var oldValue = Cessionario;
                    if (oldValue == value)
                        return;
                    m_Cessionario = value;
                    m_CessionarioID = DBUtils.GetID(value);
                    if (value is object)
                        m_NomeCessionario = value.Nome;
                    DoChanged("Cessionario", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta un valore booleano che indica se il profilo è visibile
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public bool Visibile
            {
                get
                {
                    return m_Visibile;
                }

                set
                {
                    if (m_Visibile == value)
                        return;
                    m_Visibile = value;
                    DoChanged("Visibile", value, !value);
                }
            }

            /// <summary>
        /// Restituisce o imposta il nome del profilo
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string Nome
            {
                get
                {
                    return m_Nome;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_Nome;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Nome = value;
                    DoChanged("Nome", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta una stringa che viene visualizzata nell'elenco dei profili
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string Profilo
            {
                get
                {
                    return m_Profilo;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_Profilo;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Profilo = value;
                    DoChanged("Profilo", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta l'ID del genitore di questo profilo
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int ParentID
            {
                get
                {
                    return DBUtils.GetID(m_Parent, m_ParentID);
                }

                set
                {
                    int oldValue = ParentID;
                    if (oldValue == value)
                        return;
                    m_ParentID = value;
                    m_Parent = null;
                    DoChanged("ParentID", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta un valore booleano che indica se il profilo può ereditare (ricorsivamente) i prodotti dal genitore
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public bool EreditaProdotti
            {
                get
                {
                    return m_EreditaProdotti;
                }

                set
                {
                    if (m_EreditaProdotti == value)
                        return;
                    m_EreditaProdotti = value;
                    DoChanged("EreditaProdotti", value, !value);
                }
            }

            /// <summary>
        /// Restituisce o imposta il profilo genitore di questo profilo
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public CProfilo Parent
            {
                get
                {
                    if (m_Parent is null)
                        m_Parent = Profili.GetItemById(m_ParentID);
                    return m_Parent;
                }

                set
                {
                    var oldValue = Parent;
                    if (oldValue == value)
                        return;
                    m_Parent = value;
                    m_ParentID = DBUtils.GetID(value);
                    DoChanged("Parent", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta una stringa che viene visualizzata in caso l'utente non disponga dei diritti
        /// per visualizzare il nome del profilo
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string ProfiloVisibile
            {
                get
                {
                    return m_ProfiloVisibile;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_ProfiloVisibile;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_ProfiloVisibile = value;
                    DoChanged("ProfiloVisibile", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta il percorso della backdoor per effettuare l'accesso ai profili esterni
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string Path
            {
                get
                {
                    return m_Path;
                }

                set
                {
                    string oldValue = m_Path;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Path = value;
                    DoChanged("Path", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce vero se il profilo non è esterno
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
            public bool IsOffline()
            {
                return string.IsNullOrEmpty("" + m_Path);
            }

            /// <summary>
        /// Restituisce o imposta la URL o il percorso dell'immagine che identifica il profilo
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string IconPath
            {
                get
                {
                    return m_IconPath;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_IconPath;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_IconPath = value;
                    DoChanged("IconPath", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la username utilizzata per accedere al profilo esterno
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string UserName
            {
                get
                {
                    return m_UserName;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_UserName;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_UserName = value;
                    DoChanged("UserName", value, oldValue);
                }
            }

            /// <summary>
        /// Restituisce o imposta la password utilizzata per accedere al profilo esterno
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public string Password
            {
                get
                {
                    return m_Password;
                }

                set
                {
                    string oldValue = m_Password;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Password = value;
                    DoChanged("Password");
                }
            }


            // Public Function DefaultLink() As String
            // If Me.IsOffline() Then
            // Return "/Finanziaria/preventivo/?_a=preventivo&pid=" & GetID(Me)
            // Else
            // 'tmp = Path & "?tk=" & ASPSecurity.FindTokenOrCreate("preventivatore=" & ID, ID)
            // Return "/download.asp?tk=" & ASPSecurity.FindTokenOrCreate("preventivatore=" & GetID(Me), Me.Path & "?pid=" & GetID(Me))
            // End If
            // End Function

            // Public Function DefaultTarget() As String
            // If Me.IsOffline() Then
            // Return "_self"
            // Else
            // Return "_blank"
            // End If
            // End Function


            public int CompareTo(CProfilo item)
            {
                return DMD.Strings.Compare(Profilo, item.Profilo, true);
            }

            int IComparable.CompareTo(object obj)
            {
                return CompareTo((CProfilo)obj);
            }

            public CCollection<CCQSPDProdotto> GetProdottiValidi()
            {
                CCollection<CCQSPDProdotto> items;
                var ret = new CCollection<CCQSPDProdotto>();
                items = ProdottiXProfiloRelations.GetProdotti();
                for (int i = 0, loopTo = items.Count - 1; i <= loopTo; i++)
                {
                    var prodotto = items[i];
                    if (prodotto.IsValid())
                        ret.Add(prodotto);
                }

                return ret;
            }


            /// <summary>
        /// Crea o modifica l'associazione (consenti/nega) tra il profilo corrente e l'utente
        /// </summary>
        /// <param name="user"></param>
        /// <param name="allow"></param>
        /// <returns></returns>
        /// <remarks></remarks>
            public CProfiloXUserAllowNegate SetUserAllowNegate(Sistema.CUser user, bool allow)
            {
                CProfiloXUserAllowNegate item = null;
                int i = 0;
                while (i < UserAuth.Count && item is null)
                {
                    if (UserAuth[i].UserID == DBUtils.GetID(user))
                    {
                        item = UserAuth[i];
                    }
                    else
                    {
                        i += 1;
                    }
                }

                if (item is null)
                {
                    item = new CProfiloXUserAllowNegate();
                    UserAuth.Add(item);
                }

                item.Item = this;
                item.User = user;
                item.Allow = allow;
                item.Save();
                return item;
            }

            /// <summary>
        /// Crea o modifica l'associazione (consenti/nega) tra il profilo corrente ed il gruppo
        /// </summary>
        /// <param name="group"></param>
        /// <param name="allow"></param>
        /// <returns></returns>
        /// <remarks></remarks>
            public CProfiloXGroupAllowNegate SetGroupAllowNegate(Sistema.CGroup group, bool allow)
            {
                CProfiloXGroupAllowNegate item = null;
                int i = 0;
                while (i < GroupAuth.Count && item is null)
                {
                    if (GroupAuth[i].GroupID == DBUtils.GetID(group))
                    {
                        item = GroupAuth[i];
                    }
                    else
                    {
                        i += 1;
                    }
                }

                if (item is null)
                {
                    item = new CProfiloXGroupAllowNegate();
                    GroupAuth.Add(item);
                }

                item.Item = this;
                item.Group = group;
                item.Allow = allow;
                item.Save();
                return item;
            }


            /// <summary>
        /// Restituisce vero se l'utente è abilitato all'azione corrente anche considerando i gruppi a cui appartiene l'utente stesso
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        /// <remarks></remarks>
            public bool IsVisibleToUser(Sistema.CUser user)
            {
                bool a = false;
                int i = 0;
                CProfiloXUserAllowNegate ua;
                CProfiloXGroupAllowNegate ga;
                while (i < UserAuth.Count)
                {
                    ua = UserAuth[i];
                    if (ua.UserID == DBUtils.GetID(user))
                    {
                        a = a | ua.Allow;
                    }

                    i += 1;
                }

                i = 0;
                while (i < GroupAuth.Count)
                {
                    ga = GroupAuth[i];
                    foreach (Sistema.CGroup gp in user.Groups)
                    {
                        if (ga.GroupID == DBUtils.GetID(gp))
                        {
                            a = a | ga.Allow;
                        }
                    }

                    i += 1;
                }

                return a;
            }

            public bool IsVisibleToUser(int userID)
            {
                return IsVisibleToUser(Sistema.Users.GetItemById(userID));
            }


            /// <summary>
        /// Restituisce una collezione di tutti i tipi contratto disponibili per il profilo
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
            public CCollection<CTipoContratto> GetTipiContrattoDisponibili()
            {
                var items = new Hashtable();
                foreach (CCQSPDProdotto p in GetProdottiValidi())
                {
                    if (!string.IsNullOrEmpty(p.IdTipoContratto) && items.ContainsKey(p.IdTipoContratto))
                        items.Add(p.IdTipoContratto, p.IdTipoContratto);
                }

                var ret = new CCollection<CTipoContratto>();
                foreach (string tc in items.Keys)
                {
                    var item = TipiContratto.GetItemByIdTipoContratto(tc);
                    if (item is object)
                        ret.Add(item);
                }

                ret.Sort();
                return ret;
            }

            /// <summary>
        /// Restituisce la collezione dei tipi rapporto disponibili in funzione del tipo contratto selezionato
        /// </summary>
        /// <param name="tipoContratto"></param>
        /// <returns></returns>
        /// <remarks></remarks>
            public CKeyCollection<Anagrafica.CTipoRapporto> GetTipiRapportoDisponibili(string tipoContratto)
            {
                var sc = new CStringComparer();
                var ret = new CKeyCollection<Anagrafica.CTipoRapporto>();
                tipoContratto = Strings.LCase(Strings.Trim(tipoContratto));
                var prodotti = GetProdottiValidi();
                for (int i = 0, loopTo = prodotti.Count - 1; i <= loopTo; i++)
                {
                    var p = prodotti[i];
                    if ((Strings.LCase(p.IdTipoContratto) ?? "") == (tipoContratto ?? "") && p.Visibile == true)
                    {
                        Debug.Assert(p.Stato == ObjectStatus.OBJECT_VALID);
                    }

                    var tr = Anagrafica.TipiRapporto.GetItemByName(p.IdTipoRapporto);
                    if (tr is object)
                    {
                        if (!ret.ContainsKey(p.IdTipoRapporto))
                            ret.Add(p.IdTipoRapporto, tr);
                    }
                }

                ret.Sort();
                return ret;
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Database;
            }

            public override string GetTableName()
            {
                return "tbl_Preventivatori";
            }

            protected override bool LoadFromRecordset(DBReader reader)
            {
                m_NomeCessionario = reader.Read("NomeCessionario", m_NomeCessionario);
                m_CessionarioID = reader.Read("Cessionario", m_CessionarioID);
                m_Nome = reader.Read("Nome", m_Nome);
                m_Profilo = reader.Read("Profilo", m_Profilo);
                m_Path = reader.Read("path", m_Path);
                m_ProfiloVisibile = reader.Read("ProfiloVisibile", m_ProfiloVisibile);
                m_IconPath = reader.Read("IconPath", m_IconPath);
                m_UserName = reader.Read("UserName", m_UserName);
                m_Password = reader.Read("Password", m_Password);
                m_ParentID = reader.Read("Padre", m_ParentID);
                m_Visibile = reader.Read("Visibile", m_Visibile);
                m_EreditaProdotti = reader.Read("EreditaProdotti", m_EreditaProdotti);
                m_ProvvigioneFissa = reader.Read("ProvvigioneFissa", m_ProvvigioneFissa);
                m_ConsentiProvvigione = reader.Read("ConsentiProvvigione", m_ConsentiProvvigione);
                m_DataInizio = reader.Read("DataInizio", m_DataInizio);
                m_DataFine = reader.Read("DataFine", m_DataFine);
                return base.LoadFromRecordset(reader);
            }

            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("ClassName", DMD.RunTime.vbTypeName(this));
                writer.Write("NomeCessionario", m_NomeCessionario);
                writer.Write("Cessionario", IDCessionario);
                writer.Write("Nome", m_Nome);
                writer.Write("Profilo", m_Profilo);
                writer.Write("Visibile", m_Visibile);
                writer.Write("path", m_Path);
                writer.Write("ProfiloVisibile", m_ProfiloVisibile);
                writer.Write("IconPath", m_IconPath);
                writer.Write("UserName", m_UserName);
                writer.Write("Password", m_Password);
                writer.Write("Padre", ParentID);
                writer.Write("EreditaProdotti", m_EreditaProdotti);
                writer.Write("ProvvigioneFissa", m_ProvvigioneFissa);
                writer.Write("ConsentiProvvigione", m_ConsentiProvvigione);
                writer.Write("DataInizio", m_DataInizio);
                writer.Write("DataFine", m_DataFine);
                return base.SaveToRecordset(writer);
            }

            public override string ToString()
            {
                return m_Nome + " - " + m_ProfiloVisibile;
            }

            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("m_CessionarioID", IDCessionario);
                writer.WriteAttribute("m_NomeCessionario", m_NomeCessionario);
                writer.WriteAttribute("m_Nome", m_Nome);
                writer.WriteAttribute("m_ProfiloVisibile", m_ProfiloVisibile);
                writer.WriteAttribute("m_Profilo", m_Profilo);
                writer.WriteAttribute("m_Path", m_Path);
                writer.WriteAttribute("m_Visibile", m_Visibile);
                writer.WriteAttribute("m_IconPath", m_IconPath);
                writer.WriteAttribute("m_UserName", m_UserName);
                writer.WriteAttribute("m_Password", m_Password);
                writer.WriteAttribute("m_ParentID", ParentID);
                writer.WriteAttribute("m_EreditaProdotti", m_EreditaProdotti);
                writer.WriteAttribute("m_ProvvigioneFissa", m_ProvvigioneFissa);
                writer.WriteAttribute("m_ConsentiProvvigione", m_ConsentiProvvigione);
                writer.WriteAttribute("m_DataInizio", m_DataInizio);
                writer.WriteAttribute("m_DataFine", m_DataFine);
                base.XMLSerialize(writer);
            }

            protected override void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "m_CessionarioID":
                        {
                            m_CessionarioID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "m_NomeCessionario":
                        {
                            m_NomeCessionario = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "m_Nome":
                        {
                            m_Nome = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "m_ProfiloVisibile":
                        {
                            m_ProfiloVisibile = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "m_Profilo":
                        {
                            m_Profilo = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "m_Path":
                        {
                            m_Path = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "m_Visibile":
                        {
                            m_Visibile = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                            break;
                        }

                    case "m_IconPath":
                        {
                            m_IconPath = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "m_UserName":
                        {
                            m_UserName = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "m_Password":
                        {
                            m_Password = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "m_ParentID":
                        {
                            m_ParentID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "m_EreditaProdotti":
                        {
                            m_EreditaProdotti = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                            break;
                        }

                    case "m_ProvvigioneFissa":
                        {
                            m_ProvvigioneFissa = (double)DMD.XML.Utils.Serializer.DeserializeDouble(DMD.Strings.CStr(fieldValue));
                            break;
                        }

                    case "m_ConsentiProvvigione":
                        {
                            m_ConsentiProvvigione = DMD.XML.Utils.Serializer.DeserializeBoolean(fieldValue) == true;
                            break;
                        }

                    case "m_DataInizio":
                        {
                            m_DataInizio = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "m_DataFine":
                        {
                            m_DataFine = DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    default:
                        {
                            base.SetFieldInternal(fieldName, fieldValue);
                            break;
                        }
                }
            }

            protected override void OnAfterSave(SystemEvent e)
            {
                base.OnAfterSave(e);
                Profili.UpdateCached(this);
            }

            public override void InitializeFrom(object value)
            {
                base.InitializeFrom(value);
                m_ProdottiXProfiloRelations = null;
                m_UsersAuth = null;
                m_GroupAuth = null;
            }

            protected internal void InvalidateProdottiProfilo()
            {
                m_ProdottiXProfiloRelations = null;
            }

            public object Clone()
            {
                return MemberwiseClone();
            }
        }
    }
}