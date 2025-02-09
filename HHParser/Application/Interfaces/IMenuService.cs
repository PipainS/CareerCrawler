namespace HHParser.Application.Interfaces
{
    public interface IMenuService
    {
        Task ShowMainMenuAsync(CancellationToken cancellationToken = default);
    }
}
