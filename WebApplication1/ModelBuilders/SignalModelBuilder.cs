using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;
using WebApplication1.Repositories;

namespace WebApplication1.ModelBuilders
{
    public class SignalModelBuilder:ISignalModelBuilder
    {
        private readonly ISignalRepository signalRepository;
        private readonly IDeviceListRepository deviceListRepository;

        public SignalModelBuilder(ISignalRepository signalRepository, IDeviceListRepository deviceListRepository)
        {
            this.signalRepository = signalRepository;
            this.deviceListRepository = deviceListRepository;
        }

        public List<SignalModel> GetSignalListInfo(int id)
        {
            var DeviceModel = deviceListRepository.FindRecordById(id);

            var modelList = signalRepository.GetAllRecords().Where(model => model.DeviceID == DeviceModel.DeviceID);

            List<SignalModel> SignalView = new List<SignalModel>();

            foreach (var item in modelList)
            {
                SignalView.Add(new SignalModel
                {
                    RecordID = item.RecordID,
                    DeviceID = item.DeviceID,
                    HS = item.HS,
                    Сabinet = item.Сabinet,
                    SignalType = item.SignalType,
                    SensorName = item.SensorName,
                    Section = item.Section,
                    CodeCombination = item.CodeCombination,
                    BI_Number_Lamp = item.BI_Number_Lamp,
                    Signature_BI = item.Signature_BI,
                    Comment_BI = item.Comment_BI,
                    Signature_Security = item.Signature_Security,
                    Bonus = item.Bonus
                });
            }

            return SignalView;
        }

        public SignalModel GetSignalInfoById(int id)
        {
            SignalModel model = signalRepository.FindRecordById(id);

            return new SignalModel
            {
                RecordID = model.RecordID,
                DeviceID = model.DeviceID,
                HS = model.HS,
                Сabinet = model.Сabinet,
                SignalType = model.SignalType,
                SensorName = model.SensorName,
                Section = model.Section,
                CodeCombination = model.CodeCombination,
                BI_Number_Lamp = model.BI_Number_Lamp,
                Signature_BI = model.Signature_BI,
                Comment_BI = model.Comment_BI,
                Signature_Security = model.Signature_Security,
                Bonus = model.Bonus
            };
        }
    }
}
