namespace Skeleton.Web.Logging.Serilog.Enrichers
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Text;
    using global::Serilog.Core;
    using global::Serilog.Events;
    using Murmur;

    [ExcludeFromCodeCoverage]
    public class MessageTemplateHashEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var bytes = Encoding.UTF8.GetBytes(logEvent.MessageTemplate.Text);

            var hash = MurmurHash.Create32().ComputeHash(bytes);
            var numericHash = BitConverter.ToUInt32(hash, 0);

            logEvent.AddPropertyIfAbsent(
                propertyFactory.CreateProperty("MessageTemplateHash", numericHash.ToString("x8"))
            );
        }
    }
}