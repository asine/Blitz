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

        public static Task<TNewResult> ContinueWithOnSuccess<TResult, TNewResult>(this Task<TResult> task, Func<Task<TResult>, TNewResult> continuationFunction)
        {
            return task.ContinueWith(t => continuationFunction(t), TaskContinuationOptions.OnlyOnRanToCompletion);
        }

        public static Task<T> DoOnSuccess<T>(this Task<T> task, Action<T> action)
        {
            task.ContinueWith(t => action(t.Result), TaskContinuationOptions.OnlyOnRanToCompletion);

            return task;
        }

        public static void DoAlways(this Task task, Action finalAction, TaskScheduler scheduler = null)
        {
            // http://codereview.stackexchange.com/questions/21568/task-finally-extension-good-bad-or-ugly

            if (finalAction == null)
            {
                throw new ArgumentNullException("finalAction", "cannot be null");
            }

            task.ContinueWith(t => finalAction(), scheduler ?? TaskScheduler.Default);
        }

        public static Task Catch<TException>(this Task task, Action<TException> exceptionHandler, TaskScheduler scheduler = null) 
            where TException : Exception
        {
            // http://codereview.stackexchange.com/questions/21568/task-finally-extension-good-bad-or-ugly

            if (exceptionHandler == null)
            {
                throw new ArgumentNullException("exceptionHandler", "cannot be null");
            }

            task.ContinueWith(t =>
            {
                if (t.IsCanceled || !t.IsFaulted || t.Exception == null)
                    return;

                var exception = t.Exception.Flatten().InnerExceptions.FirstOrDefault() ?? t.Exception;

                if (exception is TException)
                {
                    exceptionHandler((TException)exception);
                }
            }, scheduler ?? TaskScheduler.Default);

            return task;
        }

        public static Task<T> LogException<T>(this Task<T> task, ILog logger)
        {
            task.ContinueWith(t =>
            {
                var exception = t.Exception.Flatten();

                logger.Error(exception);
            }, TaskContinuationOptions.OnlyOnFaulted);

            return task;
        }
    }
}