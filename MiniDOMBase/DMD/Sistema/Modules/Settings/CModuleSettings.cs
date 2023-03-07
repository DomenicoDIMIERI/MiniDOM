using System;

namespace minidom
{
    public partial class Sistema
    {
        [Serializable]
        public class CModuleSettings : CSettings
        {
            public CModuleSettings()
            {
            }

            public CModuleSettings(CModule owner) : base(owner)
            {
            }

            public void Update()
            {
                Initialize(Owner);
            }
        }
    }
}