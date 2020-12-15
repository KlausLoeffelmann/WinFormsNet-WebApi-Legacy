using System.Threading;
using System.Threading.Tasks;

namespace System.Windows.Forms
{
    public class AsyncControl : Control
    {
        private SynchronizationContext _syncContext;
        private readonly SendOrPostCallback _sendOrPostCallback;

        public AsyncControl()
        {
            _syncContext = SynchronizationContext.Current;
        }

        public T Invoke<T>(Delegate action, params object[] @params)
        {
            return (T)this.Invoke(action, @params);
        }

        public async Task<T> InvokeAsync<T>(
            Func<T> invokeDelegate,
            object state = null,
            CancellationToken cancellationToken = default(CancellationToken),
            TimeSpan timeOutSpan = default(TimeSpan),
            params object[] args)
        {
            var tokenRegistration = default(CancellationTokenRegistration);
            RegisteredWaitHandle registeredWaitHandle = null;

            try
            {
                var taskCompletionSource = new TaskCompletionSource<bool>();
                var asyncResult = BeginInvoke(invokeDelegate, args);

                registeredWaitHandle = ThreadPool.RegisterWaitForSingleObject(
                    asyncResult.AsyncWaitHandle,
                    new WaitOrTimerCallback(InvokeAsyncCallBack),
                    taskCompletionSource,
                    timeOutSpan.Milliseconds,
                    true);

                tokenRegistration = cancellationToken.Register(
                    CancellationTokenRegistrationCallBack,
                    taskCompletionSource);

                await taskCompletionSource.Task;
                var returnObject = this.EndInvoke(asyncResult);
                return (T)returnObject;
            }
            finally
            {
                registeredWaitHandle?.Unregister(null);
                tokenRegistration.Dispose();
            }

            void InvokeAsyncCallBack(object state, bool timeOut)
                => ((TaskCompletionSource<bool>)state).TrySetResult(timeOut);

            void CancellationTokenRegistrationCallBack(object state)
                => ((TaskCompletionSource<bool>)state).TrySetCanceled();
        }

        public async Task<T> InvokeAsync<T>(
            Func<Task<T>> invokeDelegate,
            object state = null,
            CancellationToken cancellationToken = default(CancellationToken),
            TimeSpan timeOutSpan = default(TimeSpan),
            params object[] args)
        {
            var tokenRegistration = default(CancellationTokenRegistration);
            RegisteredWaitHandle registeredWaitHandle = null;

            try
            {
                var taskCompletionSource = new TaskCompletionSource<bool>();
                var asyncResult = BeginInvoke(invokeDelegate, args);

                registeredWaitHandle = ThreadPool.RegisterWaitForSingleObject(
                    asyncResult.AsyncWaitHandle,
                    new WaitOrTimerCallback(InvokeAsyncCallBack),
                    taskCompletionSource,
                    timeOutSpan.Milliseconds,
                    true);

                tokenRegistration = cancellationToken.Register(
                    CancellationTokenRegistrationCallBack,
                    taskCompletionSource);

                await taskCompletionSource.Task;
                var returningTask = (Task<T>)this.EndInvoke(asyncResult);
                return await returningTask;
            }
            finally
            {
                registeredWaitHandle?.Unregister(null);
                tokenRegistration.Dispose();
            }

            void InvokeAsyncCallBack(object state, bool timeOut)
                => ((TaskCompletionSource<bool>)state).TrySetResult(timeOut);

            void CancellationTokenRegistrationCallBack(object state)
                => ((TaskCompletionSource<bool>)state).TrySetCanceled();
        }
    }
}
