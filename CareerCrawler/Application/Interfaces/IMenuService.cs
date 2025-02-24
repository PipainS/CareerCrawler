namespace HHParser.Application.Interfaces
{
    public interface IMenuService
    {
        /// <summary>
        /// Displays the main menu and processes user input asynchronously. Continues until the user exits or cancels the operation.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token used to cancel the operation.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        /// 
        Task ShowMainMenuAsync(CancellationToken cancellationToken = default);
    }
}
