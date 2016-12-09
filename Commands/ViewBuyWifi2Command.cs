namespace Iap.Commands
{
  public class ViewBuyWifi2Command
    {
        private string remainingTime;

        public ViewBuyWifi2Command(string remainingTime)
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
