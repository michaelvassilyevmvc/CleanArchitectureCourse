using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace WebApp.Interfaces
{
    public interface IBackgroundJobService
    {
        void Schedule<T>(Expression<Func<T, Task>> expression);
    }
}