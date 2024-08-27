using DialogWindow.WeakEventHandler;
using GoGraph.DialogWindows;
using System.Windows;

namespace DialogWindow
{
    /// <summary>
    /// Interaction logic for DialogWindow.xaml
    /// </summary>
    public partial class DialogWindow : Window
    {
        private bool _isClosed = false;

        public DialogWindow()
        {
            InitializeComponent();
            this.DialogPresenter.DataContextChanged += DialogPresenterDataContextChanged;
            this.Closed += DialogWindowClosed;
        }

        void DialogWindowClosed(object sender, EventArgs e)
        {
            this._isClosed = true;
        }

        private void DialogPresenterDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var d = e.NewValue as IDialogResultVMHelper;

            if (d == null)
                return;

            d.RequestCloseDialog += new EventHandler<RequestCloseDialogEventArgs>(DialogResultTrueEvent)
                .MakeWeak(eh => d.RequestCloseDialog -= eh);
        }

        private void DialogResultTrueEvent(object sender, RequestCloseDialogEventArgs eventargs)
        {
            if (_isClosed) return;

            this.DialogResult = eventargs.DialogResult;
        }
    }
}
