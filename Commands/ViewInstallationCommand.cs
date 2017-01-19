namespace Iap.Commands
{
   public class ViewInstallationCommand
    {
        private string type;

        public ViewInstallationCommand(string type)
        {
            this.type = type;
        }

        public string Type
        {
            get
            {
                return this.type;
            }
        }
    }
}
