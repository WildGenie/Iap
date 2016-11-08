using System;
namespace Iap.Unitilities
{
   public class DefaultTimeProvider:TimeProvider
    {
        public override DateTime UtcNow
        {
            get { return DateTime.UtcNow; }
        }
    }
}
