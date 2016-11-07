namespace Iap.Commands
{
   public class ViewChangeLanguageCommand
    {
        private bool greekSelected;

        public ViewChangeLanguageCommand(bool greekSelected)
        {
            this.greekSelected = greekSelected;
        }

        public bool GreekSelected
        {
            get
            {
                return this.greekSelected;
            }
        }
    }
}
