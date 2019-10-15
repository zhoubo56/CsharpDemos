using System;
using System.Collections.Generic;
using Jaeger;
using Jaeger.Reporters;
using Jaeger.Samplers;
using Microsoft.Extensions.Logging;


namespace HelloWorld
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //Reference https://github.com/jaegertracing/jaeger-client-csharp
            var loggerFactory = LoggerFactory.Create(builder => { builder.AddConsole(); });
            const string serviceName = "initExampleService";

            var reporter = new LoggingReporter(loggerFactory);
            var sampler = new ConstSampler(true);
            var tracer = new Tracer.Builder(serviceName)
                .WithLoggerFactory(loggerFactory)
                .WithReporter(reporter)
                .WithSampler(sampler)
                .Build();

            const string operationName = "Get::api/values";
            var spanBuilder = tracer.BuildSpan(operationName);
            using (var scope = spanBuilder.StartActive(true))
            {
                var logData = new List<KeyValuePair<string, object>>
                {
                    new KeyValuePair<string, object>("handling number of events",6),
                    new KeyValuePair<string, object>("using legacy system",false)
                };
                scope.Span.Log(DateTimeOffset.Now, logData);
            }

            Console.ReadLine();
        }
    }
}
