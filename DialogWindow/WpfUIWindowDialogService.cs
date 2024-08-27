namespace DialogWindow
{
    public class WpfUIWindowDialogService : IUIWindowDialogService
    {
        public bool? ShowDialog(string title, object datacontext)
        {
            var win = new DialogWindow();
            win.Title = title;
            win.DataContext = datacontext;

            return win.ShowDialog();
        }
    }
}
