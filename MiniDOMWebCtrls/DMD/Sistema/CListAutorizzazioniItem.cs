
namespace minidom.Forms
{






    // ---------------------------------------------------------------------------------------------
    public class CListAutorizzazioniItem
    {
        public Sistema.CModuleAction Azione;
        public int UserAuthID;
        public bool UserAllow;
        public bool UserNegate;
        public int GroupAuthID;
        public bool GroupAllow;
        public bool GroupNegate;

        public CListAutorizzazioniItem()
        {
            DMDObject.IncreaseCounter(this);
            Azione = null;
            UserAuthID = 0;
            UserAllow = false;
            UserNegate = false;
            GroupAuthID = 0;
            GroupAllow = false;
            GroupNegate = false;
        }

        ~CListAutorizzazioniItem()
        {
            DMDObject.DecreaseCounter(this);
        }
    }
}