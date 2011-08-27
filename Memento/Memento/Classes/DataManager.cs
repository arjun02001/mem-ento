using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Data;
using System.Diagnostics;
using System.Data.OleDb;

namespace Memento.Classes
{
    class DataManager
    {
        public static DataTable GetData(string sql)
        {
            DataTable dt = new DataTable();
            OleDbConnection con = null;
            OleDbDataAdapter da = null;
            try
            {
                con = new OleDbConnection(Constants.CONNECTION_STRING);
                con.Open();
                da = new OleDbDataAdapter(sql, con);
                da.Fill(dt);
                con.Close();
                con.Dispose();
                da.Dispose();
            }
            catch (Exception ex)
            {
                con.Close();
                con.Dispose();
                da.Dispose();
                MessageBox.Show(new StackFrame().GetMethod().Name + Environment.NewLine + ex);
            }
            return dt;
        }

        public static bool SetData(string sql)
        {
            bool status = false;
            OleDbConnection con = null;
            OleDbCommand command = null;
            try
            {
                con = new OleDbConnection(Constants.CONNECTION_STRING);
                con.Open();
                command = new OleDbCommand(sql, con);
                status = (command.ExecuteNonQuery() > 0) ? true: false;
            }
            catch (Exception ex)
            {
                con.Close();
                con.Dispose();
                command.Dispose();
                status = false;
                MessageBox.Show(new StackFrame().GetMethod().Name + Environment.NewLine + ex);
            }
            return status;
        }
    }
}
