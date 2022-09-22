
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Client.WebApiService;

public static class JsonHelper
{
    public static JsonSerializerSettings AddConvertors(this JsonSerializerSettings settings)
    {
        settings.ContractResolver = new ContractResolver();
        return settings;
    }

    public static JsonSerializerSettings GetDefaultSettings()
    {
        var settings = new JsonSerializerSettings
        {
            ContractResolver = new ContractResolver()
        };
        return settings;
    }
}

public partial class WebApiService
{
    public WebApiService(HttpClient httpClient, IConfiguration configuration) : this(httpClient)
    {
        var serviceUrl = configuration["WebApiServiceUrl"];
        httpClient.BaseAddress = new Uri(serviceUrl);
    }

    private void UpdateJsonSerializerSettings(JsonSerializerSettings settings)
    {
        settings.AddConvertors();
    }
}

internal class ContractResolver : DefaultContractResolver
{
    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
    {
        var property = base.CreateProperty(member, memberSerialization);

        if (property.PropertyType == typeof(DateOnly)) property.Converter = new DateOnlyJsonConverter();
        else if (property.PropertyType == typeof(TimeOnly)) property.Converter = new TimeOnlyJsonConverter();

        return property;
    }
}

public class DateOnlyJsonConverter : JsonConverter<DateOnly>
{
    public override DateOnly ReadJson(JsonReader reader, Type objectType, DateOnly existingValue, bool hasExistingValue,
        JsonSerializer serializer)
    {
        return DateOnly.Parse(reader.Value as string ?? DateOnly.MinValue.ToString());
    }

    public override void WriteJson(JsonWriter writer, DateOnly value, JsonSerializer serializer)
    {
        var isoDate = value.ToString("O");
        writer.WriteValue(isoDate);
    }
}

public class TimeOnlyJsonConverter : JsonConverter<TimeOnly>
{
    public override TimeOnly ReadJson(JsonReader reader, Type objectType, TimeOnly existingValue, bool hasExistingValue,
        JsonSerializer serializer)
    {
        return TimeOnly.Parse(reader.Value as string ?? TimeOnly.MinValue.ToString());
    }

    public override void WriteJson(JsonWriter writer, TimeOnly value, JsonSerializer serializer)
    {
        var isoTime = value.ToString("HH:mm:ss");
        writer.WriteValue(isoTime);
    }
}