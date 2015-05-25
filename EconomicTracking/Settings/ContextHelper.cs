using EconomicTracking.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EconomicTracking
{
    public static class ContextHelper
    {
        private static EconomicsTrackingDbContext context = null;
        public static EconomicsTrackingDbContext GetContext()
        {
            if (context == null)
                context = new EconomicsTrackingDbContext();
            return context;
        }
    }
}
