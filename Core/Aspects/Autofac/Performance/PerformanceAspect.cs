using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Castle.DynamicProxy;
using Core.Utilities.Interceptors;
using Core.Utilities.IoC;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Aspects.Autofac.Performance
{
    public class PerformanceAspect : MethodInterception
    {
        private int _interval;
        private Stopwatch _stopwatch; //timer-> işlemin ne kadar süreceğini hesaplamak için 

        public PerformanceAspect(int interval)//interval->geçen süre. örneğin 5 verirsek 5 sn'yi geçerse bizi uyarsın, br yavaşlık var demek
        {
            _interval = interval;
            _stopwatch = ServiceTool.ServiceProvider.GetService<Stopwatch>();//stopwatch kronometre. stopwatchın instance'ını coremodule'e ekiyoruz
        }

        //metotun önünde kronometreyi başlatıyorum
        protected override void OnBefore(IInvocation invocation)
        {
            _stopwatch.Start();
        }
        //metot bittiğinde de o ana kadar geçen süreyi hesaplıyorum
        protected override void OnAfter(IInvocation invocation)
        {
            if (_stopwatch.Elapsed.TotalSeconds > _interval)
            {
                Debug.WriteLine($"Performance : {invocation.Method.DeclaringType.FullName}.{invocation.Method.Name}-->{_stopwatch.Elapsed.TotalSeconds}");
            }
            _stopwatch.Reset();
        }
    }
}
