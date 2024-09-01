using GoGraph.Model;
using System.Windows;

namespace GoGraph.ViewModel
{
    public class ExecuteSettingsViewModel : DialogViewModel
    {
        private bool _isRootNodeChoiceVisible;
        private bool _isGoalNodeChoiceVisible;

        public ExecuteSettingsModel Model { get; set; } = new ExecuteSettingsModel();

        public Visibility RootNodeChoiceVisibility => _isRootNodeChoiceVisible ? Visibility.Visible : Visibility.Collapsed;
        public Visibility GoalNodeChoiceVisibility => _isGoalNodeChoiceVisible ? Visibility.Visible : Visibility.Collapsed;

        public ExecuteSettingsViewModel(bool isRootNodeChoiceVisible, bool isGoalNodeChoiceVisible)
        {
            _isRootNodeChoiceVisible = isRootNodeChoiceVisible;
            _isGoalNodeChoiceVisible = isGoalNodeChoiceVisible;
        }

        public int RootNodeName
        {
            get => Model.RootNode;
            set
            {
                Model.RootNode = value;
                OnPropertyChanged(nameof(RootNodeName));
            }
        }

        public int GoalNodeName
        {
            get => Model.GoalNode;
            set
            {
                Model.GoalNode = value;
                OnPropertyChanged(nameof(GoalNodeName));
            }
        }
    }
}
