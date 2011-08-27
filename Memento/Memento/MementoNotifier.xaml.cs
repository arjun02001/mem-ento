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
using System.Windows.Shapes;
using WPFTaskbarNotifier;

namespace Memento
{
    /// <summary>
    /// Interaction logic for MementoNotifier.xaml
    /// </summary>
    public partial class MementoNotifier : TaskbarNotifier
    {
        private string message;
        public string Message
        {
            get { return message; }
            set { message = value; TaskTextblock.Text = message; }
        }

        public MementoNotifier()
        {
            InitializeComponent();
            TaskTextblock.Text = message;
        }
    }
}
