using System.Threading;
using System.Threading.Tasks;
using HHParser.Domain.Enums;
using HHParser.Domain.Models;

namespace HHParser.Application.Interfaces
{
    public interface IMenuCommand
    {
        MainMenuOption Option { get; }
        Task ExecuteAsync(CancellationToken cancellationToken);
    }
}
