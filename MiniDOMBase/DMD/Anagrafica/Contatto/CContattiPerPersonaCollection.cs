using System;
using System.Collections;
using DMD;
using DMD.Databases.Collections;
using DMD.Databases;

namespace minidom
{
    public partial class Anagrafica
    {

        /// <summary>
        /// Collezione di recapiti della persona
        /// </summary>
        [Serializable]
        public class CContattiPerPersonaCollection 
            : CCollection<CContatto>
        {
            [NonSerialized] private CPersona m_Persona; 
            [NonSerialized] private CUfficio m_Ufficio;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CContattiPerPersonaCollection()
            {
                m_Persona = null;
                m_Ufficio = null;
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="persona"></param>
            /// <param name="ufficio"></param>
            public CContattiPerPersonaCollection(CPersona persona, CUfficio ufficio = null) : this()
            {
                Load(persona, ufficio);
            }

            /// <summary>
            /// Imposta la persona
            /// </summary>
            /// <param name="value"></param>
            protected internal virtual void SetPersona(CPersona value)
            {
                m_Persona = value;
                if (value is object)
                {
                    foreach (CContatto c in this)
                        c.SetPersona(value);
                }
            }

            /// <summary>
            /// Imposta l'ufficio
            /// </summary>
            /// <param name="value"></param>
            protected internal virtual void SetUfficio(CUfficio value)
            {
                m_Ufficio = value;
                if (value is null)
                    return;
                foreach (CContatto c in this)
                    c.SetUfficio(value);
            }

            /// <summary>
            /// OnInsert
            /// </summary>
            /// <param name="index"></param>
            /// <param name="value"></param>
            protected override void OnInsert(int index, CContatto value)
            {
                if (m_Persona is object)
                    ((CContatto)value).SetPersona(m_Persona);
                
                if (m_Ufficio is object)
                    ((CContatto)value).SetUfficio(m_Ufficio);

                base.OnInsert(index, value);
            }

            /// <summary>
            /// OnSet
            /// </summary>
            /// <param name="index"></param>
            /// <param name="oldValue"></param>
            /// <param name="newValue"></param>
            protected override void OnSet(int index, CContatto oldValue, CContatto newValue)
            {
                if (m_Persona is object)
                    ((CContatto)newValue).SetPersona(m_Persona);

                if (m_Ufficio is object)
                    ((CContatto)newValue).SetUfficio(m_Ufficio);

                base.OnSet(index, oldValue, newValue);
            }

            /// <summary>
            /// Aggiunge un contatto
            /// </summary>
            /// <param name="contatto"></param>
            /// <returns></returns>
            public new CContatto Add(CContatto contatto)
            {
                foreach (CContatto c in this)
                {
                    if ((c.Tipo ?? "") == (contatto.Tipo ?? "") && (c.Valore ?? "") == (contatto.Valore ?? ""))
                    {
                        c.MergeWith(contatto);
                        return c;
                    }
                }

                contatto.Ordine = Count;
                base.Add(contatto);
                return contatto;
            }

            /// <summary>
            /// Crea un nuovo oggetto con nome, lo salva, e lo restituisce
            /// </summary>
            /// <param name="tipo"></param>
            /// <param name="nome"></param>
            /// <param name="valore"></param>
            /// <returns></returns>
            /// <remarks></remarks>
            public CContatto Add(string tipo, string nome, string valore)
            {
                var c = new CContatto();
                c.Tipo = tipo;
                c.Nome = nome;
                c.Valore = valore;
                c.Stato = ObjectStatus.OBJECT_VALID;
                // If (c.Save()
                return Add(c);
            }

            /// <summary>
            /// Restituisce il contatto predefinito per il tipo
            /// </summary>
            /// <param name="tipo"></param>
            /// <returns></returns>
            public CContatto GetContattoPredefinito(string tipo)
            {
                tipo = DMD.Strings.LCase(DMD.Strings.Trim(tipo));
                foreach (CContatto c in this)
                {
                    if ((DMD.Strings.LCase(c.Tipo) ?? "") == (tipo ?? "") && !string.IsNullOrEmpty(c.Valore) && c.Predefinito)
                    {
                        return c;
                    }
                }

                foreach (CContatto c in this)
                {
                    if ((DMD.Strings.LCase(c.Tipo) ?? "") == (tipo ?? "") && !string.IsNullOrEmpty(c.Valore))
                    {
                        return c;
                    }
                }

                return null;
            }

            /// <summary>
            /// Imposta il contatto predefinito per il tipo
            /// </summary>
            /// <param name="tipo"></param>
            /// <param name="valore"></param>
            public void SetContattoPredefinito(string tipo, string valore)
            {
                CContatto c;
                var c1 = new CContatto();
                c1.Tipo = tipo;
                c1.Valore = valore;
                foreach (var currentC in this)
                {
                    c = currentC;
                    if (DMD.Strings.Compare(c.Tipo, c1.Tipo, true) == 0)
                    {
                        if ((c.Valore ?? "") == (c1.Valore ?? ""))
                        {
                            c.Predefinito = true;
                            c.Save();
                            return;
                        }
                        else
                        {
                            c.Predefinito = false;
                            c.Save();
                            return;
                        }
                    }
                }

                c = Add(tipo, tipo, valore);
                c.Predefinito = true;
                c.Save();
            }

            /// <summary>
            /// Carica i contatti
            /// </summary>
            /// <param name="persona"></param>
            /// <param name="ufficio"></param>
            protected internal void Load(CPersona persona, CUfficio ufficio = null)
            {
                if (persona is null)
                    throw new ArgumentNullException("persona");

                Clear();

                m_Persona = persona;
                m_Ufficio = ufficio;

                if (DBUtils.GetID(persona, 0) == 0)
                    return;

                using (var cursor = new CContattoCursor())
                {
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.IgnoreRights = true;
                    cursor.PersonaID.Value = DBUtils.GetID(m_Persona, 0);
                    if (m_Ufficio is object) cursor.IDUfficio.Value = DBUtils.GetID(ufficio, 0);
                    while (cursor.Read())
                    {
                        base.Add(cursor.Item);
                    }
                }

                Sort();
            }
        }
    }
}