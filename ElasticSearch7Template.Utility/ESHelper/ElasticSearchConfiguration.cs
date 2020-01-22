using Elasticsearch.Net;
using ElasticSearch7Template.Core;
using Nest;
using System;
using System.Collections.Concurrent;

namespace ElasticSearch7Template.Utility.ESHelper
{
    public class ElasticSearchConfiguration
    {
        private readonly ConnectionSettings connectionSettings;

        public static ConcurrentDictionary<string, ElasticClient> dic = new ConcurrentDictionary<string, ElasticClient>();

        public ElasticClient client;

        public ElasticClient GetClient()
        {
            return client;
        }


        public ElasticSearchConfiguration(string indexName)
        {
            if (!dic.ContainsKey(indexName))
            {
                var nodeLength = ElasticSearchConfig.ClusterNodeUrlHosts.Length;
                var nodes = new Uri[nodeLength];
                var nodeUrlsHost = ElasticSearchConfig.ClusterNodeUrlHosts;
                for (int i = 0; i < nodeUrlsHost.Length; i++)
                {
                    nodes[i] = new Uri("http://" + nodeUrlsHost[i]);
                }
                var pool = new StaticConnectionPool(nodes);
                connectionSettings = new ConnectionSettings(pool)
                    .DefaultIndex(indexName).ThrowExceptions(true);
                if (ElasticSearchConfig.IsOpenDebugger.HasValue)
                {
                    if (ElasticSearchConfig.IsOpenDebugger.Value)
                    {
                        connectionSettings.DisableDirectStreaming(true);
                    }
                }
                client = new ElasticClient(connectionSettings);
                dic.TryAdd(indexName, client);
            }
            else
            {

                dic.TryGetValue(indexName, out client);
            }
        }
    }
}
