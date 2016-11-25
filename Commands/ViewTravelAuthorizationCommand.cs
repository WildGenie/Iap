namespace Iap.Commands
{
   public class ViewTravelAuthorizationCommand
    {
        private string remaingTime;
        
        public ViewTravelAuthorizationCommand(string remaingTime)
        {
            this.remaingTime = remaingTime;
        }

        public string RemaingTime
        {
            get
            {
                return this.remaingTime;
            }
        }
    }
}
