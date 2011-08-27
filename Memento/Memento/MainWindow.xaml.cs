using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using Memento.Classes;
using System.Data;
using System.Windows.Threading;
using System.Timers;

namespace Memento
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Task> notifiabletasks = new List<Task>();
        MementoNotifier mementonotifier = new MementoNotifier();
        bool turnoffnotification = false;
        Timer timer = null;
        int taskiterator = -1;

        public MainWindow()
        {
            InitializeComponent();
            try
            {
                TaskTextbox.Focus();
                PopulateGrid();
                PopulateIntervalComboBox();
                mementonotifier.StayOpenMilliseconds = 5000;
                mementonotifier.Show();
                StartTimer(Constants.INTERVAL[IntervalComboBox.SelectedItem.ToString()]);
            }
            catch (Exception ex)
            {
                MessageBox.Show(new StackFrame().GetMethod().Name + Environment.NewLine + ex);
            }
        }

        private void StartTimer(long interval)
        {
            try
            {
                timer = new Timer(interval);
                timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
                timer.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(new StackFrame().GetMethod().Name + Environment.NewLine + ex);
            }
        }

        void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                if (!turnoffnotification)
                {
                    mementonotifier.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, new Action(
                        delegate()
                        {
                            try
                            {
                                if (notifiabletasks.Count > 0)
                                {
                                    taskiterator = (taskiterator + 1) % notifiabletasks.Count;
                                    mementonotifier.Message = notifiabletasks[taskiterator].Description;
                                    mementonotifier.Notify();
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(new StackFrame().GetMethod().Name + Environment.NewLine + ex);
                            }
                        }));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(new StackFrame().GetMethod().Name + Environment.NewLine + ex);
            }
        }

        private void PopulateGrid()
        {
            try
            {
                System.Threading.Thread.Sleep(500);
                TaskDataGrid.ItemsSource = Task.GetTasks(Constants.INVALID_TASKID);
                TaskTextbox.Clear();
                TaskTextbox.Focus();
                notifiabletasks = Task.GetNotifiableTasks();
            }
            catch (Exception ex)
            {
                MessageBox.Show(new StackFrame().GetMethod().Name + Environment.NewLine + ex);
            }
        }

        private void PopulateIntervalComboBox()
        {
            try
            {
                IntervalComboBox.ItemsSource = Constants.INTERVAL.Keys;
                IntervalComboBox.SelectedIndex = 2;
            }
            catch (Exception ex)
            {
                MessageBox.Show(new StackFrame().GetMethod().Name + Environment.NewLine + ex);
            }
        }

        private void TurnOffNotificationCheckBox_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                turnoffnotification = ((bool)TurnOffNotificationCheckBox.IsChecked) ? true : false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(new StackFrame().GetMethod().Name + Environment.NewLine + ex);
            }
        }

        private void IntervalComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (timer != null)
                {
                    timer.Stop();
                    timer.Close();
                    timer.Dispose();
                    timer = null;
                    StartTimer(Constants.INTERVAL[IntervalComboBox.SelectedItem.ToString()]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(new StackFrame().GetMethod().Name + Environment.NewLine + ex);
            }
        }

        private void BackgrounderButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Hide();
                NotifyIcon.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                MessageBox.Show(new StackFrame().GetMethod().Name + Environment.NewLine + ex);
            }
        }

        private void TaskTextbox_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter && !string.IsNullOrEmpty(TaskTextbox.Text))
                {
                    if (Task.UpdateTask(Constants.INVALID_TASKID, string.Empty, TaskTextbox.Text))
                    {
                        PopulateGrid();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(new StackFrame().GetMethod().Name + Environment.NewLine + ex);
            }
        }

        private void DataGridCell_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                DataGridCell cell = sender as DataGridCell;
                if (cell.Column.Header.Equals(Constants.DESCRIPTION))
                {
                    cell.ToolTip = ((TextBlock)cell.Content).Text;
                    ToolTipService.SetShowDuration(cell, 60000);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(new StackFrame().GetMethod().Name + Environment.NewLine + ex);
            }
        }

        private void DataGridCell_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DataGridCell cell = sender as DataGridCell;
                if (cell != null && !cell.IsEditing && !cell.IsReadOnly)
                {
                    if (!cell.IsFocused)
                    {
                        cell.Focus();
                    }
                    DataGrid datagrid = FindVisualParent<DataGrid>(cell);
                    if (datagrid != null)
                    {
                        DataGridRow row = FindVisualParent<DataGridRow>(cell);
                        if (row != null && !row.IsSelected)
                        {
                            row.IsSelected = true;
                        }
                        if (Task.UpdateTask(((Task)row.Item).TaskID, cell.Column.Header.ToString(), string.Empty))
                        {
                            PopulateGrid();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(new StackFrame().GetMethod().Name + Environment.NewLine + ex);
            }
        }

        static T FindVisualParent<T>(UIElement element) where T : UIElement
        {
            UIElement parent = element;
            while (parent != null)
            {
                T correctlyTyped = parent as T;
                if (correctlyTyped != null)
                {
                    return correctlyTyped;
                }
                parent = VisualTreeHelper.GetParent(parent) as UIElement;
            }
            return null;
        }

        private void NotifyIcon_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                this.Show();
                this.WindowState = System.Windows.WindowState.Normal;
                this.Activate();
                NotifyIcon.Visibility = System.Windows.Visibility.Hidden;
            }
            catch (Exception ex)
            {
                MessageBox.Show(new StackFrame().GetMethod().Name + Environment.NewLine + ex);
            }
        }

        private void AboutButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(Constants.ABOUT);
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);
            NotifyIcon.Visibility = System.Windows.Visibility.Collapsed;
            if (mementonotifier != null)
            {
                mementonotifier.Close();
            }
        }
    }
}
