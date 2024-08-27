namespace DialogWindow.WeakEventHandler
{
    public delegate void UnregisterCallback<TE>(EventHandler<TE> eventHandler) where TE : EventArgs;
}
