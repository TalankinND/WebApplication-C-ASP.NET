using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Models;

namespace WebApplication1.ModelBuilders
{
    public interface ISignalModelBuilder
    {
        List<SignalModel> GetSignalListInfo(int id);
        SignalModel GetSignalInfoById(int id);
    }
}
