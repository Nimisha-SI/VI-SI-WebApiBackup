using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nest;
using WebApis.Model;

namespace WebApis.elastic
{
    public class EsLayer : ESInterface
    {
        GetSearchS1DataForCricket obj = new GetSearchS1DataForCricket();
        string con;
        string index;
        public ElasticClient CreateConnection()
        {
            con = obj.GetConnectionString("elasticsearch", "url");
            index = obj.GetConnectionString("elasticsearch", "index");
            Uri EsInstance = new Uri(con);
            ConnectionSettings EsConfiguration = new ConnectionSettings(EsInstance);
            EsConfiguration.DefaultIndex(index);
            ElasticClient EsClient = new ElasticClient(EsConfiguration);
            return EsClient;
        }

        public void BulkInsert(ElasticClient EsClient,  List<SearchS2Data> documents)
            {
           var bulkAllObservable = EsClient.BulkAll(documents, b => b
         .Index("crickets2data")
                  
         .BackOffTime("30s")
         .BackOffRetries(2)
         .RefreshOnCompleted()
         .MaxDegreeOfParallelism(Environment.ProcessorCount)
         .Size(10000)
       )
       .Wait(TimeSpan.FromMinutes(15), next =>
       {
       });
            }

        
    }
}
