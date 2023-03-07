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
        /// Rappresenta una spedizione effettuata tramite corriere
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class PassaggioSpedizione 
            : DMD.XML.DMDBaseXMLObject, IComparable, IComparable<PassaggioSpedizione> //Databases.DBObjectBase
        {
            private DateTime m_Data;
            private StatoSpedizione m_StatoSpedizione;
            private StatoConsegna m_StatoConsegna;
            private string m_Note;
            private int m_IDOperatore;
            [NonSerialized] private CUser m_Operatore;
            private string m_NomeOperatore;

            /// <summary>
            /// Costruttore
            /// </summary>
            public PassaggioSpedizione()
            {
                m_Data = DMD.DateUtils.Now();
                m_StatoSpedizione = StatoSpedizione.InPreparazione;
                m_StatoConsegna = StatoConsegna.NonConsegnata;
                m_Note = "";
                m_IDOperatore = 0;
                m_Operatore = null;
                m_NomeOperatore = "";
            }

            /// <summary>
            /// Data del passaggio
            /// </summary>
            public DateTime Data
            {
                get
                {
                    return m_Data;
                }

                set
                {
                    var oldValue = m_Data;
                    if (DMD.DateUtils.Compare(value, oldValue) == 0)
                        return;
                    m_Data = value;
                    DoChanged("Data", value, oldValue);
                }
            }

            /// <summary>
            /// Stato della spedizione dopo il passaggio
            /// </summary>
            public StatoSpedizione StatoSpedizione
            {
                get
                {
                    return m_StatoSpedizione;
                }

                set
                {
                    var oldValue = m_StatoSpedizione;
                    if (oldValue == value)
                        return;
                    m_StatoSpedizione = value;
                    DoChanged("StatoSpedizione", value, oldValue);
                }
            }

            /// <summary>
            /// Stato della consegna dopo il passaggio
            /// </summary>
            public StatoConsegna StatoConsegna
            {
                get
                {
                    return m_StatoConsegna;
                }

                set
                {
                    var oldValue = m_StatoConsegna;
                    if (oldValue == value)
                        return;
                    m_StatoConsegna = value;
                    DoChanged("StatoConsegna", value, oldValue);
                }
            }

            /// <summary>
            /// Note del passaggio
            /// </summary>
            public string Note
            {
                get
                {
                    return m_Note;
                }

                set
                {
                    string oldValue = m_Note;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Note = value;
                    DoChanged("Note", value, oldValue);
                }
            }

            /// <summary>
            /// ID dell'operatore che ha effettuato il passaggio
            /// </summary>
            public int IDOperatore
            {
                get
                {
                    return DBUtils.GetID(m_Operatore, m_IDOperatore);
                }

                set
                {
                    int oldValue = IDOperatore;
                    if (oldValue == value)
                        return;
                    m_IDOperatore = value;
                    m_Operatore = null;
                    DoChanged("IDOperatore", value, oldValue);
                }
            }

            /// <summary>
            /// Operatore che ha effettuato il passaggio
            /// </summary>
            public CUser Operatore
            {
                get
                {
                    if (m_Operatore is null)
                        m_Operatore = Sistema.Users.GetItemById(m_IDOperatore);
                    return m_Operatore;
                }

                set
                {
                    var oldValue = Operatore;
                    if (ReferenceEquals(oldValue, value))
                        return;
                    m_Operatore = value;
                    m_IDOperatore = DBUtils.GetID(value, 0);
                    m_NomeOperatore = (value is object)? value.Nominativo : "";
                    DoChanged("Operatore", value, oldValue);
                }
            }

            /// <summary>
            /// Nome dell'operatore che effettuato il passaggio
            /// </summary>
            public string NomeOperatore
            {
                get
                {
                    return m_NomeOperatore;
                }

                set
                {
                    value = DMD.Strings.Trim(value);
                    string oldValue = m_NomeOperatore;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeOperatore = value;
                    DoChanged("NomeOperatore", value, oldValue);
                }
            }
             
            

            /// <summary>
            /// Compara due oggetti per data in ordine crescente
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public int CompareTo(PassaggioSpedizione obj)
            {
                return DMD.DateUtils.Compare(m_Data, obj.m_Data);
            }

            int IComparable.CompareTo(object obj)
            {
                return CompareTo((PassaggioSpedizione)obj);
            }

            /// <summary>
            /// Serializzazione xml
            /// </summary>
            /// <param name="writer"></param>
            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Data", m_Data);
                writer.WriteAttribute("StatoSpedizione", (int?)m_StatoSpedizione);
                writer.WriteAttribute("StatoConsegna", (int?)m_StatoConsegna);
                writer.WriteAttribute("IDOperatore", IDOperatore);
                writer.WriteAttribute("NomeOperatore", m_NomeOperatore);
                base.XMLSerialize(writer);
                writer.WriteTag("Note", m_Note);
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
                    case "Data":
                        {
                            m_Data = (DateTime)DMD.XML.Utils.Serializer.DeserializeDate(fieldValue);
                            break;
                        }

                    case "StatoSpedizione":
                        {
                            m_StatoSpedizione = (StatoSpedizione)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "StatoConsegna":
                        {
                            m_StatoConsegna = (StatoConsegna)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Note":
                        {
                            m_Note = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "NomeOperatore":
                        {
                            m_NomeOperatore = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
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
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return DMD.Strings.ConcatArray(this.m_Data);
            }

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.m_Data);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(DMDBaseXMLObject obj)
            {
                return (obj is PassaggioSpedizione) && this.Equals((PassaggioSpedizione)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(PassaggioSpedizione obj)
            {
                return base.Equals(obj)
                    && DMD.DateUtils.EQ(this.m_Data, obj.m_Data)
                    && DMD.RunTime.EQ(this.m_StatoSpedizione, obj.m_StatoSpedizione)
                    && DMD.RunTime.EQ(this.m_StatoConsegna, obj.m_StatoConsegna)
                    && DMD.Strings.EQ(this.m_Note, obj.m_Note)
                    && DMD.Integers.EQ(this.m_IDOperatore, obj.m_IDOperatore)
                    && DMD.Strings.EQ(this.m_NomeOperatore, obj.m_NomeOperatore)
                    ;
            }
        }
    }
}