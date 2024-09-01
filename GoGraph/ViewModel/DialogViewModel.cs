using DialogWindow;
using GoGraph.Commands;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GoGraph.ViewModel
{
    public abstract class DialogViewModel : INotifyPropertyChanged, IDialogResultVMHelper
    {     
        private RelayCommand _okCommand;
        private RelayCommand _cancelCommand;

        protected void InvokeRequestCloseDialog(RequestCloseDialogEventArgs e)
        {
            var handler = RequestCloseDialog;
            if (handler != null)
                handler(this, e);
        }

        public RelayCommand OkCommand
        {
            get => _okCommand ??= new RelayCommand(_ => InvokeRequestCloseDialog(new RequestCloseDialogEventArgs(true)));
            set => _okCommand = value;
        }

        public RelayCommand CancelCommand
        {
            get => _cancelCommand ??= new RelayCommand(_ => InvokeRequestCloseDialog(new RequestCloseDialogEventArgs(false)));
            set => _cancelCommand = value;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler<RequestCloseDialogEventArgs> RequestCloseDialog;

        public void OnPropertyChanged([CallerMemberName] string prop = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
