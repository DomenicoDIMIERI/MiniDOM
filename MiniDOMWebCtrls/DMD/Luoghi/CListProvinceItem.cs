
namespace minidom.Forms
{
    public class CListProvinceItem
    {
        public string Value;
        public int UserAuthID;
        public bool UserAllow;
        public bool UserNegate;
        public int GroupAuthID;
        public bool GroupAllow;
        public bool GroupNegate;

        public CListProvinceItem()
        {
            DMDObject.IncreaseCounter(this);
            Value = "";
            UserAuthID = 0;
            UserAllow = false;
            UserNegate = false;
            GroupAuthID = 0;
            GroupAllow = false;
            GroupNegate = false;
        }

        ~CListProvinceItem()
        {
            DMDObject.DecreaseCounter(this);
        }
    }
}