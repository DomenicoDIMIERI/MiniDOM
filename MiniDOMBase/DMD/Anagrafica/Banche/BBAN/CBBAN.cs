using System;
using DMD;
using DMD.Databases;
using static minidom.Sistema;

namespace minidom
{
    public partial class Anagrafica
    {

        /// <summary>
        /// Classe che rappresenta un codice BBAN
        /// </summary>
        /// <remarks></remarks>
        [Serializable]
        public class CBBAN 
            : DMD.XML.DMDBaseXMLObject
        {

            private string m_Codice;

            /// <summary>
            /// Costruttore
            /// </summary>
            public CBBAN()
            {
            }

            /// <summary>
            /// Costruttore
            /// </summary>
            /// <param name="codice"></param>
            public CBBAN(string codice) : this()
            {
                m_Codice = DMD.Strings.Trim(codice);
            }

            /// <summary>
            /// Restituisce o imposta il codice
            /// </summary>
            public string Codice
            {
                get
                {
                    return m_Codice;
                }

                set
                {
                    value = DMD.Strings.UCase(DMD.Strings.Left(DMD.Strings.Trim(value), 2));
                    string oldValue = m_Codice;
                    m_Codice = value;
                    //DoChanged("Codice", value, oldValue);
                }
            }

            /// <summary>
            /// Restituisce una stringa che rappresenta l'oggetto
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                return this.m_Codice;
            }

            /// <summary>
            /// Restituisce vero se il codice rappresentato è valido
            /// </summary>
            /// <returns></returns>
            /// <remarks></remarks>
            public virtual bool IsValid()
            {
                return true;
            }

            //protected internal override DBConnection GetConnection()
            //{
            //    return Databases.APPConn;
            //}

            

            ///// <summary>
            ///// Restituisce il discriminante
            ///// </summary>
            ///// <returns></returns>
            //public override string GetTableName()
            //{
            //    return "tbl_BBANs";
            //}

            /// <summary>
            /// Restituisce il codice hash dell'oggetto
            /// </summary>
            /// <returns></returns>
            public override int GetHashCode()
            {
                return HashCalculator.Calculate(this.m_Codice);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public sealed override bool Equals(object obj)
            {
                return (obj is CBBAN) && this.Equals( (CBBAN)obj);
            }

            /// <summary>
            /// Restituisce true se i due oggetti sono uguali
            /// </summary>
            /// <param name="obj"></param>
            /// <returns></returns>
            public virtual bool Equals(CBBAN obj)
            {
                return //base.Equals(obj) && 
                    DMD.Strings.EQ(this.m_Codice, obj.m_Codice);
            }
        }
    }
}