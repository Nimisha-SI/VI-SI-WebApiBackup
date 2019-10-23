using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApis.BAL;
using static WebApis.Model.ELModels;

namespace WebApis.Controllers
{
    [ApiController]
    public class TempController : ControllerBase
    {
        private Isportz _sz;
        private ExtendedSearchResultFilterData _objSearchResults;
        private ExtendedSearchResultFilterData _objSearchResults2;
        private ExtendedSearchResultFilterData _objResult;
        public TempController(Isportz objs, ExtendedSearchResultFilterData objrs ) {
            _sz = objs;
            _objSearchResults = objrs;
        }
        [HttpGet]
        [Route("api/temptest")]
        public IActionResult Index()
        {
            
            return Ok(new { result = _sz.getInfo()
        });
        
        }


        
    }
}