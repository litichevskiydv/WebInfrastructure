namespace Skeleton.Web.Integration.BaseApiFluentClient
{
    using System.Dynamic;
    using System.Reflection;
    using System.Runtime.CompilerServices;
    using System.Runtime.ExceptionServices;
    using System.Threading.Tasks;

    public class FluentChainedTask<TApiFluentClient> : DynamicObject where TApiFluentClient : BaseFluentClient
    {
        private readonly Task<TApiFluentClient> _task;

        public FluentChainedTask(Task<TApiFluentClient> task)
        {
            _task = task;
        }

        public TaskAwaiter<TApiFluentClient> GetAwaiter()
        {
            return _task.GetAwaiter();
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            var methodInfo = typeof(TApiFluentClient).GetTypeInfo().GetDeclaredMethod(binder.Name);
            if (methodInfo == null)
                return base.TryInvokeMember(binder, args, out result);

            result = new FluentChainedTask<TApiFluentClient>(
                _task
                    .ContinueWith(t =>
                                  {
                                      try
                                      {

                                          return (Task<TApiFluentClient>) methodInfo.Invoke(t.Result, args);
                                      }
                                      catch (TargetInvocationException ex)
                                      {
                                          ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                                      }
                                      return null;
                                  })
                    .Unwrap());
            return true;
        }
    }
}