using System.Threading;
using System.Threading.Tasks;

namespace Iap.Services
{
  public  interface ILicenceProviderService
    {
        bool hasAlreadyKey();

        bool writeKeyToRegistry(string type);

        void deleteFromRegistry();

        string RetrieveTypeFromRegistry();

        void ShowUniqueIds();

        string checkLicencesStatus();

        Task<string> sendPcData(string type, CancellationToken ct);
    }
}
