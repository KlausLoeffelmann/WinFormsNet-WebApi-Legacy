#nullable enable

using System.Threading;
using System.Threading.Tasks;

namespace System.Windows.Forms
{
    public class AsyncControl : Control
    {
        public Task InvokeAsync(
            Func<Task> invokeDelegate,
            TimeSpan timeOutSpan = default,
            CancellationToken cancellationToken = default,
            params object[] args) => InvokeAsync<Task>((Delegate)invokeDelegate, timeOutSpan, cancellationToken, args);

        public Task<T> InvokeAsync<T>(
            Func<T> invokeDelegate,
            TimeSpan timeOutSpan = default,
            CancellationToken cancellationToken = default,
            params object[] args) => InvokeAsync<T>((Delegate)invokeDelegate, timeOutSpan, cancellationToken, args);

        public Task InvokeAsync(
            Action invokeDelegate,
            TimeSpan timeOutSpan = default,
            CancellationToken cancellationToken = default,
            params object[] args) => InvokeAsync<object>((Delegate)invokeDelegate, timeOutSpan, cancellationToken, args);

        public async Task<T> InvokeAsync<T>(
            Func<Task<T>> invokeDelegate,
            TimeSpan timeOutSpan = default,
            CancellationToken cancellationToken = default,
            params object[] args)
        {
            Task<T>? task = await InvokeAsync<Task<T>>((Delegate)invokeDelegate, timeOutSpan, cancellationToken, args);
            return await task;
        }

        private async Task<T> InvokeAsync<T>(
            Delegate invokeDelegate,
            TimeSpan timeOutSpan = default,
            CancellationToken cancellationToken = default,
            params object[] args)
        {
            var tokenRegistration = default(CancellationTokenRegistration);
            RegisteredWaitHandle? registeredWaitHandle = null;

            try
            {
                TaskCompletionSource<bool> taskCompletionSource = new();
                IAsyncResult? asyncResult = BeginInvoke(invokeDelegate, args);

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

                object? returnObject = EndInvoke(asyncResult);
                return (T)returnObject;
            }
            finally
            {
                registeredWaitHandle?.Unregister(null);
                tokenRegistration.Dispose();
            }

            static void CancellationTokenRegistrationCallBack(object? state)
            {
                if (state is TaskCompletionSource<bool> source)
                    source.TrySetCanceled();
            }

            static void InvokeAsyncCallBack(object? state, bool timeOut)
            {
                if (state is TaskCompletionSource<bool> source)
                    source.TrySetResult(timeOut);
            }
        }
    }
}
