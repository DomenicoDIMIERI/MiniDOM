using System;

namespace minidom.repositories
{

    /// <summary>
    /// Repository di oggetti di tipo <see cref="ClienteXCollaboratore"/>
    /// </summary>
    [Serializable]
    public sealed class ClientiXCollaboratoriClass 
        : CModulesClass<Finanziaria.ClienteXCollaboratore>
    {

        /// <summary>
        /// Costruttore
        /// </summary>
        public ClientiXCollaboratoriClass()  
            : base("modCliXCollab", typeof(Finanziaria.ClienteXCollaboratoreCursor), 0)
        {
        }


        /// <summary>
        /// Restituisce la relazione cliente x collaboratore associata al cliente
        /// </summary>
        /// <param name="p"></param>
        /// <param name="coll"></param>
        /// <returns></returns>
        public ClienteXCollaboratore GetItemByPersonaECollaboratore(
                                                            CPersonaFisica p, 
                                                            CCollaboratore coll
                                                            )
        {
            if (p is null)
                throw new ArgumentNullException("p");
            if (coll is null)
                throw new ArgumentNullException("coll");
            using (var cursor = new Finanziaria.ClienteXCollaboratoreCursor())
            {
                cursor.Stato.Value = ObjectStatus.OBJECT_VALID;
                cursor.IDPersona.Value = DBUtils.GetID(p);
                cursor.IDCollaboratore.Value = DBUtils.GetID(coll);
                cursor.DataAcquisizione.Sort = SortEnum.SORT_DESC;
                cursor.IgnoreRights = true;
                return cursor.Item;
            } 
        }
    }
}