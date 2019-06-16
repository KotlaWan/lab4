using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

namespace laba2.Filters
{
    public static class SessionExtension
    {
        public static void Set(this ISession session, string key, object pairs)
        {
            session.SetString(key, JsonConvert.SerializeObject(pairs));
        }

        public static T Get<T>(this ISession session, string key)
        {
            return JsonConvert.DeserializeObject<T>(session.GetString(key));
        }
    }
}
