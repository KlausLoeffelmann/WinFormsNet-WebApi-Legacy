using System.Threading;
using System.Threading.Tasks;

namespace System.Windows.Forms
{
    public class AsyncControl : Control
    {
        public AsyncControl()
        {
        }

        public async Task InvokeAsync(
            Func<Task> invokeDelegate,
            object state = null,
            CancellationToken cancellationToken = default,
            TimeSpan timeOutSpan = default,
            params object[] args)
        {
            await InvokeAsync<Task>((Delegate)invokeDelegate, state, cancellationToken, timeOutSpan, args);
        }

        public async Task<T> InvokeAsync<T>(
            Func<T> invokeDelegate,
            object state = null,
            CancellationToken cancellationToken = default,
            TimeSpan timeOutSpan = default,
            params object[] args)
        {
            return await InvokeAsync<T>((Delegate) invokeDelegate, state, cancellationToken, timeOutSpan, args);
        }

        public async Task InvokeAsync(
            Action invokeDelegate,
            object state = null,
            CancellationToken cancellationToken = default,
            TimeSpan timeOutSpan = default,
            params object[] args)
        {
            await InvokeAsync<object>((Delegate)invokeDelegate, state, cancellationToken, timeOutSpan, args);
        }

        public async Task<T> InvokeAsync<T>(
            Func<Task<T>> invokeDelegate,
            object state = null,
            CancellationToken cancellationToken = default,
            TimeSpan timeOutSpan = default,
            params object[] args)
        {
            var task = await InvokeAsync<Task<T>>((Delegate)invokeDelegate, state, cancellationToken, timeOutSpan, args);
            return await task;
        }

        private async Task<T> InvokeAsync<T>(
            Delegate invokeDelegate,
            object state = null,
            CancellationToken cancellationToken = default,
            TimeSpan timeOutSpan = default,
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
                return (T) returnObject;
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
