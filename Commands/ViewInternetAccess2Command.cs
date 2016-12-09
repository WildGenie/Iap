namespace Iap.Commands
{
   public class ViewInternetAccess2Command
    {
        private string remainingTime;

        public ViewInternetAccess2Command(string remainingTime)
        {
            this.remainingTime = remainingTime;
        }

        public string RemainingTime
        {
            get
            {
                return this.remainingTime;
            }
        }
    }
}
