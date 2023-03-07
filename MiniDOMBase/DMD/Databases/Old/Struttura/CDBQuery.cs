using System;

namespace minidom
{
    public partial class Databases
    {
        public class CDBQuery : CDBEntity
        {
            public CDBQuery()
            {
            }

            protected override void CreateInternal1()
            {
                Connection.CreateView(this);
            }

            protected override void DropInternal1()
            {
                Connection.DropView(this);
                Connection.Queries.RemoveByKey(Name);
            }

            protected override void UpdateInternal1()
            {
                Connection.UpdateView(this);
            }

            protected override void RenameItnernal(string newName)
            {
                throw new NotImplementedException();
            }
        }
    }
}