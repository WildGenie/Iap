using System.Collections.Generic;

using Iap.Models;

namespace Iap.Services
{
    public interface IBannerService
    {
        IReadOnlyCollection<BannerModel> GetBannerContent();
    }
}
