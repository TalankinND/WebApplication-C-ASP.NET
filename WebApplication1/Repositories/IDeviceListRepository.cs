using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public interface IDeviceListRepository
    {
        void EditDeviceRecord(DeviceListModel model);
        List<DeviceListModel> GetAllDeviceRecord();
        void RemoveDeviceRecord(DeviceListModel model);
        void SaveNewDeviceRecord(DeviceListModel model);
        void SaveNewDeviceRecordRange(List<DeviceListModel> models);
        DeviceListModel FindRecordById(int? id);
    }
}
