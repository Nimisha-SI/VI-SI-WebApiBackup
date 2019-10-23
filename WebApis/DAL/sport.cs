using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApis.BOL;
using WebApis.Model;

namespace WebApis.DAL
{
    public class sport : ISportDetails
    {
        public List<ddlValue>  GetSport()
        {
            List<ddlValue> obj = new List<ddlValue>();
            obj.Add(new ddlValue
            {
                Id = "1",
                Key = "Hdello"
            });

            return obj;
        }

    }

}
