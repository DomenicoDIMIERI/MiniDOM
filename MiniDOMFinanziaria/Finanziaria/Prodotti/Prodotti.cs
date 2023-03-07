using System;
using System.Collections;
using DMD;
using minidom.internals;

namespace minidom
{
    namespace internals
    {


        /// <summary>
    /// Gestione dei prodotti finanziari
    /// </summary>
    /// <remarks></remarks>
        public sealed class CProdottiClass : CModulesClass<Finanziaria.CCQSPDProdotto>
        {
            internal CProdottiClass() : base("modAnaProducts", typeof(Finanziaria.CProdottiCursor), -1)
            {
            }


            /// <summary>
        /// Restituisce il prodotto in base al nome. La ricerca avviene tra i soli prodotti attivi e visibili
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <remarks></remarks>
            public Finanziaria.CCQSPDProdotto GetItemByName(string value)
            {
                value = Strings.Trim(value);
                if (string.IsNullOrEmpty(value))
                    return null;
                foreach (Finanziaria.CCQSPDProdotto ret in LoadAll())
                {
                    if (DMD.Strings.Compare(ret.Nome, value, true) == 0)
                        return ret;
                }

                return null;
            }

            /// <summary>
        /// Restituisce il prodotto in base al nome. La ricerca avviene tra i soli prodotti attivi e visibili
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <remarks></remarks>
            public Finanziaria.CCQSPDProdotto GetItemByName(Finanziaria.CCQSPDCessionarioClass cessionario, string value)
            {
                if (cessionario is null)
                    throw new ArgumentNullException("cessionario");
                return GetItemByName(DBUtils.GetID(cessionario), value);
            }

            /// <summary>
        /// Restituisce il prodotto in base al nome. La ricerca avviene tra i soli prodotti attivi e visibili
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <remarks></remarks>
            public Finanziaria.CCQSPDProdotto GetItemByName(int idCessionario, string value)
            {
                value = Strings.Trim(value);
                if (string.IsNullOrEmpty(value))
                    return null;
                foreach (Finanziaria.CCQSPDProdotto ret in LoadAll())
                {
                    if (ret.CessionarioID == idCessionario && DMD.Strings.Compare(ret.Nome, value, true) == 0)
                        return ret;
                }

                return null;
            }

            // Public  Function GetItemByOldId(ByVal id As Integer) As CCQSPDProdotto
            // Dim dbRis As System.Data.IDataReader = Finanziaria.Database.ExecuteReader("SELECT * FROM tbl_Prodotti WHERE idProdotto=" & id)
            // Dim ret As CCQSPDProdotto = Nothing
            // If dbRis.Read Then
            // ret = New CCQSPDProdotto
            // Finanziaria.Database.Load(ret, dbRis)
            // End If
            // dbRis.Dispose()
            // Return ret
            // End Function

            // ''' <summary>
            // ''' Restituisce la collezione dei documenti necessari per effettuare il passaggio di stato indicato
            // ''' </summary>
            // ''' <param name="prodotto"></param>
            // ''' <param name="daStato"></param>
            // ''' <param name="aStato"></param>
            // ''' <returns></returns>
            // ''' <remarks></remarks>
            // Public Function GetDocumentiPerPassaggioStato(ByVal prodotto As CCQSPDProdotto, ByVal daStato As StatoPraticaEnum, ByVal aStato As StatoPraticaEnum) As CCollection(Of CDocumentoXProdotto)
            // Dim ret As New CCollection(Of CDocumentoXProdotto)
            // Dim cursor As New CDocumentiXProdottoCursor
            // cursor.IDProdotto.Value = GetID(prodotto)
            // cursor.StatoIniziale.Value = daStato
            // cursor.StatoFinale.Value = aStato
            // cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            // While Not cursor.EOF
            // ret.Add(cursor.Item)
            // cursor.MoveNext()
            // End While
            // cursor.Reset()
            // Return ret
            // End Function

            // ''' <summary>
            // ''' Restituisce la collezione dei documenti necessari per caricare il prodotto
            // ''' </summary>
            // ''' <param name="prodotto"></param>
            // ''' <param name="StatoIniziale"></param>
            // ''' <returns></returns>
            // ''' <remarks></remarks>
            // Public Function GetDocumentiPerCaricamento(ByVal prodotto As CCQSPDProdotto, Optional ByVal StatoIniziale As StatoPraticaEnum = StatoPraticaEnum.STATO_CONTATTO) As CCollection(Of CDocumentoXProdotto)
            // Dim ret As New CCollection(Of CDocumentoXProdotto)
            // Dim cursor As New CDocumentiXProdottoCursor
            // cursor.IDProdotto.Value = GetID(prodotto)
            // cursor.StatoIniziale.Value = StatoIniziale
            // cursor.Stato.Value = ObjectStatus.OBJECT_VALID
            // While Not cursor.EOF
            // ret.Add(cursor.Item)
            // cursor.MoveNext()
            // End While
            // cursor.Reset()
            // Return ret
            // End Function

            public CCollection<Finanziaria.CDocumentoXGruppoProdotti> GetDocumentiDaCaricare(int idp)
            {
                return GetDocumentiDaCaricare(Finanziaria.Prodotti.GetItemById(idp));
            }

            public CCollection<Finanziaria.CDocumentoXGruppoProdotti> GetDocumentiDaCaricare(Finanziaria.CCQSPDProdotto p)
            {
                var ret = new CCollection<Finanziaria.CDocumentoXGruppoProdotti>();
                if (p is object)
                {
                    var gp = p.GruppoProdotti;
                    if (gp is object)
                        ret.AddRange(gp.Documenti);
                }

                return ret;
            }

            public CCollection<Finanziaria.CTipoContratto> GetTipiContrattoDisponibili(Anagrafica.CPersonaFisica persona)
            {
                if (persona is null)
                    throw new ArgumentNullException("persona");
                var ret = new CCollection<Finanziaria.CTipoContratto>();
                string r = "";
                var items = new Hashtable();
                if (persona.ImpiegoPrincipale is object)
                    r = persona.ImpiegoPrincipale.TipoRapporto;
                foreach (Finanziaria.CCQSPDProdotto p in Finanziaria.Prodotti.LoadAll())
                {
                    if ((p.IdTipoRapporto ?? "") == (r ?? ""))
                    {
                        if (!string.IsNullOrEmpty(p.IdTipoContratto) && !items.ContainsKey(p.IdTipoContratto))
                        {
                            var c = Finanziaria.TipiContratto.GetItemByIdTipoContratto(p.IdTipoContratto);
                            if (c is object)
                                items.Add(p.IdTipoContratto, c);
                        }
                    }
                }

                foreach (string k in items.Keys)
                    ret.Add((Finanziaria.CTipoContratto)items[k]);
                ret.Sort();
                return ret;
            }
        }
    }

    public partial class Finanziaria
    {
        private static CProdottiClass m_Prodotti = null;

        public static CProdottiClass Prodotti
        {
            get
            {
                if (m_Prodotti is null)
                    m_Prodotti = new CProdottiClass();
                return m_Prodotti;
            }
        }
    }
}