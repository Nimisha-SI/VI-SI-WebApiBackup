using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApis.Model;
using static WebApis.Model.ELModels;

namespace WebApis.BAL
{
    public interface Isportz
    {
        //string onj;
        List<ddlValue> getInfo();
    }
}
