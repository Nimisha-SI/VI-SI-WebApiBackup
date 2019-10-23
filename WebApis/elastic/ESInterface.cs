using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApis.Model;

namespace WebApis.elastic
{
    interface ESInterface
    {
          ElasticClient CreateConnection();
          void BulkInsert(ElasticClient EsClient, List<SearchS2Data> cricketdata);
    }
}
