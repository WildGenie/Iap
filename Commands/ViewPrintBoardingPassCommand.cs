namespace Iap.Commands
{
  public class ViewPrintBoardingPassCommand
    {
        private string remaingTime;

        public ViewPrintBoardingPassCommand(string remaingTime)
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
