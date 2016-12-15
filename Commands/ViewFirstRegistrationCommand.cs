namespace Iap.Commands
{
   public class ViewFirstRegistrationCommand
    {
        private string kioskType;

        public ViewFirstRegistrationCommand(string kioskType)
        {
            this.kioskType = kioskType;
        }

        public string KioskType
        {
            get
            {
                return this.kioskType;
            }
        }
    }
}
