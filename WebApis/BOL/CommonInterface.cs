using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static WebApis.Model.ELModels;

namespace WebApis.BOL
{
    interface IStoryTeller {
        ExtendedSearchResultFilterData searchStoryTeller(MatchDetail _objMatchDetail, QueryContainer _objNestedQuery, dynamic _objS1Data, Dictionary<string, object> ObjectArray, IEnumerable<SearchResultFilterData> ResultData, string value, string IndexName);
        IEnumerable<SearchResultFilterData> returnSportResult(ElasticClient EsClient, QueryContainer _objNestedQuery, string IndexName);
    }
    //interface IGetDetailQueryST {
    //    //QueryContainer GetCricketPlayerDetails(dynamic _objS1Data, QueryContainer qFinal, int sportId);
    //    QueryContainer GetPlayerDetails(dynamic _objS1Data, QueryContainer qFinal, List<string> valueObj, int sportid);
    //    QueryContainer GetMatchDetailQueryST(QueryContainer _objNestedQuery, MatchDetail _objMatchDetail); // Cricket
    //}
}
