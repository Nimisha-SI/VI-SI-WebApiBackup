using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
//using System.Web.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Nest;
using Newtonsoft.Json;
using WebApis.BOL;
using WebApis.elastic;
using WebApis.Model;
using static WebApis.Model.ELModels;

namespace WebApis.Controllers
{
   //[Route("api/[controller]")]

   [ApiController]
    public class CricketController : ControllerBase
    {
        private AppConfig _settings;
        static ElasticClient EsClient;
        EsLayer oLayer = new EsLayer();
        GetSearchS1DataForCricket obj = new GetSearchS1DataForCricket();



        //[System.Web.Http.HttpGet]
        //[Route("api/getData")]
        //public IActionResult Get()
        //{


        //    EsClient = oLayer.CreateConnection();
        //    List<SearchCricketData> obj2 = new List<SearchCricketData>();
        //    string connection = obj.GetConnectionString("ConnectionStrings", "DefaultConnection");
        //    try
        //    {
        //        //dynamic DyObj = new List<SearchCricketData>();
        //        for (int i = 1; i <= 120; i++)
        //        {
        //            obj2 = obj.getCricketData(connection, true, i, 10000);
        //            if (obj2.Count >0)
        //            {
        //                oLayer.BulkInsert(EsClient, obj2);
        //            }

        //        }
        //        //  return Request.CreateResponse(HttpStatusCode.Created, "Index Created successfully.");
        //        return Ok("Success");
        //    }
        //    catch (Exception ex)
        //    {

        //        // return Request.CreateResponse(HttpStatusCode.BadRequest, ex.ToString());
        //        return BadRequest(ex.ToString());
        //        // return request.CreateResponse(HttpStatusCode.BadRequest, "Invalid operation!");
        //    }





        //}

        [System.Web.Http.HttpGet]
        [Route("api/DeliveryType")]
        public IActionResult getDeliveryType()
        {
            try
            {
                List<ddlValue> obj = new List<ddlValue>();
                HttpResponseMessage response = null;
                EsClient = oLayer.CreateConnection();
                QueryContainer q1 = new TermQuery { Field = "isTagged", Value = "1" };
                QueryContainer q2 = new TermQuery { Field = "isAsset", Value = "0" };
                var q = new QueryContainer();
                q &= q2;
                q &= q1;
                // var result = EsClient.Search<SearchCricketData>(a => a.Index("cricket").Size(0).Query(qs => qs)
                // .Aggregations(a1 => a1.Terms("my_terms_agg", t => t.Field(p => p.DeliveryType.Suffix("keyword"))
                // .Size(802407)
                // //.Aggregations(a2 => a2.Terms("my_terms_agg2", t1 => t1.Field(p1 => p1.DeliveryTypeId.Suffix("keyword")).Size(802407))
                //).Terms("my_terms_agg2", t2=>t2.Field(po=>po.DeliveryTypeId.Suffix("keyword")).Size(802407))));


                var result = EsClient.Search<SearchCricketData>(a => a.Index("cricket").Size(0).Query(qs => qs)
                .Aggregations(a1 => a1.Terms("my_terms_agg", t => t.Script(t1=>t1.Source("doc['deliveryType.keyword'].value + '|' + doc['deliveryTypeId.keyword'].value")).Size(802407))
                )
               );
                
                var agg = result.Aggregations.Terms("my_terms_agg").Buckets;
                

                foreach (var items in agg)
                {
                    obj.Add(new ddlValue
                    {
                        Id = items.Key.ToString().Split("|")[1],
                            Key = items.Key.ToString().Split("|")[0]
                    });
                    
                }
                 return Ok( new { results= obj });
              }
            catch (Exception ex)
            {
                 return BadRequest(ex.ToString());
              
            }
            }



        [System.Web.Http.HttpGet]
        [Route("api/ShotType")]
        public IActionResult getShotType()
        {
            try
            {
                List<ddlValue> obj = new List<ddlValue>();
                HttpResponseMessage response = null;
                EsClient = oLayer.CreateConnection();
                QueryContainer q1 = new TermQuery { Field = "isTagged", Value = "1" };
                QueryContainer q2 = new TermQuery { Field = "isAsset", Value = "0" };
                var q = new QueryContainer();
                q &= q2;
                q &= q1;
                var result = EsClient.Search<SearchCricketData>(a => a.Index("cricket").Size(0).Query(qs => qs)
                .Aggregations(a1 => a1.Terms("my_terms_agg", t => t.Script(t1 => t1.Source("doc['shotType.keyword'].value + '|' + doc['shotTypeId.keyword'].value")).Size(802407))
                )
               );
                var agg = result.Aggregations.Terms("my_terms_agg").Buckets;
                foreach (var items in agg)
                {
                    obj.Add(new ddlValue
                    {
                        Id = items.Key.ToString().Split("|")[1],
                        Key = items.Key.ToString().Split("|")[0]
                    });

                }
                return Ok(new { results = obj });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());

            }
        }


        [System.Web.Http.HttpGet]
        [Route("api/DeliveryTypeChange")]
        public IActionResult getDeliveryTypeOnChange(STFilterRequestData _objReqData)
        {
            try
            {
                //List<QueryContainer> myDynamicMustQuery = new List<QueryContainer>();
                //var boolMustQuery = new BoolQuery();
                List<ddlValue> obj = new List<ddlValue>();
                HttpResponseMessage response = null;
                EsClient = oLayer.CreateConnection();
                List<ResponseResult> value = new List<ResponseResult>();
                searchcricket sc = new searchcricket();
                QueryContainer _objNestedBoolQuery = new QueryContainer();
                if (_objReqData != null)
                {
                    MatchDetail _objMatchDetail = _objReqData.MatchDetail;
                    dynamic _objS1Data = _objReqData.S1Data;
                    _objNestedBoolQuery = sc.getCricketPlayerDetails(_objMatchDetail, _objNestedBoolQuery);
                }

                //QueryContainer q1 = new TermQuery { Field = "isTagged", Value = "1" };
                //QueryContainer q2 = new TermQuery { Field = "isAsset", Value = "0" };
                //QueryContainer q3 = new TermQuery { Field = "shotTypeId", Value = shotTypeId };

                //var q = new QueryContainer();
                //q &= q2;
                //q &= q1;
                //q &= q3;

                //myDynamicMustQuery.Add(q1);
                //myDynamicMustQuery.Add(q2);
                //myDynamicMustQuery.Add(q3);
                //boolMustQuery.Must = myDynamicMustQuery;

                var result = EsClient.Search<SearchCricketData>(a => a.Index("cricket").Size(0).Query(qs => _objNestedBoolQuery)
                .Aggregations(a1 => a1.Terms("my_terms_agg", t => t.Script(t1 => t1.Source("doc['deliveryType.keyword'].value + '|' + doc['deliveryTypeId.keyword'].value")).Size(802407))
                )
               );

                var agg = result.Aggregations.Terms("my_terms_agg").Buckets;


                foreach (var items in agg)
                {
                    obj.Add(new ddlValue
                    {
                        Id = items.Key.ToString().Split("|")[1],
                        Key = items.Key.ToString().Split("|")[0]
                    });

                }
                return Ok(new { results = obj });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());

            }
        }


        [System.Web.Http.HttpGet]
        [Route("api/ShotTypeChange")]
        public IActionResult getShotTypeOnChange(STFilterRequestData _objReqData)
        {
            try
            {
                
                List<ResponseResult> value = new List<ResponseResult>();
                searchcricket sc = new searchcricket();
                QueryContainer _objNestedBoolQuery = new QueryContainer();
                if (_objReqData != null) {
                    MatchDetail _objMatchDetail = _objReqData.MatchDetail;
                    dynamic _objS1Data = _objReqData.S1Data;
                    _objNestedBoolQuery = sc.getCricketPlayerDetails(_objMatchDetail, _objNestedBoolQuery);
                }
                //List<QueryContainer> myDynamicMustQuery = new List<QueryContainer>();
                //List<QueryContainer> myDynamicShouldQuery = new List<QueryContainer>();
                //var boolMustQuery = new BoolQuery();
                List<ddlValue> obj = new List<ddlValue>();
                HttpResponseMessage response = null;
                EsClient = oLayer.CreateConnection();
                //QueryContainer q1 = new TermQuery { Field = "isTagged", Value = "1" };
                //QueryContainer q2 = new TermQuery { Field = "isAsset", Value = "0" };
                
                //QueryContainer q3 = new TermQuery { Field = "isFour", Value = "1" };
                //QueryContainer q4 = new TermQuery { Field = "isSix", Value = "1" };
                //QueryContainer q5 = new TermQuery { Field = "isWicket", Value = "1" };
                //QueryContainer q6 = new TermQuery { Field = "isAppeal", Value = "1" };
                //QueryContainer q7 = new TermQuery { Field = "isDropped", Value = "1" };
                //QueryContainer q8 = new TermQuery { Field = "isMisField", Value = "1" };


                //QueryContainer q12 = new TermQuery { Field = "deliveryTypeId", Value = deliveryTypeId };
                //QueryContainer q9 = new TermQuery { Field = "clearId.keyword", Value = "HDP000015411--20160224094449152--CS--1014" };
                ////var q = new QueryContainer();
                ////q &= q2;
                ////q &= q1;
                ////q &= q3;

                //myDynamicMustQuery.Add(q1);
                //myDynamicMustQuery.Add(q2);
                //myDynamicMustQuery.Add(q9);
                //QueryContainer qs= q3 |= q4|=q5 |=q6|=q7|=q8;
                //myDynamicShouldQuery.Add(q4);
                //myDynamicShouldQuery.Add(q5);
                //myDynamicShouldQuery.Add(q6);
                //myDynamicShouldQuery.Add(q7);
                //myDynamicShouldQuery.Add(q8);


                //QueryContainer qr = q1 &= q2 &= q9 &= q12;//&= qs;
                    //boolMustQuery.Should = myDynamicShouldQuery;
                var result = EsClient.Search<SearchCricketData>(a => a.Index("cricket").Size(0).Query(s => _objNestedBoolQuery)
                .Aggregations(a1 => a1.Terms("my_terms_agg", t => t.Script(t1 => t1.Source("doc['shotType.keyword'].value + '|' + doc['shotTypeId.keyword'].value")).Size(802407))
                )
               );
                var agg = result.Aggregations.Terms("my_terms_agg").Buckets;
                foreach (var items in agg)
                {
                    obj.Add(new ddlValue
                    {
                        Id = items.Key.ToString().Split("|")[1],
                        Key = items.Key.ToString().Split("|")[0]
                    });

                }
                return Ok(new { results = obj });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());

            }
        }

        [System.Web.Http.HttpGet]
        [Route("api/batsman/{keyword}")]
        public IActionResult AutocompleteBatsman(string keyword)
        {
            try
            {
                List<ddlValue> value = new List<ddlValue>();
                EsClient = oLayer.CreateConnection();
                
                var response = EsClient.Search<SearchCricketData>(s => s.Index("cricket").Size(0).
                Query(q => q.Wildcard(c => c.Name("named_query").Field(f => f.Batsman).Value(keyword + "*"))).Size(10)
                .Aggregations(a => a.Terms("my_agg", st => st.Script(p=>p.Source("doc['batsmanId.keyword'].value + '|' + doc['batsman.keyword'].value")).Size(802407))));
                var agg = response.Aggregations.Terms("my_agg").Buckets;
                
                foreach (var hit in agg)
                {
                    value.Add(new ddlValue
                    {
                        Id=hit.Key.ToString().Split("|")[0],
                        Key = hit.Key.ToString().Split("|")[1],
                   });
                }
                
            return Ok(new { results = value });
            }
            catch (Exception ex) {
                return BadRequest(ex.Message.ToString());
            }
        }



        [System.Web.Http.HttpGet]
        //[Route("api/bowler/{keyword}")]
        [Route("api/bowler")]
        public IActionResult AutocompleteBowler(string keyword)
        {
            try
            {
                List<ddlValue> value = new List<ddlValue>();
                EsClient = oLayer.CreateConnection();

                var response = EsClient.Search<SearchCricketData>(s => s.Index("cricket").Size(0).
                Query(q => q.Wildcard(c => c.Name("named_query").Field(f => f.Bowler).Value(keyword + "*"))).Size(10)
                .Aggregations(a => a.Terms("my_agg", st => st.Script(p=>p.Source("doc['bowlerId.keyword'].value + '|' + doc['bowler.keyword'].value")).Size(802407))));
                //  .Aggregations(a1 => a1.Terms("my_terms_agg", t => t.Script(t1 => t1.Source("doc['shotType.keyword'].value + '|' + doc['shotTypeId.keyword'].value")).Size(802407))

                var agg = response.Aggregations.Terms("my_agg").Buckets;

                foreach (var hit in agg)
                {
                    value.Add(new ddlValue
                    {
                        Id = hit.Key.ToString().Split("|")[0],
                        Key = hit.Key.ToString().Split("|")[1]
                    });
                }

                return Ok(new { results = value });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }
        }

        [System.Web.Http.HttpGet]
        [Route("api/Innings")]
        public IActionResult getInnings(string keyword)
        {
            try
            {
                List<ddlValue> obj = new List<ddlValue>();
                for (int i = 1; i <= 2; i++)
                {
                    obj.Add(new ddlValue
                    {
                        Id = i.ToString(),
                        Key = i.ToString(),
                    });
                }

                return Ok(new { results = obj });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }
        }


        [System.Web.Http.HttpGet]
        //[Route("api/Segments/{bowler}/{isFour}")]
        [Route("api/SegmentsTest")]
        public IActionResult postSegments(STFilterRequestData _objReqData) {
            try
            {
                List<ResponseResult> value = new List<ResponseResult>();
                searchcricket sc = new searchcricket();
                QueryContainer _objNestedBoolQuery = new QueryContainer();
               // EsClient = oLayer.CreateConnection();
                if (_objReqData != null) {
                    MatchDetail _objMatchDetail = _objReqData.MatchDetail;
                    dynamic _objS1Data = _objReqData.S1Data;
                    Moments _objMomentsData = _objReqData.Moments;
                    if (_objMatchDetail != null) {
                        _objNestedBoolQuery = sc.GetMatchDetailQueryST(_objNestedBoolQuery, _objMatchDetail);
                    }
                    if (_objS1Data != null)
                    {
                        _objNestedBoolQuery =sc.getCricketPlayerDetails(_objS1Data, _objNestedBoolQuery);
                        _objNestedBoolQuery = sc.GetCricketMatchSituationQueryST(_objNestedBoolQuery, _objS1Data);
                        //_objSearchResults.ResultData = LSObject.SearchStoryTeller(_objNestedBoolQuery, _objMatchDetail);

                    }
                    if (_objMomentsData != null)
                    {
                        if (!string.IsNullOrEmpty(_objMomentsData.Entities) || _objMomentsData.IsBigMoment || _objMomentsData.IsFunnyMoment || _objMomentsData.IsAudioPiece)
                        {
                            QueryContainer _objNestedBoolQueryFor2 = new QueryContainer();
                            _objNestedBoolQueryFor2 = sc.GetMatchDetailQueryST(_objNestedBoolQueryFor2, _objMatchDetail);
                            _objNestedBoolQueryFor2 = sc.GetS2MomentDetailQueryForST(_objMatchDetail, _objNestedBoolQueryFor2, _objMomentsData);
                            // if (_objLstSearchQuery.Count > 3)///If the count is more than matchdetail query list (need to rethink)
                            //{
                            //    _objSearchResults2.ResultData = LuceneService.FetchSearchResultFilterData(_objNestedBoolQueryFor2, sportid, "s2");
                            //}
                        }
                    }
                }
                
                    return Ok(new { results = value });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }

        }





        [System.Web.Http.HttpGet]
        [Route("api/segments")]
        public IActionResult getSegments(STFilterRequestData _objReqData)
        {

            try
            {
                EsClient = oLayer.CreateConnection();
                List<SearchResultFilterData> obj = new List<SearchResultFilterData>();
                // var items="";
                STFilterRequestData objReqData = new STFilterRequestData();
                searchcricket sc = new searchcricket();
                QueryContainer _objNestedQuery = new QueryContainer();
                if (_objReqData != null)
                {
                    MatchDetail _objMatchDetail = _objReqData.MatchDetail;
                    dynamic _objS1Data = _objReqData.S1Data;
                    //Moments _objMomentsData = _objReqData.Moments;
                    if (_objMatchDetail != null)
                    {
                        
                        //_objNestedBoolQuery = sc.GetMatchDetailQueryST(_objNestedBoolQuery, _objS1Data);
                        //System.Web.Script.Serialization.JavaScriptSerializer _objSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                        string jsonData = JsonConvert.SerializeObject(_objS1Data);
                        dynamic data = JsonConvert.DeserializeObject(jsonData);
                        //SearchCricketData empObj = JsonConvert.DeserializeObject<SearchCricketData>(jsonData);
                        _objNestedQuery = sc.getCricketPlayerDetails(data, _objNestedQuery);
                        var result = EsClient.Search<SearchCricketData>(s => s.Index("cricket").Query(q=> _objNestedQuery).Size(10000));

                        foreach (var hit in result.Hits)
                        {
                            obj.Add(new SearchResultFilterData
                            {
                                Description = hit.Source.Description.ToString(),
                                MarkIn = hit.Source.MarkIn.ToString(),
                                MarkOut = hit.Source.MarkOut.ToString(),
                                ShotType = hit.Source.MarkOut.ToString(),
                                DeliveryType = hit.Source.DeliveryType.ToString()
                            });
                        }
                         

                    }
                }
                

                return Ok(new { results = obj });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());
            }

        }



        [System.Web.Http.HttpGet]
        [Route("api/offField")]
        public IActionResult getOffField(STFilterRequestData _objReqData)
        {
            searchcricket sc = new searchcricket();
            List<SearchResultFilterData> obj = new List<SearchResultFilterData>();
            BoolQuery _objNestedBoolQuery = new BoolQuery();
            if (_objReqData != null)
            {
                MatchDetail _objMatchDetail = _objReqData.MatchDetail;
                Moments _objMomentsData = _objReqData.Moments;
                if (_objMomentsData != null)
                {
                    if (_objMomentsData.IsBigMoment || _objMomentsData.IsFunnyMoment || _objMomentsData.IsAudioPiece)
                    {
                        QueryContainer _objNestedBoolQueryFor2 = new QueryContainer();
                        _objNestedBoolQueryFor2 = sc.GetMatchDetailQueryST(_objNestedBoolQueryFor2, _objMatchDetail);
                        _objNestedBoolQueryFor2 = sc.GetS2MomentDetailQueryForST(_objMatchDetail, _objNestedBoolQueryFor2, _objMomentsData);
                        var result = EsClient.Search<SearchResultFilterData>(s => s.Index("cricket").Query(q => _objNestedBoolQueryFor2).Size(10000));
                        foreach (var hit in result.Hits)
                        {
                            obj.Add(new SearchResultFilterData
                            {
                                Description = hit.Source.Description.ToString(),
                                MarkIn = hit.Source.MarkIn.ToString(),
                                MarkOut = hit.Source.MarkOut.ToString(),
                                ShotType = hit.Source.MarkOut.ToString(),
                                DeliveryType = hit.Source.DeliveryType.ToString()
                            });
                        }
                    }
                }
            }
            return Ok(new { results = obj });
        }


        [System.Web.Http.HttpPost]
        [Route("api/GetSearchResultForFilters")]
        public IActionResult GetSearchResultsFilter(STFilterRequestData _objReqData)
        {
            ExtendedSearchResultFilterData _objSearchResults = new ExtendedSearchResultFilterData();
            ExtendedSearchResultFilterData _objSearchResults2 = new ExtendedSearchResultFilterData();
            ExtendedSearchResultFilterData _objResult = new ExtendedSearchResultFilterData();
            searchcricket sc = new searchcricket();
            _objResult.ResultData = new List<SearchResultFilterData>();
            _objSearchResults2.ResultData = new List<SearchResultFilterData>();
            _objSearchResults.ResultData = new List<SearchResultFilterData>();
            _objResult.Master = new MasterDatas();
            _objResult.Master.MasterData = new Dictionary<string, object>();
            _objSearchResults.Master = new MasterDatas();
            _objSearchResults.Master.MasterData = new Dictionary<string, object>();
            QueryContainer _objNestedQuery = new QueryContainer();
            QueryContainer _objCricketResult = new QueryContainer();
            if (_objReqData != null)
            {
                MatchDetail _objMatchDetail = _objReqData.MatchDetail;
                int sportid = _objMatchDetail.SportID;
                dynamic _objS1Data = _objReqData.S1Data;
                MatchSituation _objMatchSituation = _objReqData.MatchSituation;
                Moments _objMomentsData = _objReqData.Moments;
                if (_objMatchDetail != null)
                {
                    _objNestedQuery = sc.GetMatchDetailQueryST(_objNestedQuery, _objMatchDetail);
                }
                if (_objS1Data != null)
                {

                    if ("CRICKET" == sc.getType(sportid)) {
                        IEnumerable<SearchResultFilterData> _objSearchResultsFilterData = new List<SearchResultFilterData>();
                        //_objNestedQuery = sc.getCricketPlayerDetails( _objS1Data, _objNestedQuery);
                        //_objNestedQuery = sc.GetCricketMatchSituationQueryST(_objNestedQuery, _objMatchSituation);
                        //_objSearchResults.ResultData = LSObject.SearchStoryTeller(_objNestedBoolQuery, _objMatchDetail);
                        if (!_objMatchDetail.IsAsset)
                        {
                            Cricket objCs = new Cricket();
                            EsClient = oLayer.CreateConnection();
                            //_objCricketResult = sc.GetMatchDetailQueryST(_objCricketResult, _objMatchDetail);
                            _objCricketResult = sc.getCricketPlayerDetails(_objS1Data, _objNestedQuery);
                            _objCricketResult = sc.GetCricketMatchSituationQueryST(_objNestedQuery, _objMatchSituation);
                            CommonFunction cf = new CommonFunction();
                            GetSearchS1DataForCricket sqObj = new GetSearchS1DataForCricket();
                            //string value = sqObj.GetKeyValueForSport(sc.getType(sportid), "DropdwonKey");
                            QueryContainer queryShould = new QueryContainer();
                            QueryContainer queryAnd = new QueryContainer();
                            QueryContainer queryAnd_should = new QueryContainer();
                            //List<string> valueObj = sqObj.GetKeyValueForSportTemp(sc.getType(sportid), "PlayerDetails");
                            //string[] valuessObj = valueObj.ToList();
                            //for (int i = 0; i <= valueObj.Count-1; i++)
                            //{
                            //    string sType = valueObj[i].Split(",")[1];
                            //    if (sType == "Boolean") {
                            //        if (Convert.ToBoolean(_objS1Data[valueObj[i].Split(":")[1].Split(",")[0]]))
                            //        {
                            //            QueryContainer query1 = new TermQuery { Field = valueObj[i].Split(",")[2], Value = "1" };
                            //            queryShould |= query1;
                            //        }
                            //    }
                            //    if (sType == "string") {
                            //        if (Convert.ToString(_objS1Data[valueObj[i].Split(":")[1]]) != "")
                            //        {
                            //            string slist = Convert.ToString(_objS1Data[valueObj[i].Split(",")[0].Split(":")[1]]);
                            //            if (slist.Contains(","))
                            //            {
                            //                string[] strArray = slist.Split(',');
                            //                foreach (string str in strArray)
                            //                {
                            //                    QueryContainer query9 = new TermQuery { Field = valueObj[i].Split(",")[2], Value = str };
                            //                    queryShould |= query9;
                            //                }
                            //            }
                            //            else {
                            //                QueryContainer query10 = new TermQuery { Field = valueObj[i].Split(",")[2], Value = Convert.ToString(_objS1Data[valueObj[i].Split(",")[0].Split(":")[1]]) };
                            //                queryAnd &= query10;

                            //            }
                            //        }

                            //        }
                            //    if (valueObj[i].Split(":")[0] == "OR") {
                            //        queryAnd_should &= queryShould;
                            //    }

                            //}
                            //queryAnd &= queryAnd_should;

                            //string[] valuess = value.Split(",");
                            //foreach (var items in valuess)
                            //{
                            //    var item = items.Split("::");
                            //    string Type = _objS1Data[item[0]];
                            //    string[] _objType = Type.Contains(",") ? _objType = Type.Split(",") : _objType = new string[] { Type };
                            //    _objSearchResults.Master.MasterData = objCs.fetchDropdowns(_objNestedQuery, _objSearchResults.Master.MasterData, EsClient, "cricket", cf.GetColumnForEntity(Convert.ToInt16(item[1])), _objType);
                            //}

                            //if (!string.IsNullOrEmpty(_objMatchSituation.Innings))
                            //{
                            //    List<FilteredEntityData> obj2 = new List<FilteredEntityData>();
                            //    Dictionary<string, object> objInning = new Dictionary<string, object>();
                            //    string[] _objReqInnings = _objMatchSituation.Innings.Contains(",") ? _objReqInnings = _objMatchSituation.Innings.Split(',') : _objReqInnings = new string[] { _objMatchSituation.Innings };

                            //    for (int i = 1; i <= 2; i++)
                            //    {
                            //        obj2.Add(new FilteredEntityData
                            //        {
                            //            EntityId = i.ToString(),
                            //            EntityName = i.ToString(),
                            //            IsSelectedEntity = _objReqInnings.Contains(i.ToString()) ? 1 : 0
                            //        });
                            //    }
                            //    objInning.Add("Innings", obj2);
                            //     _objSearchResults.Master.MasterData = objInning;
                            //}


                            string[] _objReqInnings = _objMatchSituation.Innings.Contains(",") ? _objReqInnings = _objMatchSituation.Innings.Split(',') : _objReqInnings = new string[] { _objMatchSituation.Innings };
                            string ReqShotType = _objS1Data["ShotType"];
                            string ReqDeliveryType = _objS1Data["DeliveryType"];
                            string[] _objReqShotType = ReqShotType.Contains(",") ? _objReqShotType = ReqShotType.Split(",") : _objReqShotType = new string[] { _objS1Data["ShotType"] };
                            string[] _objReqDeliveryType = ReqDeliveryType.Contains(",") ? _objReqDeliveryType = ReqDeliveryType.Split(",") : _objReqDeliveryType = new string[] { _objS1Data["DeliveryType"] };
                            _objSearchResults.Master.MasterData = sc.parseDropdown(_objCricketResult, _objResult.Master.MasterData, EsClient, _objReqInnings, _objReqShotType, _objReqDeliveryType);
                            _objResult.ResultData = sc.getSegementsData(EsClient, _objCricketResult);
                            _objSearchResults.ResultData = _objResult.ResultData;
                        }
                        else {
                            _objNestedQuery = sc.getCricketPlayerDetails(_objS1Data, _objNestedQuery);
                            _objNestedQuery = sc.GetCricketMatchSituationQueryST(_objNestedQuery, _objMatchSituation);

                        }
                        if (_objMomentsData != null)
                        {
                            if (!string.IsNullOrEmpty(_objMomentsData.Entities) || _objMomentsData.IsBigMoment || _objMomentsData.IsFunnyMoment || _objMomentsData.IsAudioPiece)
                            {
                                QueryContainer _objNestedBoolQueryFor2 = new QueryContainer();
                                _objNestedBoolQueryFor2 = sc.GetMatchDetailQueryST(_objNestedBoolQueryFor2, _objMatchDetail);
                                _objNestedBoolQueryFor2 = sc.GetS2MomentDetailQueryForST(_objMatchDetail, _objNestedBoolQueryFor2, _objMomentsData);
                                _objSearchResults2.ResultData = sc.getSegementsData(EsClient, _objNestedBoolQueryFor2);

                            }
                            _objResult.ResultData = _objSearchResults.ResultData.Union(_objSearchResults2.ResultData);
                        }
                    }



                        
                }
                }

            return Ok(new { responseText = _objSearchResults });

        }
        [System.Web.Http.HttpPost]
        [Route("api/getfilterbysportTemp")]
        public IActionResult GetFilteredEntitiesBySport(SearchEntityRequestData _objReqData)
        {

            searchcricket sc = new searchcricket();
            int _sportid = 1;
            string result = string.Empty;
            string searchtext = string.Empty;
            List<SearchQueryModel> _objLstSearchQuery = new List<SearchQueryModel>();
            var responseResult = new List<FilteredEntityForCricket>();
            SearchEntityRequestData _objEntityReqData;
            string jsonData = JsonConvert.SerializeObject(_objReqData);
            _objEntityReqData = JsonConvert.DeserializeObject<SearchEntityRequestData>(jsonData);
            
            if (_objEntityReqData  != null) {
                EsClient = oLayer.CreateConnection();
                Dictionary<string, string> _columns = sc.GetColumnForEntity(_objEntityReqData.EntityTypeId);
                _sportid = _objEntityReqData.SportId;
                searchtext = _objEntityReqData.EntityText.Trim().ToLower();
                MatchDetail _objMatchDetail = _objEntityReqData.MatchDetails;
                _objMatchDetail.SeriesId = _objEntityReqData.EntityTypeId != 5 ? _objMatchDetail.SeriesId : string.Empty;
                _objMatchDetail.MatchId = _objEntityReqData.EntityTypeId != 9 ? _objMatchDetail.MatchId : string.Empty;

                if (_columns != null && _columns.Count > 0)
                {
                    KeyValuePair<string, string> _column = _columns.FirstOrDefault();
                    //Following query is to search the entire match detail
                    if (_columns.Count > 0)
                    {

                        {
                            List<string> EntityIds = new List<string>();
                            List<string> EntityNames = new List<string>();
                            foreach (var col in _columns)
                            {
                                EntityIds.Add(col.Key);
                                EntityNames.Add(col.Value);

                            }
                            QueryContainer _objNestedQuery = new QueryContainer();
                            QueryContainer _objNestedQueryRresult = new QueryContainer(); 
                            IEnumerable<FilteredEntityForCricket> _objFilteredEntityForCricket = new List<FilteredEntityForCricket>();

                            _objNestedQuery = sc.GetMatchDetailQueryST( _objNestedQuery, _objMatchDetail);
                            _objNestedQuery = sc.GetPlayerDetailQueryForFilteredEntityBySport(_objNestedQuery, _objEntityReqData.playerDetails, _objMatchDetail.SportID);//GetCricketPlayerDetailQuery(_objEntityReqData.playerDetails, _objNestedBoolQuery);
                            if (!string.IsNullOrEmpty(_objMatchDetail.MatchDate))
                            {
                                string dlist = _objMatchDetail.MatchDate;
                                if (dlist.Contains("-"))
                                {
                                    string[] strNumbers = dlist.Split('-');
                                    int start = int.Parse(DateTime.ParseExact(strNumbers[0], "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd"));
                                    int End = int.Parse(DateTime.ParseExact(strNumbers[1], "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd"));
                                    _objNestedQuery =  sc.GetFilteredEntitiesBySport(_objMatchDetail, _objNestedQuery,  "1", start, _columns, searchtext);
                                   }
                                else
                                {
                                    int date = int.Parse(DateTime.ParseExact(_objMatchDetail.MatchDate, "dd/MM/yyyy", CultureInfo.InvariantCulture).ToString("yyyyMMdd"));
                                    _objNestedQuery = sc.GetFilteredEntitiesBySport(_objMatchDetail, _objNestedQuery, "2", date, _columns, searchtext);
                                }
                            }
                            //if (_objEntityReqData.EntityTypeId == 5)
                            //{
                            //    var s1 = _objFilteredEntityForCricket.Select(d => new { EntityId = d.GetType().GetProperty(EntityIds.ElementAt(1)).GetValue(d, null), EntityName = d.GetType().GetProperty(EntityNames.ElementAt(1)).GetValue(d, null), IsParentSeries = 1 }).Distinct().ToList();
                            //    var s2 = _objFilteredEntityForCricket.Select(d => new { EntityId = d.GetType().GetProperty(EntityIds.ElementAt(0)).GetValue(d, null), EntityName = d.GetType().GetProperty(EntityNames.ElementAt(0)).GetValue(d, null), IsParentSeries = 0 }).Distinct().ToList();
                            //    var s = s1.Union(s2).Where(r => r.EntityName.ToString() != "" && r.EntityName.ToString().ToLower().Contains(searchtext)).Distinct().OrderBy(r => r.EntityName);
                            //    //result = _objSerializer.Serialize(s);

                            //}
                            //if (_objEntityReqData.EntityTypeId == 3 || _objEntityReqData.EntityTypeId == 4)
                            //{
                            //    var s1 = _objFilteredEntityForCricket.Select(t => new { EntityId = t.Team1Id, EntityName = t.Team1 }).Distinct();
                            //    var s2 = _objFilteredEntityForCricket.Select(t => new { EntityId = t.Team2Id, EntityName = t.Team2 }).Distinct();
                            //    var s = s1.Union(s2).Where(r => r.EntityName.ToString() != "" && r.EntityName.ToLower().Contains(searchtext)).Distinct().OrderBy(r => r.EntityName);
                            //    //result = _objSerializer.Serialize(s);


                            //}
                            //else
                            if (searchtext != "")
                            {
                                //sc.GetFilteredEntitiesBySportResult(_objNestedQuery, EntityIds.ElementAt(0), EntityNames.ElementAt(0),EsClient);
                                //var s = _objFilteredEntityForCricket.Select(d => new { EntityId = d.GetType().GetProperty(EntityIds.ElementAt(0)).GetValue(d, null), EntityName = d.GetType().GetProperty(EntityNames.ElementAt(0)).GetValue(d, null) }).Distinct();
                                var Result = sc.GetFilteredEntitiesBySportResult(_objNestedQuery, EntityIds.ElementAt(0), EntityNames.ElementAt(0), EsClient, searchtext);
                                //result = _objSerializer.Serialize(Result);
                                responseResult = Result;

                            }
                            _objFilteredEntityForCricket = null;

                        }
                    }
                }

                }
            return Ok(new { responseText = responseResult });

        }



        //[System.Web.Http.HttpGet]
        //[Route("api/Testt")]
        //public IActionResult Test(dynamic _objS1Data, QueryContainer _objNestedQuery)
        //{
        //    EsClient = oLayer.CreateConnection();
        //    Dictionary<string, object> dicData = new Dictionary<string, object>();
        //    CommonFunction cf = new CommonFunction();
        //    GetSearchS1DataForCricket sqObj = new GetSearchS1DataForCricket();
        //    string value = sqObj.GetKeyValueForSport("CRICKET", "DropdwonKey");
        //    string[] valuess = value.Split(",");
        //    foreach (var items in valuess) {
        //        string Type = _objS1Data[items];
        //        string[] _objType = Type.Contains(",") ? _objType = Type.Split(",") : _objType = new string[] { Type };
        //        dicData = cf.GetDropdowns(_objNestedQuery, dicData, EsClient, "cricket", cf.GetColumnForEntity(13), _objType);
        //    }
        //    return Ok(new { responseText = value });
        //}

        //[System.Web.Http.HttpGet]
        //[Route("api/autocomplete")]
        //public IActionResult GetFilteredEntitiesBySport(STFilterRequestData _objReqData)
        //{
        //    int _sportid = 1;
        //    string result = string.Empty;
        //    string searchtext = string.Empty;
        //    searchcricket sc = new searchcricket();
        //    SearchEntityRequestData _objEntityReqData;
        //    string jsonData = JsonConvert.SerializeObject(_objReqData);
        //    _objEntityReqData = JsonConvert.DeserializeObject<SearchEntityRequestData>(jsonData);
        //    if (_objReqData != null)
        //    {
        //        Dictionary<string, string> _columns = sc.GetColumnForEntity(_objEntityReqData.EntityTypeId);
        //        _sportid = _objEntityReqData.SportId;
        //        searchtext = _objEntityReqData.EntityText.Trim().ToLower();
        //        MatchDetail _objMatchDetail = _objEntityReqData.MatchDetails;
        //        _objMatchDetail.SeriesId = _objEntityReqData.EntityTypeId != 5 ? _objMatchDetail.SeriesId : string.Empty;
        //        _objMatchDetail.MatchId = _objEntityReqData.EntityTypeId != 9 ? _objMatchDetail.MatchId : string.Empty;
        //        if (_columns != null && _columns.Count > 0)
        //        {
        //            KeyValuePair<string, string> _column = _columns.FirstOrDefault();
        //            //Following query is to search the entire match detail
        //            if (_columns.Count > 0)
        //            {

        //                {
        //                    List<string> EntityIds = new List<string>();
        //                    List<string> EntityNames = new List<string>();
        //                    foreach (var col in _columns)
        //                    {
        //                        EntityIds.Add(col.Key);
        //                        EntityNames.Add(col.Value);

        //                    }
        //                }
        //            }
        //            QueryContainer _objCricketResult = new QueryContainer();
        //            IEnumerable<FilteredEntityForCricket> _objFilteredEntityForCricket = new List<FilteredEntityForCricket>();
        //            _objCricketResult = sc.GetMatchDetailQueryST(_objCricketResult, _objMatchDetail);
        //            _objCricketResult = sc.GetPlayerDetailQueryForFilteredEntityBySport(_objCricketResult, _objEntityReqData.playerDetails, _objMatchDetail.SportID);

        //        //    var response = EsClient.Search<SearchCricketData>(s => s.Index("cricket").Size(0).
        //        //Query(q => q.Wildcard(c => c.Name("named_query").Field(f => f.Batsman).Value(keyword + "*"))).Size(10)
        //        //.Aggregations(a => a.Terms("my_agg", st => st.Script(p => p.Source("doc['batsmanId.keyword'].value + '|' + doc['batsman.keyword'].value")).Size(802407))));
        //        //    var agg = response.Aggregations.Terms("my_agg").Buckets;

        //            //foreach (var hit in agg)
        //            //{
        //            //    value.Add(new ddlValue
        //            //    {
        //            //        Id = hit.Key.ToString().Split("|")[0],
        //            //        Key = hit.Key.ToString().Split("|")[1],
        //            //    });
        //            //}

        //        }
        //    }
        //}
        #region "commented Code"

        // var response = EsClient.Search<SearchCricketData>(s => s.Index("cricketindex")
        // .Aggregations(a => a.Terms("my_agg", st => st.Field(o => o.Batsman.Suffix("keyword"))))
        //  .Suggest(su => su
        // .Completion("suggestions", c => c
        // .Field(fe => fe.CF_Batsman)

        // .Prefix(keyword)
        // .Fuzzy(f => f
        // .Fuzziness(Fuzziness.Auto)
        // )
        //// .Size(10000)
        // ))
        // //.Aggregations(a => a.Terms("my_agg", st => st.Field(o => o.Batsman.Suffix("keyword"))))
        // );
        [System.Web.Http.HttpGet]
        [Route("api/getS2Data")]
        public IActionResult GetS2Data()
        {


            EsClient = oLayer.CreateConnection();
            List<SearchS2Data> obj2 = new List<SearchS2Data>();
            List<SearchS2Data> obj3 = new List<SearchS2Data>();

            string connection = obj.GetConnectionString("ConnectionStrings", "DefaultConnection");
            Dictionary<string, object> column = new Dictionary<string, object>();
            obj2 =obj.GetAllSearchS2Data(connection,1,true);
            obj3 = obj2.ToList();
            //column.Add("S2Data", obj2);
            try
            {
              //  dynamic DyObj = new List<SearchS2Data>();
                //obj3.RemoveRange(1, 10000);
                for (int i = 1; i <= 45; i++)
                {
                    obj3 = obj3.Take(20000).ToList();
                    if (obj3.Count>0)
                    {
                        oLayer.BulkInsert(EsClient, obj3);
                    }
                    if (obj3.Count > 20000) {
                        obj3.RemoveRange(1, 20000);
                    }
                    }
                    //    var listOfObj = obj3.Take(10000);
                    //    column.Add("S2Data", listOfObj);
                    //    column.Remove("S2Data");
                    //    column.
                    //    //listOfObj.r
                    //    //var deleteList = listOfObj.RemoveRange(3, 2);



                    //}
                    //return Request.CreateResponse(HttpStatusCode.Created, "Index Created successfully.");
                    return Ok(new  { Result = obj2});
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message.ToString());
                //return BadRequest(ex.ToString());
                // return request.CreateResponse(HttpStatusCode.BadRequest, "Invalid operation!");
            }





            //}
            //[System.Web.Http.HttpGet]
            //[System.Web.Http.HttpGet]
            //[Route("api/DeliveryType")]
            //public HttpResponseMessage getDeliveryType(HttpRequestMessage request)
            //{


            //    try
            //    {
            //        HttpResponseMessage response = null;
            //        EsClient = oLayer.CreateConnection();
            //        QueryContainer q1 = new TermQuery { Field = "isTagged", Value = "1" };
            //        QueryContainer q2 = new TermQuery { Field = "IsAsset", Value = "0" };
            //        var q = new QueryContainer();

            //        q &= q2;
            //        q &= q1;
            //        var result = EsClient.Search<SearchCricketData>(a => a.Index("cricket").Size(0).Query(qs => qs).Aggregations(a1 => a1.Terms("my_terms_agg", t => t.Field(p => p.DeliveryType.Suffix("keyword")).Field(p => p.DeliveryTypeId.Suffix("keyword")).Size(802407))));
            //        var agg = result.Aggregations.Terms("my_terms_agg");

            //        return ;
            //    }
            //    catch (Exception ex)
            //    {

            //        //return Request.
            //        //return Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid operation!");
            //    }


            //}
            // var result = EsClient.Search<SearchCricketData>(a => a.Index("cricket").Size(0).Query(qs => qs)
            // .Aggregations(a1 => a1.Terms("my_terms_agg", t => t.Field(p => p.DeliveryType.Suffix("keyword"))
            // .Size(802407)
            // //.Aggregations(a2 => a2.Terms("my_terms_agg2", t1 => t1.Field(p1 => p1.DeliveryTypeId.Suffix("keyword")).Size(802407))
            //).Terms("my_terms_agg2", t2=>t2.Field(po=>po.DeliveryTypeId.Suffix("keyword")).Size(802407))));



            //.Size(802407)
            //.Aggregations(a2 => a2.Terms("my_terms_agg2", t1 => t1.Field(p1 => p1.DeliveryTypeId.Suffix("keyword")).Size(802407))

            //.Terms("my_terms_agg2", t2 => t2.Field(po => po.DeliveryTypeId.Suffix("keyword")).Size(802407)
            //var result = EsClient.Search<SearchCricketData>(a => a.Index("cricket").Size(0).Query(qs => qs)
            //.Aggregations(a1 => a1.Terms("my_terms_agg", t => t.Script(a2=>a2.Source()).Size(802407))
            ////(p => p.DeliveryType.Suffix("keyword")).Size(802407)
            //));
            //var result1 = EsClient.Search<SearchCricketData>(p => p.Index("cricket").Size(0).Query(qs => qs).Aggregations(a => a.Terms("my_terms_aggss", u=>u.Script("doc['deliveryType']").Size(1000))));
            //var aggsssss = result.Aggregations.Terms("my_terms_aggss").Buckets;
            //var aggs = result.Aggregations.Terms("my_terms_agg2").Buckets;
            #endregion








        }
        }
}