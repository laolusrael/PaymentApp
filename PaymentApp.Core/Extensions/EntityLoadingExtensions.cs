using System;
using System.Runtime.CompilerServices;

namespace PaymentApp.Persistence.Extensions
{

    public static class EntityLoadingExtensions
    {
        public static TRelated Load<TRelated>(this Action<object, string> loader, object entity, ref TRelated navigationField, [CallerMemberName] string navigationName = null) where TRelated : class
        {
            loader?.Invoke(entity, navigationName);
            return navigationField;
        }
    }
}
