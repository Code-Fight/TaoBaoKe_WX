using Dos.ORM;

namespace DB.Services.DBbase
{
    public class DbExcel
    {
        public static readonly DbSession Context = new DbSession("Conn");
    }
}
