using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess.Extentions
{
    internal static class ApplicationContextExtentions
    {
        public static IEnumerable<T> Init<T>(this ApplicationContext context, IEnumerable<T> initData)
            where T : class, IDatabased
        {
            IEnumerable<T>? initCollection = context.Set<T>().Local.ToBindingList();
            context.Set<T>().Load();
            if (initCollection == null || initCollection.Count() == 0)
            {
                context.Set<T>().AddRange(initData);
                context.SaveChanges();
            }
            return initCollection;
        }

    }
}
