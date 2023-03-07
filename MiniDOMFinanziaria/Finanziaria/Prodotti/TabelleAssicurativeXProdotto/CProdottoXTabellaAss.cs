using System;
using System.Collections;
using DMD;
using DMD.XML;
using minidom;
using static minidom.Sistema;


namespace minidom
{
    public partial class Finanziaria
    {
        /// <summary>
    /// Relazione Prodotto - Tripla di Tabella Assicurativ (vita, impiego, credito)
    /// </summary>
    /// <remarks></remarks>
        public class CProdottoXTabellaAss : Databases.DBObject, IComparable<CProdottoXTabellaAss>, ICloneable
        {
            private string m_Descrizione; // Descrizione
            private int m_ProdottoID; // ID del prodotto associato
            private CCQSPDProdotto m_Prodotto; // Oggetto Prodotto Associato
            private int m_IDRischioVita; // ID della tablla vita
            private CTabellaAssicurativa m_RischioVita; // Tablla Rischio Vita
            private int m_IDRischioImpiego; // ID della tablla impiego
            private CTabellaAssicurativa m_RischioImpiego; // Tablla Rischio Impiego
            private int m_IDRischioCredito; // ID della tabella rischio credito
            private CTabellaAssicurativa m_RischioCredito; // Tabella Rischio Credito
            private CVincoliProdottoTabellaAss m_Vincoli;    // Collezione di vincoli
            private int m_OldProdottoID;

            // Private m_Fisso() As Decimal
            // Private m_Variabile() As Decimal
            // Private m_Imposta As Decimal
            // Private m_UgualiMF As Boolean 'Se si utilizza lo stesso set di coefficienti per i maschi e per le femmine
            // Private m_ShiftMaschi As Integer 'Anni e frazioni di anni aggiunti all'età o all'anzianità di un individuo di sesso maschile
            // Private m_ShiftFemmine As Integer 'Anni e frazioni di anni aggiunti all'età o all'anzianità di un individuo di sesso femminile
            // Private m_TipoAssicurazione As Integer
            // Private m_ScattoMensile As Integer
            // Private m_MinEtaIF_M As Double
            // Private m_MaxEtaIF_M As Double
            // Private m_MinEtaIF_F As Double
            // Private m_MaxEtaIF_F As Double
            // Private m_MinEtaFF_M As Double
            // Private m_MaxEtaFF_M As Double
            // Private m_MinEtaFF_F As Double
            // Private m_MaxEtaFF_F As Double
            // Private m_Maggiorazione As Double

            public CProdottoXTabellaAss()
            {
                m_Descrizione = "";
                m_ProdottoID = 0;
                m_Prodotto = null;
                m_IDRischioVita = 0;
                m_RischioVita = null;
                m_IDRischioImpiego = 0;
                m_RischioImpiego = null;
                m_IDRischioCredito = 0;
                m_RischioCredito = null;
                m_Vincoli = null;
            }

            public override CModulesClass GetModule()
            {
                return null;
            }

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
        /// ID del prodotto associato
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public int IDProdotto
            {
                get
                {
                    return DBUtils.GetID(m_Prodotto, m_ProdottoID);
                }

                set
                {
                    int oldValue = IDProdotto;
                    if (oldValue == value)
                        return;
                    m_ProdottoID = value;
                    m_Prodotto = null;
                    DoChanged("IDProdotto", value, oldValue);
                }
            }

            /// <summary>
        /// Oggetto Prodotto Associato
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
            public CCQSPDProdotto Prodotto
            {
                get
                {
                    if (m_Prodotto is null)
                        m_Prodotto = Prodotti.GetItemById(m_ProdottoID);
                    return m_Prodotto;
                }

                set
                {
                    var oldValue = Prodotto;
                    if (oldValue == value)
                        return;
                    m_Prodotto = value;
                    m_ProdottoID = DBUtils.GetID(value);
                    DoChanged("Prodotto", value, oldValue);
                }
            }

            public int IDRischioVita
            {
                get
                {
                    return DBUtils.GetID(m_RischioVita, m_IDRischioVita);
                }

                set
                {
                    int oldValue = IDRischioVita;
                    if (oldValue == value)
                        return;
                    m_IDRischioVita = value;
                    m_RischioVita = null;
                    DoChanged("IDRischioVita", value, oldValue);
                }
            }

            public CTabellaAssicurativa RischioVita
            {
                get
                {
                    if (m_RischioVita is null)
                        m_RischioVita = TabelleAssicurative.GetItemById(m_IDRischioVita);
                    return m_RischioVita;
                }

                set
                {
                    var oldValue = RischioVita;
                    if (oldValue == value)
                        return;
                    m_RischioVita = value;
                    m_IDRischioVita = DBUtils.GetID(value);
                    DoChanged("RischioVita", value, oldValue);
                }
            }

            public int IDRischioImpiego
            {
                get
                {
                    return DBUtils.GetID(m_RischioImpiego, m_IDRischioImpiego);
                }

                set
                {
                    int oldValue = IDRischioImpiego;
                    if (oldValue == value)
                        return;
                    m_IDRischioImpiego = value;
                    m_RischioImpiego = null;
                    DoChanged("IDRischioImpiego", value, oldValue);
                }
            }

            public CTabellaAssicurativa RischioImpiego
            {
                get
                {
                    if (m_RischioImpiego is null)
                        m_RischioImpiego = TabelleAssicurative.GetItemById(m_IDRischioImpiego);
                    return m_RischioImpiego;
                }

                set
                {
                    var oldValue = RischioImpiego;
                    if (oldValue == value)
                        return;
                    m_RischioImpiego = value;
                    m_IDRischioImpiego = DBUtils.GetID(value);
                    DoChanged("RischioImpiego", value, oldValue);
                }
            }

            public int IDRischioCredito
            {
                get
                {
                    return DBUtils.GetID(m_RischioCredito, m_IDRischioCredito);
                }

                set
                {
                    int oldValue = IDRischioCredito;
                    if (oldValue == value)
                        return;
                    m_IDRischioCredito = value;
                    m_RischioCredito = null;
                    DoChanged("IDRischioCredito", value, oldValue);
                }
            }

            public CTabellaAssicurativa RischioCredito
            {
                get
                {
                    if (m_RischioCredito is null)
                        m_RischioCredito = TabelleAssicurative.GetItemById(m_IDRischioCredito);
                    return m_RischioCredito;
                }

                set
                {
                    var oldValue = RischioCredito;
                    if (oldValue == value)
                        return;
                    m_RischioCredito = value;
                    m_IDRischioCredito = DBUtils.GetID(value);
                    DoChanged("RischioCredito", value, oldValue);
                }
            }

            public CVincoliProdottoTabellaAss Vincoli
            {
                get
                {
                    if (m_Vincoli is null)
                    {
                        m_Vincoli = new CVincoliProdottoTabellaAss();
                        m_Vincoli.Initialize(this);
                    }

                    return m_Vincoli;
                }
            }





            // Public Function EntroILimiti(ByVal offerta As COffertaCQS, ByVal tanni As Double, ByRef errorCode As Integer, ByRef errorMessage As String) As Boolean
            // Dim mii, mai, mif, maf, anni As Double

            // anni = offerta.Eta ' Users.ToDouble(tanni)

            // If LCase(Left(offerta.Sesso, 1)) = "m" Then
            // mii = Me.MinEtaIF_M
            // mai = Me.MaxEtaIF_M
            // mif = Me.MinEtaFF_M
            // maf = Me.MaxEtaFF_M
            // Else
            // mii = Me.MinEtaIF_F
            // mai = Me.MaxEtaIF_F
            // mif = Me.MinEtaFF_F
            // maf = Me.MaxEtaFF_F
            // End If


            // If (mii > 0) And (anni < mii) Then
            // errorCode = -3
            // errorMessage = "Troppo giovane ad inizio finanziamento: " & anni & " - " & mii
            // Return False
            // End If
            // If (mai > 0) And (anni > mai) Then
            // errorCode = -4
            // errorMessage = "Troppo vecchio ad inizio finanziamento: " & anni & " - " & mai
            // Return False
            // End If

            // anni = anni + offerta.Durata / 12

            // If (mif > 0) And (anni < mif) Then
            // errorCode = -3
            // errorMessage = "Troppo giovane alla fine del finanziamento: " & anni & " - " & mif
            // Return False
            // End If
            // If (maf > 0) And (anni > maf) Then
            // errorCode = -4
            // errorMessage = "Troppo vecchio alla fine del finanziamento: " & anni & " - " & maf
            // Return False
            // End If
            // Return True
            // End Function

            public double CalcolaAnni(DateTime fromDate, DateTime toDate, int scattoMensile)
            {
                if (scattoMensile < 0 | scattoMensile > 12)
                    throw new ArgumentOutOfRangeException("scattoMensile");
                return Maths.Floor((DMD.DateUtils.DateDiff("m", fromDate, toDate) + scattoMensile) / 12d);
            }

            public void Calcola(COffertaCQS offerta)
            {
                if (offerta is null)
                    throw new ArgumentNullException("offerta");

                // Dim eta, anzianita As Double
                int scattoEta = 0;
                int scattoAnzianita = 0;
                double? pVita = default, pImpiego = default, pCredito = default;
                if (RischioVita is object)
                    scattoEta = RischioVita.MeseScatto;
                if (RischioImpiego is object)
                {
                    scattoAnzianita = RischioImpiego.MeseScatto;
                }
                else if (RischioCredito is object)
                {
                    scattoAnzianita = RischioCredito.MeseScatto;
                }

                offerta.Eta = 0d;
                offerta.Anzianita = 0d;
                if (offerta.DataNascita.HasValue && offerta.DataDecorrenza.HasValue)
                {
                    offerta.Eta = CalcolaAnni((DateTime)offerta.DataNascita, (DateTime)offerta.DataDecorrenza, scattoEta);
                    offerta.Anzianita = CalcolaAnni((DateTime)offerta.DataAssunzione, (DateTime)offerta.DataDecorrenza, scattoAnzianita);
                }

                offerta.PremioVita = 0m;
                offerta.PremioCredito = 0m;
                offerta.PremioImpiego = 0m;
                if (RischioVita is object)
                    pVita = RischioVita.GetCoefficiente(offerta.Sesso, (int)offerta.Eta, offerta.Durata);
                if (RischioImpiego is object)
                    pImpiego = RischioImpiego.GetCoefficiente(offerta.Sesso, (int)offerta.Anzianita, offerta.Durata);
                if (RischioCredito is object)
                    pCredito = RischioCredito.GetCoefficiente(offerta.Sesso, (int)offerta.Anzianita, offerta.Durata);
                if (pVita.HasValue)
                    offerta.PremioVita = (decimal)((double)offerta.MontanteLordo * pVita.Value / 100d);
                if (pImpiego.HasValue)
                    offerta.PremioImpiego = (decimal)((double)offerta.MontanteLordo * pImpiego.Value / 100d);
                if (pCredito.HasValue)
                    offerta.PremioCredito = (decimal)((double)offerta.MontanteLordo * pCredito.Value / 100d);
            }

            /// <summary>
        /// Controlla che la relazione sia applicazione
        /// </summary>
        /// <param name="offerta"></param>
        /// <returns></returns>
        /// <remarks></remarks>
            public bool Check(COffertaCQS offerta)
            {
                return Vincoli.Check(offerta);
            }

            public override string GetTableName()
            {
                return "tbl_FIN_ProdXTabAss";
            }

            protected override bool DropFromDatabase(Databases.CDBConnection dbConn, bool force)
            {
                Vincoli.Delete(force);
                return base.DropFromDatabase(dbConn, force);
            }

            public override bool IsChanged()
            {
                bool ret = base.IsChanged();
                if (ret == false && m_Vincoli is object)
                    ret = DBUtils.IsChanged(m_Vincoli);
                return ret;
            }

            protected override bool SaveToDatabase(Databases.CDBConnection dbConn, bool force)
            {
                bool ret = base.SaveToDatabase(dbConn, force);
                if (ret & m_Vincoli is object)
                    m_Vincoli.Save(force);
                TabelleAssicurative.UpdateRelations(this);

                // Me.UpdateProdotto()
                var p = Prodotti.GetItemById(m_OldProdottoID);
                if (p is object)
                    p.InvalidateAssicurazioni();
                p = Prodotto;
                if (p is object)
                    p.InvalidateAssicurazioni();
                return ret;
            }

            protected override Databases.CDBConnection GetConnection()
            {
                return Database;
            }

            protected override bool LoadFromRecordset(DBReader reader)
            {
                this.m_ProdottoID = reader.Read("Prodotto", this.m_ProdottoID);
                this.m_OldProdottoID = this.m_ProdottoID;
                this.m_Descrizione = reader.Read("Descrizione", this.m_Descrizione);
                this.m_IDRischioVita = reader.Read("RischioVita", this.m_IDRischioVita);
                this.m_IDRischioImpiego = reader.Read("RischioImpiego", this.m_IDRischioImpiego);
                this.m_IDRischioCredito = reader.Read("RischioCredito", this.m_IDRischioCredito);
                return base.LoadFromRecordset(reader);
            }

            protected override bool SaveToRecordset(DBWriter writer)
            {
                writer.Write("Prodotto", this.IDProdotto);
                writer.Write("Descrizione", this.m_Descrizione);
                writer.Write("RischioVita", this.IDRischioVita);
                writer.Write("RischioImpiego", this.IDRischioImpiego);
                writer.Write("RischioCredito", this.IDRischioCredito);
                return base.SaveToRecordset(writer);
            }

            public override string ToString()
            {
                return m_Descrizione;
            }

            protected override void XMLSerialize(XMLWriter writer)
            {
                writer.WriteAttribute("Descrizione", m_Descrizione);
                writer.WriteAttribute("Prodotto", IDProdotto);
                writer.WriteAttribute("RischioVita", IDRischioVita);
                writer.WriteAttribute("RischioImpiego", IDRischioImpiego);
                writer.WriteAttribute("RischioCredito", IDRischioCredito);
                writer.WriteAttribute("OldProdottoID", m_OldProdottoID);
                base.XMLSerialize(writer);
                writer.WriteTag("Vincoli", Vincoli.ToArray());
            }

            protected override void SetFieldInternal(string fieldName, object fieldValue)
            {
                switch (fieldName ?? "")
                {
                    case "Descrizione":
                        {
                            m_Descrizione = DMD.XML.Utils.Serializer.DeserializeString(fieldValue);
                            break;
                        }

                    case "Prodotto":
                        {
                            m_ProdottoID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "OldProdottoID":
                        {
                            m_OldProdottoID = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "RischioVita":
                        {
                            m_IDRischioVita = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "RischioImpiego":
                        {
                            m_IDRischioImpiego = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "RischioCredito":
                        {
                            m_IDRischioCredito = (int)DMD.XML.Utils.Serializer.DeserializeInteger(fieldValue);
                            break;
                        }

                    case "Vincoli":
                        {
                            Vincoli.Clear();
                            if (DMD.Arrays.IsArray(fieldValue))
                            {
                                Vincoli.AddRange((IEnumerable)fieldValue);
                            }
                            else if (fieldValue is CProdTabAssConstraint)
                            {
                                Vincoli.Add((CProdTabAssConstraint)fieldValue);
                            }

                            break;
                        }

                    default:
                        {
                            base.SetFieldInternal(fieldName, fieldValue);
                            break;
                        }
                }
            }




            // Private Sub UpdateProdotto()
            // SyncLock Me
            // Dim p As CCQSPDProdotto
            // Dim tmp As CProdottoXTabellaAss
            // If (Me.m_OldProdottoID <> 0 AndAlso Me.IDProdotto <> Me.m_OldProdottoID) Then
            // p = Finanziaria.Prodotti.GetItemById(Me.m_OldProdottoID)
            // If (p IsNot Nothing) Then
            // 'tmp = p.TabelleAssicurativeRelations.GetItemById(GetID(Me))
            // 'If (tmp IsNot Nothing) Then p.TabelleAssicurativeRelations.Remove(tmp)

            // End If
            // End If
            // Me.m_OldProdottoID = Me.IDProdotto
            // p = Me.Prodotto
            // If (p IsNot Nothing) Then
            // tmp = p.TabelleAssicurativeRelations.GetItemById(GetID(Me))
            // If (tmp IsNot Nothing) Then p.TabelleAssicurativeRelations.Remove(tmp)
            // If (Me.Stato = ObjectStatus.OBJECT_VALID) Then
            // p.TabelleAssicurativeRelations.Add(Me)
            // p.TabelleAssicurativeRelations.Sort()
            // End If
            // End If
            // End SyncLock
            // End Sub

            public int CompareTo(CProdottoXTabellaAss other)
            {
                return DMD.Strings.Compare(m_Descrizione, other.m_Descrizione, true);
            }

            protected internal virtual void SetProdotto(CCQSPDProdotto value)
            {
                m_Prodotto = value;
                m_ProdottoID = DBUtils.GetID(value);
            }

            public object Clone()
            {
                return MemberwiseClone();
            }
        }
    }
}