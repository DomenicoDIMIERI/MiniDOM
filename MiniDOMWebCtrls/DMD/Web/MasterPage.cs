
namespace minidom
{
    public class MasterPage : System.Web.UI.MasterPage
    {
        public MasterPage()
        {
            DMDObject.IncreaseCounter(this);
        }

        ~MasterPage()
        {
            DMDObject.DecreaseCounter(this);
        }
    }
}