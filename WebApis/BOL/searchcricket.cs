using Microsoft.AspNetCore.Http.Extensions;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApis.Model;
using static WebApis.Model.ELModels;

namespace WebApis.BOL
{
    public class searchcricket
    {
        public QueryContainer getCricketPlayerDetails(dynamic _objS1Data, QueryContainer qFinal) {
            if (_objS1Data != null)
            {
                if (_objS1Data["IsDefault"] != null && Convert.ToBoolean(_objS1Data["IsDefault"]))
                {
                    QueryContainer query = new QueryContainer();
                    QueryContainer q1 = new TermQuery { Field = "isFour", Value = "1" };
                    QueryContainer q2 = new TermQuery { Field = "isSix", Value = "1" };
                    QueryContainer q3 = new TermQuery { Field = "isWicket", Value = "1" };
                    QueryContainer q5 = new TermQuery { Field = "isAppeal", Value = "1" };
                    QueryContainer q6 = new TermQuery { Field = "isDropped", Value = "1" };
                    QueryContainer q7 = new TermQuery { Field = "isMisField", Value = "1" };
                    query = q1 |= q2 |= q3 |= q5 |= q6 |= q7;
                    qFinal &= query;
                    //qAdd.Must query;
                }
                else
                {
                    //List<QueryContainer> myDynamicMustQuery = new List<QueryContainer>();
                    //List<QueryContainer> myDynamicShouldQuery = new List<QueryContainer>();
                    //QueryContainer queryMust = new QueryContainer();
                    QueryContainer queryShould = new QueryContainer();
                    var myQuery = new BoolQuery();
                    if (Convert.ToString(_objS1Data["BatsmanID"]) != "")
                    {
                        QueryContainer q1 = new TermQuery { Field = "batsmanId", Value = _objS1Data["BatsmanID"] };
                        qFinal &= q1;
                        //myDynamicMustQuery.Add(q1);
                        //qAdd.Must = myDynamicMustQuery;

                    }
                    if (Convert.ToBoolean(_objS1Data["BatsmanFours"]) || Convert.ToBoolean(_objS1Data["BatsmanSixes"]) || Convert.ToBoolean(_objS1Data["BatsmanDismissal"]) ||
                        Convert.ToBoolean(_objS1Data["BatsmanAppeal"]) || Convert.ToBoolean(_objS1Data["FielderMisFields"]) || Convert.ToBoolean(_objS1Data["FielderDrops"]))
                    {
                        if (Convert.ToBoolean(_objS1Data["BatsmanFours"]))
                        {
                            QueryContainer query1 = new TermQuery { Field = "isFour", Value = "1" };
                            //myDynamicShouldQuery.Add(query1);
                            //myQuery.Should= myDynamicShouldQuery;
                            //qAdd.Should = myDynamicShouldQuery;
                            queryShould |= query1;
                        }
                        if (Convert.ToBoolean(_objS1Data["BatsmanSixes"]))
                        {
                            QueryContainer query2 = new TermQuery { Field = "isSix", Value = "1" };
                            //myDynamicShouldQuery.Add(query2);
                            //myQuery.Should = myDynamicShouldQuery;
                            //qAdd.Should = myDynamicShouldQuery;
                            queryShould |= query2;
                        }

                        if (Convert.ToBoolean(_objS1Data["BatsmanDismissal"]))
                        {
                            QueryContainer query3 = new TermQuery { Field = "isWicket", Value = "1" };
                            //myDynamicShouldQuery.Add(query3);
                            //myQuery.Should = myDynamicShouldQuery;
                            //qAdd.Should = myDynamicShouldQuery;
                            queryShould |= query3;
                        }
                        if (Convert.ToBoolean(_objS1Data["BatsmanAppeal"]))
                        {
                            QueryContainer query4 = new TermQuery { Field = "isAppeal", Value = "1" };
                            //myDynamicShouldQuery.Add(query4);
                            //myQuery.Should = myDynamicShouldQuery;
                            //qAdd.Should = myDynamicShouldQuery;
                            queryShould |= query4;
                        }
                        if (Convert.ToBoolean(_objS1Data["FielderDrops"]))
                        {

                            QueryContainer query5 = new TermQuery { Field = "isDropped", Value = "1" };
                            //myDynamicShouldQuery.Add(query5);
                            //myQuery.Should = myDynamicShouldQuery;
                            //qAdd.Should = myDynamicShouldQuery;
                            queryShould |= query5;
                        }
                        if (Convert.ToBoolean(_objS1Data["FielderMisFields"]))
                        {
                            QueryContainer query6 = new TermQuery { Field = "isMisField", Value = "1" };
                            //myDynamicShouldQuery.Add(query6);
                            //myQuery.Should = myDynamicShouldQuery;
                            //qAdd.Should = myDynamicShouldQuery;
                            queryShould |= query6;
                        }
                        qFinal &= queryShould;
                        //qAdd.Must=myDynamicShouldQuery;
                        // _objNestedQuery.Add(Bq, Occur.MUST);
                        //Counts.Add(1);
                    }
                    if (_objS1Data["ShotType"] != null || _objS1Data["ShotType"] != "")
                    {

                        string Dlist = Convert.ToString(_objS1Data["ShotType"]);
                        string[] strnumbers = Dlist.Split(',');
                        foreach (string str in strnumbers)
                        {

                            QueryContainer query9 = new TermQuery { Field = "shotTypeId", Value = str };
                            //myDynamicShouldQuery.Add(query9);
                            //myQuery.Should = myDynamicShouldQuery;
                            //qAdd.Should = myDynamicShouldQuery;
                            queryShould |= query9;
                        }
                        qFinal &= queryShould;
                        // qAdd.Must = myDynamicTermQuery;
                    }
                    if (_objS1Data["BowlerID"] != null || _objS1Data["BowlerID"] != "")
                    {
                        QueryContainer query16 = new TermQuery { Field = "bowlerId", Value = Convert.ToString(_objS1Data["BowlerID"]) };
                        //myDynamicMustQuery.Add(query6);
                        //qAdd.Must = myDynamicMustQuery;
                        //queryMust |= query16;
                        qFinal &= query16;
                    }
                    if (_objS1Data["DeliveryType"] != null || _objS1Data["DeliveryType"] != "")
                    {
                        string Dlist = Convert.ToString(_objS1Data["DeliveryType"]);
                        string[] strnumbers = Dlist.Split(",");
                        int count = 1;
                        foreach (string str in strnumbers)
                        {
                            QueryContainer query17 = new TermQuery { Field = "deliveryTypeId", Value = str };
                            queryShould |= query17;
                        }
                        qFinal &= queryShould;


                    }


                }


            }
            
            return qFinal;
        }
        public QueryContainer GetMatchDetailQueryST(QueryContainer _objNestedQuery, MatchDetail _objMatchDetail)
        {
            if (_objMatchDetail != null)
            {
                //List<QueryContainer> myDynamicMustQuery = new List<QueryContainer>();
                //List<QueryContainer> myDynamicShouldQuery = new List<QueryContainer>();
                QueryContainer queryShould = new QueryContainer();


                if (!string.IsNullOrEmpty(_objMatchDetail.MatchFormat))
                {
                    QueryContainer q1 = new TermQuery { Field = "compType", Value = _objMatchDetail.MatchFormat.ToLower() };
                    //myDynamicMustQuery.Add(q1);
                    //_objNestedQuery.Must = myDynamicMustQuery;
                    _objNestedQuery &= q1;
                }
                if (!string.IsNullOrEmpty(_objMatchDetail.VenueId))
                {
                    QueryContainer q2 = new TermQuery { Field = "venueId", Value = _objMatchDetail.MatchFormat.ToLower().ToString() };
                    //myDynamicMustQuery.Add(q2);
                    //_objNestedQuery.Must = myDynamicMustQuery;
                    _objNestedQuery &= q2;
                }

                if (!string.IsNullOrEmpty(_objMatchDetail.Team1Id))
                {
                    var myQuery = new BoolQuery();
                    QueryContainer q3 = new TermQuery { Field = "team1Id", Value = _objMatchDetail.Team1Id.ToLower().ToString() };
                    QueryContainer q4 = new TermQuery { Field = "team2Id", Value = _objMatchDetail.Team1Id.ToLower().ToString() };
                    //myDynamicShouldQuery.Add(q3);
                    //myDynamicShouldQuery.Add(q4);
                    //_objNestedQuery.Should = myDynamicShouldQuery;
                    queryShould |= q3 |= q4;
                    _objNestedQuery &= queryShould;
                }
                if (!string.IsNullOrEmpty(_objMatchDetail.Team2Id))
                {
                
                    QueryContainer q5 = new TermQuery { Field = "team1Id", Value = _objMatchDetail.Team2Id.ToLower().ToString() };
                    QueryContainer q6 = new TermQuery { Field = "team2Id", Value = _objMatchDetail.Team2Id.ToLower().ToString() };
                    // myDynamicShouldQuery.Add(q5);
                    // myDynamicShouldQuery.Add(q6);
                    //// myQuery.Should = myDynamicTermQuery;
                    // _objNestedQuery.Should = myDynamicShouldQuery;
                    queryShould |= q5 |= q6;
                    _objNestedQuery &= queryShould;
                }

                if (!string.IsNullOrEmpty(_objMatchDetail.SeriesId) && _objMatchDetail.IsParentSeries)
                {
                    QueryContainer q7 = new TermQuery { Field = "parentSeriesId", Value = _objMatchDetail.SeriesId };
                    //myDynamicMustQuery.Add(q7);
                    //_objNestedQuery.Must = myDynamicMustQuery;
                    
                    _objNestedQuery &= q7;

                }
                if (!string.IsNullOrEmpty(_objMatchDetail.SeriesId) && !_objMatchDetail.IsParentSeries)
                {
                     QueryContainer q8 = new TermQuery { Field = "seriesId", Value = _objMatchDetail.SeriesId };
                    //myDynamicMustQuery.Add(q8);
                    //_objNestedQuery.Must = myDynamicMustQuery;
                    
                    _objNestedQuery &= q8;
                }
                if (!string.IsNullOrEmpty(_objMatchDetail.MatchId))
                {
                    QueryContainer q9 = new TermQuery { Field = "matchId", Value = _objMatchDetail.MatchId };
                    //myDynamicMustQuery.Add(q9);
                    //_objNestedQuery.Must = myDynamicMustQuery;
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
                    //myDynamicShouldQuery.Add(q10);
                    //myDynamicShouldQuery.Add(q11);
                    //myDynamicShouldQuery.Add(q12);
                    //myDynamicShouldQuery.Add(q13);
                    //myDynamicShouldQuery.Add(q14);
                    //myDynamicShouldQuery.Add(q15);
                    ////myQuery.Should = myDynamicTermQuery;
                    //_objNestedQuery.Should = myDynamicShouldQuery;


                    queryShould |= q10 |= q11 |= q12 |= q13 |= q14 |= q15;
                    _objNestedQuery &= queryShould;
                    // _objNestedQuery.Add(bq, Occur.MUST);
                }
                if (!string.IsNullOrEmpty(_objMatchDetail.MatchStageId))
                {

                    var myQuery = new BoolQuery();
                    string Dlist = Convert.ToString(_objMatchDetail.MatchStageId);
                    string[] strnumbers = Dlist.Split(',');
                    foreach (string str in strnumbers)
                    {
                        QueryContainer q16 = new TermQuery { Field = "matchStageId", Value = str };
                        //myDynamicShouldQuery.Add(q16);
                        //_objNestedQuery.Should = myDynamicShouldQuery;
                        queryShould |= q16;
                       
                    }
                    _objNestedQuery &= queryShould;
                    //_objNestedQuery.Add(bq, Occur.MUST);

                }
                if (!string.IsNullOrEmpty(_objMatchDetail.CompTypeId))
                {
                    var myQuery = new BoolQuery();
                    string Dlist = Convert.ToString(_objMatchDetail.CompTypeId);
                    string[] strnumbers = Dlist.Split(',');
                    foreach (string str in strnumbers)
                    {
                        QueryContainer q17 = new TermQuery { Field = "CompTypeId", Value = str };
                        //myDynamicShouldQuery.Add(q17);
                        //_objNestedQuery.Should = myDynamicShouldQuery;
                        queryShould |= q17;
                    }
                    _objNestedQuery &= queryShould;
                    //_objNestedQuery.Add(bq, Occur.MUST);
                }


                if (_objMatchDetail.HasShortClip)
                {
                    QueryContainer q21 = new TermQuery { Field = "hasShortClip", Value = "1" };
                    //myDynamicMustQuery.Add(q21);
                    //_objNestedQuery.Must=myDynamicMustQuery;
                    _objNestedQuery &= q21;
                }

                if (!_objMatchDetail.IsAssetSearch)
                {
                    QueryContainer q20 = new TermQuery { Field = "isTagged", Value = "1" };
                    //myDynamicMustQuery.Add(q20);
                    //_objNestedQuery.Must = myDynamicMustQuery;
                    _objNestedQuery &= q20;

                }

                if (!string.IsNullOrEmpty(_objMatchDetail.LanguageId) && _objMatchDetail.LanguageId != "0")
                {
                       

                    QueryContainer q19 = new TermQuery { Field = "languageId", Value = _objMatchDetail.LanguageId };
                    //myDynamicMustQuery.Add(q19);
                    //_objNestedQuery.Must = myDynamicMustQuery;
                    _objNestedQuery &= q19;

                }
                string input = Convert.ToInt32(Convert.ToBoolean(_objMatchDetail.IsAsset)).ToString();
                QueryContainer q18 = new TermQuery { Field = "isAsset", Value =input };
                //myDynamicMustQuery.Add(q18);
                //_objNestedQuery.Must = myDynamicMustQuery;
                _objNestedQuery &= q18;

            }
            return _objNestedQuery;
        }
        public QueryContainer GetCricketMatchSituationQueryST(QueryContainer _objNestedQuery, MatchSituation _objMatchSituation)
        {
            if (_objMatchSituation != null)
            {
                if (!string.IsNullOrEmpty(_objMatchSituation.Innings))
                {
                    List<QueryContainer> myDynamicTermQuery = new List<QueryContainer>();
                    QueryContainer q1 = new TermQuery { Field = "Innings", Value = Convert.ToString(_objMatchSituation.Innings) };
                    //myDynamicTermQuery.Add(q1);
                    //_objNestedQuery.Must = myDynamicTermQuery;
                    _objNestedQuery &= q1;


                }

            }
            return _objNestedQuery;
        }
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
        public Dictionary<string, object> parseDropdown(QueryContainer _objNestedQuery, Dictionary<string, object> ObjectArray, ElasticClient EsClient, string[] sInnings, string[] sShotType, string[] sDeliveryTpe) {
            List<SearchResultFilterData> _objSearchResultFilterData = new List<SearchResultFilterData>();
            
            List<FilteredEntityData> obj = new List<FilteredEntityData>();
            List<FilteredEntityData> obj1 = new List<FilteredEntityData>();
            List<FilteredEntityData> obj2 = new List<FilteredEntityData>();
            var result = EsClient.Search<SearchCricketData>(a => a.Index("cricket").Size(0).Query(s => _objNestedQuery)
                .Aggregations(a1 => a1.Terms("terms_agg_shotType", t => t.Script(t1 => t1.Source("doc['shotType.keyword'].value + '|' + doc['shotTypeId.keyword'].value")).Size(802407))
                )
               );
            var agg = result.Aggregations.Terms("terms_agg_shotType").Buckets;
            //ObjectArray.Add("ShotType1", agg);
            foreach (var items in agg)
            {
                obj.Add(new FilteredEntityData
                {
                    EntityId = items.Key.ToString().Split("|")[1],
                    EntityName = items.Key.ToString().Split("|")[0],
                    IsSelectedEntity= sShotType.Contains(items.Key.ToString().Split("|")[1]) ? 1 : 0
                });
           }
            ObjectArray.Add("ShotType", obj);
            var results = EsClient.Search<SearchCricketData>(a => a.Index("cricket").Size(0).Query(qs => _objNestedQuery)
             .Aggregations(a1 => a1.Terms("terms_agg_deliveryType", t => t.Script(t1 => t1.Source("doc['deliveryType.keyword'].value + '|' + doc['deliveryTypeId.keyword'].value")).Size(802407))
             )
            );
            var aggs = results.Aggregations.Terms("terms_agg_deliveryType").Buckets;
            foreach (var items in aggs)
            {
                obj1.Add(new FilteredEntityData
                {
                    EntityId = items.Key.ToString().Split("|")[1],
                    EntityName = items.Key.ToString().Split("|")[0],
                    IsSelectedEntity = sDeliveryTpe.Contains(items.Key.ToString().Split("|")[1]) ? 1 : 0
                });
            }
            ObjectArray.Add("DeliveryType", obj1);

            
            for (int i = 1; i <= 2; i++)
            {
                obj2.Add(new FilteredEntityData
                {
                    EntityId = i.ToString(),
                    EntityName = i.ToString(),
                    IsSelectedEntity = sInnings.Contains(i.ToString()) ? 1 : 0
                });
            }
            ObjectArray.Add("Innings", obj2);
            return ObjectArray;
        }
        public IEnumerable<SearchResultFilterData> getSegementsData(ElasticClient EsClient, QueryContainer _objNestedQuery) {
            IEnumerable<SearchResultFilterData> _objSearchResultFilterData = new List<SearchResultFilterData>();
            var result = EsClient.Search<SearchCricketData>(s => s.Index("cricket").Query(q => _objNestedQuery).Size(802407));
            _objSearchResultFilterData = SearchResultFilterDataMap(result);
            return _objSearchResultFilterData;
        }
        private static List<SearchResultFilterData> SearchResultFilterDataMap( ISearchResponse<SearchCricketData> result)
        {
            List<SearchResultFilterData> ListObj = new List<SearchResultFilterData>();
            foreach (var hit in result.Hits)
            {
                ListObj.Add(new SearchResultFilterData
                {
                    ClearId = hit.Source.ClearId.ToString(),
                    Description = hit.Source.Description.ToString(),
                    MarkIn = hit.Source.MarkIn.ToString(),
                    MarkOut = hit.Source.MarkOut.ToString(),
                    ShotType = hit.Source.MarkOut.ToString(),
                    Duration = hit.Source.Duration.ToString(),
                    DeliveryType = hit.Source.DeliveryType.ToString(),
                    DeliveryTypeId = hit.Source.DeliveryTypeId.ToString(),
                    EventId = hit.Source.EventId.ToString(),
                    EventName = hit.Source.EventText.ToString(),
                    Id = hit.Source.Id.ToString(),
                    MatchId = hit.Source.MatchId.ToString(),
                    MediaId = hit.Source.MediaId.ToString(),
                    Title = hit.Source.Title.ToString(),
                    ShotTypeId = hit.Source.ShotTypeId.ToString(),
                });
            }
            return ListObj;
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
                default:
                    break;
            }
            return Column;
        }
        public QueryContainer GetFilteredEntitiesBySport(MatchDetail _objReqData, QueryContainer _objNestedQuery, string sCase, int sDate, Dictionary<string, string> _columns, string searchText) {

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
        public List<FilteredEntityForCricket> GetFilteredEntitiesBySportResult(QueryContainer qc, string EntityId, string EntityName, ElasticClient EsClient, string searchText, int sDate=0, int Edate=0) {
            List<FilteredEntityForCricket> obj = new List<FilteredEntityForCricket>();
            string eName = "", eId = "";
            if (EntityName == "Bowler")
            {
                eName = "bowler";
            }
            if (EntityId == "BowlerId")
            {
                eId = "bowlerId";
            }
            if (EntityName == "Batsman")
            {
                eName = "batsman";
            }
            if (EntityId == "BatsmanId")
            {
                eId = "batsmanId";
            }
            var terms = searchText.Trim().Replace("-", " ").Split(' ').Where(x => !string.IsNullOrEmpty(x)).Select(x => x.Trim() + "*");
            searchText = string.Join(" ", terms).ToLower();
            
            if (sDate != 0 && Edate != 0)
            {
                var response = EsClient.Search<SearchCricketData>(s => s.Index("cricket").Size(0).
                Query(q => qc && q.Range(v=>v.Field(p=>p.MatchDate).GreaterThanOrEquals(sDate).LessThanOrEquals(Edate)))
                .Aggregations(a => a.Terms("my_agg", st => st.Script(p => p.Source("doc['" + eName + ".keyword'].value + '|' + doc['" + eId + ".keyword'].value")).Size(802407))));
                var agg = response.Aggregations.Terms("my_agg").Buckets;
                foreach (var hits in agg) {
                    obj.Add(new FilteredEntityForCricket
                    {
                        entityname = EntityName,
                        entityplayername = hits.Key.ToString().Split("|")[0],
                        entityid = hits.Key.ToString().Split("|")[1],
                    });
                }
               

            }
            else {
                if (searchText != "")
                {
                    
                    if (eName == "bowler") {
                        var response = EsClient.Search<SearchCricketData>(s => s.Index("cricket").Size(0).
                Query(q => qc && q.Wildcard(c => c.Name("named_query").Field(f => f.Bowler).Value(searchText)))
                .Aggregations(a => a.Terms("my_agg", st => st.Script(p => p.Source("doc['" + eName + ".keyword'].value + '|' + doc['" + eId + ".keyword'].value")).Size(802407))));
                        var agg = response.Aggregations.Terms("my_agg").Buckets;
                        foreach (var hits in agg)
                        {
                            obj.Add(new FilteredEntityForCricket
                            {
                                entityname = EntityName,
                                entityplayername = hits.Key.ToString().Split("|")[0],
                                entityid = hits.Key.ToString().Split("|")[1],
                            });
                        }
                    }
                    if (eName == "batsman")
                    {
                        var response = EsClient.Search<SearchCricketData>(s => s.Index("cricket").Size(0).
                Query(q => qc && q.Wildcard(c => c.Name("named_query").Field(f => f.Batsman).Value(searchText)))
                .Aggregations(a => a.Terms("my_agg", st => st.Script(p => p.Source("doc['" + eName + ".keyword'].value + '|' + doc['" + eId + ".keyword'].value")).Size(802407))));
                        var agg = response.Aggregations.Terms("my_agg").Buckets;
                        foreach (var hits in agg)
                        {
                            obj.Add(new FilteredEntityForCricket
                            {
                                entityname = EntityName,
                                entityplayername = hits.Key.ToString().Split("|")[0],
                                entityid = hits.Key.ToString().Split("|")[1],
                            });
                        }
                    }

                }
                else {
                    var response = EsClient.Search<SearchCricketData>(s => s.Index("cricket").Size(0).
                Query(q => qc)
                .Aggregations(a => a.Terms("my_agg", st => st.Script(p => p.Source("doc['" + eName + ".keyword'].value + '|' + doc['" + eId + ".keyword'].value")).Size(802407))));
                    var agg = response.Aggregations.Terms("my_agg").Buckets;
                    foreach (var hits in agg)
                    {
                        obj.Add(new FilteredEntityForCricket
                        {
                            entityname = EntityName,
                            entityplayername = hits.Key.ToString().Split("|")[0],
                            entityid = hits.Key.ToString().Split("|")[1],
                        });
                    }

                }

               
               
                    
                    
            }
            return obj;


        }
        public enum SportType {
        CRICKET = 1,
        FOOTBALL = 2,
        kabadibulk = 3,
        F1 = 4,
        TENNIS = 7,
        HOCKEY = 9,
        RUSHES = 19
        }
        public string getType(int value) {
            SportType objSportType = (SportType)value;
            return objSportType.ToString();
        }


        public Dictionary<string, string> dhyGetPlayerDetails(int entitytypeid)
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
                default:
                    break;
            }
            return Column;
        }




    }
}
