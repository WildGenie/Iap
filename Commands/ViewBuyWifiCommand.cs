namespace Iap.Commands
{
   public class ViewBuyWifiCommand
    {
        private string remainingTime;

        public ViewBuyWifiCommand(string remainingTime)
        {
            this.remainingTime = remainingTime;
        }

        public string RemaingTime
        {
            get
            {
                return this.remainingTime;
            }
        }
    }
}
