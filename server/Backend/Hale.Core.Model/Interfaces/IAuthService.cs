namespace Hale.Core.Model.Interfaces
{
    using Hale.Core.Model.Models;

    public interface IAuthService
    {
        bool Authorize(string userName, string password);

        void ChangePassword(string userName, string newPassword);

        bool Activate(ActivationAttemptDTO attempt);
    }
}
