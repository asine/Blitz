using System;
using System.Linq;
using System.Threading.Tasks;

using Blitz.Common.Core;

namespace Blitz.Client.Core
{
    public static class TaskExtensions
    {
        public static Task<T> ToTask<T>(this T t)
        {
            // http://stackoverflow.com/questions/7514375/chaining-two-functions-taska-and-a-taskb

            return new Task<T>(() => t);
        }

        public static Task<U> SelectMany<T, U>(this Task<T> task, Func<T, Task<U>> f)
        {
            // http://stackoverflow.com/questions/7514375/chaining-two-functions-taska-and-a-taskb

            return new Task<U>(() =>
            {
                task.Start();
                var t = task.Result;
                var ut = f(t);
                ut.Start();
                return ut.Result;
            });
        }

        public static Task<V> SelectMany<T, U, V>(this Task<T> task, Func<T, Task<U>> f, Func<T, U, V> c)
        {
            // http://stackoverflow.com/questions/7514375/chaining-two-functions-taska-and-a-taskb

            return new Task<V>(() =>
            {
                task.Start();
                var t = task.Result;
                var ut = f(t);
                ut.Start();
                var utr = ut.Result;
                return c(t, utr);
            });
        }

        public static Task Then(this Task first, Func<Task> next)
        {
            // http://blogs.msdn.com/b/pfxteam/archive/2010/11/21/10094564.aspx?Redirected=true

            if (first == null) throw new ArgumentNullException("first");
            if (next == null) throw new ArgumentNullException("next");

            var tcs = new TaskCompletionSource<object>();
            first.ContinueWith(_ =>
            {
                if (first.IsFaulted) tcs.TrySetException(first.Exception.InnerExceptions);
                else if (first.IsCanceled) tcs.TrySetCanceled();
                else
                {
                    try
                    {
                        var t = next();
                        if (t == null) tcs.TrySetCanceled();
                        else
                            t.ContinueWith(__ =>
                            {
                                if (t.IsFaulted) tcs.TrySetException(t.Exception.InnerExceptions);
                                else if (t.IsCanceled) tcs.TrySetCanceled();
                                else tcs.TrySetResult(null);
                            }, TaskContinuationOptions.ExecuteSynchronously);
                    }
                    catch (Exception exc)
                    {
                        tcs.TrySetException(exc);
                    }
                }
            }, TaskContinuationOptions.ExecuteSynchronously);

            return tcs.Task;
        }

        public static Task<T2> Then<T1, T2>(this Task<T1> first, Func<T1, Task<T2>> next)
        {
            // http://blogs.msdn.com/b/pfxteam/archive/2010/11/21/10094564.aspx?Redirected=true

            if (first == null) throw new ArgumentNullException("first");
            if (next == null) throw new ArgumentNullException("next");

            var tcs = new TaskCompletionSource<T2>();
            first.ContinueWith(_ =>
            {
                if (first.IsFaulted)
                {
                    tcs.TrySetException(first.Exception.InnerExceptions);
                }
                else if (first.IsCanceled)
                {
                    tcs.TrySetCanceled();
                }
                else
                {
                    try
                    {
                        var t = next(first.Result);
                        if (t == null)
                        {
                            tcs.TrySetCanceled();
                        }
                        else
                        {
                            t.ContinueWith(__ =>
                            {
                                if (t.IsFaulted)
                                {
                                    tcs.TrySetException(t.Exception.InnerExceptions);
                                }
                                else if (t.IsCanceled)
                                {
                                    tcs.TrySetCanceled();
                                }
                                else
                                {
                                    tcs.TrySetResult(t.Result);
                                }
                            }, TaskContinuationOptions.ExecuteSynchronously);
                        }
                    }
                    catch (Exception exc)
                    {
                        tcs.TrySetException(exc);
                    }
                }
            }, TaskContinuationOptions.ExecuteSynchronously);

            return tcs.Task;
        }

        public static Task<T2> ThenDo<T1, T2>(this Task<T1> first, Func<T1, T2> next)
        {
            if (first == null) throw new ArgumentNullException("first");
            if (next == null) throw new ArgumentNullException("next");

            var tcs = new TaskCompletionSource<T2>();
            first.ContinueWith(_ =>
            {
                if (first.IsFaulted)
                {
                    tcs.TrySetException(first.Exception.InnerExceptions);
                }
                else if (first.IsCanceled)
                {
                    tcs.TrySetCanceled();
                }
                else
                {
                    try
                    {
                        var result = next(first.Result);
                        
                        tcs.TrySetResult(result);
                    }
                    catch (Exception exc)
                    {
                        tcs.TrySetException(exc);
                    }
                }
            }, TaskContinuationOptions.ExecuteSynchronously);

            return tcs.Task;
        }

        public static Task ThenDo<T1>(this Task<T1> first, Action<T1> next)
        {
            if (first == null) throw new ArgumentNullException("first");
            if (next == null) throw new ArgumentNullException("next");

            var tcs = new TaskCompletionSource<object>();
            first.ContinueWith(_ =>
            {
                if (first.IsFaulted)
                {
                    tcs.TrySetException(first.Exception.InnerExceptions);
                }
                else if (first.IsCanceled)
                {
                    tcs.TrySetCanceled();
                }
                else
                {
                    try
                    {
                        next(first.Result);

                        tcs.TrySetResult(null);
                    }
                    catch (Exception exc)
                    {
                        tcs.TrySetException(exc);
                    }
                }
            }, TaskContinuationOptions.ExecuteSynchronously);

            return tcs.Task;
        }

        public static Task ThenDo(this Task first, Action next)
        {
            if (first == null) throw new ArgumentNullException("first");
            if (next == null) throw new ArgumentNullException("next");

            var tcs = new TaskCompletionSource<object>();
            first.ContinueWith(_ =>
            {
                if (first.IsFaulted)
                {
                    tcs.TrySetException(first.Exception.InnerExceptions);
                }
                else if (first.IsCanceled)
                {
                    tcs.TrySetCanceled();
                }
                else
                {
                    try
                    {
                        next();

                        tcs.TrySetResult(null);
                    }
                    catch (Exception exc)
                    {
                        tcs.TrySetException(exc);
                    }
                }
            }, TaskContinuationOptions.ExecuteSynchronously);

            return tcs.Task;
        }

        public static void Finally(this Task task, Action action)
        {
            if (action == null) throw new ArgumentNullException("action");

            task.ContinueWith(t => action(), TaskContinuationOptions.ExecuteSynchronously);
        }

        public static Task Catch<TException>(this Task task, Action<TException> exceptionHandler)
            where TException : Exception
        {
            if (task == null) throw new ArgumentNullException("task");
            if (exceptionHandler == null) throw new ArgumentNullException("exceptionHandler");

            var tcs = new TaskCompletionSource<object>();
            task.ContinueWith(t =>
            {
                if (!task.IsFaulted && task.Exception == null)
                {
                    try
                    {
                        tcs.TrySetResult(null);
                    }
                    catch (Exception exc)
                    {
                        tcs.TrySetException(exc);
                    }
                }
                else if (task.IsCanceled)
                {
                    tcs.TrySetCanceled();
                }
                else if (task.IsFaulted)
                {
                    var exception = t.Exception.Flatten().InnerExceptions.FirstOrDefault() ?? t.Exception;

                    if (exception is TException)
                    {
                        exceptionHandler((TException)exception);
                    }

                    tcs.TrySetException(exception);
                }
            }, TaskContinuationOptions.ExecuteSynchronously);

            return tcs.Task;
        }

        public static Task<T> Catch<T, TException>(this Task<T> task, Action<TException> exceptionHandler)
            where TException : Exception
        {
            if (task == null) throw new ArgumentNullException("task");
            if (exceptionHandler == null) throw new ArgumentNullException("exceptionHandler");

            var tcs = new TaskCompletionSource<T>();
            task.ContinueWith(t =>
            {
                if (!task.IsFaulted && task.Exception == null)
                {
                    try
                    {
                        tcs.TrySetResult(t.Result);
                    }
                    catch (Exception exc)
                    {
                        tcs.TrySetException(exc);
                    }
                }
                else if (task.IsCanceled)
                {
                    tcs.TrySetCanceled();
                }
                else if (task.IsFaulted)
                {
                    var exception = t.Exception.Flatten().InnerExceptions.FirstOrDefault() ?? t.Exception;

                    if (exception is TException)
                    {
                        exceptionHandler((TException)exception);
                    }

                    tcs.TrySetException(exception);
                }
            }, TaskContinuationOptions.ExecuteSynchronously);

            return tcs.Task;
        }

        public static Task CatchAndHandle<TException>(this Task task, Action<TException> exceptionHandler)
            where TException : Exception
        {
            if (task == null) throw new ArgumentNullException("task");
            if (exceptionHandler == null) throw new ArgumentNullException("exceptionHandler");

            var tcs = new TaskCompletionSource<object>();
            task.ContinueWith(t =>
            {
                if (!task.IsFaulted && task.Exception == null)
                {
                    try
                    {
                        tcs.TrySetResult(null);
                    }
                    catch (Exception exc)
                    {
                        tcs.TrySetException(exc);
                    }
                }
                else if (task.IsCanceled)
                {
                    tcs.TrySetCanceled();
                }
                else if (task.IsFaulted)
                {
                    var exception = t.Exception.Flatten().InnerExceptions.FirstOrDefault() ?? t.Exception;

                    if (exception is TException)
                    {
                        exceptionHandler((TException)exception);
                    }

                    tcs.TrySetResult(null);
                }
            }, TaskContinuationOptions.ExecuteSynchronously);

            return tcs.Task;
        }

        public static Task HandleExceptions(this Task task)
        {
            if (task == null) throw new ArgumentNullException("task");

            var tcs = new TaskCompletionSource<object>();
            task.ContinueWith(t =>
            {
                if (!task.IsFaulted && task.Exception == null)
                {
                    try
                    {
                        t.Wait();
                        tcs.TrySetResult(null);
                    }
                    catch (Exception exc)
                    {
                        tcs.TrySetResult(null);
                    }
                }
                else if (task.IsCanceled)
                {
                    tcs.TrySetCanceled();
                }
                else if (task.IsFaulted)
                {
                    var exception = t.Exception.Flatten();

                    tcs.TrySetResult(null);
                }
            }, TaskContinuationOptions.ExecuteSynchronously);

            return tcs.Task;
        }

        public static Task<T> LogException<T>(this Task<T> task, ILog logger)
        {
            task.ContinueWith(t =>
            {
                var exception = t.Exception.Flatten();

                logger.Error(exception);
            }, TaskContinuationOptions.ExecuteSynchronously | TaskContinuationOptions.OnlyOnFaulted);

            return task;
        }

        public static Task LogException(this Task task, ILog logger)
        {
            task.ContinueWith(t =>
            {
                var exception = t.Exception.Flatten();

                logger.Error(exception);
            }, TaskContinuationOptions.ExecuteSynchronously | TaskContinuationOptions.OnlyOnFaulted);

            return task;
        }
    }
}