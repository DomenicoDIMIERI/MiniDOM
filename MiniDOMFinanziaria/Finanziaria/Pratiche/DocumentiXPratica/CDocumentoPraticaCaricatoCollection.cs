using System;
using System.Collections;
using DMD;
using DMD.XML;
using minidom;
using static minidom.Sistema;
using static minidom.Office;

namespace minidom
{
    public partial class Finanziaria
    {

        /// <summary>
        /// Collezione di documenti caricati per una pratica
        /// </summary>
        /// <remarks></remarks>
        public class CDocumentoPraticaCaricatoCollection : CCollection<CDocumentoPraticaCaricato>
        {
            private CPraticaCQSPD m_Pratica;

            public CDocumentoPraticaCaricatoCollection()
            {
            }

            public CDocumentoPraticaCaricatoCollection(CPraticaCQSPD pratica) : this()
            {
                Load(pratica);
            }

            public CPraticaCQSPD Pratica
            {
                get
                {
                    return m_Pratica;
                }
            }

            public CDocumentoPraticaCaricato AddDoc(CDocumentoXGruppoProdotti doc)
            {
                var ret = new CDocumentoPraticaCaricato();
                ret.Documento = doc;
                Add(ret);
                return ret;
            }

            protected override void OnInsert(int index, object value)
            {
                if (m_Pratica is object)
                {
                    ((CDocumentoPraticaCaricato)value).Pratica = m_Pratica;
                }

                base.OnInsert(index, value);
            }

            protected override void OnInsertComplete(int index, object value)
            {
                if (m_Pratica is object)
                {
                    for (int i = index, loopTo = Count - 1; i <= loopTo; i++)
                        this[i].SetProgressivo(index);
                }

                base.OnInsertComplete(index, value);
            }

            protected override void OnSet(int index, object oldValue, object newValue)
            {
                if (m_Pratica is object)
                {
                    {
                        var withBlock = (CDocumentoPraticaCaricato)newValue;
                        withBlock.SetPratica(m_Pratica);
                        withBlock.SetProgressivo(index);
                    }
                }

                base.OnSet(index, oldValue, newValue);
            }

            public int IndexOfDoc(DocumentoCaricato doc)
            {
                int docID;
                docID = DBUtils.GetID(doc);
                for (int i = 0, loopTo = Count - 1; i <= loopTo; i++)
                {
                    if (this[i].IDDocumento == docID)
                    {
                        return i;
                    }
                }

                return -1;
            }

            public bool ContainsDoc(DocumentoCaricato doc)
            {
                return IndexOfDoc(doc) >= 0;
            }

            public void RemoveDoc(DocumentoCaricato doc)
            {
                RemoveAt(IndexOfDoc(doc));
            }

            protected void Load(CPraticaCQSPD pratica)
            {
                if (pratica is null)
                    throw new ArgumentNullException("pratica");
                Clear();
                m_Pratica = pratica;
                if (DBUtils.GetID(pratica) == 0)
                    return;
                var cursor = new CDocumentoPraticaCaricatoCursor();
                try
                {
                    cursor.IDPratica.Value = DBUtils.GetID(pratica, 0);
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.IgnoreRights = true;
                    cursor.Progressivo.SortOrder = SortEnum.SORT_ASC;
                    while (!cursor.EOF())
                    {
                        Add(cursor.Item);
                        cursor.MoveNext();
                    }
                }
                catch (Exception ex)
                {
                    Sistema.Events.NotifyUnhandledException(ex);
                    throw;
                }
                finally
                {
                    if (cursor is object)
                    {
                        cursor.Dispose();
                        cursor = null;
                    }
                }
            }

            // CDocumentiPraticaPropertyPage.prototype.GetDocumentiDaCaricare = function () { if (this.m_DocumentiProdotto == null) this.m_DocumentiProdotto = Finanziaria.Prodotti.GetDocumentiDaCaricareXProdotto(this.getItem().getProdotto()); return this.m_DocumentiProdotto; }
            // CDocumentiPraticaPropertyPage.prototype.GetDocumentiCaricati = function () {
            // if (this.m_DocumentiPratica == null) this.m_DocumentiPratica = Finanziaria.Pratiche.GetDocumentiCaricatiPerPratica(this.getItem());
            // return this.m_DocumentiPratica;
            public CDocumentoPraticaCaricato GetDocPratica(CDocumentoXGruppoProdotti docProdotto)
            {
                // Dim caricati As CDocumne = this.GetDocumentiCaricati();
                foreach (CDocumentoPraticaCaricato d in this) // (var j = 0; j < caricati.Count() ; j++) {
                {
                    if (d.IDDocumento == DBUtils.GetID(docProdotto))
                    {
                        return d;
                    }
                }

                return null;
            }

            private class CAttSorter : IComparer
            {
                public int Compare(object x, object y)
                {
                    Sistema.CAttachment a = (Sistema.CAttachment)x;
                    Sistema.CAttachment b = (Sistema.CAttachment)y;
                    return -DMD.DateUtils.Compare(a.DataInizio, b.DataInizio);
                }
            }

            /// <summary>
        /// Restituisce l'elenco dei documenti da caricare in base al prodotto selezionato nella pratica
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
            public CCollection<CDocumentoXGruppoProdotti> GetDocumentiDaCaricare()
            {
                return Prodotti.GetDocumentiDaCaricare(m_Pratica.Prodotto);
            }

            /// <summary>
        /// Controlla tra gli allegati associati al cliente se sono disponibili eventuali documenti da caricare e li associa
        /// </summary>
        /// <remarks></remarks>
            public void AllineaDocumentiCaricati()
            {
                var attachments = m_Pratica.Cliente.Attachments;  // s = this.getItem().getCliente().getAttachments(); // new CAttachmentsCollection(this.getItem().getCliente());
                var daCaricare = GetDocumentiDaCaricare();
                // var caricati = this.GetDocumentiCaricati();

                // ordiniamo gli allegati in funzione della data di rilascio inversa
                attachments.Comparer = new CAttSorter();
                attachments.Sort();
                foreach (CDocumentoXGruppoProdotti dDoc in daCaricare)
                {
                    var dPrat = GetDocPratica(dDoc);
                    if (dPrat is null)
                    {
                        dPrat = new CDocumentoPraticaCaricato();
                        dPrat.Pratica = m_Pratica;
                        dPrat.Documento = dDoc;
                        dPrat.Stato = ObjectStatus.OBJECT_VALID;
                        Add(dPrat);
                        dPrat.Save();
                    }

                    switch (dDoc.Disposizione)
                    {
                        case DocumentoXProdottoDisposition.CARICA:
                            {
                                if (dDoc.Documento is object && dPrat.DocumentoCaricato is null)
                                {
                                    foreach (Sistema.CAttachment att in attachments)
                                    {
                                        if (dDoc.Documento.Categoria == att.Categoria && (!dDoc.Documento.ValiditaLimitata || att.IsValid(DMD.DateUtils.Now())) && (!dDoc.Documento.LegatoAlContesto || att.IDContesto == DBUtils.GetID(m_Pratica) && (att.TipoContesto ?? "") == (DMD.RunTime.vbTypeName(m_Pratica) ?? "")))


                                        {
                                            dPrat.DocumentoCaricato = att;
                                            dPrat.DataCaricamento = DMD.DateUtils.Now();
                                            dPrat.OperatoreCaricamento = Sistema.Users.CurrentUser;
                                            dPrat.Save();
                                            break; // break;
                                        }
                                    }
                                }

                                break;
                            }

                        default:
                            {
                                break;
                            }
                    }
                }
            }

            public bool Check()
            {
                foreach (CDocumentoPraticaCaricato dPrat in this)
                {
                    var dProd = dPrat.Documento;
                    if (dProd is object && dProd.Richiesto)
                    {
                        switch (dProd.Disposizione)
                        {
                            case DocumentoXProdottoDisposition.CARICA:
                                {
                                    // break;
                                    if (dPrat.DocumentoCaricato is null)
                                        return false;
                                    break;
                                }

                            default:
                                {
                                    if (dPrat.Verificato == false)
                                        return false;
                                    break;
                                }
                        }
                    }
                }

                return true;
            }
        }
    }
}