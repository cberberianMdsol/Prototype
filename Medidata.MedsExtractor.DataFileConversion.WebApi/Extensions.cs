using System;
using System.Collections.Generic;
using System.Linq;
using Medidata.MedsExtractor.DataFileConversion.WebApi.Config;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using zipkin4net;
using zipkin4net.Middleware;
using zipkin4net.Tracers.Zipkin;
using zipkin4net.Transport;
using zipkin4net.Transport.Http;

namespace Medidata.MedsExtractor.DataFileConversion.WebApi
{
    public static class Extensions
    {
        public static IApplicationLifetime RegisterZipkinOnStarted(this IApplicationLifetime lifetime, AppSettings appSettings, ILoggerFactory loggerFactory)
        {
            lifetime.ApplicationStarted.Register(() =>
            {
                TraceManager.SamplingRate = appSettings.Zipkin.SamplingRate;
                TraceManager.RegisterTracer(new ZipkinTracer(new HttpZipkinSender(appSettings.Zipkin.BaseUri.AbsoluteUri, "application/json"), new JSONSpanSerializer()));
                TraceManager.Start(new TracingLogger(loggerFactory, "zipkin4net"));
            });

            return lifetime;
        }
        public static IApplicationLifetime RegisterZipkinOnStopped(this IApplicationLifetime lifetime)
        {
            lifetime.ApplicationStopped.Register(() => TraceManager.Stop());

            return lifetime;
        }

        public static IEnumerable<T> AsEnumerable<T>(this IConfiguration configuration)
        {
            var result = new List<T>();

            configuration.Bind(result);

            return result.AsEnumerable();
        }

        public static IApplicationBuilder UseZipkin(this IApplicationBuilder app, string serviceName, Func<HttpRequest, bool> bypass = null)
        {
            var extractor = new ZipkinHttpTraceExtractor();

            return app.Use(async (context, next) =>
            {
                if (bypass?.Invoke(context.Request) ?? false)
                {
                    await next.Invoke();
                    return;
                }

                if (string.IsNullOrWhiteSpace(serviceName))
                {
                    throw new ArgumentNullException(nameof(serviceName), "The name of the service (or application) is missing or filled with whitespace. Please check your app settings.");
                }

                if (!extractor.TryExtract(context.Request.Headers, (c, key) => c[key], out var trace))
                {
                    trace = Trace.Create();
                }

                Trace.Current = trace;

                using (var serverTrace = new ServerTrace(serviceName, context.Request.Method))
                {
                    trace.Record(Annotations.Tag("http.host", context.Request.Host.ToString()));
                    trace.Record(Annotations.Tag("http.uri", context.Request.GetDisplayUrl()));
                    trace.Record(Annotations.Tag("http.path", context.Request.Path));

                    try
                    {
                        await serverTrace.TracedActionAsync(next());

                        if (context.Response.StatusCode >= 400)
                        {
                            trace.Record(Annotations.Tag("error", context.Response.StatusCode.ToString()));
                        }
                    }
                    catch (Exception ex)
                    {
                        serverTrace.Error(ex);
                        throw;
                    }
                }
            });
        }
    }
}