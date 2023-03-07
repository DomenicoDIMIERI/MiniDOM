using System;
using DMD;
using minidom.internals;

namespace minidom
{
    namespace repositories
    {

        /// <summary>
        /// Repository di oggetti di topo <see cref="CCollaboratore"/>
        /// </summary>
        [Serializable]
        public sealed partial class CCollaboratoriClass 
            : CModulesClass<CCollaboratore>
        {
            [NonSerialized]
            private ClientiXCollaboratoriClass m_ClientiXCollaboratore = null;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CCollaboratoriClass() 
                : base("modCQSPDCollaboratori", typeof(Finanziaria.CCollaboratoriCursor), -1)
            {
            }

            public string FormatStatoProduttore(Finanziaria.StatoProduttore value)
            {
                switch (value)
                {
                    case Finanziaria.StatoProduttore.STATO_ATTIVO:
                        {
                            return "Attivo";
                        }

                    case Finanziaria.StatoProduttore.STATO_DISABILITATO:
                        {
                            return "Disabilitato";
                        }

                    case Finanziaria.StatoProduttore.STATO_ELIMINATO:
                        {
                            return "Eliminato";
                        }

                    case Finanziaria.StatoProduttore.STATO_INATTIVAZIONE:
                        {
                            return "In Attivazione";
                        }

                    case Finanziaria.StatoProduttore.STATO_SOSPESO:
                        {
                            return "Sospeso";
                        }

                    default:
                        {
                            // Case Finanziaria.StatoProduttore.STATO_INVALID : Return "Non Valido"
                            return "Sconosciuto";
                        }
                }
            }

            public Finanziaria.StatoProduttore ParseStatoProduttore(string value)
            {
                switch (Strings.LCase(Strings.Trim(value)) ?? "")
                {
                    case "attivo":
                        {
                            return Finanziaria.StatoProduttore.STATO_ATTIVO;
                        }

                    case "disabilitato":
                        {
                            return Finanziaria.StatoProduttore.STATO_DISABILITATO;
                        }

                    case "eliminato":
                        {
                            return Finanziaria.StatoProduttore.STATO_ELIMINATO;
                        }

                    case "in attivazione":
                        {
                            return Finanziaria.StatoProduttore.STATO_INATTIVAZIONE;
                        }

                    case "sospeso":
                        {
                            return Finanziaria.StatoProduttore.STATO_SOSPESO;
                        }

                    default:
                        {
                            // Case "non valido" : Return StatoProduttore.STATO_INVALID
                            return 0;
                        }
                }
            }

            // Public  Function GetItemByCodiceFiscale(ByVal valore As String) As CCollaboratore
            // Dim cursor As New CCollaboratoriCursor
            // Dim ret As CCollaboratore
            // cursor.PageSize = 1
            // cursor.IgnoreRights = True
            // cursor.CodiceFiscale.Value = Formats.ParseCodiceFiscale(valore)
            // ret = cursor.Item
            // cursor.Reset()
            // Return ret
            // End Function

            // Public  Function GetItemByPartitaIVA(ByVal valore As String) As CCollaboratore
            // Dim cursor As New CCollaboratoriCursor
            // Dim ret As CCollaboratore
            // cursor.PageSize = 1
            // cursor.IgnoreRights = True
            // 'cursor.PartitaIVA.Value = Formats.ParsePartitaIVA(valore)
            // ret = cursor.Item
            // cursor.Reset()
            // Return ret
            // End Function

            // Public  Function GetItemByEMail(ByVal valore As String) As CCollaboratore
            // Dim cursor As New CCollaboratoriCursor
            // Dim ret As CCollaboratore
            // cursor.PageSize = 1
            // cursor.IgnoreRights = True
            // cursor.eMail.Value = valore
            // ret = cursor.Item
            // cursor.Reset()
            // Return ret
            // End Function

            // Public Function GetItemByUIF(ByVal valore As String) As CCollaboratore
            // Dim cursor As New CCollaboratoriCursor
            // Dim ret As CCollaboratore
            // For   ret In Me.CachedItems
            // If ret.NumeroIsci Then
            // Next
            // cursor.PageSize = 1
            // cursor.IgnoreRights = True
            // cursor.NumeroIscrizioneUIF.Value = valore
            // cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            // ret = cursor.Item
            // cursor.Reset()
            // Return ret
            // End Function

            public Finanziaria.CCollaboratore GetItemByRUI(string valore)
            {
                valore = Strings.Trim(valore);
                if (string.IsNullOrEmpty(valore))
                    return null;
                foreach (Finanziaria.CCollaboratore item in LoadAll())
                {
                    if (item.Stato == ObjectStatus.OBJECT_VALID 
                        && DMD.Strings.Compare(item.NumeroIscrizioneRUI, valore, true) == 0)
                        return item;
                }

                return null;
            }

            public Finanziaria.CCollaboratore GetItemByISVAP(string valore)
            {
                valore = Strings.Trim(valore);
                if (string.IsNullOrEmpty(valore))
                    return null;
                foreach (Finanziaria.CCollaboratore item in LoadAll())
                {
                    if (DMD.Strings.Compare(item.NumeroIscrizioneISVAP, valore, true) == 0)
                        return item;
                }

                return null;
            }

            public Finanziaria.CCollaboratore GetItemByPersona(int personID)
            {
                if (personID == 0)
                    return null;
                foreach (Finanziaria.CCollaboratore item in LoadAll())
                {
                    if (item.Stato == ObjectStatus.OBJECT_VALID && item.IDPersona == personID)
                        return item;
                }

                return null;
            }

            public Finanziaria.CCollaboratore GetItemByUser(int userId)
            {
                if (userId == 0)
                    return null;
                return GetItemByUser(Sistema.Users.GetItemById(userId));
            }

            public Finanziaria.CCollaboratore GetItemByUser(Sistema.CUser user)
            {
                if (user is null)
                    throw new ArgumentNullException("user");
                foreach (Finanziaria.CCollaboratore item in LoadAll())
                {
                    if (item.Stato == ObjectStatus.OBJECT_VALID && item.UserID == DBUtils.GetID(user))
                    {
                        return item;
                    }
                }

                return null;
            }

            public Finanziaria.CCollaboratore GetItemByName(string value)
            {
                value = DMD.Strings.Trim(value);
                if (string.IsNullOrEmpty(value))
                    return null;
                foreach (Finanziaria.CCollaboratore item in LoadAll())
                {
                    if (item.Stato == ObjectStatus.OBJECT_VALID 
                        && 
                        DMD.Strings.Compare(item.NomePersona, value, true) == 0
                        )
                        return item;
                }

                return null;
            }

            // Public Function CalcolaPremio(ByVal value As Decimal) As Decimal
            // Dim dbRis As System.Data.IDataReader
            // Dim dbSQL As String
            // Dim ret, somma, termine, finoA As Decimal
            // Dim perc As Double
            // dbSQL = "SELECT * FROM [tbl_CollaboratoriPremi] ORDER BY [FinoA] ASC"
            // dbRis = Finanziaria.Database.ExecuteReader(dbSQL)
            // somma = value
            // ret = 0
            // While dbRis.Read And (somma > 0)
            // finoA = Formats.ToValuta(dbRis("FinoA"))
            // perc = Formats.ToDouble(dbRis("Percentuale"))
            // If "" & finoA = "" Then finoA = 0
            // If "" & perc = "" Then perc = 0
            // termine = IIf(somma <= finoA, somma, finoA)
            // somma = somma - termine
            // termine = termine * perc / 100
            // ret = ret + termine
            // End While
            // dbRis.Dispose()
            // dbRis = Nothing
            // Return ret
            // End Function


            public ClientiXCollaboratoriClass ClientiXCollaboratori
            {
                get
                {
                    if (m_ClientiXCollaboratore is null)
                        m_ClientiXCollaboratore = new ClientiXCollaboratoriClass();
                    return m_ClientiXCollaboratore;
                }
            }

            internal void InvalidateTrattative()
            {
                foreach (CacheItem c in CachedItems)
                    c.Item.InvalidateTrattative();
            }
        }
    }

    public partial class Finanziaria
    {
        private static CCollaboratoriClass m_Collaboratori = null;

        public static CCollaboratoriClass Collaboratori
        {
            get
            {
                if (m_Collaboratori is null)
                    m_Collaboratori = new CCollaboratoriClass();
                return m_Collaboratori;
            }
        }
    }
}