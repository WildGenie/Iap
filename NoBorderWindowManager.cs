using Caliburn.Micro;
using System.Collections.Generic;
using System.Windows;

namespace Iap
{
   internal class NoBorderWindowManager:WindowManager
    {
        protected override Window CreateWindow(
          object rootModel,
          bool isDialog,
          object context,
          IDictionary<string, object> settings)
        {
            return base.CreateWindow(
                rootModel,
                isDialog,
                context,
                new Dictionary<string, object>
                {
                    { "WindowStyle", WindowStyle.None },
                    { "WindowState", WindowState.Maximized },
                    { "WindowStartupLocation", WindowStartupLocation.CenterScreen },
                    { "ResizeMode", ResizeMode.NoResize }
                });
        }
    }
}
