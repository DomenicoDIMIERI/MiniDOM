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
        /// Repository di oggetti di tipo <see cref="MotivoAppuntamento"/>
        /// </summary>
        [Serializable]
        public sealed class MotiviAppuntamentoClass 
            : CModulesClass<Anagrafica.MotivoAppuntamento>
        {

            /// <summary>
            /// Costruttore
            /// </summary>
            public MotiviAppuntamentoClass() 
                : base("modCRMMotiviAppuntamento", typeof(Anagrafica.MotiviAppuntamentoCursor), -1)
            {
            }

            /// <summary>
            /// Restituisce l'elemento in base al nome
            /// </summary>
            /// <param name="nome"></param>
            /// <returns></returns>
            public Anagrafica.MotivoAppuntamento GetItemByName(string nome)
            {
                nome = DMD.Strings.Trim(nome);
                if (string.IsNullOrEmpty(nome))
                    return null;
                foreach (Anagrafica.MotivoAppuntamento item in LoadAll())
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
            /// <returns></returns>
            public CCollection<Anagrafica.MotivoAppuntamento> GetItemsByTipoAppuntamento(string tipo)
            {
                var ret = new CCollection<Anagrafica.MotivoAppuntamento>();
                tipo = DMD.Strings.Trim(tipo);
                if (string.IsNullOrEmpty(tipo))
                    return ret;
                foreach (Anagrafica.MotivoAppuntamento item in LoadAll())
                {
                    if (
                        item.Stato == ObjectStatus.OBJECT_VALID 
                        && Sistema.TestFlag(item.Flags, Anagrafica.MotivoAppuntamentoFlags.Attivo) 
                        && 
                        (
                            string.IsNullOrEmpty(item.TipoAppuntamento) 
                            || DMD.Strings.EQ(item.TipoAppuntamento, tipo, true)
                        )
                       )
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
        private static MotiviAppuntamentoClass m_MotiviAppuntamento = null;

        /// <summary>
        /// Repository di oggetti di tipo <see cref="MotivoAppuntamento"/>
        /// </summary>
        public static MotiviAppuntamentoClass MotiviAppuntamento
        {
            get
            {
                if (m_MotiviAppuntamento is null)
                    m_MotiviAppuntamento = new MotiviAppuntamentoClass();
                return m_MotiviAppuntamento;
            }
        }
    }
}