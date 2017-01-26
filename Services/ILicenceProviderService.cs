using System.Threading;
using System.Threading.Tasks;

namespace Iap.Services
{
  public  interface ILicenceProviderService
    {
        bool hasAlreadyKey();

        bool writeKeyToRegistry(string type, string id, string licenceName);

        void deleteFromRegistry();

        string RetrieveTypeFromRegistry();

        void ShowUniqueIds();

        string checkLicencesStatus();

        Task<string> checkPcLicence(CancellationTokenSource ct);

        Task<string> sendPcData(string type, string licenceName, CancellationToken ct);
    }
}
