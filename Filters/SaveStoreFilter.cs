using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using laba2.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

namespace laba2.Filters
{
    public class SaveStoreFilter : Attribute, IActionFilter
    {
        string _key;


        public SaveStoreFilter(string key)
        {
            _key = key;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ModelState != null && context.ModelState.Count > 0)
            {
                Dictionary<string, string> pairs = new Dictionary<string, string>();
                if (context.HttpContext.Session.Keys.Contains(_key))
                    pairs = context.HttpContext.Session.Get<Dictionary<string, string>>(_key);
                //pairs = JsonConvert.DeserializeObject<Dictionary<string, string>>(context.HttpContext.Session.GetString(_key));
                //model.sortOrder = context.ModelState["sortOrder"].AttemptedValue;
                //model.type = context.ModelState["Type"].AttemptedValue;
                foreach (var item in context.ModelState)
                {
                    if (pairs.ContainsKey(item.Key)) pairs.Remove(item.Key);
                    pairs.Add(item.Key, item.Value.AttemptedValue);
                }
                //context.HttpContext.Session.SetString(_key, JsonConvert.SerializeObject(pairs));
                context.HttpContext.Session.Set(_key, pairs);
            }
        }
    }
}
