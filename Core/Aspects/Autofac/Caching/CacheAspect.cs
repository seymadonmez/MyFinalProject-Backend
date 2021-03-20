using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.DynamicProxy;
using Core.CrossCuttingConcerns.Caching;
using Core.Utilities.Interceptors;
using Core.Utilities.IoC;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Aspects.Autofac.Caching
{
    public class CacheAspect : MethodInterception
    {
        private int _duration;
        private ICacheManager _cacheManager;

        public CacheAspect(int duration = 60)
        {
            _duration = duration;
            _cacheManager = ServiceTool.ServiceProvider.GetService<ICacheManager>();
        }

        public override void Intercept(IInvocation invocation)
        {
            //namecpace+class ismi+method ismi +ve varsa parametreleri ile bir key oluşturuyoruz
            var methodName = string.Format($"{invocation.Method.ReflectedType.FullName}.{invocation.Method.Name}"); //reflection
            var arguments = invocation.Arguments.ToList();
            var key = $"{methodName}({string.Join(",", arguments.Select(x => x?.ToString() ?? "<Null>"))})";//metodun parametreleri varsa onları ekliyor,yoksa null

            //cache'te var mı diye bakıyoruz
            if (_cacheManager.IsAdd(key))
            {
                invocation.ReturnValue = _cacheManager.Get(key);//cahche'te varsa metodu hiç çalıştırmadan cache'ten getirsin 
                return;
            }
            //cache'te yoksa metodu çalıştırmaya devam etsin, veritabanından veriyi getirsin
            invocation.Proceed();
            _cacheManager.Add(key, invocation.ReturnValue, _duration);//ve cache'e eklesin
        }
    }
}
