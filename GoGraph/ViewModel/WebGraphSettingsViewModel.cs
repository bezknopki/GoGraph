using GoGraph.Model;

namespace GoGraph.ViewModel
{
    public class WebGraphSettingsViewModel : DialogViewModel
    {
        public WebGraphSettingsModel Model { get; set; } = new WebGraphSettingsModel();        

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
    }
}
