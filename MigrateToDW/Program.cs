using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigrateToDW
{
    class Program
    {
        static void Main(string[] args)
        {

            using(var db =new AxisAutomation())
            {
                db.Categories.ToList();
            }
        }
    }
}
