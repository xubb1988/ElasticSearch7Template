using Nest;

namespace ElasticSearch7Template.Utility.ESHelper
{
    public class ElasticSearchHelper
    {
        private readonly IElasticClient client;
        public ElasticSearchHelper(IElasticClient client)
        {
            this.client = client;
        }
    }
}
