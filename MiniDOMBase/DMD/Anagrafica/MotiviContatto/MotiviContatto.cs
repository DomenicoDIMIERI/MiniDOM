using System;
using System.Collections;
using minidom.repositories;
using DMD.XML;
using DMD.Databases;
using DMD.Databases.Collections;
using static minidom.Anagrafica;


namespace minidom
{
    namespace repositories
    {

        /// <summary>
        /// Repository di oggetti di tipo <see cref="MotivoContatto"/>
        /// </summary>
        [Serializable]
        public sealed class MotiviContattoClass 
            : CModulesClass<Anagrafica.MotivoContatto>
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public MotiviContattoClass() 
                : base("modCRMMotiviContatto", typeof(Anagrafica.MotiviContattoCursor), -1)
            {
            }

            /// <summary>
            /// Restituisce l'elemento in base al nome
            /// </summary>
            /// <param name="nome"></param>
            /// <returns></returns>
            public Anagrafica.MotivoContatto GetItemByName(string nome)
            {
                nome = DMD.Strings.Trim(nome);
                if (string.IsNullOrEmpty(nome))
                    return null;
                foreach (Anagrafica.MotivoContatto item in LoadAll())
                {
                    if (DMD.Strings.Compare(item.Nome, nome, true) == 0)
                        return item;
                }

                return null;
            }

            /// <summary>
            /// Restituisce la collezione dei motivi di tipo specifico
            /// </summary>
            /// <param name="tipo"></param>
            /// <param name="isIn"></param>
            /// <returns></returns>
            public CCollection<Anagrafica.MotivoContatto> GetItemsByTipoContatto(string tipo, bool isIn)
            {
                var ret = new CCollection<Anagrafica.MotivoContatto>();
                tipo = DMD.Strings.Trim(tipo);
                if (string.IsNullOrEmpty(tipo))
                    return ret;
                foreach (Anagrafica.MotivoContatto item in LoadAll())
                {
                    if (item.Stato == ObjectStatus.OBJECT_VALID && Sistema.TestFlag(item.Flags, Anagrafica.MotivoContattoFlags.Attivo) && (Sistema.TestFlag(item.Flags, Anagrafica.MotivoContattoFlags.InEntrata) && isIn || Sistema.TestFlag(item.Flags, Anagrafica.MotivoContattoFlags.InUscita) && !isIn) && (string.IsNullOrEmpty(item.TipoContatto) || DMD.Strings.Compare(item.TipoContatto, tipo, true) == 0))
                    {
                        ret.Add(item);
                    }
                }

                return ret;
            }
        }
    }

    public partial class Anagrafica
    {
        private static MotiviContattoClass m_MotiviContatto = null;

        /// <summary>
        /// Repository di oggetti di tipo <see cref="MotivoContatto"/>
        /// </summary>
        public static MotiviContattoClass MotiviContatto
        {
            get
            {
                if (m_MotiviContatto is null)
                    m_MotiviContatto = new MotiviContattoClass();
                return m_MotiviContatto;
            }
        }
    }
}