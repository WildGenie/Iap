namespace Iap.Commands
{
  public class ViewInternetAccessCommand
    {
        private string remaingTime;

        public ViewInternetAccessCommand(string remaingTime)
        {
            this.remaingTime = remaingTime;
        }

        public string RemainingTime
        {
            get
            {
                return this.remaingTime;
            }
        }
    }
}
