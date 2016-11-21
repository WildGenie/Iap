using Iap.Models;
using System.Collections.Generic;

namespace Iap.Commands
{
  public class ViewRedirectToBrowserCommand
    {
        private readonly string viewName;
        private readonly string url;
        private readonly List<ButtonLinkModel> buttonsDetails;
        private readonly string position;

        public ViewRedirectToBrowserCommand(string viewName,
            string url, List<ButtonLinkModel> buttonsDetails,
            string position)
        {
            this.viewName = viewName;
            this.url = url;
            this.buttonsDetails = buttonsDetails;
            this.position = position;
        }

        public string ViewName
        {
            get{return this.viewName;}
        }

        public string Url
        {
            get{return this.url;}
        }

        public string ViewNamePrefixedWithView
        {
            get { return "View" + this.viewName; }
        }

        public List<ButtonLinkModel> ButtonsDetails
        {
            get { return this.buttonsDetails; }
        }

        public string Position
        {
            get { return this.position;}
        }
    }
}
