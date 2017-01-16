using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using System.Windows.Controls;
using Iap.Commands;
using Iap.Services;
using System.Threading;

namespace Iap
{
   public class SelectVersionViewModel:Screen
    {
        private readonly IEventAggregator events;
        private readonly ILicenceProviderService licenceProvider;
        private bool nextEnabled;
      

        RadioButton wciRadio;
        RadioButton iapRadio;

        public SelectVersionViewModel(IEventAggregator events, ILicenceProviderService licenceProvider)
        {
            this.events = events;
            this.licenceProvider = licenceProvider;
        }

        protected override void OnViewLoaded(object view)
        {
            this.NextEnabled = true;
            wciRadio = ((SelectVersionView)view).WCI;
            iapRadio = ((SelectVersionView)view).IAP;
            base.OnViewLoaded(view);
        }

        public bool NextEnabled
        {
           set
            {
                this.nextEnabled = value;
                NotifyOfPropertyChange(() => this.nextEnabled);
            }
            get
            {
                return this.nextEnabled;
            }
        }

       

        public IEventAggregator Events
        {
            get { return this.events; }
        }

       
        public  void Next()
        {
            this.NextEnabled = false;

            if(wciRadio.IsChecked==true)
            {
                CheckAndValidate("WCI");
               
            }
            else if(iapRadio.IsChecked==true)
            {
                CheckAndValidate("IAP");
            }
            
        }

        private bool IsNumeric(string licence)
        {
            int n;
            bool isNumeric = int.TryParse("123", out n);
            return isNumeric;
        }

        private async void CheckAndValidate(string type)
        {

            CancellationTokenSource cts;
            cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromSeconds(20));

            bool canInstall = this.licenceProvider.checkLicencesStatus() == "1" ? true : false;
            
            if(canInstall)
            {
                string response = await this.licenceProvider.sendPcData(type, cts.Token);

                if(response.TrimStart().TrimEnd()!=null)
                {
                    try
                    {
                        string licenceID = response.TrimStart().TrimEnd();
                        if (licenceID != "")
                        {

                            if (this.licenceProvider.writeKeyToRegistry(type, licenceID))
                            {
                                Handlers.GlobalCounters.kioskID = licenceID;
                                this.events.PublishOnCurrentThread(new ViewFirstRegistrationCommand(type));
                            }
                            else
                            {
                                System.Windows.MessageBox.Show("Error: Please try to run as administrator");
                            }
                        }
                        else
                        {
                            System.Windows.MessageBox.Show("Error, please try later");
                            this.events.PublishOnCurrentThread(new ViewShutDownCommand());
                        }
                    }

                    catch (OperationCanceledException)
                    {
                        System.Windows.MessageBox.Show("Error, please try later");
                        this.events.PublishOnCurrentThread(new ViewShutDownCommand());
                    }

                    catch {
                        try
                        {
                            this.licenceProvider.deleteFromRegistry();
                        }
                        catch { }
                    }
                }
            }

            else
            {
                System.Windows.MessageBox.Show("Can not install another version");
                this.events.PublishOnCurrentThread(new ViewShutDownCommand());
            }
        }

    }
}
