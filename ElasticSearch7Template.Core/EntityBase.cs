namespace ElasticSearch7Template.Core
{
    public  class EntityBase
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        public long Id { get; set; }

        public EntityBase()
        {
           Id= GeneratePrimaryKeyIdHelper.GetPrimaryKeyId();
        }
    }
}
