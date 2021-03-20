using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Castle.DynamicProxy;
using Core.Aspects.Autofac.Transaction;

namespace Core.Utilities.Interceptors
{
    public class AspectInterceptorSelector : IInterceptorSelector
        {
            //metodun üstüne aspect'lerine bakıyor ve onları çalıştırıyor
            public IInterceptor[] SelectInterceptors(Type type, MethodInfo method, IInterceptor[] interceptors)
            {
                var classAttributes = type.GetCustomAttributes<MethodInterceptionBaseAttribute>
                    (true).ToList();
                var methodAttributes = type.GetMethod(method.Name)
                    .GetCustomAttributes<MethodInterceptionBaseAttribute>(true);
                classAttributes.AddRange(methodAttributes);

               // classAttributes.Add(new TransactionScopeAspect());//böyle yazarsam tüm metotlar için transaction yönetimini çalıştırır

            return classAttributes.OrderBy(x => x.Priority).ToArray();
            }
        }
}
