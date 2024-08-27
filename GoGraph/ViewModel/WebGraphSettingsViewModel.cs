using DialogWindow;
using GoGraph.Commands;
using GoGraph.Model;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GoGraph.ViewModel
{
    public class WebGraphSettingsViewModel : INotifyPropertyChanged, IDialogResultVMHelper
    {
        public WebGraphSettingsModel Model { get; set; } = new WebGraphSettingsModel();

        private RelayCommand _okCommand;
        private RelayCommand _cancelCommand;

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

        public int Rows
        {
            get => Model.Rows;
            set
            {
                Model.Rows = value;
                OnPropertyChanged(nameof(Rows));
            }
        }

        public int Columns
        {
            get => Model.Columns;
            set
            {
                Model.Columns = value;
                OnPropertyChanged(nameof(Columns));
            }
        }

        private void InvokeRequestCloseDialog(RequestCloseDialogEventArgs e)
        {
            var handler = RequestCloseDialog;
            if (handler != null)
                handler(this, e);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler<RequestCloseDialogEventArgs> RequestCloseDialog;

        public void OnPropertyChanged([CallerMemberName] string prop = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
