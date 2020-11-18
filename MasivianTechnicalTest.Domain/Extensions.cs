using System;
using System.Collections.Generic;
using System.Text;

namespace MasivianTechnicalTest.Domain
{
    public static class Extensions
    {
        public static string ToJson<T>(this T obj)
        {           
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }
    }
}
