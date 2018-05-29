using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using paypart_service_gateway.Services;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Caching.Distributed;
using paypart_service_gateway.Models;
using System.Threading;
using System.Net;

namespace paypart_service_gateway.Controllers
{
    [Produces("application/json")]
    [Route("api/service")]
    public class ServiceController : Controller
    {
        private readonly IServiceSqlServerRepository serviceSqlRepo;

        IOptions<Settings> settings;
        IDistributedCache cache;

        public ServiceController(IOptions<Settings> _settings,
          IServiceSqlServerRepository _serviceSqlRepo, IDistributedCache _cache)
        {
            settings = _settings;
            serviceSqlRepo = _serviceSqlRepo;
            cache = _cache;
        }
        [HttpGet("getallservices")]
        [ProducesResponseType(typeof(Service), 200)]
        [ProducesResponseType(typeof(ServiceError), 400)]
        [ProducesResponseType(typeof(ServiceError), 500)]
        public async Task<IActionResult> getallservices()
        {
            List<Service> services = null;
            ServiceError e = new ServiceError();

            CancellationTokenSource cts;
            cts = new CancellationTokenSource();
            cts.CancelAfter(settings.Value.redisCancellationToken);

            Redis redis = new Redis(settings, cache);
            string key = "all_services";

            //check redis cache for details
            try
            {
                services = await redis.getservices(key, cts.Token);

                if (services != null && services.Count > 0)
                {
                    return CreatedAtAction("getallservices", services);
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }

            //Get billers from Mongo
            try
            {
                //billercategories = await billercategoryMongoRepo.GetAllBillerCategories();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }

            //Get Billers from Sql
            try
            {
                services = await serviceSqlRepo.GetAllServices();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }

            //Write to Redis
            try
            {
                if (services != null && services.Count > 0)
                    await redis.setservices(key, services, cts.Token);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
            return CreatedAtAction("getallservices", services);
        }
        [HttpPost("addservice")]
        [ProducesResponseType(typeof(Service), 200)]
        [ProducesResponseType(typeof(ServiceError), 400)]
        [ProducesResponseType(typeof(ServiceError), 500)]
        public async Task<IActionResult> addservice([FromBody]Service service)
        {
            Service _service = null;
            ServiceError e = new ServiceError();
            Redis redis = new Redis(settings, cache);

            CancellationTokenSource cts;
            cts = new CancellationTokenSource();
            cts.CancelAfter(settings.Value.redisCancellationToken);

            //validate request
            if (!ModelState.IsValid)
            {
                var modelErrors = new List<ServiceError>();
                var eD = new List<string>();
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var modelError in modelState.Errors)
                    {
                        eD.Add(modelError.ErrorMessage);
                    }
                }
                e.error = ((int)HttpStatusCode.BadRequest).ToString();
                e.errorDetails = eD;

                return BadRequest(e);
            }
            service.created_on = DateTime.Now;

            //Add to mongo
            try
            {
                //_billercategory = await billercategoryMongoRepo.AddBillerCategory(billercategory);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }

            //Add to sql server
            try
            {
                if (service.id == 0)
                {
                    _service = await serviceSqlRepo.AddService(service);
                }
                else if (service.id > 0)
                {
                    _service = await serviceSqlRepo.UpdateService(service);
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);

            }

            //Write to Redis
            try
            {
                string key = "all_services";

                if (_service != null)
                    await redis.setservice(key, _service, cts.Token);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }

            return CreatedAtAction("addservice", _service);

        }
    }
}