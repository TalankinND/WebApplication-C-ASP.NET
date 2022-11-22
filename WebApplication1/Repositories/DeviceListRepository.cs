using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public class DeviceListRepository : IDeviceListRepository
    {
        public AppContext dbcontext;

        public DeviceListRepository(AppContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }

        public List<DeviceListModel> GetAllDeviceRecord()
        {
            return dbcontext.DeviceLists.ToList();
        }

        public void SaveNewDeviceRecord(DeviceListModel model)
        {
            dbcontext.DeviceLists.Add(model);
            dbcontext.SaveChanges();
        }

        public void SaveNewDeviceRecordRange(List<DeviceListModel> models)
        {
            dbcontext.DeviceLists.AddRange(models);
            dbcontext.SaveChanges();
        }

        public void RemoveDeviceRecord(DeviceListModel model)
        {
            dbcontext.DeviceLists.Remove(model);
            dbcontext.SaveChanges();
        }

        public void EditDeviceRecord(DeviceListModel model)
        {
            dbcontext.Entry(model).State = EntityState.Modified;
            dbcontext.SaveChanges();
        }

        public DeviceListModel FindRecordById(int? id)
        {
            return dbcontext.DeviceLists.Find(id);
        }
    }
}
