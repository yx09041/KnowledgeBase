using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySoft.Data.Repository;

namespace MyConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            var db = SugarDbContext.GetInstance();
            db.DbFirst.Where("knowledgeInfo").CreateClassFile("D:\\sugarEntityGenerate\\1");
        }
    }
}
