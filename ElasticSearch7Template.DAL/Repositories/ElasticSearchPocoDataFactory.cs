
using ElasticSearch7Template.Utility.Cache;
using System;

namespace ElasticSearch7Template.DAL
{
    public class ElasticSearchPocoDataFactory
    {
        private readonly static PocoCache<Type, ElasticSearchPocoData> pocoDatas = PocoCache<Type, ElasticSearchPocoData>.CreateStaticCache();

        public ElasticSearchPocoData ForType(Type type)
        {

            var pocoData = pocoDatas.Get(type, () => new ElasticSearchPocoData(type));
            return pocoData;
        }
    }
}
