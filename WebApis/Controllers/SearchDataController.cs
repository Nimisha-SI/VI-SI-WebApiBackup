using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nest;
using Newtonsoft.Json;
using WebApis.BOL;
using WebApis.elastic;
using WebApis.Model;
using static WebApis.Model.ELModels;

namespace WebApis.Controllers
{
    [ApiController]
    public class SearchDataController : ControllerBase
    {
        GetSearchS1DataForCricket sqObj = new GetSearchS1DataForCricket();
         ElasticClient EsClient_obj;
        searchcricket sc = new searchcricket();
        CommonFunction objCf = new CommonFunction();
        //GetMatchDetails matchDetails;
        EsLayer oLayer = new EsLayer();

        //new EsClient_obj =oLayer.CreateConnection();
        //SearchDataController(ElasticClient EsClient) {
        //    EsClient_obj = oLayer.CreateConnection();
        //}

        [System.Web.Http.HttpPost]
        [Route("api/GetSearchResultForFiltersTemp")]
        public IActionResult GetSearchResultsFilterTemp(STFilterRequestData _objReqData)
        {
            try
            {
                Cricket objCS = new Cricket();
                GetMatchDetails objMatchDet = new GetMatchDetails();
                EsClient_obj = oLayer.CreateConnection();
                ExtendedSearchResultFilterData _objSearchResults = new ExtendedSearchResultFilterData();
                ExtendedSearchResultFilterData _objSearchResults2 = new ExtendedSearchResultFilterData();
                ExtendedSearchResultFilterData _objResult = new ExtendedSearchResultFilterData();

                _objResult.ResultData = new List<SearchResultFilterData>();
                _objSearchResults2.ResultData = new List<SearchResultFilterData>();
                _objSearchResults.ResultData = new List<SearchResultFilterData>();
                _objResult.Master = new MasterDatas();
                _objResult.Master.MasterData = new Dictionary<string, object>();
                _objSearchResults.Master = new MasterDatas();
                _objSearchResults.Master.MasterData = new Dictionary<string, object>();
                QueryContainer _objNestedQuery = new QueryContainer();
                if (_objReqData != null)
                {
                    MatchDetail _objMatchDetail = _objReqData.MatchDetail;
                    dynamic _objS1Data = _objReqData.S1Data;
                    MatchSituation _objMatchSituation = _objReqData.MatchSituation;
                    Moments _objMomentsData = _objReqData.Moments;
                    string value = sqObj.GetKeyValueForSport(sc.getType(_objMatchDetail.SportID), "DropdwonKey");
                    List<string> valueObj = sqObj.GetKeyValueForSportTemp(sc.getType(_objMatchDetail.SportID).ToLower(), "PlayerDetails");
                    string SportName = sc.getType(_objMatchDetail.SportID);
                    if (_objS1Data != null)
                    {
                        _objNestedQuery = objMatchDet.getDetailsAsPerSport(_objS1Data, _objNestedQuery, _objMatchDetail, _objMatchSituation, valueObj, _objMatchDetail.SportID);
                        _objSearchResults = objCf.searchStoryTeller(_objMatchDetail, _objNestedQuery, _objS1Data, _objResult.Master.MasterData, _objResult.ResultData, value, SportName.ToLower());

                    }
                    if (_objMomentsData != null)
                    {
                        QueryContainer objMoment = new QueryContainer();
                        objMoment = objCf.getMomentDetailsQueryST(_objMatchDetail, objMoment, _objMomentsData);
                        _objSearchResults2.ResultData = objCS.returnSportResult(EsClient_obj, objMoment, SportName);
                    }
                    _objResult.ResultData = _objSearchResults.ResultData.Union(_objSearchResults2.ResultData);
                    _objResult.Master = _objSearchResults.Master;

                    if (_objMatchDetail.SportID == 1)
                    {
                        string[] _objReqInnings = _objMatchSituation.Innings.Contains(",") ? _objReqInnings = _objMatchSituation.Innings.Split(',') : _objReqInnings = new string[] { _objMatchSituation.Innings };
                        var innings = objCS.fetchDropDownForMatch(_objResult.Master.MasterData, _objReqInnings);
                        //_objResult.Master.MasterData.Add("Innings", innings);
                    }

                }
                return Ok(new { responseText = _objResult });
            }
            catch (Exception ex) {
                return BadRequest();
            }
           

            

        }


        [System.Web.Http.HttpPost]
        [Route("api/getfilterbysport")]
        public IActionResult GetFilteredEntitiesBySport(SearchEntityRequestData _objReqData)
        {
            try
            {
                EsClient_obj = oLayer.CreateConnection();
                int _sportid = 1;

                string result = string.Empty;
                string searchtext = string.Empty;
                searchtext = _objReqData.EntityText.Trim().ToLower();
                List<SearchQueryModel> _objLstSearchQuery = new List<SearchQueryModel>();
                var responseResult = new List<FilteredEntityForCricket>();
                SearchEntityRequestData _objEntityReqData;
                string jsonData = JsonConvert.SerializeObject(_objReqData);
                _objEntityReqData = JsonConvert.DeserializeObject<SearchEntityRequestData>(jsonData);
                //GetMatchDetails obj = new GetMatchDetails();
                MatchDetail _objMatchDetail = _objEntityReqData.MatchDetails;
                _objMatchDetail.SeriesId = _objEntityReqData.EntityTypeId != 5 ? _objMatchDetail.SeriesId : string.Empty;
                _objMatchDetail.MatchId = _objEntityReqData.EntityTypeId != 9 ? _objMatchDetail.MatchId : string.Empty;
                if (_objEntityReqData != null)
                {
                    Dictionary<string, string> _columns = sc.GetColumnForEntity(_objEntityReqData.EntityTypeId);
                    QueryContainer _objNestedQuery = new QueryContainer();
                    QueryContainer _objNestedQueryRresult = new QueryContainer();
                    IEnumerable<FilteredEntityForCricket> _objFilteredEntityForCricket = new List<FilteredEntityForCricket>();

                    // _objNestedQuery = obj.GetMatchDetailQueryST(_objNestedQuery, _objMatchDetail);
                    //_objNestedQuery = obj.GetPlayerDetailQueryForFilteredEntityBySport(_objNestedQuery, _objEntityReqData.playerDetails, _objMatchDetail.SportID);//GetCricketPlayerDetailQuery(_objEntityReqData.playerDetails, _objNestedBoolQuery);
                    if (!string.IsNullOrEmpty(_objMatchDetail.MatchDate))
                    {
                        //_objNestedQuery= obj.GetEntityBySport(_objNestedQuery, _objMatchDetail, _columns, searchtext);
                    }
                    if (_columns.Count > 0)
                    {
                        List<string> EntityIds = new List<string>();
                        List<string> EntityNames = new List<string>();
                        foreach (var col in _columns)
                        {
                            EntityIds.Add(col.Key);
                            EntityNames.Add(col.Value);

                        }
                        if (searchtext != "")
                        {
                            var Result = sc.GetFilteredEntitiesBySportResult(_objNestedQuery, EntityIds.ElementAt(0), EntityNames.ElementAt(0), EsClient_obj, searchtext);
                            responseResult = Result;
                        }
                    }

                }
                return Ok(new { responseText = responseResult });
                //return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }



        }



    

    }
}