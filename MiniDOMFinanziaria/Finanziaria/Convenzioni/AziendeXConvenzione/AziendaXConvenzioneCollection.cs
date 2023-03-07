using System;
using System.Data;

namespace minidom
{
    public partial class Finanziaria
    {

        /// <summary>
    /// Collezione di convenzioni associate ad un'azienda
    /// </summary>
    /// <remarks></remarks>
        [Serializable]
        public class AziendaXConvenzioneCollection : CCollection<AziendaXConvenzione>
        {
            [NonSerialized]
            private CQSPDConvenzione m_Convenzione;

            public AziendaXConvenzioneCollection()
            {
                m_Convenzione = null;
            }

            public AziendaXConvenzioneCollection(CQSPDConvenzione convenzione) : this()
            {
                Load(convenzione);
            }

            public AziendaXConvenzione Create(string nome, Anagrafica.CAzienda azienda)
            {
                var item = GetItemByName(nome);
                if (item is object)
                    throw new DuplicateNameException("nome");
                item = new AziendaXConvenzione();
                item.Nome = nome;
                item.Azienda = azienda;
                item.Stato = ObjectStatus.OBJECT_VALID;
                Add(item);
                item.Save();
                return item;
            }

            public AziendaXConvenzione GetItemByName(string nome)
            {
                foreach (AziendaXConvenzione rel in this)
                {
                    if (DMD.Strings.Compare(rel.Nome, nome, true) == 0)
                        return rel;
                }

                return null;
            }

            protected override void OnInsert(int index, object value)
            {
                if (m_Convenzione is object)
                    ((AziendaXConvenzione)value).SetConvenzione(m_Convenzione);
                base.OnInsert(index, value);
            }

            protected override void OnSet(int index, object oldValue, object newValue)
            {
                if (m_Convenzione is object)
                    ((AziendaXConvenzione)newValue).SetConvenzione(m_Convenzione);
                base.OnSet(index, oldValue, newValue);
            }

            protected void Load(CQSPDConvenzione convenzione)
            {
                if (convenzione is null)
                    throw new ArgumentNullException("convenzione");
                Clear();
                SetConvenzione(convenzione);
                if (DBUtils.GetID(convenzione) == 0)
                    return;
                AziendaXConvenzioneCursor cursor = null;
                /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
                cursor = new AziendaXConvenzioneCursor();
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                cursor.IDConvenzione.Value = DBUtils.GetID(convenzione);
                cursor.IgnoreRights = true;
                while (!cursor.EOF())
                {
                    Add(cursor.Item);
                    cursor.MoveNext();
                }

                Sort();
                /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
                if (cursor is object)
                {
                    cursor.Dispose();
                    cursor = null;
                }
                /* TODO ERROR: Skipped IfDirectiveTrivia *//* TODO ERROR: Skipped DisabledTextTrivia *//* TODO ERROR: Skipped EndIfDirectiveTrivia */
            }

            protected internal virtual void SetConvenzione(CQSPDConvenzione value)
            {
                m_Convenzione = value;
                if (value is object)
                {
                    foreach (AziendaXConvenzione item in this)
                        item.SetConvenzione(value);
                }
            }
        }
    }
}