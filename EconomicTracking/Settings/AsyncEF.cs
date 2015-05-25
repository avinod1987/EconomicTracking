using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EconomicTracking
{
    public class AsyncBatch<T> where T : DbContext, new()
    {
        /// <summary>
        /// Creates a new instance of <see cref="AsyncBatch"/> with the given DbContext
        /// </summary>
        public static AsyncBatch<T> Create()
        {
            var unitOfWork = new AsyncBatch<T>();
            unitOfWork.context = new T();
            unitOfWork.actions = new List<Expression<Action<T>>>();
            return unitOfWork;
        }

        /// <summary>
        /// Execute all actions that have been queued asynchronously
        /// </summary>
        /// <param name="callback">The method to execute when all actions have been completed</param>
        /// <param name="exceptionCallback">The method to exeute if there was an unhandled exception</param>
        public void ExecuteAsync(Action callback, Action<Exception> exceptionCallback = null)
        {
            var worker = new BackgroundWorker();
            worker.DoWork += (s, e) =>
            {
                foreach (var action in this.actions)
                    action.Compile()(this.context);
            };
            worker.RunWorkerCompleted += (s, e) =>
            {
                context.Dispose();

                if (e.Error == null && callback != null)
                    callback();
                else if (e.Error != null && exceptionCallback != null)
                    exceptionCallback(e.Error);
            };
            worker.RunWorkerAsync();
        }

        /// <summary>
        /// Queue an action to be executed asynchronously
        /// </summary>
        /// <param name="action">The action</param>
        public AsyncBatch<T> Queue(Expression<Action<T>> action)
        {
            actions.Add(action);
            return this;
        }

        private List<Expression<Action<T>>> actions;
        private T context;
    }
    public static class BackgroundWorkerHelper
    {
        public static void Run(DoWorkEventHandler doWork,
                  RunWorkerCompletedEventHandler completed = null,
                  ProgressChangedEventHandler progressChanged = null)
        {
            using (var backgroundWorker = new BackgroundWorker())
            {
                backgroundWorker.DoWork += doWork;
                if (completed != null)
                    backgroundWorker.RunWorkerCompleted += completed;
                if (progressChanged != null)
                {
                    backgroundWorker.WorkerReportsProgress = true;
                    backgroundWorker.ProgressChanged += progressChanged;
                }
                backgroundWorker.RunWorkerAsync();
            }
        }
    }
}
