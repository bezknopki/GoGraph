namespace DialogWindow.WeakEventHandler
{
    public interface IWeakEventHandler<TE> where TE : EventArgs
    {
        EventHandler<TE> Handler { get; }
    }
}
