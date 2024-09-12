using System.Security.Claims;

namespace Chat.Api.Helper
{
    public class UserHelper
    {
        private  readonly IHttpContextAccessor? _httpContextAccessor;
        private readonly IHostEnvironment _hostEnvironment;
        public UserHelper(IHttpContextAccessor? httpContextAccessor, IHostEnvironment hostEnvironment)
        {
            _httpContextAccessor = httpContextAccessor;
            _hostEnvironment = hostEnvironment;
        }

        public  Guid GetUserId()
        {
            if (_httpContextAccessor.HttpContext?.User?.Claims == null)
            {
                throw new InvalidOperationException("HttpContext or User claims are not available.");
            }

            var nameIdentifierClaim = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (nameIdentifierClaim == null || string.IsNullOrEmpty(nameIdentifierClaim))
            {
                throw new InvalidOperationException("NameIdentifier claim is not present.");
            }

            if (Guid.TryParse(nameIdentifierClaim, out var userId))
            {
                return userId;
            }
            else
            {
                throw new FormatException("Invalid GUID format for user ID.");
            }
        }

    }
}
