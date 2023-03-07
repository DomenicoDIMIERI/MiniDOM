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
using static minidom.Contabilita;

namespace minidom
{
    public partial class Contabilita
    {

        /// <summary>
        /// Voci di pagamento associate ad un documento contabile
        /// </summary>
        [Serializable]
        public class VociDiPagamentoPerDocumento
            : CCollection<VoceDiPagamento>
        {
            [NonSerialized]
            private DocumentoContabile m_Documento;

            /// <summary>
            /// Costruttore
            /// </summary>
            public VociDiPagamentoPerDocumento()
            {
                m_Documento = null;
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="documento"></param>
            public VociDiPagamentoPerDocumento(DocumentoContabile documento) 
                : this()
            {
                if (documento is null)
                    throw new ArgumentNullException("documento");
                Load(documento);
            }

            /// <summary>
            /// Restituisce un riferimento al documento
            /// </summary>
            public DocumentoContabile Documento
            {
                get
                {
                    return m_Documento;
                }
            }

            /// <summary>
            /// Imposta il documento
            /// </summary>
            /// <param name="value"></param>
            protected internal virtual void SetDocumento(DocumentoContabile value)
            {
                m_Documento = value;
                if (value is null)
                    return;
                int i = 0;
                foreach (VoceDiPagamento voce in this)
                {
                    voce.SetSourceParams(i.ToString());
                    voce.SetSource(value);                    
                    i += 1;
                }
            }

         
            /// <summary>
            /// OnInsertComplete
            /// </summary>
            /// <param name="index"></param>
            /// <param name="value"></param>
            protected override void OnInsertComplete(int index, VoceDiPagamento value)
            {
                base.OnInsertComplete(index, value);
                if (m_Documento is object)
                {
                    value.SetSource(this.m_Documento);
                    for (int i = index; i < this.Count; i++)
                        this[i].SourceParams = i.ToString();
                }
            }

            /// <summary>
            /// OnSet
            /// </summary>
            /// <param name="index"></param>
            /// <param name="oldValue"></param>
            /// <param name="newValue"></param>
            protected override void OnSet(int index, VoceDiPagamento oldValue, VoceDiPagamento newValue)
            {
                if (m_Documento is object)
                {
                    newValue.SetSource(m_Documento);
                    newValue.SetSourceParams(index.ToString());
                }
                base.OnSet(index, oldValue, newValue);
            }

            /// <summary>
            /// Carica le voci di pagamento associate al documento
            /// </summary>
            /// <param name="documento"></param>
            protected virtual void Load(DocumentoContabile documento)
            {
                if (documento is null)
                    throw new ArgumentNullException("documento");
                Clear();
                m_Documento = documento;
                if (DBUtils.GetID(documento, 0) == 0)
                    return;
                using (var cursor = new VoceDiPagamentoCursor())
                {
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.SourceType.Value = DMD.RunTime.vbTypeName(m_Documento);
                    cursor.SourceID.Value = DBUtils.GetID(m_Documento, 0);
                    cursor.SourceParams.SortOrder = SortEnum.SORT_ASC;
                    cursor.IgnoreRights = true;
                    while (cursor.Read())
                    {
                        Add(cursor.Item);
                    }
                }
            }

            /// <summary>
            /// Salva tutti gli oggetti nel db
            /// </summary>
            /// <param name="force"></param>
            public virtual void Save(bool force = false)
            {
                this.Save(minidom.Contabilita.VociDiPagamento.Database, force);                 
            }
        }
    }
}