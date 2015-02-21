using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace BlueSheep.Common.Extensions
{
    static class ReflectionExtensions // Classe de bouh²
    {
        #region Méthodes publiques
        public static T CreateDelegate<T>(this ConstructorInfo ctor)
        {
            var parameters = ctor.GetParameters().Select(param => Expression.Parameter(param.ParameterType)).ToList();

            var lamba = Expression.Lambda<T>(Expression.New(ctor, parameters), parameters);

            return lamba.Compile();
        }
        #endregion
    }
}
