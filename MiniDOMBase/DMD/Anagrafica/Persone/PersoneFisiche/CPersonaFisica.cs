using System;
using System.Collections;
using System.Diagnostics;
using DMD;
using DMD.XML;
using DMD.Databases;
using DMD.Databases.Collections;
using minidom;
using static minidom.Sistema;
using minidom.repositories;

namespace minidom
{
    public partial class Anagrafica
    {


        /// <summary>
        /// Rappresenta un persona fisica
        /// </summary>
        [Serializable]
        public class CPersonaFisica 
            : CPersona
        {
            private string m_Nome; // Nome della persona fisica
            private string m_Cognome; // Cognome della persona fisica o ragione sociale
            private string m_StatoCivile;
            [NonSerialized] private CImpieghi m_Impieghi;
            private CImpiegato m_ImpiegoPrincipale;
            private string m_Disabilita;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CPersonaFisica()
            {
                m_Nome = "";
                m_Cognome = "";
                m_StatoCivile = "";
                m_Disabilita = "";
                m_Impieghi = null;
                m_ImpiegoPrincipale = new CImpiegato();
                m_ImpiegoPrincipale.SetPersona(this);
                m_ImpiegoPrincipale.Stato = ObjectStatus.OBJECT_VALID;
                ResidenteA.Nome = "Residenza";
                ResidenteA.SetChanged(false);
                DomiciliatoA.Nome = "Domicilio";
                DomiciliatoA.SetChanged(false);
            }

            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Anagrafica.PersoneFisiche; //.Module;
            }

            /// <summary>
            /// Restituisce il tipo persona
            /// </summary>
            public override TipoPersona TipoPersona
            {
                get
                {
                    return TipoPersona.PERSONA_FISICA;
                }
            }

            /// <summary>
            /// Restituisce o imposta una stringa che indica una eventuale disabilità da evidenziare
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Disabilita
            {
                get
                {
                    return m_Disabilita;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_Disabilita;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Disabilita = value;
                    DoChanged("Disabilita", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta una stringa che descrive lo stato civile della persona
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string StatoCivile
            {
                get
                {
                    return m_StatoCivile;
                }

                set
                {
                    value = DMD.Strings.Left(DMD.Strings.Trim(value), 64);
                    string oldValue = m_StatoCivile;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_StatoCivile = value;
                    DoChanged("StatoCivile", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il nome della persona
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
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_Nome;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Nome = value;
                    DoChanged("Nome", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta il cognome della persona
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Cognome
            {
                get
                {
                    return m_Cognome;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_Cognome;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Cognome = value;
                    DoChanged("Cognome", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce il nominativo della persona
            /// </summary>
            public override string Nominativo
            {
                get
                {
                    return DMD.Strings.Combine(DMD.Strings.ToNameCase(this.m_Nome) , DMD.Strings.UCase(this.m_Cognome), " ");
                }
            }

            /// <summary>
            /// Restituisce la posizione lavorativa principale
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public CImpiegato ImpiegoPrincipale
            {
                get
                {
                    return m_ImpiegoPrincipale;
                }
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(CPersona obj)
            {
                return (obj is CPersonaFisica) && this.Equals((CPersonaFisica)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(CPersonaFisica obj)
            {
                return base.Equals(obj)
                     && DMD.Strings.EQ(this.m_Nome, obj.m_Nome)
                     && DMD.Strings.EQ(this.m_Cognome, obj.m_Cognome)
                     && DMD.Strings.EQ(this.m_StatoCivile, obj.m_StatoCivile)
                     && DMD.Strings.EQ(this.m_Disabilita, obj.m_Disabilita)
                     && this.ImpiegoPrincipale.Equals(obj.ImpiegoPrincipale);
            }

            /// <summary>
            /// Cambia l'impiego principale
            /// </summary>
            /// <param name="value"></param>
            /// <remarks></remarks>
            public void CambiaImpiegoPrincipale(CImpiegato value)
            {
                m_ImpiegoPrincipale.Save();
                var tmp = new CImpiegato();
                tmp.InitializeFrom(value);
                tmp.Persona = this;
                tmp.Save();
                if (m_Impieghi is object)
                    m_Impieghi.Insert(0, tmp);
                m_ImpiegoPrincipale = tmp;
                this.Save(true);
            }

            /// <summary>
            /// Restituisce la collezione degli impieghi correnti e passati 
            /// </summary>
            public CImpieghi Impieghi
            {
                get
                {
                    if (m_Impieghi is null)
                    {
                        m_Impieghi = new CImpieghi(this);
                        var tmp = m_Impieghi.GetItemById(DBUtils.GetID(m_ImpiegoPrincipale));
                        if (tmp is object)
                            m_Impieghi.Remove(tmp);
                        m_Impieghi.Insert(0, m_ImpiegoPrincipale);
                    }

                    return m_Impieghi;
                }
            }

            /// <summary>
            /// Restituisce la collezione degli impieghi correnti 
            /// </summary>
            public CCollection<CImpiegato> ImpieghiValidi
            {
                get
                {
                    var col = this.Impieghi;
                    var ret = new CCollection<CImpiegato>();
                    for (int i = 0, loopTo = col.Count - 1; i <= loopTo; i++)
                    {
                        var item = col[i];
                        if (item.DataLicenziamento.HasValue == false & item.Stato == ObjectStatus.OBJECT_VALID)
                        {
                            ret.Add(item);
                        }
                    }

                    return ret;
                }
            }

            /// <summary>
            /// Restituisce true se la persona ha subito modifiche
            /// </summary>
            /// <returns></returns>
            public override bool IsChanged()
            {
                bool ret = base.IsChanged() || m_ImpiegoPrincipale.IsChanged();
                if (ret == false && m_Impieghi is object)
                    ret = DBUtils.IsChanged(m_Impieghi);
                return ret;
            }

            /// <summary>
            /// Salva
            /// </summary>
            /// <param name="force"></param>
            public override void Save(bool force = false)
            {
                bool isNew = ID == 0;
                
                if (isNew)
                {
                    DBUtils.ResetID(this.m_ImpiegoPrincipale);
                }
                else if (DBUtils.GetID(this.m_ImpiegoPrincipale, 0) == 0)
                {
                    Debug.Print("oops");
                }

                int impID = DBUtils.GetID(this.m_ImpiegoPrincipale, 0);
                m_ImpiegoPrincipale.Stato = Stato;
                m_ImpiegoPrincipale.Save(force);

                base.Save(force);

                if (m_Impieghi is object)
                    m_Impieghi.Save(this.GetConnection(), force);

                m_ImpiegoPrincipale.Save(impID == 0);
                m_ImpiegoPrincipale.SetChanged(false);

            }

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                this.m_Nome = reader.Read("Nome", this. m_Nome);
                this.m_Cognome = reader.Read("Cognome", this.m_Cognome);
                this.m_StatoCivile = reader.Read("StatoCivile", this.m_StatoCivile);
                this.m_Disabilita = reader.Read("Categoria", this.m_Disabilita);
                int idImpiego =  reader.Read("IDImpiego", 0);
                DBUtils.SetID(m_ImpiegoPrincipale, idImpiego);
                {
                    var withBlock = m_ImpiegoPrincipale;
                    withBlock.NomePersona = Nominativo;
                    withBlock.IDAzienda = reader.Read("IMP_IDAzienda", withBlock.IDAzienda);
                    withBlock.NomeAzienda = reader.Read("IMP_NomeAzienda", withBlock.NomeAzienda);
                    withBlock.IDEntePagante = reader.Read("IMP_IDEntePagante", withBlock.IDEntePagante);
                    withBlock.NomeEntePagante = reader.Read("IMP_NomeEntePagante", withBlock.NomeEntePagante);
                    withBlock.Posizione = reader.Read("IMP_Posizione", withBlock.Posizione);
                    withBlock.DataAssunzione = reader.Read("IMP_DataAssunzione", withBlock.DataAssunzione);
                    withBlock.DataLicenziamento = reader.Read("IMP_DataLicenziamento", withBlock.DataLicenziamento);
                    withBlock.StipendioNetto = reader.Read("IMP_StipendioNetto", withBlock.StipendioNetto);
                    withBlock.StipendioLordo = reader.Read("IMP_StipendioLordo", withBlock.StipendioLordo);
                    withBlock.TipoContratto = reader.Read("IMP_TipoContratto", withBlock.TipoContratto);
                    withBlock.TipoRapporto = reader.Read("IMP_TipoRapporto", withBlock.TipoRapporto);
                    withBlock.TFR = reader.Read("IMP_TFR", withBlock.TFR);
                    withBlock.MensilitaPercepite = reader.Read("IMP_MensilitaPercepite", withBlock.MensilitaPercepite);
                    withBlock.PercTFRAzienda = reader.Read("IMP_PercTFRAzienda", withBlock.PercTFRAzienda);
                    withBlock.NomeFPC = reader.Read("IMP_NomeFPC", withBlock.NomeFPC);
                    withBlock.Flags = reader.Read("IMP_Flags", withBlock.Flags);
                    withBlock.IDSede = reader.Read("IMP_IDSede", withBlock.IDSede);
                    withBlock.NomeSede = reader.Read("IMP_NomeSede", withBlock.NomeSede);
                    withBlock.Stato = ObjectStatus.OBJECT_VALID;
                    withBlock.SetChanged(false);
                }

                return base.LoadFromRecordset(reader);
            }

            /// <summary>
            /// Salva nel db
            /// </summary>
            /// <param name="writer"></param>
            /// <returns></returns>
            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("Nome", m_Nome);
                writer.Write("Cognome", m_Cognome);
                // writer.Write("RNome1", DMD.Strings.OnlyChars(Me.m_Nome & " " & Me.m_Cognome))
                // writer.Write("RNome2", DMD.Strings.OnlyChars(Me.m_Cognome & " " & Me.m_Nome))
                writer.Write("StatoCivile", m_StatoCivile);
                writer.Write("Categoria", m_Disabilita);
                writer.Write("IDImpiego", DBUtils.GetID(m_ImpiegoPrincipale));
                {
                    var withBlock = m_ImpiegoPrincipale;
                    writer.Write("IMP_IDAzienda", withBlock.IDAzienda);
                    writer.Write("IMP_NomeAzienda", withBlock.NomeAzienda);
                    writer.Write("IMP_IDEntePagante", withBlock.IDEntePagante);
                    writer.Write("IMP_NomeEntePagante", withBlock.NomeEntePagante);
                    writer.Write("IMP_Posizione", withBlock.Posizione);
                    writer.Write("IMP_DataAssunzione", withBlock.DataAssunzione);
                    writer.Write("IMP_DataLicenziamento", withBlock.DataLicenziamento);
                    writer.Write("IMP_StipendioNetto", withBlock.StipendioNetto);
                    writer.Write("IMP_StipendioLordo", withBlock.StipendioLordo);
                    writer.Write("IMP_TipoContratto", withBlock.TipoContratto);
                    writer.Write("IMP_TipoRapporto", withBlock.TipoRapporto);
                    writer.Write("IMP_TFR", withBlock.TFR);
                    writer.Write("IMP_MensilitaPercepite", withBlock.MensilitaPercepite);
                    writer.Write("IMP_PercTFRAzienda", withBlock.PercTFRAzienda);
                    writer.Write("IMP_NomeFPC", withBlock.NomeFPC);
                    writer.Write("IMP_Flags", withBlock.Flags);
                    writer.Write("IMP_IDSede", withBlock.IDSede);
                    writer.Write("IMP_NomeSede", withBlock.NomeSede);
                }

                return base.SaveToRecordset(writer);
            }

            /// <summary>
            /// Prepara lo schema
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaFields(DBTable table)
            {
                base.PrepareDBSchemaFields(table);

                var c = table.Fields.Ensure("Nome", typeof(string), 255);
                c = table.Fields.Ensure("Cognome", typeof(string), 255);
                c = table.Fields.Ensure("StatoCivile", typeof(string), 255);
                c = table.Fields.Ensure("Categoria", typeof(string), 255);
                c = table.Fields.Ensure("IDImpiego", typeof(int), 1);
                
                {
                    c = table.Fields.Ensure("IMP_IDAzienda", typeof(int), 1);
                    c = table.Fields.Ensure("IMP_NomeAzienda", typeof(string), 255);
                    c = table.Fields.Ensure("IMP_IDEntePagante", typeof(int), 1);
                    c = table.Fields.Ensure("IMP_NomeEntePagante", typeof(string), 255);
                    c = table.Fields.Ensure("IMP_Posizione", typeof(string), 255);
                    c = table.Fields.Ensure("IMP_DataAssunzione", typeof(DateTime), 1);
                    c = table.Fields.Ensure("IMP_DataLicenziamento", typeof(DateTime), 1);
                    c = table.Fields.Ensure("IMP_StipendioNetto", typeof(decimal), 1);
                    c = table.Fields.Ensure("IMP_StipendioLordo", typeof(decimal), 1);
                    c = table.Fields.Ensure("IMP_TipoContratto", typeof(string), 255);
                    c = table.Fields.Ensure("IMP_TipoRapporto", typeof(string), 255);
                    c = table.Fields.Ensure("IMP_TFR", typeof(decimal), 1);
                    c = table.Fields.Ensure("IMP_MensilitaPercepite", typeof(int), 1);
                    c = table.Fields.Ensure("IMP_PercTFRAzienda", typeof(double), 1);
                    c = table.Fields.Ensure("IMP_NomeFPC", typeof(string), 255);
                    c = table.Fields.Ensure("IMP_Flags", typeof(int), 1);
                    c = table.Fields.Ensure("IMP_IDSede", typeof(int), 1);
                    c = table.Fields.Ensure("IMP_NomeSede", typeof(string), 255);
                }


            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.Ensure("idxNome", new string[] { "Nome" } , DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxCognome", new string[] { "Cognome" }, DBFieldConstraintFlags.None);

                c = table.Constraints.Ensure("idxTipoAzienda", new string[] { "Tipologia", "TipoAzienda", "Categoria", "TipoRapporto" }, DBFieldConstraintFlags.None);
                
                c = table.Constraints.Ensure("idxImpiegoID", new string[] { "IDImpiego", "IMP_IDAzienda", "IMP_IDEntePagante", "IMP_IDSede" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxImpiegoNomi", new string[] { "IMP_NomeAzienda", "IMP_NomeEntePagante", "IMP_NomeSede", "IMP_Posizione" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxImpiegoDate", new string[] { "IMP_DataAssunzione", "IMP_DataLicenziamento", "IMP_MensilitaPercepite", "IMP_Flags" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxImpiegoValuta", new string[] { "IMP_StipendioLordo", "IMP_StipendioNetto", "IMP_TFR", "IMP_PercTFRAzienda" }, DBFieldConstraintFlags.None);
                c = table.Constraints.Ensure("idxImpiegoContratti", new string[] { "IMP_TipoRapporto", "IMP_TipoContratto", "IMP_NomeFPC"  }, DBFieldConstraintFlags.None);
                 
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Nome", m_Nome);
                writer.WriteAttribute("Cognome", m_Cognome);
                writer.WriteAttribute("StatoCivile", m_StatoCivile);
                writer.WriteAttribute("Disabilita", m_Disabilita);
                base.XMLSerialize(writer);
                writer.WriteTag("ImpiegoPrincipale", m_ImpiegoPrincipale);
            }

            /// <summary>
            /// Deserializzazione xml
            /// </summary>
            /// <param name="fieldName"></param>
            /// <param name="fieldValue"></param>
            protected override void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "Nome":
                        {
                            m_Nome = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Cognome":
                        {
                            m_Cognome = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "StatoCivile":
                        {
                            m_StatoCivile = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Disabilita":
                        {
                            m_Disabilita = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "ImpiegoPrincipale":
                        {
                            if (fieldValue is CImpiegato)
                            {
                                m_ImpiegoPrincipale = (CImpiegato)fieldValue;
                            }
                            else
                            {
                                m_ImpiegoPrincipale = new CImpiegato();
                            }

                            m_ImpiegoPrincipale.SetPersona(this);
                            break;
                        }

                    default:
                        {
                            base.SetFieldInternal(fieldName, fieldValue);
                            break;
                        }
                }
            }

            /// <summary>
            /// Unisce le due persone
            /// </summary>
            /// <param name="persona"></param>
            /// <param name="autoDelete"></param>
            public override void MergeWith(CPersona persona, bool autoDelete = true)
            {
                {
                    var withBlock = (CPersonaFisica)persona;
                    if (string.IsNullOrEmpty(m_Nome))
                        m_Nome = withBlock.Nome;
                    if (string.IsNullOrEmpty(m_Cognome))
                        m_Cognome = withBlock.Cognome;
                    if (string.IsNullOrEmpty(m_StatoCivile))
                        m_StatoCivile = withBlock.StatoCivile;
                    m_Disabilita = DMD.Strings.Combine(m_Disabilita, withBlock.Disabilita, ", ");
                    if ((ImpiegoPrincipale.IsEmpty() || ImpiegoPrincipale.DataAssunzione == withBlock.ImpiegoPrincipale.DataAssunzione && (ImpiegoPrincipale.TipoRapporto ?? "") == (withBlock.ImpiegoPrincipale.TipoRapporto ?? "")) == true)
                    {
                        ImpiegoPrincipale.MergeWith(withBlock.ImpiegoPrincipale);
                    }
                    else
                    {
                        m_Impieghi = null;
                    }
                }

                base.MergeWith(persona, autoDelete);
            }

            /// <summary>
            /// Restituisce le parole indicizzate
            /// </summary>
            /// <returns></returns>
            protected override string[] GetIndexedWords()
            {
                var arr = new ArrayList();
                string[] a;

                // Dim a As String() = MyBase.GetIndexedWords
                // If (DMD.Arrays.Len(a) > 0) Then arr.AddRange(a)
                string str = DMD.Strings.Replace(Cognome, " ", "");
                str = DMD.Strings.Replace(str, "'", "");
                if (!string.IsNullOrEmpty(str))
                    arr.Add(str);
                str = DMD.Strings.Replace(Nome, " ", "");
                str = DMD.Strings.Replace(str, "'", "");
                if (!string.IsNullOrEmpty(str))
                    arr.Add(str);
                str = Cognome;
                if (DMD.Strings.InStr(str, "'") > 0)
                {
                    a = DMD.Strings.Split(str, "'");
                    arr.AddRange(a);
                }

                str = Nome;
                if (DMD.Strings.InStr(str, "'") > 0)
                {
                    a = DMD.Strings.Split(str, "'");
                    arr.AddRange(a);
                }

                return (string[])arr.ToArray(typeof(string));
            }

            /// <summary>
            /// Restituisce la combinazione Cognome + " " + Nome
            /// </summary>
            public string CognomeENome
            {
                get
                {
                    return DMD.Strings.Combine(Cognome, Nome, " ");
                }
            }

            /// <summary>
            /// Restituisce la combinazione Nome + " " + Cognome
            /// </summary>
            public string NomeECognome
            {
                get
                {
                    return DMD.Strings.Combine(Nome, Cognome, " ");
                }
            }

            /// <summary>
            /// Resetta l'ID
            /// </summary>
            protected override void ResetID()
            {
                base.ResetID();
                DBUtils.ResetID(ImpiegoPrincipale);
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return base.GetHashCode();
            }
        }
    }
}