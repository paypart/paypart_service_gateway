using Microsoft.EntityFrameworkCore;
using paypart_service_gateway.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace paypart_service_gateway.Services
{
    public class ServiceSqlServerRepository : IServiceSqlServerRepository
    {
        private readonly ServiceSqlServerContext _context = null;

        public ServiceSqlServerRepository(ServiceSqlServerContext context)
        {
            _context = context;
        }

        public async Task<List<Service>> GetAllServices()
        {
            return await _context.Service.Include(s => s.serviceaccount).ToListAsync();
        }

        public async Task<Service> GetService(int id)
        {
            return await _context.Service.Where(c => c.id == id)
                                 .FirstOrDefaultAsync();
        }

        public async Task<List<ServiceAccount>> GetAllServiceAccount()
        {
            return await _context.ServiceAccount.ToListAsync();
        }

        public async Task<ServiceAccount> GetServiceAccount(int id)
        {
            return await _context.ServiceAccount.Where(c => c.id == id)
                                 .FirstOrDefaultAsync();
        }
        public async Task<ServiceAccount> GetServiceAccount(int id,int status)
        {
            return await _context.ServiceAccount.Where(c => c.serviceid == id && c.status == status)
                                 .FirstOrDefaultAsync();
        }
        public async Task<List<Service>> GetServiceByTitle(string title)
        {
            return await _context.Service.Where(c => c.title == title)
                                 .ToListAsync();
        }

        public async Task<Service> AddService(Service item)
        {
            await _context.Service.AddAsync(item);
            await _context.SaveChangesAsync();
            return await GetService(item.id);
        }

        public async Task<ServiceAccount> AddServiceAccount(ServiceAccount item)
        {
            await _context.ServiceAccount.AddAsync(item);
            await _context.SaveChangesAsync();
            return await GetServiceAccount(item.id);
        }
        public async Task<Service> UpdateService(Service service)
        {
            Service s = await GetService(service.id);
            ServiceAccount sc = await GetServiceAccount(service.serviceaccount.id);

            sc.accountname = service.serviceaccount.accountname;
            sc.accountnumber = service.serviceaccount.accountnumber;
            sc.accountref = service.serviceaccount.accountref;
            sc.bankcode = service.serviceaccount.bankcode;
            sc.bankname = service.serviceaccount.bankname;
            sc.serviceid = service.serviceaccount.serviceid;
            sc.status = service.serviceaccount.status;

            s.billerid = service.billerid;
            s.categoryid = service.categoryid;
            s.title = service.title;
            s.status = service.status;

            await _context.SaveChangesAsync();

            s.serviceaccount = sc;
            return s;
        }
        //public async Task<DeleteResult> RemoveBiller(string id)
        //{
        //    return await _context.Billers.Remove(
        //                 Builders<Biller>.Filter.Eq(s => s._id, id));
        //}

        //public async Task<UpdateResult> UpdateBiller(string id, string title)
        //{
        //    var filter = Builders<Biller>.Filter.Eq(s => s._id.ToString(), id);
        //    var update = Builders<Biller>.Update
        //                        .Set(s => s.title, title)
        //                        .CurrentDate(s => s.createdOn);
        //    return await _context.Billers.UpdateOneAsync(filter, update);
        //}

        //public async Task<ReplaceOneResult> UpdateBiller(string id, Biller item)
        //{
        //    return await _context.Billers
        //                         .ReplaceOneAsync(n => n._id.Equals(id)
        //                                             , item
        //                                             , new UpdateOptions { IsUpsert = true });
        //}

        //public async Task<DeleteResult> RemoveAllBillers()
        //{
        //    return await _context.Billers.DeleteManyAsync(new BsonDocument());
        //}
    }
}
