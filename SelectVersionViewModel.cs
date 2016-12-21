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
                //System.Windows.MessageBox.Show(this.licenceProvider.checkLicencesStatus());
               /* this.licenceProvider.ShowUniqueIds();

                this.licenceProvider.writeKeyToRegistry("WCI");
                this.events.PublishOnCurrentThread(new ViewFirstRegistrationCommand("WCI"));*/
            }
            else if(iapRadio.IsChecked==true)
            {
                CheckAndValidate("IAP");
               // System.Windows.MessageBox.Show(this.licenceProvider.checkLicencesStatus());
                /* this.licenceProvider.writeKeyToRegistry("IAP");
                 this.events.PublishOnCurrentThread(new ViewFirstRegistrationCommand("IAP"));*/
            }
            
        }

        private async void CheckAndValidate(string type)
        {

            CancellationTokenSource cts;
            cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromSeconds(20));

            bool canInstall = this.licenceProvider.checkLicencesStatus() == "1" ? true : false;
            
            if(canInstall)
            {
                if (this.licenceProvider.writeKeyToRegistry(type))
                {
                    try
                    {

                        this.licenceProvider.writeKeyToRegistry(type);


                        string response = await this.licenceProvider.sendPcData(type, cts.Token);
                        if (response.TrimStart().TrimEnd() == "OK")
                        {
                            this.events.PublishOnCurrentThread(new ViewFirstRegistrationCommand(type));
                        }

                        else
                        {
                            System.Windows.MessageBox.Show("Error, please try later");
                            this.events.PublishOnCurrentThread(new ViewShutDownCommand());
                            try
                            {
                                this.licenceProvider.deleteFromRegistry();
                            }
                            catch { }
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        System.Windows.MessageBox.Show("Error, please try later");
                        this.events.PublishOnCurrentThread(new ViewShutDownCommand());
                    }

                    catch (Exception)
                    {
                        try
                        {
                            this.licenceProvider.deleteFromRegistry();
                        }
                        catch { }
                        System.Windows.MessageBox.Show("Error, please try later");
                        this.events.PublishOnCurrentThread(new ViewShutDownCommand());
                    }
                }
                else
                {
                    System.Windows.MessageBox.Show("Error, try to run as an administrator");
                    this.events.PublishOnCurrentThread(new ViewShutDownCommand());
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
