using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApis.DAL;
using WebApis.Model;

namespace WebApis.BAL
{
    public class sportzBal : Isportz
    {
        //ISportDetails _sObj;
        //sportzBal(ISportDetails sObj)
        //{
        //    _sObj = sObj;
        //}
        public List<ddlValue> getInfo(){
            
            List<ddlValue> obj = new List<ddlValue>();
            obj.Add(new ddlValue
            {
                Id = "1",
                Key = "Hdello"
            });

            return obj;
        }
        public List<ddlValue> getInfo2()
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
