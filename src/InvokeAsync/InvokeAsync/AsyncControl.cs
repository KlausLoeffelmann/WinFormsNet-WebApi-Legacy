using System.Diagnostics;
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
            _sendOrPostCallback = new SendOrPostCallback(ThingsToKickOfAsync);

            _syncContext = SynchronizationContext.Current;
            //_syncContext.Post(_sendOrPostCallback, null);
        }

        private async void ThingsToKickOfAsync(object _)
        {
            try
            {
                await Task.Delay(100);
                Debug.Print("Waited 1");
                await Task.Delay(100);
                Debug.Print("Waited 2");
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
        }

        //protected async override void OnHandleCreated(EventArgs e)
        //{
        //    base.OnHandleCreated(e);

        //    var test = await Task.Run(async () =>
        //    {
        //        Thread.SpinWait(100000);
        //        var value1 = await this.InvokeAsync(AnInvokableAsyncMethod);
        //        var value2 = await this.InvokeAsync(AnInvokableMethod);
        //        return value1 + value2;
        //    });
        //}

        private int AnInvokableMethod()
        {
            Thread.Sleep(1000);
            Debug.Print("In non-async AnInvokableMethod!");
            Thread.Sleep(1000);
            Debug.Print("In non-async AnInvokableMethod!");
            return 19;
        }

        private async Task<int> AnInvokableAsyncMethod()
        {
            await Task.Delay(1000);
            Debug.Print("In AnAsyncMethod!");
            await Task.Delay(1000);
            Debug.Print("In AnAsyncMethod!");
            return await Task.FromResult(42);

        }

        private int SupposedToRunOnUiThread(int intVar)
        {
            return intVar + intVar;
        }

        public T Invoke<T>(Delegate action, params object[] @params)
        {
            return (T)this.Invoke(action, @params);
        }

        public async Task<T> InvokeAsync<T>(
            Func<T> invokeDelegate,
            object state=null,
            CancellationToken cancellationToken=default(CancellationToken),
            TimeSpan timeOutSpan=default(TimeSpan),
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
