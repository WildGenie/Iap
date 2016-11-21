using System.Collections.Generic;
using System.Threading.Tasks;
using Iap.Models;

namespace Iap.Services
{
   public interface IGetScreenDetailsService
    {
        IReadOnlyCollection<ButtonLinkModel> GetButtonLinksDetails();
    }
}
