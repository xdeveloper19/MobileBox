using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Firebase.JobDispatcher;

namespace GeoGeometry.Service
{
    public class StartUp
    {
        static readonly string TAG = "X:StartService";
        static FirebaseJobDispatcher dispatcher;

        /// <summary>
        /// Запуск задачи.
        /// </summary>
        public static void StartTracking()
        {
            Log.Debug(TAG, "Starting Tracking");

            // This is the "Java" way to create a FirebaseJobDispatcher object
            IDriver driver = new GooglePlayDriver(Application.Context);
            dispatcher = new FirebaseJobDispatcher(driver);

            //RetryStrategy retry = dispatcher.NewRetryStrategy(RetryStrategy.RetryPolicyLinear, retryTime, deadline);
            JobTrigger myTrigger = Trigger.ExecutionWindow(10, 15);

            // FirebaseJobDispatcher dispatcher = context.CreateJobDispatcher();
            Job myJob = dispatcher.NewJobBuilder()
                       .SetService<WebService>("demo-job-tag")
                       .SetTrigger(myTrigger)
                       .AddConstraint(Constraint.OnAnyNetwork)
                       .Build();

            // This method will not throw an exception; an integer result value is returned
            int scheduleResult = dispatcher.Schedule(myJob);

            Log.Debug(TAG, "Scheduling LocationJobService...");

            if (scheduleResult != FirebaseJobDispatcher.ScheduleResultSuccess)
            {
                Log.Warn(TAG, "Job Scheduler failed to schedule job!");
            }
        }


        /// <summary>
        /// Отмена задачи.
        /// </summary>
        public static void StopTracking()
        {
            Log.Debug(TAG, "Stopping Tracking");

            int cancelResult = dispatcher.CancelAll();

            // to cancel a single job:

            //int cancelResult = dispatcher.Cancel("unique-tag-for-job");
        }
    }
}