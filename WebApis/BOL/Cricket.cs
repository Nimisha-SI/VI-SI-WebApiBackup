using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nest;
using WebApis.elastic;
using WebApis.Model;
using static WebApis.Model.ELModels;

namespace WebApis.BOL
{
    //private static ElasticClient EsClient;
   
    public class Cricket : AbstractClasses 
    {
        EsLayer oLayer = new EsLayer();

        public override Dictionary<string, object> fetchDropDownForMatch(Dictionary<string, object> ObjectArray, string[] sInnings)
        {
            List<FilteredEntityData> obj2 = new List<FilteredEntityData>();
            for (int i = 1; i <= 2; i++)
            {
                obj2.Add(new FilteredEntityData
                {
                    EntityId = i.ToString(),
                    EntityName = i.ToString(),
                    IsSelectedEntity = sInnings.Contains(i.ToString()) ? 1 : 0
                });
            }
            ObjectArray.Add("Innings", obj2.AsEnumerable());
            return ObjectArray;
        }

        public  Dictionary<string, object> fetchDropdowns(QueryContainer _objNestedQuery, Dictionary<string, object> ObjectArray, ElasticClient EsClient, string IndexName, Dictionary<string, string> _columns, string[] sFilterArray)
        {
            //IEnumerable<FilteredEntityData> _objSearchResultsFilterDataT;
            //IEnumerator _objSearchResultsFilterDataT;
            IEnumerable<SearchResultFilterData> _objSearchResultsFilterData = new List<SearchResultFilterData>();
            List<SearchResultFilterData> _objSearchResultFilterData = new List<SearchResultFilterData>();
            List<FilteredEntityData> obj = new List<FilteredEntityData>();

            if (_columns != null && _columns.Count > 0)
            {
                KeyValuePair<string, string> _column = _columns.FirstOrDefault();
                List<string> EntityIds = new List<string>();
                List<string> EntityNames = new List<string>();
                foreach (var col in _columns)
                {
                    EntityIds.Add(col.Key);
                    EntityNames.Add(col.Value);

                }
                if (EntityNames.ElementAt(0) == "BowlingArm")
                {
                    var result = EsClient.Search<SearchCricketData>(a => a.Index(IndexName).Size(0).Query(s => _objNestedQuery)
                                   .Aggregations(a1 => a1.Terms("terms_agg", t => t.Script(t1 => t1.Source("doc['" + EntityNames.ElementAt(0) + ".keyword'].value")).Size(409846)) //crickets2-802407
                                   )
                                  );
                    var agg = result.Aggregations.Terms("terms_agg").Buckets;
                    foreach (var items in agg)
                    {
                        obj.Add(new FilteredEntityData
                        {
                            EntityId = items.Key.ToString().Split("|")[1],
                            EntityName = items.Key.ToString().Split("|")[0],
                            IsSelectedEntity = sFilterArray.Contains(items.Key.ToString().Split("|")[1]) ? 1 : 0
                        });
                    }
                }
                else {
                    var result = EsClient.Search<SearchCricketData>(a => a.Index(IndexName).Size(0).Query(s => _objNestedQuery)
               .Aggregations(a1 => a1.Terms("terms_agg", t => t.Script(t1 => t1.Source("doc['" + EntityNames.ElementAt(0) + ".keyword'].value + '|' + doc['" + EntityIds.ElementAt(0) + ".keyword'].value")).Size(409846)) //crickets2-802407
               )
              );
                    var agg = result.Aggregations.Terms("terms_agg").Buckets;
                    foreach (var items in agg)
                    {
                        obj.Add(new FilteredEntityData
                        {
                            EntityId = items.Key.ToString().Split("|")[1],
                            EntityName = items.Key.ToString().Split("|")[0],
                            IsSelectedEntity = sFilterArray.Contains(items.Key.ToString().Split("|")[1]) ? 1 : 0
                        });
                    }
                }
               

                // var result = EsClient.Search<SearchCricketData>(a => a.Index(IndexName).Size(0).Query(s => _objNestedQuery)
                // .Aggregations(a1 => a1.Terms("terms_agg", t => t.Script(t1 => t1.Source("doc['shotType.keyword'].value + '|' + doc['shotTypeId.keyword'].value")).Size(802407)) //crickets2-409846
                // )
                //);

              
               // _objSearchResultsFilterDataT = obj.GetEnumerator();
               // var enumobj = obj.ToAsyncEnumerable();
                ObjectArray.Add(EntityNames.ElementAt(0), obj);

            }
            return ObjectArray;
        }

        public override IEnumerable<ELModels.SearchResultFilterData> returnSportResult(ElasticClient EsClient, QueryContainer _objNestedQuery, string IndexName)
        {
            EsClient = oLayer.CreateConnection();
            searchcricket sc = new searchcricket();
            IEnumerable<SearchResultFilterData> _objSearchResultFilterData = new List<SearchResultFilterData>();
            var result = EsClient.Search<SearchCricketData>(s => s.Index(IndexName).Query(q => _objNestedQuery).Sort(q=>q.Ascending(u=>u.Id.Suffix("keyword"))).Size(802407));
            _objSearchResultFilterData = SearchResultFilterDataMap(result);
            return _objSearchResultFilterData;
        }

        public override List<SearchResultFilterData> SearchResultFilterDataMap(ISearchResponse<SearchCricketData> result)
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
    }
}
