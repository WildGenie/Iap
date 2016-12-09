using Iap.Models;
using System.Collections.Generic;

namespace Iap.Commands
{
   public class ViewDynamicBannerEnCommand
    {
        private readonly List<ButtonLinkModel> buttonsDetails;

        public ViewDynamicBannerEnCommand(List<ButtonLinkModel> buttonsDetails)
        {
            this.buttonsDetails = buttonsDetails;
        }

        public List<ButtonLinkModel> ButtonDetails
        {
            get
            {
                return this.buttonsDetails;
            }
        }
    }
}
