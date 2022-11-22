using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public interface ISignalRepository
    {
        void EditSignalRecord(SignalModel model);
        List<SignalModel> GetAllRecords();
        void SaveNewSignalRecord(SignalModel model);
        void RemoveSignalRecord(SignalModel model);
        SignalModel FindRecordById(int? id);
        void SaveSignalRecordRange(List<SignalModel> models);
    }
}
