namespace Iap.Commands
{
   public class ViewTwoButtonsAdvertCommand
    {
        private string remainingTime;

        public ViewTwoButtonsAdvertCommand(string remainingTime)
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
