using System.Diagnostics;
using System.Diagnostics.Metrics;
//using App.Metrics.Counter;

namespace MetricsWebApp
{
    // https://localhost:7101/metrics   -- Metrics about the machine
    // http://localhost:9464/metrics    -- Your personal metrics
    // http://localhost:9090            -- Prometheus Web site
    
    // irate(WeatherOperationDuration_ms_sum [5m])  -- Histogram query

    public class WebAppMetrics
    {
        public const string Name = "webAppMetrics";

        private readonly Meter meter;

        public Counter<int> RequestCounter { get; }
        public Histogram<double> WeatherOperationDuration { get; }


        public WebAppMetrics()
        {
            meter = new Meter(Name);

            RequestCounter = meter.CreateCounter<int>("RequestCounter");
            WeatherOperationDuration = meter.CreateHistogram<double>("WeatherOperationDuration", unit: "ms");
        }

        public void IncrementRequestCounter(int value = 1)
        {
            RequestCounter.Add(value);
        }

        public Stopwatch StartWeatherOperationDuration()
        {
            return Stopwatch.StartNew();
        }

        public void StopWeatherOperationDuration(Stopwatch sw)
        {
            sw.Stop();

            WeatherOperationDuration.Record(
                sw.ElapsedMilliseconds, 
                tag: KeyValuePair.Create<string, object?>("Operation", "GetWeatherForecast"));
        }
    }
}