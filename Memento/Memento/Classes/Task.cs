using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Diagnostics;
using System.Data;

namespace Memento.Classes
{
    class Task
    {
        public int TaskID { get; set; }
        public string Description { get; set; }
        public bool Notify { get; set; }

        public static List<Task> GetTasks(int taskid)
        {
            List<Task> tasks = new List<Task>();
            Task task = null;
            try
            {
                DataTable dt = (taskid == Constants.INVALID_TASKID) ? DataManager.GetData(Constants.GET_TASKS_QUERY) : DataManager.GetData(string.Format(Constants.GET_TASK_FROM_TASKID_QUERY, taskid));
                foreach (DataRow dr in dt.Rows)
                {
                    task = new Task();
                    task.TaskID = Convert.ToInt32(dr["TaskID"]);
                    task.Description = dr["Description"].ToString();
                    task.Notify = Convert.ToBoolean(dr["Notify"]);
                    tasks.Add(task);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(new StackFrame().GetMethod().Name + Environment.NewLine + ex);
            }
            return tasks;
        }

        public static bool UpdateTask(int taskid, string header, string description)
        {
            bool status = true;
            string sql = string.Empty;
            try
            {
                if (header.Equals(Constants.NOTIFY) && taskid != Constants.INVALID_TASKID)
                {
                    bool currentnotifystatus = Task.GetTasks(taskid)[0].Notify;
                    sql = string.Format(Constants.UPDATE_TASK_QUERY, !currentnotifystatus, taskid);
                }
                else if (header.Equals(Constants.DONE) && taskid != Constants.INVALID_TASKID)
                {
                    sql = string.Format(Constants.DELETE_TASK_QUERY, taskid);
                }
                else if (header.Equals(string.Empty) && taskid == Constants.INVALID_TASKID)
                {
                    sql = string.Format(Constants.INSERT_TASK_QUERY, description);
                }
                if(!DataManager.SetData(sql))
                {
                    status = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(new StackFrame().GetMethod().Name + Environment.NewLine + ex);
                status = false;
            }
            return status;
        }

        public static List<Task> GetNotifiableTasks()
        {
            List<Task> notifiabletasks = new List<Task>();
            Task task = null;
            try
            {
                DataTable dt = DataManager.GetData(Constants.GET_NOTIFIABLE_TASKS_QUERY);
                foreach (DataRow dr in dt.Rows)
                {
                    task = new Task();
                    task.TaskID = Convert.ToInt32(dr["TaskID"]);
                    task.Description = dr["Description"].ToString();
                    task.Notify = Convert.ToBoolean(dr["Notify"]);
                    notifiabletasks.Add(task);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(new StackFrame().GetMethod().Name + Environment.NewLine + ex);
            }
            return notifiabletasks;

        }
    }
}
