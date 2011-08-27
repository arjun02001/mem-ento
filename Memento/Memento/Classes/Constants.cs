using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Memento.Classes
{
    //db password = arjunmukherji
    class Constants
    {
        //public const string CONNECTION_STRING = "Provider=Microsoft.Ace.OLEDB.12.0;Data Source=..\\..\\Data\\MementoDB.accdb;Jet OLEDB:Database Password=arjunmukherji;";
        public const string CONNECTION_STRING = "Provider=Microsoft.Ace.OLEDB.12.0;Data Source=Data\\MementoDB.accdb;Jet OLEDB:Database Password=arjunmukherji;";

        public const string GET_TASKS_QUERY = "select * from Task";
        public const string GET_TASK_FROM_TASKID_QUERY = "select * from Task where TaskID = {0}";
        public const string UPDATE_TASK_QUERY = "update Task set Notify = {0} where TaskID = {1}";
        public const string DELETE_TASK_QUERY = "delete from Task where TaskID = {0}";
        public const string INSERT_TASK_QUERY = "insert into Task (Description, Notify) values ('{0}', true)";
        public const string GET_NOTIFIABLE_TASKS_QUERY = "select * from Task where Notify = true";

        public const int INVALID_TASKID = -1234;

        public const string NOTIFY = "Notify";
        public const string DONE = "Done";
        public const string DESCRIPTION = "Description";
        public const string ABOUT = "Developed by - Arjun Mukherji. Send feedbacks to arjun02001@gmail.com.";

        public static Dictionary<string, long> INTERVAL = new Dictionary<string, long> {{"10 secs", 10000}, {"1 min", 60000}, {"5 mins", 300000}, {"15 mins", 900000},
                                                                                        {"30 mins", 1800000}, {"1 hr", 3600000}, {"2 hrs", 7200000}, {"5 hrs", 18000000}};
    }
}
