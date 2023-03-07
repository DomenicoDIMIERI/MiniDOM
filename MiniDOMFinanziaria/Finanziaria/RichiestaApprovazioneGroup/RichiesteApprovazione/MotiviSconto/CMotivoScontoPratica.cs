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

namespace minidom
{
    public partial class Finanziaria
    {

        /// <summary>
        /// Opzioni dei motivi sconto
        /// </summary>
        [Flags]
        public enum MotivoScontoFlags : int
        {
            /// <summary>
            /// Nessun flag
            /// </summary>
            /// <remarks></remarks>
            None = 0,

            /// <summary>
            /// Flag che indica che l'oggetto è attivo
            /// </summary>
            /// <remarks></remarks>
            Attivo = 1,

            /// <summary>
            /// Flag che indica che il tipo di sconto può essere autorizzato solo dagli utenti privilegiati
            /// </summary>
            /// <remarks></remarks>
            Privilegiato = 2,

            /// <summary>
            /// Flag che indica che il tipo di sconto causa solo una segnalazione e non richiede l'autorizzazione
            /// </summary>
            /// <remarks></remarks>
            SoloSegnalazione = 4,

            /// <summary>
            /// Se vero forza impone di specificare una descrizione per il motivo sconto
            /// </summary>
            RichiedeDescrizione = 8
        }

        /// <summary>
        /// Rappresenta un motivo di sconto per una pratica
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class CMotivoScontoPratica
            : Databases.DBObject, IComparable, ICloneable
        {
            private string m_Nome;
            private string m_Descrizione;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CMotivoScontoPratica()
            {
                m_Nome = "";
                m_Flags = (int)MotivoScontoFlags.Attivo;
                m_Descrizione = "";
            }

            /// <summary>
            /// Restituisce o imposta il nome dell'obiettivo
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
            /// Restituisce o imposta la descrizione dell'obiettivo
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public string Descrizione
            {
                get
                {
                    return m_Descrizione;
                }

                set
                {
                    string oldValue = m_Descrizione;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Descrizione = value;
                    DoChanged("Descrizione", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta dei flags
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public new MotivoScontoFlags Flags
            {
                get
                {
                    return (MotivoScontoFlags)m_Flags;
                }

                set
                {
                    var oldValue = (MotivoScontoFlags)m_Flags;
                    if (oldValue == value)
                        return;
                    m_Flags = (int)value;
                    DoChanged("Flags", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce o imposta un valore booleano che indica se il motivo è attivo
            /// </summary>
            /// <value></value>
            /// <returns></returns>
            /// <remarks></remarks>
            public bool Attivo
            {
                get
                {
                    return DMD.RunTime.TestFlag(this.Flags, MotivoScontoFlags.Attivo);
                }

                set
                {
                    if (Attivo == value)
                        return;
                    m_Flags = (int)DMD.RunTime.SetFlag(this.Flags, MotivoScontoFlags.Attivo, value);
                    DoChanged("Attivo", value, !value);
                }
            }

            /// <summary>
        /// Restituisce o imposta un valore booleano che indica se il motivo è approvabile solo da utenti appartenenti al
        /// gruppo CQSPDPrivilegiati
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public bool Privilegiato
            {
                get
                {
                    return DMD.RunTime.TestFlag(this.Flags, MotivoScontoFlags.Privilegiato);
                }

                set
                {
                    if (Privilegiato == value)
                        return;
                    m_Flags = (int)DMD.RunTime.SetFlag(this.Flags, MotivoScontoFlags.Privilegiato, value);
                    DoChanged("Privilegiato", value, !value);
                }
            }

            /// <summary>
        /// Restituisce o imposta un valore booleano che indica che il motivo di sconto causa solo una segnalazione ai supervisori e non richiede l'approvazione
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public bool SoloSegnalazione
            {
                get
                {
                    return DMD.RunTime.TestFlag(this.Flags, MotivoScontoFlags.SoloSegnalazione);
                }

                set
                {
                    if (SoloSegnalazione == value)
                        return;
                    m_Flags = (int)DMD.RunTime.SetFlag(this.Flags, MotivoScontoFlags.SoloSegnalazione, value);
                    DoChanged("SoloSegnalazione", value, !value);
                }
            }

            /// <summary>
            /// Restituisce o imposta un valore booleano che indica se è richiesta una descrizione estesa per usare questo motivo sconto
            /// </summary>
            /// <returns></returns>
            public bool RichiedeDescrizione
            {
                get
                {
                    return DMD.RunTime.TestFlag(this.Flags, MotivoScontoFlags.RichiedeDescrizione);
                }

                set
                {
                    if (RichiedeDescrizione == value)
                        return;
                    m_Flags = (int) DMD.RunTime.SetFlag(this.Flags, MotivoScontoFlags.RichiedeDescrizione, value);
                    DoChanged("RichiedeDescrizione", value, !value);
                }
            }

            
            /// <summary>
            /// Repository
            /// </summary>
            /// <returns></returns>
            public override CModulesClass GetModule()
            {
                return minidom.Finanziaria.Obiettivi;
            }

            /// <summary>
            /// Discriminator
            /// </summary>
            /// <returns></returns>
            public override string GetTableName()
            {
                return "tbl_CQSPDMotiviSconti";
            }
 

            /// <summary>
            /// Carica dal db
            /// </summary>
            /// <param name="reader"></param>
            /// <returns></returns>
            protected override bool LoadFromRecordset(DBReader reader)
            {
                this.m_Nome = reader.Read("Nome", this.m_Nome);
                this.m_Descrizione = reader.Read("Descrizione", this.m_Descrizione);
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
                writer.Write("Descrizione", m_Descrizione);
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
                c = table.Fields.Ensure("Descrizione", typeof(string), 0);    
            }

            /// <summary>
            /// Prepara i vincoli
            /// </summary>
            /// <param name="table"></param>
            protected override void PrepareDBSchemaConstraints(DBTable table)
            {
                base.PrepareDBSchemaConstraints(table);

                var c = table.Constraints.GetItemByName("idxStato");
                if (c is object)
                    c.Drop();
                c = table.Constraints.Ensure("idxNome", new string[] { "Nome", "Stato" }, DBFieldConstraintFlags.PrimaryKey);
                c = table.Constraints.Ensure("idxDescr", new string[] { "Descrizione" }, DBFieldConstraintFlags.Unique);
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Nome", m_Nome);
                base.XMLSerialize(writer);
                writer.WriteTag("Descrizione", m_Descrizione);
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
                    case "Nome": m_Nome = DMD.XML.Utils.Serializer.DeserializeString(fieldValue); break;
                    case "Descrizione": m_Descrizione = DMD.XML.Utils.Serializer.DeserializeString(fieldValue); break;
                    default: base.SetFieldInternal(fieldName, fieldValue); break;
                }
            }

            /// <summary>
            /// Compara due oggetti
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public int CompareTo(CMotivoScontoPratica obj)
            {
                return DMD.Strings.Compare(m_Nome, obj.m_Nome, true);
            }

            int IComparable.CompareTo(object obj)
            {
                return CompareTo((CMotivoScontoPratica)obj);
            }

            /// <summary>
            /// Clona l'oggetto
            /// </summary>
            /// <returns></returns>
            public CMotivoScontoPratica Clone()
            {
                return (CMotivoScontoPratica) this.MemberwiseClone();
            }

            /// <summary>
            /// Clona l'oggetto
            /// </summary>
            /// <returns></returns>
            object ICloneable.Clone()
            {
                return this.Clone();
            }


        }
    }
}