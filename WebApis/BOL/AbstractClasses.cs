using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApis.Model;
using static WebApis.Model.ELModels;

namespace WebApis.BOL
{
    public abstract class AbstractClasses
    {
        public QueryContainer GetMatchDetailQueryST(QueryContainer _objNestedQuery, MatchDetail _objMatchDetail) { return _objNestedQuery; }
        public Dictionary<string, object> fetchDropdowns(QueryContainer _objNestedQuery, Dictionary<string, object> ObjectArray, ElasticClient EsClient, string IndexName, Dictionary<string, string> _columns, string[] sFilterArray) { return ObjectArray; }
        public abstract Dictionary<string, object> fetchDropDownForMatch(Dictionary<string, object> ObjectArray, string[] sInnings);
        public abstract IEnumerable<SearchResultFilterData> returnSportResult(ElasticClient EsClient, QueryContainer _objNestedQuery, string IndexName);
        public abstract List<SearchResultFilterData> SearchResultFilterDataMap(ISearchResponse<SearchCricketData> result);


    }
}
