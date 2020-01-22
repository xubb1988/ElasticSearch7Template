using System;
using System.Collections.Generic;
using System.Text;

namespace ElasticSearch7Template.Utility
{
   public static class MessageFactory
    {
        public static string CreateNotFoundMessage(string entityName, string id)
        {
            return $"Cannot find {entityName} with id {id}.";
        }

        public static string CreatePageParamsInvalidMessage()
        {
            return "pageSize and pageNumber must larger than 0.";
        }

        public static string CreateParamsIsNullMessage()
        {
            return "Parameter is empty";
        }

    }
}
