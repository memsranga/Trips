using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Trips.Services.Interfaces
{
    public interface ILocationService
    {
        Task StartListeningAsyc(ICommand changedCommand);
        Task StopListeningAsyc();
    }
}
