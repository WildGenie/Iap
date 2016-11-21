using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Iap.Models
{
  public class ButtonLinkModel
    {
        private BitmapImage externalEngImg;
        private BitmapImage externalGrImg;
        private BitmapImage internalEngImg;
        private BitmapImage internalGrImg;
        private BitmapImage selectedEngImg;
        private BitmapImage selectedGrImg;
        private string enUrl;
        private string grUrl;

        public ButtonLinkModel(BitmapImage externalEngImg, BitmapImage externalGrImg, BitmapImage internalEngImg, BitmapImage internalGrImg,
            BitmapImage selectedEngImg, BitmapImage selectedGrImg, string enUrl, string grUrl)
        {
            this.externalEngImg = externalEngImg;
            this.externalGrImg = externalGrImg;
            this.internalEngImg = internalEngImg;
            this.internalGrImg = internalGrImg;
            this.selectedEngImg = selectedEngImg;
            this.SelectedGrImg = selectedEngImg;
            this.enUrl = enUrl;
            this.grUrl = grUrl;
        }

        public BitmapImage ExternalEngImg
        {
            get
            {
                return externalEngImg;
            }

            set
            {
                externalEngImg = value;
            }
        }

        public BitmapImage ExternalGrImg
        {
            get
            {
                return externalGrImg;
            }

            set
            {
                externalGrImg = value;
            }
        }

        public BitmapImage InternalEngImg
        {
            get
            {
                return internalEngImg;
            }

            set
            {
                internalEngImg = value;
            }
        }

        public BitmapImage InternalGrImg
        {
            get
            {
                return internalGrImg;
            }

            set
            {
                internalGrImg = value;
            }
        }

        public BitmapImage SelectedEngImg
        {
            get
            {
                return selectedEngImg;
            }

            set
            {
                selectedEngImg = value;
            }
        }

        public BitmapImage SelectedGrImg
        {
            get
            {
                return selectedGrImg;
            }

            set
            {
                selectedGrImg = value;
            }
        }

        public string EnUrl
        {
            get
            {
                return enUrl;
            }

            set
            {
                enUrl = value;
            }
        }

        public string GrUrl
        {
            get
            {
                return grUrl;
            }

            set
            {
                grUrl = value;
            }
        }
    }
}
