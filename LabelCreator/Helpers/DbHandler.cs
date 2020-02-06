using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabelCreator.Helpers
{
    public class DbHandler
    {
        public static List<t1> T1GetGroups()
        {
            using (var db = new etykietyEntities())
            {
                
                return db.t1.ToList();
            }
        }
    }
}
