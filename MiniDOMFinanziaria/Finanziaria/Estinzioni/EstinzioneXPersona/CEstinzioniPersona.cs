using System;

namespace minidom
{
    public partial class Finanziaria
    {


        /// <summary>
    /// Rappresenta una collezione di estinzioni associate ad una persona
    /// </summary>
    /// <remarks></remarks>
        [Serializable]
        public class CEstinzioniPersona : CEstinzioniCollection
        {
            [NonSerialized]
            private Anagrafica.CPersona m_Persona;

            public CEstinzioniPersona()
            {
                m_Persona = null;
            }

            public CEstinzioniPersona(Anagrafica.CPersona p) : this()
            {
                Initialize(p);
            }

            public Anagrafica.CPersona Persona
            {
                get
                {
                    return m_Persona;
                }
            }

            protected override void OnInsert(int index, object value)
            {
                if (m_Persona is object)
                    ((CEstinzione)value).SetPersona(m_Persona);
                base.OnInsert(index, value);
            }

            protected override void OnSet(int index, object oldValue, object newValue)
            {
                if (m_Persona is object)
                    ((CEstinzione)newValue).SetPersona(m_Persona);
                base.OnSet(index, oldValue, newValue);
            }

            protected internal bool Initialize(Anagrafica.CPersona persona)
            {
                if (persona is null)
                    throw new ArgumentNullException("persona");
                Clear();
                m_Persona = persona;
                if (DBUtils.GetID(persona) != 0)
                {
                    using (var cursor = new CEstinzioniCursor())
                    {
                        cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                        cursor.IDPersona.Value = DBUtils.GetID(persona);
                        cursor.DataInizio.SortOrder = SortEnum.SORT_ASC;
                        cursor.IgnoreRights = true;
                        while (!cursor.EOF())
                        {
                            Add(cursor.Item);
                            cursor.MoveNext();
                        }
                    }
                }

                return true;
            }
        }
    }
}