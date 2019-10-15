using System;
using Jaeger.Metrics;
using Jaeger.Reporters;
using Jaeger.Samplers;
using Jaeger.Senders;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTracing;
using OpenTracing.Util;

namespace Jaeger.Example.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            //Reference https://github.com/jaegertracing/jaeger-client-csharp/tree/master/examples/Jaeger.Example.WebApi

            // Use "OpenTracing.Contrib.NetCore" to automatically generate spans for ASP.NET Core, Entity Framework Core, ...
            // See https://github.com/opentracing-contrib/csharp-netcore for details.
            services.AddOpenTracing();

            services.AddSingleton<ITracer>(serviceProvider =>
            {
                var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
                var serviceName = serviceProvider.GetRequiredService<IHostEnvironment>().ApplicationName;
                var reporter = new RemoteReporter.Builder()
                    .WithLoggerFactory(loggerFactory)
                    //.WithSender(new HttpSender("192.168.132.105:14268"))
                    .WithSender(new UdpSender("192.168.132.105", 6831, 0))
                    .Build();
                //var reporter = new LoggingReporter(loggerFactory);

                var trace = new Tracer.Builder(serviceName)
                    .WithLoggerFactory(loggerFactory)
                    .WithReporter(reporter)
                    .WithSampler(new ConstSampler(true))
                    .Build();
                GlobalTracer.Register(trace);

                return trace;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
