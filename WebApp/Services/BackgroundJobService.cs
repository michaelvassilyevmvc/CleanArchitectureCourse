using Hangfire;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using WebApp.Interfaces;

namespace WebApp.Services
{
    public class BackgroundJobService : IBackgroundJobService
    {
        public void Schedule<T>(Expression<Func<T, Task>> expression)
        {
            BackgroundJob.Schedule(expression, TimeSpan.Zero);
        }
    }
}