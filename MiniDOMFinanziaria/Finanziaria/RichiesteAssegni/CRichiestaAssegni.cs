using System;
using DMD;
using minidom;

namespace minidom
{
    public partial class Finanziaria
    {
        public class CRichiestaAssegni : Databases.DBObject
        {
            private string m_Banca; // Nome della banca presso cui si è effettuata la richiesta
            private string m_NomeRichiedente; // Nome del richiedente
            private string m_CognomeRichiedente; // Cognome del richiedente
            private string m_IndirizzoRichiedente;
            private string m_Dipendenza;
            private DateTime m_Data;
            private CBeneficiariCollection m_AssegniRichiesti;
            private bool m_PerCassa;
            private bool m_ConAddebitoSuCC;
            private string m_NumeroContoCorrente;
            private string m_IntestazioneContoCorrente;

            public CRichiestaAssegni()
            {
            }

            public override CModulesClass GetModule()
            {
                return RichiesteAssegni.Module;
            }

            public string Banca
            {
                get
                {
                    return m_Banca;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_Banca;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Banca = value;
                    DoChanged("Banca", value, oldValue);
                }
            }

            public string NomeRichiedente
            {
                get
                {
                    return m_NomeRichiedente;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_NomeRichiedente;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NomeRichiedente = value;
                    DoChanged("NomeRichiedente", value, oldValue);
                }
            }

            public string CognomeRichiedente
            {
                get
                {
                    return m_CognomeRichiedente;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_CognomeRichiedente;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_CognomeRichiedente = value;
                    DoChanged("CognomeRichiedente", value, oldValue);
                }
            }

            public string IndirizzoRichiedente
            {
                get
                {
                    return m_IndirizzoRichiedente;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_IndirizzoRichiedente;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_IndirizzoRichiedente = value;
                    DoChanged("IndirizzoRichiedente", value, oldValue);
                }
            }

            public string Dipendenza
            {
                get
                {
                    return m_Dipendenza;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_Dipendenza;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_Dipendenza = value;
                    DoChanged("Dipendenza", value, oldValue);
                }
            }

            public DateTime Data
            {
                get
                {
                    return m_Data;
                }

                set
                {
                    var oldValue = m_Data;
                    if (oldValue == value)
                        return;
                    m_Data = value;
                    DoChanged("Data", value, oldValue);
                }
            }

            public CBeneficiariCollection AssegniRichiesti
            {
                get
                {
                    if (m_AssegniRichiesti is null)
                    {
                        m_AssegniRichiesti = new CBeneficiariCollection();
                        m_AssegniRichiesti.Load(this);
                    }

                    return m_AssegniRichiesti;
                }
            }

            public bool PerCassa
            {
                get
                {
                    return m_PerCassa;
                }

                set
                {
                    if (m_PerCassa == value)
                        return;
                    m_PerCassa = value;
                    DoChanged("PerCassa", value, !value);
                }
            }

            public bool ConAddebitoSuCC
            {
                get
                {
                    return m_ConAddebitoSuCC;
                }

                set
                {
                    if (m_ConAddebitoSuCC == value)
                        return;
                    m_ConAddebitoSuCC = value;
                    DoChanged("ConAddebitoSuCC", value, !value);
                }
            }

            public string NumeroContoCorrente
            {
                get
                {
                    return m_NumeroContoCorrente;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_NumeroContoCorrente;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_NumeroContoCorrente = value;
                    DoChanged("NumeroContoCorrente", value, oldValue);
                }
            }

            public string IntestazioneContoCorrente
            {
                get
                {
                    return m_IntestazioneContoCorrente;
                }

                set
                {
                    value = Strings.Trim(value);
                    string oldValue = m_IntestazioneContoCorrente;
                    if ((oldValue ?? "") == (value ?? ""))
                        return;
                    m_IntestazioneContoCorrente = value;
                    DoChanged("IntestazioneContoCorrente", value, oldValue);
                }
            }

            /// <summary>
        /// Necessaria per l'utilizzo con i template
        /// </summary>
        /// <param name="propName"></param>
        /// <returns></returns>
        /// <remarks></remarks>
            protected internal object GetProperty(string propName)
            {
                object ret;
                int i;
                propName = Strings.LCase(Strings.Trim(propName));
                switch (propName ?? "")
                {
                    case "txtnomerichiedente":
                        {
                            ret = NomeRichiedente;
                            break;
                        }

                    case "txtcognomerichiedente":
                        {
                            ret = CognomeRichiedente;
                            break;
                        }

                    case "txtindirizzorichiedente":
                        {
                            ret = IndirizzoRichiedente;
                            break;
                        }

                    case "txtdipendenza":
                        {
                            ret = Dipendenza;
                            break;
                        }

                    case "txtdata":
                        {
                            ret = Data;
                            break;
                        }

                    case "txtimportototale":
                        {
                            ret = ImportoTotale;
                            break;
                        }

                    case "txtnumerocc":
                        {
                            ret = NumeroContoCorrente;
                            break;
                        }

                    case "txtintestazionecc":
                        {
                            ret = IntestazioneContoCorrente;
                            break;
                        }

                    case "chkpercassa":
                        {
                            ret = PerCassa;
                            break;
                        }

                    case "chkconaddebitosucc":
                        {
                            ret = ConAddebitoSuCC;
                            break;
                        }

                    default:
                        {
                            if (Strings.Left(propName, 20) == "txtbeneficiario_nome")
                            {
                                i = DMD.Integers.Parse(Strings.Mid(propName, 21));
                                ret = AssegniRichiesti[i].Nome;
                            }
                            else if (Strings.Left(propName, 21) == "txtbeneficiario_field")
                            {
                                i = DMD.Integers.Parse(Strings.Mid(propName, 22));
                                ret = AssegniRichiesti[i].Field;
                            }
                            else if (Strings.Left(propName, 23) == "txtbeneficiario_importo")
                            {
                                i = DMD.Integers.Parse(Strings.Mid(propName, 24));
                                ret = AssegniRichiesti[i].Importo;
                            }
                            else
                            {
                                ret = null;
                            }

                            break;
                        }
                }

                return ret;
            }

            /// <summary>
        /// Restituisce la somma degli importi degli assegni
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public decimal ImportoTotale
            {
                get
                {
                    decimal sum = 0m;
                    for (int i = 0, loopTo = AssegniRichiesti.Count - 1; i <= loopTo; i++)
                    {
                        var item = AssegniRichiesti[i];
                        sum += item.Importo;
                    }

                    return sum;
                }
            }

            public override string GetTableName()
            {
                return "tbl_RichiestaAssegniCircolari";
            }

            public override bool IsChanged()
            {
                bool ret = base.IsChanged();
                if (!ret && m_AssegniRichiesti is object)
                    ret = DBUtils.IsChanged(m_AssegniRichiesti);
                return ret;
            }

            protected override bool SaveToDatabase(Databases.CDBConnection dbConn, bool force)
            {
                bool ret = base.SaveToDatabase(dbConn, force);
                if (ret & m_AssegniRichiesti is object)
                    m_AssegniRichiesti.Save(force);
                return ret;
            }

            protected override bool LoadFromRecordset(DBReader reader)
            {
                this.m_Banca = reader.Read("Banca", this.m_Banca);
                this.m_NomeRichiedente = reader.Read("NomeRichiedente", this.m_NomeRichiedente);
                this.m_CognomeRichiedente = reader.Read("CognomeRichiedente", this.m_CognomeRichiedente);
                this.m_IndirizzoRichiedente = reader.Read("IndirizzoRichiedente", this.m_IndirizzoRichiedente);
                this.m_Dipendenza = reader.Read("Dipendenza", this.m_Dipendenza);
                this.m_Data = reader.Read("Data", this.m_Data);
                this.m_PerCassa = reader.Read("PerCassa", this.m_PerCassa);
                this.m_ConAddebitoSuCC = reader.Read("ConAddebitoSuCC", this.m_ConAddebitoSuCC);
                this.m_NumeroContoCorrente = reader.Read("NumeroCCBancario", this.m_NumeroContoCorrente);
                this.m_IntestazioneContoCorrente = reader.Read("IntestazioneCC", this.m_IntestazioneContoCorrente);
                return base.LoadFromRecordset(reader);
            }

            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("Banca", m_Banca);
                writer.Write("NomeRichiedente", m_NomeRichiedente);
                writer.Write("CognomeRichiedente", m_CognomeRichiedente);
                writer.Write("IndirizzoRichiedente", m_IndirizzoRichiedente);
                writer.Write("Dipendenza", m_Dipendenza);
                writer.Write("Data", m_Data);
                writer.Write("PerCassa", m_PerCassa);
                writer.Write("ConAddebitoSuCC", m_ConAddebitoSuCC);
                writer.Write("NumeroCCBancario", m_NumeroContoCorrente);
                writer.Write("IntestazioneCC", m_IntestazioneContoCorrente);
                return base.SaveToRecordset(writer);
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Database;
            }
        }
    }
}