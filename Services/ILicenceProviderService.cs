namespace Iap.Services
{
  public  interface ILicenceProviderService
    {
        bool hasAlreadyKey();

        void writeKeyToRegistry(string type);

        string RetrieveTypeFromRegistry();
    }
}
