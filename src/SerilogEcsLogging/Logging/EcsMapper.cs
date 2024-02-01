using Serilog.Events;

namespace SerilogEcsLogging.Logging;

public static class EcsMapper
{
    public static Elastic.CommonSchema.EcsDocument MapLogEvent(Elastic.CommonSchema.EcsDocument ecsDocument, LogEvent logEvent)
    {
        var result = ecsDocument;
        
        // Add custom event data
        if (ecsDocument.Metadata?.TryGetValue(EcsDocument.DataFieldName, out var data) == true && data != null)
        {
            result = new EcsDocument(ecsDocument, data);
        }
        
        // Tags
        if (ecsDocument.Metadata?.TryGetValue(LoggerExtensions.Tags, out var tagsObject) == true && tagsObject is object[] tags)
        {
            string[] tagsStrings = Array.ConvertAll(tags, o => o.ToString()).Where(t => t != null).ToArray()!;
            result.Tags = result.Tags != null ? result.Tags.Concat(tagsStrings).ToArray() : tagsStrings;
        }
        
        // Add AssemblyName to Tags
        if (logEvent.Properties.TryGetValue("AssemblyName", out var assemblyName) && assemblyName is ScalarValue) 
        { 
            var serviceName = assemblyName.ToString().Trim('"'); 
            result.Tags = result.Tags != null ? result.Tags.Concat(new[] { serviceName }).ToArray() : new[] { serviceName }; 
        }

        if (result.Tags != null)
        {
            result.Tags = result.Tags.Distinct().ToArray();
            Array.Sort(result.Tags);
        }

        result.Metadata = null;
        result.Labels = null;
        
        return result;
    }
}