using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using paypart_service_gateway.Models;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Caching.Distributed;
using System.Threading;

namespace paypart_service_gateway.Services
{
    public class Redis
    {
        IOptions<Settings> settings;
        IDistributedCache redis;
        public delegate void SetService(string key, ServiceModel service);
        public delegate void SetServices(string key, IEnumerable<ServiceModel> services);

        public Redis(IOptions<Settings> _settings, IDistributedCache _redis)
        {
            settings = _settings;
            redis = _redis;
        }
        public async Task<ServiceModel> getbillerservice(string key, CancellationToken ctx)
        {
            ServiceModel services = new ServiceModel();
            try
            {
                var service = await redis.GetStringAsync(key, ctx);
                services = JsonHelper.fromJson<ServiceModel>(service);
            }
            catch (Exception)
            {

            }
            return services;
        }

        public async Task<List<Service>> getservices(string key, CancellationToken ctx)
        {
            List<Service> services = new List<Service>();
            try
            {
                var service = await redis.GetStringAsync(key, ctx);
                services = JsonHelper.fromJson<List<Service>>(service);
            }
            catch (Exception)
            {

            }
            return services;
        }

        public async Task setservice(string key, Service services, CancellationToken cts)
        {
            try
            {
                var service = await redis.GetStringAsync(key, cts);
                if (!string.IsNullOrEmpty(service))
                {
                    redis.Remove(key);
                }
                string value = JsonHelper.toJson(services);

                await redis.SetStringAsync(key, value, cts);
            }
            catch (Exception)
            {

            }

        }
        public async Task setserviceAsync(string key, ServiceModel services, CancellationToken cts)
        {
            try
            {
                var service = await redis.GetStringAsync(key);
                if (!string.IsNullOrEmpty(service))
                {
                    redis.Remove(key);
                }
                string value = JsonHelper.toJson(services);

                await redis.SetStringAsync(key, value, cts);
            }
            catch (Exception ex)
            {

            }

        }
        public async Task setservices(string key, List<Service> services, CancellationToken cts)
        {
            try
            {
                var service = await redis.GetStringAsync(key);
                if (!string.IsNullOrEmpty(service))
                {
                    redis.Remove(key);
                }
                string value = JsonHelper.toJson(services);

                await redis.SetStringAsync(key, value, cts);
            }
            catch (Exception)
            {

            }

        }
    }
}
