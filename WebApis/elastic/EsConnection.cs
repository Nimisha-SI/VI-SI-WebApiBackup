using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApis.Model;

namespace WebApis
{
    public class EsConnection
    {
        Uri esInstance;
     
        public  ElasticClient CreateConnection()
        {
            GetSearchS1DataForCricket obj = new GetSearchS1DataForCricket();
             string con =   obj.GetConnectionString("elasticsearch", "url");
            string index = obj.GetConnectionString("elasticsearch", "index");
            Uri EsInstance = new Uri(con);
            ConnectionSettings EsConfiguration = new ConnectionSettings(EsInstance);
            EsConfiguration.DefaultIndex(index);
            ElasticClient EsClient = new ElasticClient(EsConfiguration);
            return EsClient;
        }
        public void BulkInsert(ElasticClient EsClient, List<SearchCricketData> documents) {
                    var bulkAllObservable = EsClient.BulkAll(documents, b => b
          .Index("kabadibulk")
          .BackOffTime("30s")
          .BackOffRetries(2)
          .RefreshOnCompleted()
          .MaxDegreeOfParallelism(Environment.ProcessorCount)
          .Size(9999)
        )
        .Wait(TimeSpan.FromMinutes(15), next =>
{
});
        }

    }
}
