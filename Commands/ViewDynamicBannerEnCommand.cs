using Iap.Models;
using System.Collections.Generic;

namespace Iap.Commands
{
   public class ViewDynamicBannerEnCommand
    {
        private readonly List<ButtonLinkModel> buttonsDetails;

        private readonly string adLink;

        public ViewDynamicBannerEnCommand(
                        List<ButtonLinkModel> buttonsDetails,
                        string adLink
        )
        {
            this.buttonsDetails = buttonsDetails;
            this.adLink = adLink;
        }

        public List<ButtonLinkModel> ButtonDetails
        {
            get
            {
                return this.buttonsDetails;
            }
        }

        public string AdLink
        {
            get { return this.adLink; }
        }
    }
}
