using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;
using WebApplication1.Repositories;

namespace WebApplication1.ModelBuilders
{
    public class BKIModelBuilder:IBKIModelBuilder
    {
        private readonly IBKIRepository BKIRepository;
        private readonly IDeviceListRepository deviceListRepository;

        public BKIModelBuilder(IBKIRepository BKIRepository, IDeviceListRepository deviceListRepository)
        {
            this.BKIRepository = BKIRepository;
            this.deviceListRepository = deviceListRepository;
        }

        public List<BKIModel> GetBKIListInfo(int id)
        {
            var DeviceModel = deviceListRepository.FindRecordById(id);

            var modelList = BKIRepository.GetAllRecords().Where(model => model.DeviceID == DeviceModel.DeviceID);

            List<BKIModel> BKIView = new List<BKIModel>();

            foreach (var item in modelList)
            {
                BKIView.Add(new BKIModel
                {
                    RecordId = item.RecordId,
                    DeviceID = item.DeviceID,
                    Lamp = item.Lamp,
                    Object = item.Object,
                    OSType = item.OSType,
                    CommonCondition = item.CommonCondition,
                    PKNumber = item.PKNumber,
                    SHNumber = item.SHNumber,
                    Deposit_Withdraw_code = item.Deposit_Withdraw_code,
                    BKI_Commentary = item.BKI_Commentary,
                    First_Inscription = item.First_Inscription,
                    Second_Incription = item.Second_Incription,
                    Section_Number = item.Section_Number
                });
            }

            return BKIView;
        }

        public BKIModel GetBKIInfoById(int id)
        {
            BKIModel model = BKIRepository.FindRecordById(id);

            return new BKIModel
            {
                RecordId = model.RecordId,
                DeviceID = model.DeviceID,
                Lamp = model.Lamp,
                Object = model.Object,
                OSType = model.OSType,
                CommonCondition = model.CommonCondition,
                PKNumber = model.PKNumber,
                SHNumber = model.SHNumber,
                Deposit_Withdraw_code = model.Deposit_Withdraw_code,
                BKI_Commentary = model.BKI_Commentary,
                First_Inscription = model.First_Inscription,
                Second_Incription = model.Second_Incription,
                Section_Number = model.Section_Number
            };
        }
    }
}
