using System;
using System.Collections;
using DMD;
using DMD.XML;
using DMD.Databases;
using DMD.Databases.Collections;
using static minidom.Sistema;
using static minidom.Anagrafica;
using minidom.repositories;

namespace minidom
{
    public partial class Sistema
    {

        /// <summary>
        /// Collezione di annotazioni su un oggetto
        /// </summary>
        [Serializable]
        public class CAnnotazioni 
            : CCollection<CAnnotazione>
        {
            [NonSerialized]  private object m_Contesto;
            [NonSerialized] private object m_Owner;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CAnnotazioni()
            {
                m_Owner = null;
                m_Contesto = null;
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="owner"></param>
            public CAnnotazioni(object owner) : this()
            {
                if (owner is null)
                    throw new ArgumentNullException("owner");
                SetOwner(owner);
                Initialize();
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="owner"></param>
            /// <param name="contesto"></param>
            public CAnnotazioni(object owner, object contesto) : this()
            {
                if (owner is null)
                    throw new ArgumentNullException("owner");
                SetOwner(owner);
                SetContesto(contesto);
                Initialize();
            }

            /// <summary>
            /// Imposta il possessore
            /// </summary>
            /// <param name="value"></param>
            public void SetOwner(object value)
            {
                m_Owner = value;
                if (value is object)
                {
                    foreach (CAnnotazione a in this)
                        a.SetOwner(value);
                }
            }

            /// <summary>
            /// Imposta il contesto
            /// </summary>
            /// <param name="value"></param>
            protected internal void SetContesto(object value)
            {
                m_Contesto = value;
                foreach (CAnnotazione a in this)
                    a.SetContesto(value);
            }

            /// <summary>
            /// OnInsert
            /// </summary>
            /// <param name="index"></param>
            /// <param name="value"></param>
            protected override void OnInsert(int index, CAnnotazione value)
            {
                var item = (CAnnotazione)value;
                if (m_Owner is object)
                    item.SetOwner(m_Owner);

                if (m_Contesto is object)
                    item.SetContesto(m_Contesto);
                base.OnInsert(index, value);
            }

            /// <summary>
            /// OnSet
            /// </summary>
            /// <param name="index"></param>
            /// <param name="oldValue"></param>
            /// <param name="newValue"></param>
            protected override void OnSet(int index, CAnnotazione oldValue, CAnnotazione newValue)
            {
                var item = (CAnnotazione)newValue;
                if (m_Owner is object)
                    item.SetOwner(m_Owner);

                if (m_Contesto is object)
                    item.SetContesto(m_Contesto);
                base.OnSet(index, oldValue, newValue);
            }

            /// <summary>
            /// Aggiunge
            /// </summary>
            /// <param name="item"></param>
            public void AddItem(CAnnotazione item)
            {
                Add(item);
            }

            /// <summary>
            /// Aggiunge
            /// </summary>
            /// <param name="nota"></param>
            /// <returns></returns>
            public CAnnotazione Add(string nota)
            {
                var item = new CAnnotazione();
                // item.Produttore = Anagrafica.Aziende.AziendaPrincipale
                item.Valore = nota;
                item.SetContesto(m_Contesto);
                item.SetOwner(m_Owner);
                item.Stato = ObjectStatus.OBJECT_VALID;
                AddItem(item);
                return item;
            }

            /// <summary>
            /// Restituisce una stringa compatta
            /// </summary>
            /// <returns></returns>
            public string GetCompactString()
            {
                int i;
                var html = new System.Text.StringBuilder();
                string nomeCreatoDa, nomeModificatoDa;
                var loopTo = base.Count - 1;
                for (i = 0; i <= loopTo; i++)
                {
                    if (html.Length > 0)
                    {
                        html.Append(DMD.Strings.vbCrLf);
                    }

                    nomeCreatoDa = "";
                    nomeModificatoDa = "";
                    if (base[i] is object)
                    {
                        if (this[i].CreatoDa is object)
                            nomeCreatoDa = this[i].CreatoDa.Nominativo;
                        if (this[i].ModificatoDa is object)
                            nomeModificatoDa = this[i].ModificatoDa.Nominativo;
                    }

                    html.Append(Formats.FormatUserDateTime(this[i].ModificatoIl));
                    html.Append(" ");
                    html.Append(nomeModificatoDa);
                    html.Append(" ");
                    html.Append(this[i].Valore);
                }

                return html.ToString();
            }

            /// <summary>
            /// Restituisce la stringa completa
            /// </summary>
            /// <returns></returns>
            public string GetFullString()
            {
                int i;
                var html = new System.Text.StringBuilder();
                string nomeCreatoDa, nomeModificatoDa;
                var loopTo = base.Count - 1;
                for (i = 0; i <= loopTo; i++)
                {
                    if (html.Length > 0)
                        html.Append(DMD.Strings.vbCrLf);

                    nomeCreatoDa = "";
                    nomeModificatoDa = "";
                    if (base[i] is object)
                    {
                        if (base[i].CreatoDa is object)
                            nomeCreatoDa = base[i].CreatoDa.Nominativo;
                        if (base[i].ModificatoDa is object)
                            nomeModificatoDa = base[i].ModificatoDa.Nominativo;
                    }

                    html.Append(Formats.FormatUserDateTime(base[i].ModificatoIl));
                    html.Append(" ");
                    html.Append(nomeModificatoDa);
                    html.Append(" ");
                    html.Append(base[i].Valore);
                }

                return html.ToString();
            }

            /// <summary>
            /// Inizializza la collezione
            /// </summary>
            /// <returns></returns>
            protected bool Initialize()
            {
                Clear();
                if (DBUtils.GetID(m_Owner, 0) == 0)
                    return true;

                using (var cursor = new CAnnotazioniCursor())
                {
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.OwnerID.Value = DBUtils.GetID(m_Owner, 0);
                    cursor.OwnerType.Value = DMD.RunTime.vbTypeName(m_Owner);
                    cursor.CreatoIl.SortOrder = SortEnum.SORT_DESC;
                    if (m_Contesto is object)
                    {
                        cursor.IDContesto.Value = DBUtils.GetID(m_Contesto, 0);
                        cursor.TipoContesto.Value = DMD.RunTime.vbTypeName(m_Contesto);
                    }

                    while (cursor.Read())
                    {
                        Add(cursor.Item);
                    }

                }

                return true;
            }
        }
    }
}