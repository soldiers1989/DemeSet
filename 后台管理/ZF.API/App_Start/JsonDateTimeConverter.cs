using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ZF.API
{
    /// <summary>  
    /// Json日期带T格式转换  
    /// </summary>  
    public class JsonDateTimeConverter : IsoDateTimeConverter
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            DateTime dataTime;
            if (DateTime.TryParse(reader.Value.ToString(), out dataTime))
            {
                return dataTime;
            }
            else
            {
                return existingValue;
            }
        }

        public JsonDateTimeConverter()
        {
            DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
        }
    }
}