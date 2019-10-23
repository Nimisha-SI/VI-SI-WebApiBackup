using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nest;
using WebApis.elastic;
using WebApis.Model;
using static WebApis.Model.ELModels;

namespace WebApis.BOL
{
    public class CommonFunction :  IStoryTeller
    {
        //string SportType = "";
        Cricket objCs = new  Cricket();
        searchcricket sc = new searchcricket();
       //GetMatchDetails matchDetails = new GetMatchDetails();
        static ElasticClient EsClient;
        EsLayer oLayer = new EsLayer();
        

        
        public QueryContainer getMomentDetailsQueryST(MatchDetail _ObjMatchDetails, QueryContainer _objNestedQuery, Moments _objMomentsData)
        {
            if (_objMomentsData != null)
            {
                if (!string.IsNullOrEmpty(_objMomentsData.Entities) || _objMomentsData.IsBigMoment || _objMomentsData.IsFunnyMoment || _objMomentsData.IsAudioPiece)
                {
                    //_objNestedQuery = matchDetails.GetMatchDetailQueryST(_objNestedQuery, _ObjMatchDetails);
                    _objNestedQuery = GetS2MomentDetailQueryForST(_ObjMatchDetails, _objNestedQuery, _objMomentsData);
                }
               
            }
            return _objNestedQuery;
        }

        //public override IEnumerable<ELModels.SearchResultFilterData> returnSportResult(ElasticClient EsClient, QueryContainer _objNestedQuery, string IndexName)
        //{
        //    EsClient = oLayer.CreateConnection();
        //    searchcricket sc = new searchcricket();
        //    IEnumerable<SearchResultFilterData> _objSearchResultFilterData = new List<SearchResultFilterData>();
        //    var result = EsClient.Search<SearchCricketData>(s => s.Index(IndexName).Query(q => _objNestedQuery).Size(27394));
        //    _objSearchResultFilterData = SearchResultFilterDataMap(result);
        //    return _objSearchResultFilterData;
        //    //throw new NotImplementedException();
        //}

        public ExtendedSearchResultFilterData searchStoryTeller(ELModels.MatchDetail _objMatchDetail, QueryContainer _objNestedQuery, dynamic _objS1Data, Dictionary<string, object> ObjectArray, IEnumerable<SearchResultFilterData> obj, string value, string IndexName)
        {
            EsClient = oLayer.CreateConnection();
            ExtendedSearchResultFilterData _objSearchResults = new ExtendedSearchResultFilterData();
            _objSearchResults.ResultData = new List<SearchResultFilterData>();
            _objSearchResults.Master = new MasterDatas();
            _objSearchResults.Master.MasterData = new Dictionary<string, object>();
            CommonFunction cf = new CommonFunction();
            Cricket objDetails =new Cricket();
            //searchcricket sc = new searchcricket();
            // SportType = sc.getType(_objMatchDetail.SportID);
            //if ("CRICKET" == SportType) {
            //string ReqShotType = _objS1Data["ShotType"]; string ReqDeliveryType = _objS1Data["DeliveryType"];
            //string[] _objReqShotType = ReqShotType.Contains(",") ? _objReqShotType = ReqShotType.Split(",") : _objReqShotType = new string[] { _objS1Data["ShotType"] };
            //string[] _objReqDeliveryType = ReqDeliveryType.Contains(",") ? _objReqDeliveryType = ReqDeliveryType.Split(",") : _objReqDeliveryType = new string[] { _objS1Data["DeliveryType"] };
            //ObjectArray = GetDropdowns(_objNestedQuery, ObjectArray, EsClient, "cricket", GetColumnForEntity(13), _objReqShotType);
            //ObjectArray = GetDropdowns(_objNestedQuery, ObjectArray, EsClient, "cricket", GetColumnForEntity(32), _objReqDeliveryType);

            //_objSearchResults.Master.MasterData = ObjectArray;
            Dictionary<string, object> ddlDropdowns = new Dictionary<string, object>();
            GetMatchDetails _objMatchDetails = new GetMatchDetails();
            ddlDropdowns = _objMatchDetails.bindS1Dropdown(_objS1Data);
            if (value != null) {
                string[] valuess = value.Split(",");
                foreach (var items in valuess)
                {
                    var item = items.Split("::");
                    string Type = _objS1Data[item[0]];
                    string[] _objType = Type.Contains(",") ? _objType = Type.Split(",") : _objType = new string[] { Type };
                    foreach (KeyValuePair<string, object> entry in ddlDropdowns)
                    {
                       // if (_objType.ToString() != "") {
                            if (item.ToString().Split(",")[0] != entry.Key.ToString())
                            {
                            if(entry.Value.ToString() != "")
                            {
                                QueryContainer query = new TermQuery { Field = entry.Key, Value = entry.Value };
                                _objNestedQuery &= query;
                            }
                              
                            }
                        //}
                        

                    }
                    
                    _objSearchResults.Master.MasterData = objCs.fetchDropdowns(_objNestedQuery, _objSearchResults.Master.MasterData, EsClient, IndexName, cf.GetColumnForEntity(Convert.ToInt16(item[1])), _objType);
                }
            }
            
            obj = objDetails.returnSportResult(EsClient, _objNestedQuery, IndexName);
            _objSearchResults.ResultData = obj;
            return _objSearchResults;
            //throw new NotImplementedException();
        }
        
        //public ExtendedSearchResultFilterData searchStoryTeller2(ELModels.MatchDetail _objMatchDetail, QueryContainer _objNestedQuery, dynamic _objS1Data, Dictionary<string, object> ObjectArray, IEnumerable<SearchResultFilterData> obj)
        //{
        //    ExtendedSearchResultFilterData _objSearchResults = new ExtendedSearchResultFilterData();
        //    _objSearchResults.ResultData = new List<SearchResultFilterData>();
        //    _objSearchResults.Master.MasterData = new Dictionary<string, object>();
        //    searchcricket sc = new searchcricket();
        //    SportType = sc.getType(_objMatchDetail.SportID);
        //    if ("CRICKET" == SportType)
        //    {
                
        //        string ReqShotType = _objS1Data["ShotType"]; string ReqDeliveryType = _objS1Data["DeliveryType"];
        //        string[] _objReqShotType = ReqShotType.Contains(",") ? _objReqShotType = ReqShotType.Split(",") : _objReqShotType = new string[] { _objS1Data["ShotType"] };
        //        string[] _objReqDeliveryType = ReqDeliveryType.Contains(",") ? _objReqDeliveryType = ReqDeliveryType.Split(",") : _objReqDeliveryType = new string[] { _objS1Data["DeliveryType"] };
        //        ObjectArray = GetDropdowns(_objNestedQuery, ObjectArray, EsClient, "cricket", GetColumnForEntity(13), _objReqShotType);
        //        ObjectArray = GetDropdowns(_objNestedQuery, ObjectArray, EsClient, "cricket", GetColumnForEntity(32), _objReqDeliveryType);
        //        obj = returnSportResult(EsClient, _objNestedQuery, "cricket");
        //        _objSearchResults.ResultData = obj;
        //        _objSearchResults.Master.MasterData = ObjectArray;
        //    }
        //    return _objSearchResults;
        //    //throw new NotImplementedException();
        //}
        

        //public override Dictionary<string, object> fetchDropdowns(QueryContainer _objNestedQuery, Dictionary<string, object> ObjectArray, ElasticClient EsClient, string IndexName, Dictionary<string, string> _columns, string[] sFilterArray)
        //{
        //    List<SearchResultFilterData> _objSearchResultFilterData = new List<SearchResultFilterData>();
        //    List<FilteredEntityData> obj = new List<FilteredEntityData>();

        //    if (_columns != null && _columns.Count > 0)
        //    {
        //        KeyValuePair<string, string> _column = _columns.FirstOrDefault();
        //        List<string> EntityIds = new List<string>();
        //        List<string> EntityNames = new List<string>();
        //        foreach (var col in _columns)
        //        {
        //            EntityIds.Add(col.Key);
        //            EntityNames.Add(col.Value);

        //        }
        //        var result = EsClient.Search<SearchCricketData>(a => a.Index(IndexName).Size(0).Query(s => _objNestedQuery)
        //        .Aggregations(a1 => a1.Terms("terms_agg", t => t.Script(t1 => t1.Source("doc['"+ EntityNames.ElementAt(0) + ".keyword'].value + '|' + doc['"+ EntityIds.ElementAt(0) + ".keyword'].value")).Size(802407))
        //        )
        //       );
        //        var agg = result.Aggregations.Terms("terms_agg").Buckets;
        //        foreach (var items in agg)
        //        {
        //            obj.Add(new FilteredEntityData
        //            {
        //                EntityId = items.Key.ToString().Split("|")[1],
        //                EntityName = items.Key.ToString().Split("|")[0],
        //                IsSelectedEntity = sFilterArray.Contains(items.Key.ToString().Split("|")[1]) ? 1 : 0
        //            });
        //        }
        //        ObjectArray.Add(EntityNames.ElementAt(0), obj);

        //    }
        //    return ObjectArray;

        //}

        //public override Dictionary<string,object> fetchDropDownForMatch(Dictionary<string, object> ObjectArray, string[] sInnings) {
        //    List<FilteredEntityData> obj2 = new List<FilteredEntityData>();
        //    for (int i = 1; i <= 2; i++)
        //    {
        //        obj2.Add(new FilteredEntityData
        //        {
        //            EntityId = i.ToString(),
        //            EntityName = i.ToString(),
        //            IsSelectedEntity = sInnings.Contains(i.ToString()) ? 1 : 0
        //        });
        //    }
        //    ObjectArray.Add("Innings", obj2);
        //    return ObjectArray;
        //}
        
        public QueryContainer GetS2MomentDetailQueryForST(MatchDetail _objMatchDetail, QueryContainer _objNestedQuery, Moments _objMomentData)
        {
            //List<QueryContainer> myDynamicShouldQuery = new List<QueryContainer>();
            //List<QueryContainer> myDynamicMustQuery = new List<QueryContainer>();
            QueryContainer qShould = new QueryContainer();
            if (_objMomentData != null)
            {
                if (!string.IsNullOrEmpty(_objMomentData.Entities))
                {

                    if (_objMomentData.Entities.Contains(","))
                    {
                        string[] arrEntities = _objMomentData.Entities.Split(',');
                        for (int iEntCtr = 0; iEntCtr < arrEntities.Length; iEntCtr++)
                        {
                            if (!string.IsNullOrEmpty(arrEntities[iEntCtr]))
                            {
                                QueryContainer q1 = new TermQuery { Field = "EntityId_1", Value = arrEntities[iEntCtr] };
                                QueryContainer q2 = new TermQuery { Field = "EntityId_2", Value = arrEntities[iEntCtr] };
                                QueryContainer q3 = new TermQuery { Field = "EntityId_3", Value = arrEntities[iEntCtr] };
                                QueryContainer q4 = new TermQuery { Field = "EntityId_4", Value = arrEntities[iEntCtr] };
                                QueryContainer q5 = new TermQuery { Field = "EntityId_5", Value = arrEntities[iEntCtr] };
                                //myDynamicShouldQuery.Add(q1);
                                //myDynamicShouldQuery.Add(q2);
                                //myDynamicShouldQuery.Add(q3);
                                //myDynamicShouldQuery.Add(q4);
                                qShould |= q1 |= q2 |= q3 |= q4 |= q5;
                                _objNestedQuery &= qShould;
                            }

                        }

                    }
                    else
                    {
                        QueryContainer q12 = new TermQuery { Field = "EntityId_1", Value = _objMomentData.Entities };

                        _objNestedQuery &= q12;

                    }
                }

                if (_objMomentData.IsBigMoment || _objMomentData.IsFunnyMoment || _objMomentData.IsAudioPiece)
                {

                    if (_objMomentData.IsBigMoment)
                    {
                        QueryContainer q6 = new TermQuery { Field = "IsBigMoment", Value = "1" };
                        //myDynamicMustQuery.Add(q6);
                        _objNestedQuery &= q6;

                    }
                    if (_objMomentData.IsFunnyMoment)
                    {
                        QueryContainer q7 = new TermQuery { Field = "IsFunnyMoment", Value = "1" };
                        //myDynamicMustQuery.Add(q7);
                        _objNestedQuery &= q7;
                    }
                    if (_objMomentData.IsAudioPiece)
                    {
                        QueryContainer q8 = new TermQuery { Field = "IsAudioPiece", Value = "1" };
                        //myDynamicMustQuery.Add(q8);
                        _objNestedQuery &= q8;


                    }

                }
                else
                {
                    QueryContainer q9 = new TermQuery { Field = "IsBigMoment", Value = "1" };
                    QueryContainer q10 = new TermQuery { Field = "IsFunnyMoment", Value = "1" };
                    QueryContainer q11 = new TermQuery { Field = "IsAudioPiece", Value = "1" };
                    //myDynamicShouldQuery.Add(q9);
                    //myDynamicShouldQuery.Add(q10);
                    //myDynamicShouldQuery.Add(q11);
                    qShould |= q9 |= q10 |= q11;
                    _objNestedQuery &= qShould;

                    //Bq.Add(bq, Occur.MUST);
                }
            }
            return _objNestedQuery;
        }

        
        public string[] isGetDefaultType(int sportId) {
            string[] Arry= new string[] { };
            switch (sportId) {
                case 1:
                    Arry = new string[] { "isSix", "isFour", "isWicket", "isAppeal", "isDropped", "isMisField", "shotTypeId", "deliveryTypeId" };
                    break;
                    case 3:
                     Arry = new string[] { "1", "2", "9", "10", "11" };
                    break;
                default:
                    break;
                    
            }

            return Arry;
        }

        public Dictionary<string, string> GetColumnForEntity(int entitytypeid)
        {
            Dictionary<string, string> Column = new Dictionary<string, string>();
            switch (entitytypeid)
            {
                case 1:
                    Column.Add("CompType", "CompType");
                    break;
                case 2:
                    Column.Add("VenueId", "Venue");
                    break;
                case 3:
                    Column.Add("Team1Id", "Team1");
                    Column.Add("Team2Id", "Team2");
                    break;
                case 4:
                    Column.Add("Team2Id", "Team2");
                    Column.Add("Team1Id", "Team1");
                    break;
                case 5:
                    Column.Add("SeriesId", "Series");
                    Column.Add("ParentSeriesId", "ParentSeriesName");
                    break;
                case 6:
                    Column.Add("batsmanId", "batsman");
                    break;
                case 7:
                    Column.Add("BowlerId", "Bowler");
                    break;
                case 8:
                    Column.Add("FielderId", "Fielder");
                    break;
                case 9:
                    Column.Add("MatchId", "Match");
                    break;
                case 10:
                    Column.Add("OffensivePlayerId", "OffensivePlayerName");
                    break;
                case 11:
                    Column.Add("DefensivePlayerId", "DefensivePlayerName");
                    break;
                case 12:
                    Column.Add("AssistingPlayerId", "AssistingPlayer");
                    break;
                case 13:
                    Column.Add("shotTypeId", "shotType");
                    break;
                case 14:
                    Column.Add("ShotResultId", "ShotResult");
                    break;
                case 15:
                    Column.Add("EventReasonId", "EventReason");
                    break;
                case 16:
                    Column.Add("GoalZoneId", "GoalZone");
                    break;
                case 17:
                    Column.Add("LanguageId", "Language");
                    break;
                case 18:
                    Column.Add("BattingOrder", "BattingOrder");
                    break;
                case 19:
                    Column.Add("BowlingArm", "BowlingArm");
                    break;
                case 20:
                    Column.Add("DriverId", "DriverName");
                    break;
                case 21:
                    Column.Add("DriverTeamId", "DriverTeamName");
                    break;
                case 22:
                    Column.Add("MatchStageId", "MatchStage");
                    break;
                case 23:
                    Column.Add("PlayerId", "PlayerName");
                    Column.Add("ConceededPlayerId", "ConceededPlayerName");
                    Column.Add("ServePlayerId", "ServePlayerName");
                    break;
                case 24:
                    Column.Add("CompTypeId", "CompType");
                    break;
                case 25:
                    Column.Add("AssistPlayer1Id", "AssistPlayer1");
                    Column.Add("AssistPlayer2Id", "AssistPlayer2");
                    break;
                case 26:
                    Column.Add("AssistPlayer2Id", "AssistPlayer2");
                    break;
                case 27:
                    Column.Add("GameTypeId", "GameType"); //new
                    break;
                case 28:
                    Column.Add("PlayerId", "PlayerName");
                    Column.Add("ServePlayerId", "ServePlayerName");
                    break;
                case 29:
                    Column.Add("ConceededPlayerId", "ConceededPlayerName"); //new
                    break;
                case 30:
                    Column.Add("OffensivePlayerId", "OffensivePlayerName");
                    Column.Add("Team1Id", "Team1");
                    break;
                case 31:
                    Column.Add("DefensivePlayerId", "DefensivePlayerName");
                    Column.Add("Team2Id", "Team2");
                    break;
                case 32:
                    Column.Add("deliveryTypeId", "deliveryType");
                    break;

                case 33:
                    Column.Add("shotZoneId", "shotZone");
                    break;
                case 34:
                    Column.Add("bowlingLengthId", "bowlingLength");
                    break;
                case 35:
                    Column.Add("dismissalId", "dismissal");
                    break;
                case 36:
                    Column.Add("bowlingLineId", "bowlingLine");
                    break;
                case 37:
                    Column.Add("fielderPositionId", "fielderPosition");
                    break;
                case 38:
                    Column.Add("battingOrder", "battingOrder");
                    break;
                case 39:
                    Column.Add("BowlingArm", "BowlingArm");
                    break;
                default:
                    break;
            }
            return Column;
        }

        public IEnumerable<SearchResultFilterData> returnSportResult(ElasticClient EsClient, QueryContainer _objNestedQuery, string IndexName)
        {
            throw new NotImplementedException();
        }

        public enum SportType
        {
            CRICKET = 1,
            FOOTBALL = 2,
            kabadibulk = 3,
            F1 = 4,
            TENNIS = 7,
            HOCKEY = 9,
            RUSHES = 19
        }
        public string getType(int value)
        {
            SportType objSportType = (SportType)value;
            return objSportType.ToString();
        }





        //public List<string> GenerateKeyValuePair(Dictionary<string, string> _columns) {
        //    List<EntityList> objlist = new List<EntityList>();
        //    List<string> EntityIds = new List<string>();
        //    List<string> EntityNames = new List<string>();
        //    foreach (var col in _columns)
        //    {
        //        objlist.Add(new EntityList { 
        //            EntityIds = Convert.ToInt32(col.Key)
        //        });
        //        objlist.EntityIds.Add(col.Key);
        //        objlist.EntityNames.Add(col.Value);

        //    }
        //    return objlist
        //}
    }
}
