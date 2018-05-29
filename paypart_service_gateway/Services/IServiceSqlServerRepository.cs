using System.Collections.Generic;
using paypart_service_gateway.Models;
using System.Threading.Tasks;

namespace paypart_service_gateway.Services
{
    public interface IServiceSqlServerRepository
    {
        Task<List<Service>> GetAllServices();
        Task<Service> GetService(int id);
        Task<List<Service>> GetServiceByTitle(string title);
        Task<Service> AddService(Service item);
        Task<ServiceAccount> GetServiceAccount(int id);
        Task<ServiceAccount> AddServiceAccount(ServiceAccount item);
        Task<List<ServiceAccount>> GetAllServiceAccount();
        Task<Service> UpdateService(Service service);

        //Task<DeleteResult> RemoveBiller(string id);
        //Task<UpdateResult> UpdateBiller(string id, string title);

        // demo interface - full document update
        //Task<ReplaceOneResult> UpdateBiller(string id, Biller item);

        // should be used with high cautious, only in relation with demo setup
        //Task<DeleteResult> RemoveAllBillers();

    }
}
