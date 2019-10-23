using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApis.BOL;
using static WebApis.Model.ELModels;

namespace WebApis.Controllers
{
    [ApiController]
    public class SearchDataFilterController : ControllerBase
    {
        private ISearchDataFilter _sObj;
        private ExtendedSearchResultFilterData _objResult = new ExtendedSearchResultFilterData();
        //private ExtendedSearchResultFilterData _objSearchResults2;
        //private ExtendedSearchResultFilterData _objResult;
        private MatchDetail _objMatchDetail = new MatchDetail();
        public SearchDataFilterController(ISearchDataFilter sObj) {
                _sObj = sObj;
            // _objSearchResults = objSearchResults;
            //_objSearchResults2 = objSearchResults2;
            // _objResult = objResult;
            //_objMatchDetail = objMatchDetail;, ExtendedSearchResultFilterData objSearchResults, MatchDetail objMatchDetail
        }
        [System.Web.Http.HttpPost]
        [Route("api/GetSearchResultForFiltersTemp1")]
        public IActionResult GetSearchResultsFilterTemp(STFilterRequestData _objReqData)
        {
            try
            {
                if (_objReqData != null) {
                    _objResult = _sObj.GetSearchResultsFilter(_objReqData);
                }
                return Ok(new { responseText = _objResult });
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [System.Web.Http.HttpPost]
        [Route("api/GetFilterBySportTemp")]
        public IActionResult GetFilteredEntitiesBySport(SearchEntityRequestData _objReqData)
        {
            try
            {
                var responseResult = new List<FilteredEntityForCricket>();
                CommonFunction objCF = new CommonFunction();
             
                string SportName = objCF.getType(_objReqData.MatchDetails.SportID);
                if (_objReqData != null) {
                    if (SportName == "Cricket") {
                        responseResult = _sObj.GetFilteredEntitiesBySport(_objReqData);
                    }

                }

                
                return Ok(new { responseText = responseResult });
            }
            catch (Exception ex)
            {
                return BadRequest();
            }



        }



        [System.Web.Http.HttpPost]
        [Route("api/GetSearchedFilters")]
        public IActionResult GetSearchedFilters(SaveSearchesRequestData _objReqData) {
            try
            {
                string response = string.Empty;
                //SaveSearchesRequestData _objReqData = new SaveSearchesRequestData();
                // string jsonData = JsonConvert.SerializeObject(data);
                // _objReqData = JsonConvert.DeserializeObject<SaveSearchesRequestData>(jsonData);
                if (_objReqData != null)
                {
                    response = _sObj.GetSavedSearches(_objReqData);
                }
                return Ok(new { responseText = response });
                //return Ok();
            }
            catch (Exception ex) {
                return BadRequest(ex.Message.ToString());
            }
            
          
        }

        [System.Web.Http.HttpPost]
        [Route("api/GetSearchResultCount")]
        public IActionResult GetSearchResultCount(IEnumerable<SearchRequestData> _objReqData) {
            try
            {

                List<SearchRequestData> _objLstReqData = new List<SearchRequestData>();
               // _objLstReqData.MatchDetails = new List<MatchDetail>();
                //_objLstReqData.MatchSituations = new List<MatchSituation>();
                //_objLstReqData.PlayerDetails = new List<PlayerDetail>();
                string jsonData = JsonConvert.SerializeObject(_objReqData);
                _objLstReqData = JsonConvert.DeserializeObject<List<SearchRequestData>>(jsonData);
                //_objLstReqData.MatchDetails = JsonConvert.DeserializeObject<List<MatchDetail>>(jsonData);
                //_objLstReqData.MatchSituations = JsonConvert.DeserializeObject<List<MatchSituation>>(jsonData);
                //_objLstReqData.PlayerDetails = JsonConvert.DeserializeObject<List<PlayerDetail>>(jsonData);
                if (_objLstReqData != null)
                {
                    SearchRequestData _objReqDataRes = _objLstReqData.FirstOrDefault();
                    _sObj.GetSearchResultCount(_objReqDataRes);
                }
                return Ok();
            }
            catch (Exception ex) {
                return BadRequest(ex.Message.ToString());
            }

        }

    }
}