using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApis.Model;
using static WebApis.Model.ELModels;

namespace WebApis.DAL
{
    interface ISportDetails
    {
        List<ddlValue> GetSport();

    }
}
