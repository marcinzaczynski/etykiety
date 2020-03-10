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

        public static List<t3> T2GetGroupElements(int idGrupa)
        {
            using (var db = new etykietyEntities())
            {
                return db.t3.Where(r => r.id_grupa == idGrupa).ToList();

                //return db.t2.Where(r => r.id_grupa == idGrupa).ToList();
            }
        }
    }
}
