using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;
using WebApplication1.Repositories;

namespace WebApplication1.ModelBuilders
{
    public class DeviceListModelBuilder : IDeviceListModelBuilder
    {
        private readonly IDeviceListRepository deviceListRepository;

        public DeviceListModelBuilder(IDeviceListRepository deviceListRepository)
        {
            this.deviceListRepository = deviceListRepository;
        }

        public List<DeviceListModel> GetDeviceListInfo()
        {
            var modelList = deviceListRepository.GetAllDeviceRecord();

            List<DeviceListModel> deviceView = new List<DeviceListModel>();

            foreach (var item in modelList)
            {
                deviceView.Add(new DeviceListModel
                {
                    DeviceID = item.DeviceID,
                    DeviceName = item.DeviceName,
                    DeviceShortName = item.DeviceShortName,
                    DeviceType = item.DeviceType
                });
            }

            return deviceView;
        }

        public DeviceListModel GetDeviceInfoById(int id)
        {
            DeviceListModel model = deviceListRepository.FindRecordById(id);

            return new DeviceListModel
            {
                DeviceID = model.DeviceID,
                DeviceName = model.DeviceName,
                DeviceShortName = model.DeviceShortName,
                DeviceType = model.DeviceType
            };
        }
    }
}
