using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Nest;
using Newtonsoft.Json;
using WebApis.elastic;
using WebApis.Model;
using static WebApis.Model.ELModels;

namespace WebApis.BOL
{
    public class GetMatchDetails : ISearchDataFilter
    {
        string SportType = "";
        searchcricket sc = new searchcricket();
        GetSearchS1DataForCricket _searchResult = new GetSearchS1DataForCricket();
        ExtendedSearchResultFilterData _objSearchResults = new ExtendedSearchResultFilterData();
        //ExtendedSearchResultFilterData _objSearchResults;
        ExtendedSearchResultFilterData _objSearchResults2 = new ExtendedSearchResultFilterData();
        ExtendedSearchResultFilterData _objResult = new ExtendedSearchResultFilterData();
        List<SearchQueryModel> _objLstSearchQuery;
        SearchEntityRequestData _objEntityReqData;
        MatchDetail _objMatchDetail;
        PlayerDetail _objPlayerDetails;
        Cricket objCS = new Cricket();
        CommonFunction objCF = new CommonFunction();
        MatchSituation _objMatchSituation;
        Moments _objMomentsData;
        QueryContainer _objNestedQuery = new QueryContainer();
        GetSearchS1DataForCricket sqObj = new GetSearchS1DataForCricket();
        EsLayer oLayer = new EsLayer();
        ElasticClient EsClient_obj;
        Dictionary<string, string> _columns;

      
    

        public ExtendedSearchResultFilterData GetSearchResultsFilter(STFilterRequestData _objReqData)
        {
            if (_objReqData != null)
            {
                
                dynamic _objS1Data = _objReqData.S1Data;
                _objResult.ResultData = new List<SearchResultFilterData>();
                _objSearchResults2.ResultData = new List<SearchResultFilterData>();
                _objSearchResults.ResultData = new List<SearchResultFilterData>();
                _objResult.Master = new MasterDatas();
                _objResult.Master.MasterData = new Dictionary<string, object>();
                _objSearchResults.Master = new MasterDatas();
                _objSearchResults.Master.MasterData = new Dictionary<string, object>();
                _objMatchDetail = _objReqData.MatchDetail;
                _objMatchSituation = _objReqData.MatchSituation;
                _objMomentsData = _objReqData.Moments;
                string value = sqObj.GetKeyValueForSport(objCF.getType(_objMatchDetail.SportID), "DropdwonKey");
                List<string> valueObj = sqObj.GetKeyValueForSportTemp(objCF.getType(_objMatchDetail.SportID).ToLower(), "PlayerDetails");
                string SportName = objCF.getType(_objMatchDetail.SportID);
                if (_objS1Data != null)
                {
                    _objNestedQuery = getDetailsAsPerSport(_objS1Data, _objNestedQuery, _objMatchDetail, _objMatchSituation, valueObj, _objMatchDetail.SportID);
                    _objSearchResults = objCF.searchStoryTeller(_objMatchDetail, _objNestedQuery, _objS1Data, _objResult.Master.MasterData, _objResult.ResultData, value, SportName.ToLower());

                }
                if (_objMomentsData != null)
                {
                    QueryContainer objMoment = new QueryContainer();
                    objMoment = objCF.getMomentDetailsQueryST(_objMatchDetail, objMoment, _objMomentsData);
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
            return _objResult;
        }

        public List<FilteredEntityForCricket> GetFilteredEntitiesBySport(SearchEntityRequestData _objReqData)
        {
            var responseResult = new List<FilteredEntityForCricket>();
            string searchtext = string.Empty;
            if (_objReqData != null) {
                EsClient_obj = oLayer.CreateConnection();
                _objLstSearchQuery = new List<SearchQueryModel>();
                searchtext = _objEntityReqData.EntityText.Trim().ToLower();
                string jsonData = JsonConvert.SerializeObject(_objReqData);
                _objEntityReqData = JsonConvert.DeserializeObject<SearchEntityRequestData>(jsonData);
                _objMatchDetail.SeriesId = _objEntityReqData.EntityTypeId != 5 ? _objMatchDetail.SeriesId : string.Empty;
                _objMatchDetail.MatchId = _objEntityReqData.EntityTypeId != 9 ? _objMatchDetail.MatchId : string.Empty;
                if (_objEntityReqData != null)
                {
                    _columns = objCF.GetColumnForEntity(_objEntityReqData.EntityTypeId);
                    IEnumerable<FilteredEntityForCricket> _objFilteredEntityForCricket = new List<FilteredEntityForCricket>();
                    _objNestedQuery = GetMatchDetailQueryST(_objNestedQuery, _objMatchDetail);
                    _objNestedQuery = GetPlayerDetailQueryForFilteredEntityBySport(_objNestedQuery, _objEntityReqData.playerDetails, _objMatchDetail.SportID);//GetCricketPlayerDetailQuery(_objEntityReqData.playerDetails, _objNestedBoolQuery);
                    if (!string.IsNullOrEmpty(_objMatchDetail.MatchDate))
                    {
                        _objNestedQuery = GetEntityBySport(_objNestedQuery, _objMatchDetail, _columns, searchtext);
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

            }
            return responseResult;
        }

        public string GetSavedSearches(SaveSearchesRequestData objSavedSearchData)
        {
            string response = string.Empty;
            string conString = _searchResult.GetConnectionString("ConnectionStrings", "DefaultConnection");
            response = _searchResult.GetSavedSearches(conString, objSavedSearchData);
            return response;
        }

        public string GetSearchResultCount(SearchRequestData  _objReqData)
        {

           
            EsClient_obj = oLayer.CreateConnection();
            Dictionary<string, object> ddlDropdowns = new Dictionary<string, object>();
            SearchCricketExtendedResultData _objSearchResults = new SearchCricketExtendedResultData();
            string response = string.Empty;
            int sportid = 1;
            string languageid = "1";
            _objSearchResults.ResultDerivedData = new CricketDerivedData();
            _objSearchResults.ResultDerivedData.RangeData = new Dictionary<string, long>();
            QueryContainer _objNestedQuery = new QueryContainer();
            QueryContainer _objNestedDropdownQuery = new QueryContainer();
            _objMatchDetail = _objReqData.MatchDetails.FirstOrDefault();
            _objPlayerDetails = _objReqData.PlayerDetails.FirstOrDefault();
            string objDropdown = sqObj.GetKeyValueForSport(objCF.getType(_objMatchDetail.SportID), "S2Dropdowns");
            dynamic _objS2Data = _objPlayerDetails;
            _objSearchResults.ResultCount = new Dictionary<string, Int64>();
            _objSearchResults.ResultDerivedData = new CricketDerivedData();
            _objSearchResults.ResultDerivedData.RangeData = new Dictionary<string, long>();
            MatchSituation _objMatchSituation = _objReqData.MatchSituations.FirstOrDefault();
            List<string> valueObj = sqObj.GetKeyValueForSportTemp(objCF.getType(_objMatchDetail.SportID).ToLower() +"S2" , "PlayerDetails");
            if (_objMatchDetail != null)
            {
                sportid = _objMatchDetail.SportID;
                languageid = _objMatchDetail.LanguageId;
                _objNestedQuery = GetMatchDetailQuery(_objMatchDetail,_objNestedQuery);
                if (_objPlayerDetails != null)
                {
                    

                    //_objNestedQuery = GetPlayerDetails(_objS2Data, _objNestedQuery, valueObj, sportid, "S2");//new

                    _objNestedQuery = GetCricketPlayerDetailQueryS2(_objPlayerDetails, _objNestedQuery);
                    if (_objMatchSituation != null)
                    {
                        _objNestedQuery = GetCricketMatchSituationQuery(_objMatchDetail, _objPlayerDetails, _objMatchSituation, _objNestedQuery);//new
                    }
                }

                if (_objReqData.PlayerDetails != null && _objPlayerDetails.IsDefault)
                {
                   
                    MatchSituation _objMatchSituations = _objReqData.MatchSituations.FirstOrDefault();
                    if (!string.IsNullOrEmpty(_objMatchDetail.MatchDate) || _objMatchSituation != null)
                    {
                        _objNestedQuery = FinalSearchCricketData(_objMatchDetail, _objMatchSituation, _objNestedQuery);
                    }
                    if (!_objMatchDetail.IsAsset && _objMatchDetail.MasterData)
                    {
                        _objSearchResults.ResultDerivedData.MasterData = new Dictionary<string, object>();

                        if (_objSearchResults.ResultData.ToList().Count > 0)
                        {
                            ddlDropdowns = bindS2Dropdown(_objPlayerDetails);

                            if (objDropdown != null)
                            {
                                string[] valuess = objDropdown.Split(",");

                                foreach (var items in valuess)
                                {
                                    var item = items.Split("::");

                                    foreach (KeyValuePair<string, object> entry in ddlDropdowns)
                                    {
                                        if (item.ToString().Split(",")[0] != entry.Key.ToString())
                                        {
                                            QueryContainer query = new TermQuery { Field = entry.Key, Value = entry.Value };
                                            _objNestedDropdownQuery &= query;
                                        }

                                    }

                                    var Type = ddlDropdowns[item.ToString().Split(",")[0]].ToString();
                                    string[] _objType = Type.Contains(",") ? _objType = Type.Split(",") : _objType = new string[] { Type };
                                    _objSearchResults.ResultDerivedData.MasterData = objCS.fetchDropdowns(_objNestedQuery, _objSearchResults.ResultDerivedData.MasterData, EsClient_obj, "cricket", objCF.GetColumnForEntity(Convert.ToInt16(item[1])), _objType);
                                }
                            }



                        }
                    }
                }
                else if (_objReqData.PlayerDetails != null && !_objPlayerDetails.IsDefault)
                {
                    MatchSituation _objMatchSituations = _objReqData.MatchSituations.FirstOrDefault();
                    if (!_objMatchDetail.IsAsset)
                    {
                        _objSearchResults.ResultDerivedData.MasterData = new Dictionary<string, object>();
                        string currentselector = _objPlayerDetails.CurrentSelector;
                        _objNestedQuery = GetMatchDetailQuery(_objMatchDetail, _objNestedQuery, true);//new 
                        _objNestedQuery = GetCricketPlayerDetailQueryS2(_objPlayerDetails, _objNestedQuery, true);
                        _objNestedQuery = GetCricketMatchSituationQuery(_objMatchDetail, _objPlayerDetails, _objMatchSituation, _objNestedQuery);//new


                        ddlDropdowns = bindS2Dropdown(_objPlayerDetails);

                        if (objDropdown != null)
                        {
                            string[] valuess = objDropdown.Split(",");

                            foreach (var items in valuess)
                            {
                                var item = items.Split("::");
                                if (ddlDropdowns.Count > 0)
                                {
                                    foreach (KeyValuePair<string, object> entry in ddlDropdowns)
                                    {
                                        if (item.ToString().Split(",")[0] != entry.Key.ToString())
                                        {
                                            QueryContainer query = new TermQuery { Field = entry.Key, Value = entry.Value };
                                            _objNestedDropdownQuery &= query;
                                        }

                                    }
                                }
                               
                                if (ddlDropdowns.Count > 0)
                                {
                                    var Type = ddlDropdowns[item.ToString().Split(",")[0]].ToString();
                                    string[] _objType = Type.Contains(",") ? _objType = Type.Split(",") : _objType = new string[] { Type };
                                    _objSearchResults.ResultDerivedData.MasterData = objCS.fetchDropdowns(_objNestedQuery, _objSearchResults.ResultDerivedData.MasterData, EsClient_obj, "cricket", objCF.GetColumnForEntity(Convert.ToInt16(item[1])), _objType);

                                }
                                else {
                                    //var Type = ddlDropdowns[item.ToString().Split(",")[0]].ToString();
                                    string[] _objType = new string[] { "0" };
                                    _objSearchResults.ResultDerivedData.MasterData = objCS.fetchDropdowns(_objNestedQuery, _objSearchResults.ResultDerivedData.MasterData, EsClient_obj, "cricket", objCF.GetColumnForEntity(Convert.ToInt16(item[1])), _objType);

                                }
                            }
                        }
                        _objSearchResults.ResultCount.Add("Matches", getMatchCount(_objNestedQuery, EsClient_obj));
                        _objSearchResults.ResultCount.Add("Videos", getIsAssetCount(_objNestedQuery, EsClient_obj));
                        _objSearchResults.ResultCount.Add("Assets", getIsAssetZeroCount(_objNestedQuery, EsClient_obj)); 
                    }
                }
            }
            

            


            return response;
        }


        public Dictionary<string,object> bindS2Dropdown(PlayerDetail _objPlayerDetail) {
            Dictionary<string, object> ddlS2Dropwons = new Dictionary<string, object>();
            string[] _objReqShotTypes = _objPlayerDetail.ShotType.Contains(",") ? _objReqShotTypes = _objPlayerDetail.ShotType.Split(',') : _objReqShotTypes = new string[] { _objPlayerDetail.ShotType };
            string[] _objReqShotZones = _objPlayerDetail.ShotZone.Contains(",") ? _objReqShotZones = _objPlayerDetail.ShotZone.Split(',') : _objReqShotZones = new string[] { _objPlayerDetail.ShotZone };
            string[] _objReqDismissalTypes = _objPlayerDetail.DismissalType.Contains(",") ? _objReqDismissalTypes = _objPlayerDetail.DismissalType.Split(',') : _objReqDismissalTypes = new string[] { _objPlayerDetail.DismissalType };
            string[] _objReqDeliveryType = _objPlayerDetail.DeliveryType.Contains(",") ? _objReqDeliveryType = _objPlayerDetail.DeliveryType.Split(',') : _objReqDeliveryType = new string[] { _objPlayerDetail.DeliveryType };
            string[] _objReqBowlingLength = _objPlayerDetail.BowlingLength.Contains(",") ? _objReqBowlingLength = _objPlayerDetail.BowlingLength.Split(',') : _objReqBowlingLength = new string[] { _objPlayerDetail.BowlingLength };
            string[] _objReqBowlingLine = _objPlayerDetail.BowlingLine.Contains(",") ? _objReqBowlingLine = _objPlayerDetail.BowlingLine.Split(',') : _objReqBowlingLine = new string[] { _objPlayerDetail.BowlingLine };
            string[] _objReqFielderPosition = _objPlayerDetail.FieldingPosition.Contains(",") ? _objReqFielderPosition = _objPlayerDetail.FieldingPosition.Split(',') : _objReqFielderPosition = new string[] { _objPlayerDetail.FieldingPosition };
            string[] _objReqBattingOrder = _objPlayerDetail.BattingOrder.Contains(",") ? _objReqBattingOrder = _objPlayerDetail.BattingOrder.Split(',') : _objReqBattingOrder = new string[] { _objPlayerDetail.BattingOrder };
            string[] _objReqBowlingArm = _objPlayerDetail.BowlingArm.Contains(",") ? _objReqBowlingArm = _objPlayerDetail.BowlingArm.Split(',') : _objReqBowlingArm = new string[] { _objPlayerDetail.BowlingArm };

            if (_objPlayerDetail.ShotZone != "" && _objReqShotZones.Length > 0) {
                ddlS2Dropwons.Add("shotZoneId", _objReqShotZones);
            }
            if (_objPlayerDetail.ShotType != "" && _objReqShotTypes.Length > 0)
            {
                ddlS2Dropwons.Add("shotTypeId", _objReqShotTypes);
            }
            if (_objPlayerDetail.DismissalType != "" && _objReqDismissalTypes.Length > 0)
            {
                ddlS2Dropwons.Add("dismissalTypeId", _objReqDismissalTypes);
            }
            if (_objPlayerDetail.DeliveryType != "" && _objReqDeliveryType.Length > 0)
            {
                ddlS2Dropwons.Add("deliveryTypeId", _objReqDeliveryType);
            }
            if (_objPlayerDetail.BowlingLength != "" && _objReqBowlingLength.Length > 0)
            {
                ddlS2Dropwons.Add("bowlingLengthId", _objReqBowlingLength);
            }
            if (_objPlayerDetail.BowlingLine != "" && _objReqBowlingLine.Length > 0)
            {
                ddlS2Dropwons.Add("bowlingLineId", _objReqBowlingLine);
            }
            if (_objPlayerDetail.FieldingPosition != "" && _objReqFielderPosition.Length > 0)
            {
                ddlS2Dropwons.Add("fieldingPositionId", _objReqFielderPosition);
            }
            if (_objPlayerDetail.BattingOrder != "" && _objReqBattingOrder.Length > 0)
            {
                ddlS2Dropwons.Add("battingOrder", _objReqBattingOrder);
            }
            if (_objPlayerDetail.BowlingArm != "" && _objReqBowlingArm.Length > 0)
            {
                ddlS2Dropwons.Add("bowlingArm", _objReqBowlingArm);
            }
            return ddlS2Dropwons;
        }


        public Dictionary<string, object> bindS1Dropdown(dynamic _objS1Data) {
            Dictionary<string, object> ddlS1Dropwons = new Dictionary<string, object>();
            string ReqShotType = _objS1Data["ShotType"]; string ReqDeliveryType = _objS1Data["DeliveryType"];
            string[] _objReqShotType = ReqShotType.Contains(",") ? _objReqShotType = ReqShotType.Split(",") : _objReqShotType = new string[] { _objS1Data["ShotType"] };
            string[] _objReqDeliveryType = ReqDeliveryType.Contains(",") ? _objReqDeliveryType = ReqDeliveryType.Split(",") : _objReqDeliveryType = new string[] { _objS1Data["DeliveryType"] };

            if (_objS1Data["ShotType"] != "")
            {
                ddlS1Dropwons.Add("shotTypeId", _objReqShotType[0].ToString());
            }
            if (_objS1Data["DeliveryType"] != "")
            {
                ddlS1Dropwons.Add("deliveryTypeId", _objReqDeliveryType[0].ToString());
            }
            return ddlS1Dropwons;

        }



        private QueryContainer FinalSearchCricketData(MatchDetail _objMatchDetail, MatchSituation _objMatchSituation,QueryContainer _objNested)
        {
            QueryContainer qRangeQuery = new QueryContainer();
            if (_objMatchSituation != null)
            {
                if (!string.IsNullOrEmpty(_objMatchSituation.BatsmanRunsRange))
                {
                    string dlist = _objMatchSituation.BatsmanRunsRange;
                    string[] strNumbers = dlist.Split('-');
                    int start = int.Parse(strNumbers[0]);
                    int End = int.Parse(strNumbers[1]);
                    qRangeQuery = new TermRangeQuery { Field = "BatsmanRunsRange", GreaterThanOrEqualTo = start.ToString(), LessThanOrEqualTo = End.ToString() };
                }
                if (!string.IsNullOrEmpty(_objMatchSituation.BatsmanBallsFacedRange))
                {
                    string dlist = _objMatchSituation.BatsmanBallsFacedRange;
                    string[] strNumbers = dlist.Split('-');
                    int start = int.Parse(strNumbers[0]);
                    int End = int.Parse(strNumbers[1]);
                    qRangeQuery = new TermRangeQuery { Field = "BatsmanBallsFacedRange", GreaterThanOrEqualTo = start.ToString(), LessThanOrEqualTo = End.ToString() };
                }
                if (!string.IsNullOrEmpty(_objMatchSituation.BowlerBallsBowledRange))
                {

                    string dlist = _objMatchSituation.BowlerBallsBowledRange;
                    string[] strNumbers = dlist.Split('-');
                    int start = int.Parse(strNumbers[0]);
                    int End = int.Parse(strNumbers[1]);
                    qRangeQuery = new TermRangeQuery { Field = "BowlerBallsBowledRange", GreaterThanOrEqualTo = start.ToString(), LessThanOrEqualTo = End.ToString() };
                }
                if (!string.IsNullOrEmpty(_objMatchSituation.BowlerWicketsRange))
                {

                    string dlist = _objMatchSituation.BowlerWicketsRange;
                    string[] strNumbers = dlist.Split('-');
                    int start = int.Parse(strNumbers[0]);
                    int End = int.Parse(strNumbers[1]);
                    qRangeQuery = new TermRangeQuery { Field = "BowlerWicketsRange", GreaterThanOrEqualTo = start.ToString(), LessThanOrEqualTo = End.ToString() };
                }
                if (!string.IsNullOrEmpty(_objMatchSituation.BowlerRunsRange))
                {

                    string dlist = _objMatchSituation.BowlerRunsRange;
                    string[] strNumbers = dlist.Split('-');
                    int start = int.Parse(strNumbers[0]);
                    int End = int.Parse(strNumbers[1]);
                    qRangeQuery = new TermRangeQuery { Field = "BowlerRunsRange", GreaterThanOrEqualTo = start.ToString(), LessThanOrEqualTo = End.ToString() };
                }
                if (!string.IsNullOrEmpty(_objMatchSituation.TeamOversRange))
                {
                    string dlist = _objMatchSituation.TeamOversRange;
                    string[] strNumbers = dlist.Split('-');
                    int start = int.Parse(strNumbers[0]);
                    int End = int.Parse(strNumbers[1]);
                    qRangeQuery = new TermRangeQuery { Field = "TeamOversRange", GreaterThanOrEqualTo = start.ToString(), LessThanOrEqualTo = End.ToString() };
                }
                if (!string.IsNullOrEmpty(_objMatchSituation.TeamScoreRange))
                {

                    string dlist = _objMatchSituation.TeamScoreRange;
                    string[] strNumbers = dlist.Split('-');
                    int start = int.Parse(strNumbers[0]);
                    int End = int.Parse(strNumbers[1]);
                    qRangeQuery = new TermRangeQuery { Field = "TeamScoreRange", GreaterThanOrEqualTo = start.ToString(), LessThanOrEqualTo = End.ToString() };
                }
            }
            if (_objMatchDetail != null)
            {

                if (!string.IsNullOrEmpty(_objMatchDetail.MatchDate))
                {
                    string dlist = _objMatchDetail.MatchDate;
                    if (dlist.Contains("-"))
                    {
                        string[] strNumbers = dlist.Split('-');
                        int start = int.Parse(DateTime.ParseExact(strNumbers[0], "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd"));
                        int End = int.Parse(DateTime.ParseExact(strNumbers[1], "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd"));
                        qRangeQuery = new TermRangeQuery { Field = "MatchDate", GreaterThanOrEqualTo = start.ToString(), LessThanOrEqualTo = End.ToString() };
                    }
                    else
                    {
                        int date = int.Parse(DateTime.ParseExact(_objMatchDetail.MatchDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd"));
                        qRangeQuery = new TermRangeQuery { Field = "MatchDate", Name = date.ToString() };
                    }
                }

            }
            _objNested &= qRangeQuery;
            return _objNested;
        }
        public QueryContainer GetMatchDetailQueryST(QueryContainer _objNestedQuery, MatchDetail _objMatchDetail)
        {
            searchcricket sc = new searchcricket();
            SportType = sc.getType(_objMatchDetail.SportID);
            if ("CRICKET" == SportType)
            {
                if (_objMatchDetail != null)
                {
                    QueryContainer queryShould = new QueryContainer();
                    if (!string.IsNullOrEmpty(_objMatchDetail.MatchFormat))
                    {
                        QueryContainer q1 = new TermQuery { Field = "compType", Value = _objMatchDetail.MatchFormat.ToLower() };
                        
                        _objNestedQuery &= q1;
                    }
                    if (!string.IsNullOrEmpty(_objMatchDetail.VenueId))
                    {
                        QueryContainer q2 = new TermQuery { Field = "venueId", Value = _objMatchDetail.MatchFormat.ToLower().ToString() };
                        _objNestedQuery &= q2;
                    }

                    if (!string.IsNullOrEmpty(_objMatchDetail.Team1Id))
                    {
                        var myQuery = new BoolQuery();
                        QueryContainer q3 = new TermQuery { Field = "team1Id", Value = _objMatchDetail.Team1Id.ToLower().ToString() };
                        QueryContainer q4 = new TermQuery { Field = "team2Id", Value = _objMatchDetail.Team1Id.ToLower().ToString() };
                        queryShould |= q3 |= q4;
                        _objNestedQuery &= queryShould;
                    }
                    if (!string.IsNullOrEmpty(_objMatchDetail.Team2Id))
                    {

                        QueryContainer q5 = new TermQuery { Field = "team1Id", Value = _objMatchDetail.Team2Id.ToLower().ToString() };
                        QueryContainer q6 = new TermQuery { Field = "team2Id", Value = _objMatchDetail.Team2Id.ToLower().ToString() };
                        queryShould |= q5 |= q6;
                        _objNestedQuery &= queryShould;
                    }

                    if (!string.IsNullOrEmpty(_objMatchDetail.SeriesId) && _objMatchDetail.IsParentSeries)
                    {
                        QueryContainer q7 = new TermQuery { Field = "parentSeriesId", Value = _objMatchDetail.SeriesId };
                        _objNestedQuery &= q7;

                    }
                    if (!string.IsNullOrEmpty(_objMatchDetail.SeriesId) && !_objMatchDetail.IsParentSeries)
                    {
                        QueryContainer q8 = new TermQuery { Field = "seriesId", Value = _objMatchDetail.SeriesId };
                        _objNestedQuery &= q8;
                    }
                    if (!string.IsNullOrEmpty(_objMatchDetail.MatchId))
                    {
                        QueryContainer q9 = new TermQuery { Field = "matchId", Value = _objMatchDetail.MatchId };
                        _objNestedQuery &= q9;
                    }
                    if (!string.IsNullOrEmpty(_objMatchDetail.ClearId))
                    {
                        var myQuery = new BoolQuery();
                        QueryContainer q10 = new TermQuery { Field = "qClearId", Value = Convert.ToString(_objMatchDetail.ClearId).Replace("-", string.Empty).ToLower() };
                        QueryContainer q11 = new TermQuery { Field = "qClearId2", Value = Convert.ToString(_objMatchDetail.ClearId).Replace("-", string.Empty).ToLower() };
                        QueryContainer q12 = new TermQuery { Field = "qClearId3", Value = Convert.ToString(_objMatchDetail.ClearId).Replace("-", string.Empty).ToLower() };
                        QueryContainer q13 = new TermQuery { Field = "qClearId4", Value = _objMatchDetail.ClearId.Replace("-", string.Empty).ToLower() };
                        QueryContainer q14 = new TermQuery { Field = "qClearId5", Value = _objMatchDetail.ClearId.Replace("-", string.Empty).ToLower() };
                        QueryContainer q15 = new TermQuery { Field = "qClearId6", Value = _objMatchDetail.ClearId.Replace("-", string.Empty).ToLower() };
                        queryShould |= q10 |= q11 |= q12 |= q13 |= q14 |= q15;
                        _objNestedQuery &= queryShould;
                        
                    }
                    if (!string.IsNullOrEmpty(_objMatchDetail.MatchStageId))
                    {

                        var myQuery = new BoolQuery();
                        string Dlist = Convert.ToString(_objMatchDetail.MatchStageId);
                        string[] strnumbers = Dlist.Split(',');
                        foreach (string str in strnumbers)
                        {
                            QueryContainer q16 = new TermQuery { Field = "matchStageId", Value = str };
                            queryShould |= q16;

                        }
                        _objNestedQuery &= queryShould;
                        
                    }
                    if (!string.IsNullOrEmpty(_objMatchDetail.CompTypeId))
                    {
                        var myQuery = new BoolQuery();
                        string Dlist = Convert.ToString(_objMatchDetail.CompTypeId);
                        string[] strnumbers = Dlist.Split(',');
                        foreach (string str in strnumbers)
                        {
                            QueryContainer q17 = new TermQuery { Field = "CompTypeId", Value = str };
                             queryShould |= q17;
                        }
                        _objNestedQuery &= queryShould;
                      
                    }


                    if (_objMatchDetail.HasShortClip)
                    {
                        QueryContainer q21 = new TermQuery { Field = "hasShortClip", Value = "1" };
                        _objNestedQuery &= q21;
                    }

                    if (!_objMatchDetail.IsAssetSearch)
                    {
                        QueryContainer q20 = new TermQuery { Field = "isTagged", Value = "1" };
                        _objNestedQuery &= q20;

                    }

                    if (!string.IsNullOrEmpty(_objMatchDetail.LanguageId) && _objMatchDetail.LanguageId != "0")
                    {
                        QueryContainer q19 = new TermQuery { Field = "languageId", Value = _objMatchDetail.LanguageId };
                        _objNestedQuery &= q19;

                    }
                    string input = Convert.ToInt32(Convert.ToBoolean(_objMatchDetail.IsAsset)).ToString();
                    QueryContainer q18 = new TermQuery { Field = "isAsset", Value = input };
                    _objNestedQuery &= q18;

                }
            }
                return _objNestedQuery;
         }
        public QueryContainer GetPlayerDetails(dynamic _objS1Data, QueryContainer qFinal, List<string> valueObj, int sportid, string _sType, bool isMasterData = false)
        {
            CommonFunction objCf = new CommonFunction();
            QueryContainer queryShouldS = new QueryContainer();
            QueryContainer queryShould = new QueryContainer();
            QueryContainer queryShouldB = new QueryContainer();
            QueryContainer queryAnd_should = new QueryContainer();
            if (_objS1Data != null)
                {
                    if (_objS1Data["IsDefault"] != null && Convert.ToBoolean(_objS1Data["IsDefault"]))
                    {
                        //QueryContainer query = new QueryContainer();
                    string[] isDefaultvalues = objCf.isGetDefaultType(sportid);
                    for (int i = 0; i <= isDefaultvalues.Length - 1; i++) {
                        if (isDefaultvalues[i] != "shotTypeId" && isDefaultvalues[i] != "deliveryTypeId") {
                            QueryContainer query1 = new TermQuery { Field = isDefaultvalues[i], Value = 1 };
                            queryShouldB |= query1;
                        }
                        
                    }
                    //QueryContainer q1 = new TermQuery { Field = "isFour", Value = "1" };
                    //QueryContainer q2 = new TermQuery { Field = "isSix", Value = "1" };
                    //QueryContainer q3 = new TermQuery { Field = "isWicket", Value = "1" };
                    //QueryContainer q5 = new TermQuery { Field = "isAppeal", Value = "1" };
                    //QueryContainer q6 = new TermQuery { Field = "isDropped", Value = "1" };
                    //QueryContainer q7 = new TermQuery { Field = "isMisField", Value = "1" };
                    //query = q1 |= q2 |= q3 |= q5 |= q6 |= q7;
                    
                    qFinal &= queryShould;
                        //qAdd.Must query;
                    }
                    else
                    {
                        for (int i = 0; i <= valueObj.Count - 1; i++)
                        {
                            string sType = valueObj[i].Split(",")[1];
                            if (sType == "Boolean")
                            {
                                if (Convert.ToBoolean(_objS1Data[valueObj[i].Split(":")[1].Split(",")[0]]))
                                {
                                    QueryContainer query1 = new TermQuery { Field = valueObj[i].Split(",")[2], Value = "1" };
                                    queryShouldB |= query1;
                                }
                            //qFinal &= queryShould;
                            }
                            if (sType == "string")
                            {
                                if (Convert.ToString(_objS1Data[valueObj[i].Split(":")[1]]) != "")
                                {
                                    string slist = Convert.ToString(_objS1Data[valueObj[i].Split(",")[0].Split(":")[1]]);
                                    if (slist.Contains(","))
                                    {
                                    if (_sType == "S2" && !isMasterData)
                                    {
                                        string[] strArray = slist.Split(',');
                                        foreach (string str in strArray)
                                        {
                                            QueryContainer query9 = new TermQuery { Field = valueObj[i].Split(",")[2], Value = str };
                                            queryShouldS |= query9;
                                        }
                                    }
                                    else {
                                        string[] strArray = slist.Split(',');
                                        foreach (string str in strArray)
                                        {
                                            QueryContainer query9 = new TermQuery { Field = valueObj[i].Split(",")[2], Value = str };
                                            queryShouldS |= query9;
                                        }
                                    }

                                    
                                    //qFinal &= queryShould;
                                }
                                    else
                                    {
                                    if (_sType != "S2")
                                    {
                                        if (Convert.ToString(_objS1Data[valueObj[i].Split(",")[0].Split(":")[1]]) != "")
                                        {
                                            QueryContainer query10 = new TermQuery { Field = valueObj[i].Split(",")[2], Value = Convert.ToString(_objS1Data[valueObj[i].Split(",")[0].Split(":")[1]]) };
                                            qFinal &= query10;
                                        }
                                    }
                                    else {
                                        string sEntityName = _objS1Data[valueObj[i].Split(",")[0].Split(":")[1]];
                                        if (sEntityName == "RunsSaved" && Convert.ToString(_objS1Data[valueObj[i].Split(",")[0].Split(":")[1]]) != 0) {
                                            QueryContainer query10 = new TermQuery { Field = valueObj[i].Split(",")[2], Value = Convert.ToString(_objS1Data[valueObj[i].Split(",")[0].Split(":")[1]]) };
                                            qFinal &= query10;
                                        }
                                    }
                                    
                                    }
                                }

                            }
                        //if (valueObj[i].Split(":")[0] == "OR")
                        //{
                        //    queryAnd_should &= queryShould;
                        //}
                       

                    }
                    }


                }
          
           // if (queryShouldB != null)
            //{
                qFinal &= queryShouldB;
            //}
            //else {
            return qFinal;
            //}

            
        }
        public QueryContainer GetCricketMatchSituationQueryST(QueryContainer _objNestedQuery, MatchSituation _objMatchSituation)
        {
            if (_objMatchSituation != null)
            {
                if (!string.IsNullOrEmpty(_objMatchSituation.Innings))
                {
                    List<QueryContainer> myDynamicTermQuery = new List<QueryContainer>();
                    QueryContainer q1 = new TermQuery { Field = "innings", Value = Convert.ToString(_objMatchSituation.Innings) };
                    _objNestedQuery &= q1;


                }

            }
            return _objNestedQuery;
        }

        private static QueryContainer GetCricketMatchSituationQuery(MatchDetail _objMatchDetail, PlayerDetail _objPlayerDetail, MatchSituation _objMatchSituation, QueryContainer _objNestedQuery)
        {
            QueryContainer qcShould = new QueryContainer();
            QueryContainer qcAnd = new QueryContainer();

            if (_objMatchSituation != null)
            {
                if (!string.IsNullOrEmpty(_objMatchSituation.Result))
                {
                    SearchQueryModel _objSqModel = new SearchQueryModel();
                    if (_objMatchSituation.Result.Contains("Wins"))
                    {
                        if (!string.IsNullOrEmpty(_objMatchDetail.Team1Id) || !string.IsNullOrEmpty(_objMatchDetail.Team2Id))
                        {
                            
                            if (!string.IsNullOrEmpty(_objMatchDetail.Team1Id))
                            {
                                QueryContainer q1 = new TermQuery { Field = "winner", Value = _objMatchDetail.Team1Id };
                                qcAnd &= q1;
                            }
                            else if (!string.IsNullOrEmpty(_objMatchDetail.Team2Id))
                            {
                                QueryContainer q2 = new TermQuery { Field = "winner", Value = _objMatchDetail.Team2Id };
                                qcAnd &= q2;
                            }

                        }
                        if (!string.IsNullOrEmpty(_objPlayerDetail.BatsmanID))
                        {
                        
                            QueryContainer q3 = new TermQuery { Field = "winnerPlayer", Value = _objPlayerDetail.BatsmanID };
                            qcAnd &= q3;
                        }
                        if (!string.IsNullOrEmpty(_objPlayerDetail.BowlerID))
                        {
                           
                            QueryContainer q4 = new TermQuery { Field = "winnerPlayer", Value = _objPlayerDetail.BowlerID };
                            qcAnd &= q4;
                        }
                        if (!string.IsNullOrEmpty(_objPlayerDetail.FielderID))
                        {
                            QueryContainer q5 = new TermQuery { Field = "winningFielder", Value = _objPlayerDetail.FielderID };
                            qcAnd &= q5;
                        }
                    }
                    else if (_objMatchSituation.Result.Contains("Losses"))
                    {
                        if (!string.IsNullOrEmpty(_objMatchDetail.Team1Id) || !string.IsNullOrEmpty(_objMatchDetail.Team2Id))
                        {

                            if (!string.IsNullOrEmpty(_objMatchDetail.Team1Id))
                            {
                                QueryContainer q6 = new TermQuery { Field = "loser", Value = _objMatchDetail.Team1Id };
                                qcAnd &= q6;

                            }
                            else if (!string.IsNullOrEmpty(_objMatchDetail.Team2Id))
                            {
                                QueryContainer q7 = new TermQuery { Field = "loser", Value = _objMatchDetail.Team2Id };
                                qcAnd &= q7;
                            }
                        }
                        if (!string.IsNullOrEmpty(_objPlayerDetail.BatsmanID))
                        {

                            QueryContainer q8 = new TermQuery { Field = "loserPlayer", Value = _objPlayerDetail.BatsmanID };
                            qcAnd &= q8;
                        }
                        if (!string.IsNullOrEmpty(_objPlayerDetail.BowlerID))
                        {
                            QueryContainer q9 = new TermQuery { Field = "loserPlayer", Value = _objPlayerDetail.BowlerID };
                            qcAnd &= q9;
                        }
                        if (!string.IsNullOrEmpty(_objPlayerDetail.FielderID))
                        {
                            QueryContainer q10 = new TermQuery { Field = "winningFielder", Value = _objPlayerDetail.FielderID };
                            qcAnd &= q10;
                        }
                    }
                    else
                    {

                        string matchresult = string.Empty;
                        if (_objMatchSituation.Result.Contains("No Result"))
                        {
                            matchresult = "No Result";
                            QueryContainer q11 = new TermQuery { Field = "matchResult", Value = matchresult.ToLower() };
                            qcAnd &= q11;
                        }
                        else if (_objMatchSituation.Result.Contains("Draw"))
                        {
                            matchresult = "Drawn";
                            QueryContainer q12 = new TermQuery { Field = "matchResult", Value = matchresult.ToLower() };
                            qcAnd &= q12;
                        }
                        else if (_objMatchSituation.Result.Contains("Tie"))
                        {
                            matchresult = "Tied";
                            QueryContainer q13 = new TermQuery { Field = "matchResult", Value = matchresult.ToLower() };
                            qcAnd &= q13;
                        }

                    }
                }
                if (!string.IsNullOrEmpty(_objMatchSituation.Innings))
                {
                    QueryContainer q14 = new TermQuery { Field = "innings", Value = _objMatchSituation.Innings };
                    qcAnd &= q14;
                }

                if (!string.IsNullOrEmpty(_objMatchSituation.MatchInstance))
                {

                    if (_objMatchSituation.MatchInstance.Contains("Powerplay"))
                    {

                        QueryContainer q15 = new TermQuery { Field = "isPowerPlayOver", Value =1 };
                        qcAnd &= q15;
                    }
                    else if (_objMatchSituation.MatchInstance.Contains("Middle"))
                    {
                        QueryContainer q16 = new TermQuery { Field = "is_middle", Value = 1};
                        qcAnd &= q16;
                    }
                    else if (_objMatchSituation.MatchInstance.Contains("Death"))
                    {
                        QueryContainer q17 = new TermQuery { Field = "is_death", Value = 1 };
                        qcAnd &= q17;
                    }
                    else if (_objMatchSituation.MatchInstance.Contains("Final Over"))
                    {
                        QueryContainer q18 = new TermQuery { Field = "is_lastover", Value =1 };
                        qcAnd &= q18;
                    }
                    else if (_objMatchSituation.MatchInstance.Contains("Last Ball"))
                    {
                        QueryContainer q19 = new TermQuery { Field = "is_lastBall", Value = 1 };
                        qcAnd &= q19;
                    }

                }
            }



            return _objNestedQuery;
        }


        public QueryContainer getDetailsAsPerSport(dynamic _objS1Data, QueryContainer _objNestedQuery, MatchDetail _ObjMatchDetails, MatchSituation _objMatchSituation, List<string> valueObj, int sportid)
        {
            //QueryContainer _objMatchResult = new QueryContainer();
            if (_ObjMatchDetails != null)
            {
                try
                {
                    _objNestedQuery = GetMatchDetailQueryST(_objNestedQuery, _ObjMatchDetails);
                }
                catch (Exception ex)
                {
                }

            }
            _objNestedQuery = GetPlayerDetails(_objS1Data, _objNestedQuery, valueObj, sportid, "S1");
            if (sportid == 1)// Only For Cricket
            {
                _objNestedQuery = GetCricketMatchSituationQueryST(_objNestedQuery, _objMatchSituation);
            }
            return _objNestedQuery;
        }
        public QueryContainer GetPlayerDetailQueryForFilteredEntityBySport(QueryContainer _objNestedQuery, dynamic _objS1Data, int SportsId = 1)
        {
            QueryContainer qShould = new QueryContainer();
            if (Convert.ToString(_objS1Data["BatsmanID"]) != "")
            {
                QueryContainer q1 = new TermQuery { Field = "batsmanId", Value = _objS1Data["BatsmanID"] };

                _objNestedQuery &= q1;
            }
            if (Convert.ToString(_objS1Data["BowlerID"]) != null)
            {
                QueryContainer q2 = new TermQuery { Field = "bowlerId", Value = _objS1Data["BowlerID"] };
                _objNestedQuery &= q2;

            }
            if (Convert.ToString(_objS1Data["FielderID"]) != null)
            {
                QueryContainer q3 = new TermQuery { Field = "fielderId", Value = _objS1Data["fielderId"] };
                _objNestedQuery &= q3;

            }
            return _objNestedQuery;

        }
        public QueryContainer GetFilteredEntitiesBySport(MatchDetail _objReqData, QueryContainer _objNestedQuery, string sCase, int sDate, Dictionary<string, string> _columns, string searchText)
        {
            dynamic Result = null;
            string input = Convert.ToInt32(Convert.ToBoolean(_objReqData.IsAsset)).ToString();
            List<string> EntityId = new List<string>();
            List<string> EntityName = new List<string>();
            foreach (var col in _columns)
            {
                EntityId.Add(col.Key);
                EntityName.Add(col.Value);

            }
            var terms = searchText.Trim().Replace("-", " ").Split(' ').Where(x => !string.IsNullOrEmpty(x)).Select(x => x.Trim() + "*");
            searchText = string.Join(" ", terms).ToLower();

            if (searchText != "")
            {
                QueryContainer query = new QueryContainer();
                if (EntityName.ElementAt(0) != "CompType" && EntityName.ElementAt(0) != "MatchStage")
                {
                    for (int i = 0; i < _columns.Count; i++)
                    {
                        QueryContainer q2 = new TermQuery { Field = EntityName.ElementAt(i), Value = searchText };
                        query |= q2;
                    }
                    _objNestedQuery &= query;
                    //Bq.Add(bq, Occur.MUST);
                }
                QueryContainer q3 = new TermQuery { Field = "isAsset", Value = searchText };
                _objNestedQuery &= q3;
                if (sCase == "2")
                {
                    QueryContainer q4 = new TermQuery { Field = "matchDate", Value = sDate };
                    _objNestedQuery &= q4;
                }
            }
            return _objNestedQuery;
        }
        public QueryContainer GetEntityBySport(QueryContainer _objNestedQuery, MatchDetail _objMatchDetail, Dictionary<string,string> _columns, string searchtext) {
            string dlist = _objMatchDetail.MatchDate;
            if (dlist.Contains("-"))
            {
                string[] strNumbers = dlist.Split('-');
                int start = int.Parse(DateTime.ParseExact(strNumbers[0], "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd"));
                int End = int.Parse(DateTime.ParseExact(strNumbers[1], "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd"));
                _objNestedQuery = GetFilteredEntitiesBySport(_objMatchDetail, _objNestedQuery, "1", start, _columns, searchtext);
            }
            else
            {
                int date = int.Parse(DateTime.ParseExact(_objMatchDetail.MatchDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd"));
                _objNestedQuery = GetFilteredEntitiesBySport(_objMatchDetail, _objNestedQuery, "2", date, _columns, searchtext);
            }
            return _objNestedQuery;

        }
        private QueryContainer GetMatchDetailQuery(MatchDetail _objMatchDetail, QueryContainer _objNestedQuery, bool isMasterData = false)
        {
            if (_objMatchDetail != null)
            {
                QueryContainer queryShould = new QueryContainer();

                if (!string.IsNullOrEmpty(_objMatchDetail.MatchFormat))
                {
                    if (_objMatchDetail.MatchFormat.Contains(","))
                    {
                        string[] _objaaray = _objMatchDetail.MatchFormat.Split(',');
                        foreach (var v in _objaaray)
                        {
                            QueryContainer q1 = new TermQuery { Field = "compType", Value = v.ToLower() };
                            queryShould |= q1;
                        }
                        //_objNestedQuery.Add(bq, Occur.MUST);
                    }
                    else {
                        QueryContainer q1 = new TermQuery { Field = "compType", Value = _objMatchDetail.MatchFormat.ToLower() };
                        _objNestedQuery &= q1;
                    }
                }
                if (!string.IsNullOrEmpty(_objMatchDetail.VenueId))
                {
                    QueryContainer q2 = new TermQuery { Field = "venueId", Value = _objMatchDetail.MatchFormat.ToLower().ToString() };
                    _objNestedQuery &= q2;
                }

                if (!string.IsNullOrEmpty(_objMatchDetail.Team1Id))
                {
                    QueryContainer q3 = new TermQuery { Field = "team1Id", Value = _objMatchDetail.Team1Id.ToLower().ToString() };
                    queryShould |= q3;
                    _objNestedQuery &= queryShould;
                }
                if (!string.IsNullOrEmpty(_objMatchDetail.Team2Id))
                {
                    QueryContainer q4 = new TermQuery { Field = "team2Id", Value = _objMatchDetail.Team1Id.ToLower().ToString() };
                    queryShould |= q4;
                    _objNestedQuery &= queryShould;
                }
                if (!string.IsNullOrEmpty(_objMatchDetail.Team2Id))
                {

                    QueryContainer q5 = new TermQuery { Field = "team1Id", Value = _objMatchDetail.Team2Id.ToLower().ToString() };
                    QueryContainer q6 = new TermQuery { Field = "team2Id", Value = _objMatchDetail.Team2Id.ToLower().ToString() };
                    queryShould |= q5 |= q6;
                    _objNestedQuery &= queryShould;
                }

                if (!string.IsNullOrEmpty(_objMatchDetail.SeriesId) && _objMatchDetail.IsParentSeries)
                {
                    string Dlist = _objMatchDetail.SeriesId;
                    string[] strnumbers = Dlist.Split(',');
                    foreach (string str in strnumbers)
                    {
                        QueryContainer q7 = new TermQuery { Field = "parentSeriesId", Value = str };
                        queryShould  |= q7;
                    }
                    // QueryContainer q7 = new TermQuery { Field = "parentSeriesId", Value = _objMatchDetail.SeriesId };
                    //_objNestedQuery &= q7;

                }
                if (!string.IsNullOrEmpty(_objMatchDetail.SeriesId) && !_objMatchDetail.IsParentSeries)
                {
                  

                    string Dlist = _objMatchDetail.SeriesId;
                    string[] strnumbers = Dlist.Split(',');
                    foreach (string str in strnumbers)
                    {
                       
                        QueryContainer q8 = new TermQuery { Field = "seriesId", Value = str };
                        queryShould |= q8;
                    }


                }
                if (!string.IsNullOrEmpty(_objMatchDetail.MatchId))
                {
                   
                    string Dlist = _objMatchDetail.MatchId;
                    string[] strnumbers = Dlist.Split(',');
                    foreach (string str in strnumbers)
                    {
                        QueryContainer q9 = new TermQuery { Field = "matchId", Value = str };
                        queryShould |= q9;
                    }
                }
                if (!string.IsNullOrEmpty(_objMatchDetail.ClearId))
                {
                    var myQuery = new BoolQuery();
                    QueryContainer q10 = new TermQuery { Field = "qClearId", Value = Convert.ToString(_objMatchDetail.ClearId).Replace("-", string.Empty).ToLower() };
                    QueryContainer q11 = new TermQuery { Field = "qClearId2", Value = Convert.ToString(_objMatchDetail.ClearId).Replace("-", string.Empty).ToLower() };
                    QueryContainer q12 = new TermQuery { Field = "qClearId3", Value = Convert.ToString(_objMatchDetail.ClearId).Replace("-", string.Empty).ToLower() };
                    QueryContainer q13 = new TermQuery { Field = "qClearId4", Value = _objMatchDetail.ClearId.Replace("-", string.Empty).ToLower() };
                    QueryContainer q14 = new TermQuery { Field = "qClearId5", Value = _objMatchDetail.ClearId.Replace("-", string.Empty).ToLower() };
                    QueryContainer q15 = new TermQuery { Field = "qClearId6", Value = _objMatchDetail.ClearId.Replace("-", string.Empty).ToLower() };
                    queryShould |= q10 |= q11 |= q12 |= q13 |= q14 |= q15;
                    _objNestedQuery &= queryShould;

                }
                if (!string.IsNullOrEmpty(_objMatchDetail.MatchStageId))
                {

                    var myQuery = new BoolQuery();
                    string Dlist = Convert.ToString(_objMatchDetail.MatchStageId);
                    string[] strnumbers = Dlist.Split(',');
                    foreach (string str in strnumbers)
                    {
                        QueryContainer q16 = new TermQuery { Field = "matchStageId", Value = str };
                        queryShould |= q16;

                    }
                    _objNestedQuery &= queryShould;

                }
                if (!string.IsNullOrEmpty(_objMatchDetail.CompTypeId))
                {
                    var myQuery = new BoolQuery();
                    string Dlist = Convert.ToString(_objMatchDetail.CompTypeId);
                    string[] strnumbers = Dlist.Split(',');
                    foreach (string str in strnumbers)
                    {
                        QueryContainer q17 = new TermQuery { Field = "CompTypeId", Value = str };
                        queryShould |= q17;
                    }
                    _objNestedQuery &= queryShould;

                }

                if (!string.IsNullOrEmpty(_objMatchDetail.GameTypeId))
                {
                    var myQuery = new BoolQuery();
                    string Dlist = Convert.ToString(_objMatchDetail.GameTypeId);
                    string[] strnumbers = Dlist.Split(',');
                    foreach (string str in strnumbers)
                    {
                        QueryContainer q23 = new TermQuery { Field = "gameTypeId", Value = str };
                        queryShould |= q23;
                    }
                   // _objNestedQuery &= queryShould;

                }



                if (_objMatchDetail.HasShortClip)
                {
                    QueryContainer q21 = new TermQuery { Field = "hasShortClip", Value = "1" };
                    _objNestedQuery &= q21;
                }

                if (!_objMatchDetail.IsAssetSearch)
                {
                    QueryContainer q20 = new TermQuery { Field = "isTagged", Value = "1" };
                    _objNestedQuery &= q20;


                }

                //if (!string.IsNullOrEmpty(_objMatchDetail.LanguageId) && _objMatchDetail.LanguageId != "0")
                //{
                //    QueryContainer q19 = new TermQuery { Field = "languageId", Value = _objMatchDetail.LanguageId };
                //    _objNestedQuery &= q19;

                //}


                if (!string.IsNullOrEmpty(_objMatchDetail.LanguageId) && _objMatchDetail.LanguageId != "0" && _objMatchDetail.IsAsset)
                {
                    string[] Id = _objMatchDetail.LanguageId.Split(',');

                    foreach (string str in Id)
                    {

                        QueryContainer q19 = new TermQuery { Field = "languageId", Value = str };
                        queryShould |= q19;
                    }
                    //_objNestedQuery.Add(bq, Occur.MUST);
                }




                string input = Convert.ToInt32(Convert.ToBoolean(_objMatchDetail.IsAsset)).ToString();
                QueryContainer q18 = new TermQuery { Field = "isAsset", Value = input };
                _objNestedQuery &= q18;
                _objNestedQuery &= queryShould;

            }

            return _objNestedQuery;
        }
        private QueryContainer GetCricketPlayerDetailQueryS2(PlayerDetail _objPlayerDetail, QueryContainer _objNestedQuery, bool isMasterData = false)
        {
            QueryContainer qShould = new QueryContainer();
            if (_objPlayerDetail != null)
            {
                
                if (!string.IsNullOrEmpty(_objPlayerDetail.BatsmanID))
                {
                    QueryContainer q1 = new TermQuery { Field = "batsmanId", Value = _objPlayerDetail.BatsmanID };
                    _objNestedQuery &= q1;
                    //_objNestedQuery.Add(query, Occur.MUST);
                }
                if (_objPlayerDetail.BatsmanFours || _objPlayerDetail.BatsmanSixes || _objPlayerDetail.BatsmanDots || _objPlayerDetail.BatsmanEdged || _objPlayerDetail.BastsmanBeaten || _objPlayerDetail.BatsmanDismissal || _objPlayerDetail.BatsmanAppeal)
                {
                   
                    if (_objPlayerDetail.BatsmanFours)
                    {
                        QueryContainer q2 = new TermQuery { Field = "isFour", Value = 1 };
                        qShould |= q2;
                      
                    }
                    if (_objPlayerDetail.BatsmanSixes)
                    {
                        QueryContainer q3 = new TermQuery { Field = "isSix", Value = 1 };
                        qShould |= q3;
                    }
                    if (_objPlayerDetail.BatsmanDots)
                    {
                        
                        QueryContainer q4 = new TermQuery { Field = "isDot", Value = 1 };
                        qShould |= q4;
                    }
                    if (_objPlayerDetail.BatsmanEdged)
                    {
                     

                        QueryContainer q5 = new TermQuery { Field = "isEdged", Value = 1 };
                        qShould |= q5;
                    }
                    if (_objPlayerDetail.BastsmanBeaten)
                    {
                        
                        QueryContainer q6 = new TermQuery { Field = "isBeaten", Value = 1 };
                        qShould |= q6;

                    }
                    if (_objPlayerDetail.BatsmanDismissal)
                    {
                        QueryContainer q7 = new TermQuery { Field = "isWicket", Value = 1 };
                        qShould |= q7;
                    }
                    if (_objPlayerDetail.BatsmanAppeal)
                    {
                        QueryContainer q8 = new TermQuery { Field = "isAppeal", Value = 1 };
                        qShould |= q8;
                     
                    }
                    //_objNestedQuery.Add(Bq, Occur.MUST);
                }
                if (!string.IsNullOrEmpty(_objPlayerDetail.ShotType) && !isMasterData)
                {
                    
                    string Dlist = _objPlayerDetail.ShotType;
                    string[] strnumbers = Dlist.Split(',');
                    foreach (string str in strnumbers)
                    {
                      
                        QueryContainer q9 = new TermQuery { Field = "shotTypeId", Value = str };
                        qShould |= q9;
                    }
                   
                }
                if (!string.IsNullOrEmpty(_objPlayerDetail.ShotZone) && !isMasterData)
                {
                    //BooleanQuery bq = new BooleanQuery();
                    string Dlist = _objPlayerDetail.ShotZone;
                    string[] strnumbers = Dlist.Split(',');
                    foreach (string str in strnumbers)
                    {
                        QueryContainer q10 = new TermQuery { Field = "shotZoneId", Value = str };
                        qShould |= q10;
                    }
                    //_objNestedQuery.Add(bq, Occur.MUST);
                }
                if (!string.IsNullOrEmpty(_objPlayerDetail.DismissalType) && !isMasterData)
                {
                    
                    string Dlist = _objPlayerDetail.DismissalType;
                    string[] strnumbers = Dlist.Split(',');
                    foreach (string str in strnumbers)
                    {
                      
                        QueryContainer q11 = new TermQuery { Field = "dismissalId", Value = str };
                        qShould |= q11;
                    }
                    
                }
                if (_objPlayerDetail.FielderStumping)
                {
                   
                    QueryContainer q12 = new TermQuery { Field = "dismissalId", Value = "st" };
                    _objNestedQuery &= q12;
                }
                if (!string.IsNullOrEmpty(_objPlayerDetail.BowlerID))
                {
                  

                    QueryContainer q13 = new TermQuery { Field = "bowlerId", Value = _objPlayerDetail.BowlerID };
                    _objNestedQuery &= q13;
                }
                if (_objPlayerDetail.BowlerWides || _objPlayerDetail.BowlerNoBalls || _objPlayerDetail.BowlerBeaten || _objPlayerDetail.BowlerDots || _objPlayerDetail.BowlerEdged || _objPlayerDetail.BowlerDismissal || _objPlayerDetail.BowlerAppeal)
                {
                    ;
                    if (_objPlayerDetail.BowlerWides)
                    {
                       

                        QueryContainer q14 = new TermQuery { Field = "isWide", Value = "1" };
                        qShould |= q14;
                    }
                    if (_objPlayerDetail.BowlerNoBalls)
                    {
                     
                        QueryContainer q15 = new TermQuery { Field = "isNoBall", Value = "1" };
                        qShould |= q15;
                    }
                    if (_objPlayerDetail.BowlerDots)
                    {
                     
                        QueryContainer q16 = new TermQuery { Field = "isDot", Value = "1" };
                        qShould |= q16;
                    }
                    if (_objPlayerDetail.BowlerEdged)
                    {
                        QueryContainer q17 = new TermQuery { Field = "isEdged", Value = "1" };
                        qShould |= q17;
                    }
                    if (_objPlayerDetail.BowlerBeaten)
                    {
                        QueryContainer q18 = new TermQuery { Field = "isBeaten", Value = "1" };
                        qShould |= q18;
                    }
                    if (_objPlayerDetail.BowlerDismissal)
                    {
                      
                        QueryContainer q19 = new TermQuery { Field = "isWicket", Value = "1" };
                        qShould |= q19;

                    }
                    if (_objPlayerDetail.BowlerAppeal)
                    {
                        QueryContainer q20 = new TermQuery { Field = "isAppeal", Value = "1" };
                        qShould |= q20;
                    }
                
                }
                if (!string.IsNullOrEmpty(_objPlayerDetail.DeliveryType) && !isMasterData)
                {
                  
                    string Dlist = _objPlayerDetail.DeliveryType;
                    string[] strnumbers = Dlist.Split(',');
                    foreach (string str in strnumbers)
                    {
                        QueryContainer q21 = new TermQuery { Field = "deliveryTypeId", Value = str };
                        qShould |= q21;
                    }
                    
                }
                if (!string.IsNullOrEmpty(_objPlayerDetail.BowlingLength) && !isMasterData)
                {
                    
                    string Dlist = _objPlayerDetail.BowlingLength;
                    string[] strnumbers = Dlist.Split(',');
                    foreach (string str in strnumbers)
                    {
                        QueryContainer q22 = new TermQuery { Field = "bowlingLengthId", Value = str };
                        qShould |= q22;
                    }
                   
                }
                if (!string.IsNullOrEmpty(_objPlayerDetail.BowlingLine) && !isMasterData)
                {
                  
                    string Dlist = _objPlayerDetail.BowlingLine;
                    string[] strnumbers = Dlist.Split(',');
                    foreach (string str in strnumbers)
                    {
                        QueryContainer q23 = new TermQuery { Field = "bowlingLineId", Value = str };
                        qShould |= q23;
                    }
                   
                }
                if (_objPlayerDetail.BowlingOver || _objPlayerDetail.BowlingRound)
                {
                    if (_objPlayerDetail.BowlingOver)
                    {

                        QueryContainer q24 = new TermQuery { Field = "isOverTheWicket", Value = "1" };
                        qShould |= q24;
                    }
                    if (_objPlayerDetail.BowlingRound)
                    {
                        QueryContainer q25 = new TermQuery { Field = "isRoundTheWicket", Value = "1" };
                        qShould |= q25;

                    }
                   
                }
                if (_objPlayerDetail.BowlerSpin || _objPlayerDetail.BowlerPace)
                {
                   
                    if (_objPlayerDetail.BowlerSpin)
                    {
                        QueryContainer q26 = new TermQuery { Field = "skill", Value = "s" };
                        qShould |= q26;

                     }

                    if (_objPlayerDetail.BowlerPace)
                    {
                      

                        QueryContainer q27 = new TermQuery { Field = "skill", Value = "p" };
                        qShould |= q27;
                    }
                    
                }
                if (!string.IsNullOrEmpty(_objPlayerDetail.FielderID))
                {
                  

                    QueryContainer q27 = new TermQuery { Field = "fielderId", Value = _objPlayerDetail.FielderID };
                    _objNestedQuery &= q27;
                }
                if (_objPlayerDetail.FielderCatch || _objPlayerDetail.FielderRunOut || _objPlayerDetail.FielderDrops || _objPlayerDetail.FielderMisFields)
                {
                    
                    if (_objPlayerDetail.FielderCatch)
                    {
                      
                        QueryContainer q28 = new TermQuery { Field = "isCatch", Value = "1" };
                        qShould |= q28;
                    }
                    if (_objPlayerDetail.FielderRunOut)
                    {
                   
                        QueryContainer q29 = new TermQuery { Field = "isRunOut", Value = "1" };
                        qShould |= q29;
                    }
                    if (_objPlayerDetail.FielderDrops)
                    {
                        QueryContainer q30 = new TermQuery { Field = "isDropped", Value = "1" };
                        qShould |= q30;
                    }
                    if (_objPlayerDetail.FielderMisFields)
                    {
                    
                        QueryContainer q31 = new TermQuery { Field = "isMisField", Value = "1" };
                        qShould |= q31;
                    }
                    
                }
                if (!string.IsNullOrEmpty(_objPlayerDetail.FieldingPosition) && !isMasterData)
                {
                   
                    string Dlist = _objPlayerDetail.FieldingPosition;
                    string[] strnumbers = Dlist.Split(',');
                    foreach (string str in strnumbers)
                    {
                         QueryContainer q32 = new TermQuery { Field = "fielderPositionId", Value = str };
                        qShould |= q32;
                    }
                  
                }
                if (!string.IsNullOrEmpty(_objPlayerDetail.RunsSaved) && _objPlayerDetail.RunsSaved != "0")
                {
                    QueryContainer q33 = new TermQuery { Field = "runsSaved", Value = Convert.ToString(_objPlayerDetail.RunsSaved) };
                    _objNestedQuery &= q33;

                }
                if (!string.IsNullOrEmpty(_objPlayerDetail.BattingOrder) && !isMasterData)
                {
                    string Dlist = _objPlayerDetail.BattingOrder;
                    string[] strnumbers = Dlist.Split(',');
                    foreach (string str in strnumbers)
                    {
                        QueryContainer q34 = new TermQuery { Field = "battingOrder", Value = str };
                        qShould |= q34;
                    }
                    
                }
                if (!string.IsNullOrEmpty(_objPlayerDetail.BowlingArm) && !isMasterData)
                {
                    string Dlist = _objPlayerDetail.BowlingArm;
                    string[] strnumbers = Dlist.Split(',');
                    foreach (string str in strnumbers)
                    {
                        QueryContainer q35 = new TermQuery { Field = "bowlingArm", Value = str.ToLower() };
                        qShould |= q35;
                    }
                   
                }
            }
            _objNestedQuery &= qShould;
            return _objNestedQuery;
        }

        private int getMatchCount(QueryContainer _objNestedQuery, ElasticClient EsClient) {
            int count;
            var response = EsClient.Search<SearchCricketData>(a => a.Index("cricket").Size(0)
             .
             Query(q => _objNestedQuery)
             //.Aggregations(t=>t.ValueCount("commit_count", sa=>sa.Field(p => p.MatchId))
             .Aggregations(a1 => a1.Terms("commit_count", t => t.Field(p => p.MatchId.Suffix("keyword")).Size(802407)
             )));
            var agg = response.Aggregations.Terms("commit_count");
            count = agg.Buckets.Count;
            return count;
       // }

    }
        private int getIsAssetCount(QueryContainer _objNestedQuery, ElasticClient EsClient)
        {
            int count;
            var response = EsClient.Search<SearchS2Data>(a => a.Index("cricket").Size(0)
             .
             Query(q => _objNestedQuery)
             //.Aggregations(t=>t.ValueCount("commit_count", sa=>sa.Field(p => p.MatchId))
             .Aggregations(a1 => a1.Terms("commit_count", t => t.Field(p => p.IsAsset.Suffix("keyword")).Size(409846)
             )));
            var agg = response.Aggregations.Terms("commit_count");
            count = agg.Buckets.Count;
            return count;
            // }

        }
        private int getIsAssetZeroCount(QueryContainer _objNestedQuery, ElasticClient EsClient)
        {
            int count;
            var response = EsClient.Search<SearchS2Data>(a => a.Index("cricket").Size(0)
             .
             Query(q => _objNestedQuery)
             //.Aggregations(t=>t.ValueCount("commit_count", sa=>sa.Field(p => p.MatchId))
             .Aggregations(a1 => a1.Terms("commit_count", t => t.Field(p => p.MatchId.Suffix("keyword")).Size(409846)
             )));
            var agg = response.Aggregations.Terms("commit_count");
            count = agg.Buckets.Count;
            return count;
            // }

        }
    }
}
