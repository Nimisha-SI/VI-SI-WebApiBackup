using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static WebApis.Model.ELModels;

namespace WebApis.BOL
{
    public interface ISearchDataFilter
    {
        ExtendedSearchResultFilterData GetSearchResultsFilter(STFilterRequestData _objReqData);
        List<FilteredEntityForCricket> GetFilteredEntitiesBySport(SearchEntityRequestData _objReqData);
        string GetSavedSearches(SaveSearchesRequestData objSavedSearchData);
        string GetSearchResultCount(SearchRequestData _objReqData);
    }
}
