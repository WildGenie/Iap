namespace Iap.Commands
{
  public  class ViewAdvertCommand
    {
        private string remainingTime;

        public ViewAdvertCommand(string remainingTime)
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
