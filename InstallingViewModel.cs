﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using System.Threading;
using Iap.Services;
using Iap.Commands;

namespace Iap
{
   public class InstallingViewModel:Screen
    {
        private readonly IEventAggregator events;
        private readonly ILicenceProviderService licenceProvider;

        public InstallingViewModel(IEventAggregator events, ILicenceProviderService licenceProvider)
        {
            this.events = events;
            this.licenceProvider = licenceProvider;
        }

        public IEventAggregator Events
        {
            get
            {
                return this.events;
            }
        }

        public string Type
        {
            get;
            set;
        }

        private string generateRandomLicence()
        {
            Random random = new Random();
            int length = 16;
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public async void CheckAndValidate(string type)
        {
              CancellationTokenSource cts;
           cts = new CancellationTokenSource();
           cts.CancelAfter(TimeSpan.FromSeconds(20));

           bool canInstall = this.licenceProvider.checkLicencesStatus() == "1" ? true : false;

           if(canInstall)
           {
                string randomLicenceName = this.generateRandomLicence();

               string response = await this.licenceProvider.sendPcData(type, randomLicenceName, cts.Token);

               if(response.TrimStart().TrimEnd()!=null)
               {
                   try
                   {
                       string licenceID = response.TrimStart().TrimEnd();
                       if (licenceID != "")
                       {

                           if (this.licenceProvider.writeKeyToRegistry(type, licenceID, randomLicenceName))
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
