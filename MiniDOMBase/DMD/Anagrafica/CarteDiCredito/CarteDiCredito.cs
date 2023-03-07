using System;
using DMD;
using DMD.Databases;
using DMD.Databases.Collections;
using static minidom.Sistema;
using static minidom.Anagrafica;
using minidom.repositories;

namespace minidom
{
    namespace repositories
    {

        /// <summary>
        /// Repository di oggetti di tipo <see cref="CartaDiCredito"/>
        /// </summary>
        [Serializable]
        public class CCarteDiCreditoClass 
            : CModulesClass<Anagrafica.CartaDiCredito>
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public CCarteDiCreditoClass() 
                : base("modCarteDiCredito", typeof(Anagrafica.CartaDiCreditoCursor), 0)
            {
            }

            /// <summary>
            /// Restituisce l'elemento in base al nome
            /// </summary>
            /// <param name="value"></param>
            /// <returns></returns>
            public Anagrafica.CartaDiCredito GetItemByName(string value)
            {
                value = DMD.Strings.Trim(value);
                if (string.IsNullOrEmpty(value))
                    return null;

                using (var cursor = new Anagrafica.CartaDiCreditoCursor())
                {
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.Name.Value = value;
                    return cursor.Item;
                }
            }

            /// <summary>
            /// Restituisce la carta di credito corrente corrispondente al numero
            /// </summary>
            /// <param name="circuito"></param>
            /// <param name="numeroCarta"></param>
            /// <returns></returns>
            public Anagrafica.CartaDiCredito GetItemByNumero(string circuito, string numeroCarta)
            {
                circuito = DMD.Strings.Trim(circuito);
                numeroCarta = DMD.Strings.Trim(numeroCarta);
                if (string.IsNullOrEmpty(circuito) || string.IsNullOrEmpty(numeroCarta))
                    return null;

                using (var cursor = new Anagrafica.CartaDiCreditoCursor())
                {
                    cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                    cursor.CircuitoCarta.Value = circuito;
                    cursor.NumeroCarta.Value = numeroCarta;
                    cursor.DataInizio.SortOrder = SortEnum.SORT_DESC;
                    return cursor.Item;
                }
            }
        }
    }

    public partial class Anagrafica
    {
        private static CCarteDiCreditoClass m_CarteDiCredito = null;

        /// <summary>
        /// Repository di oggetti di tipo <see cref="CartaDiCredito"/>
        /// </summary>
        public static CCarteDiCreditoClass CarteDiCredito
        {
            get
            {
                if (m_CarteDiCredito is null)
                    m_CarteDiCredito = new CCarteDiCreditoClass();
                return m_CarteDiCredito;
            }
        }
    }
}