namespace DialogWindow.WeakEventHandler
{
    public static class EventHandlerUtils
    {
        public static EventHandler<TE> MakeWeak<TE>(this EventHandler<TE> eventHandler, UnregisterCallback<TE> unregister)
            where TE : EventArgs
        {
            if (eventHandler == null)
                throw new ArgumentNullException("eventHandler");

            if (eventHandler.Method.IsStatic || eventHandler.Target == null)
                throw new ArgumentException("Only instance methods are supported.", "eventHandler");

            var wehType = typeof(WeakEventHandler<,>).MakeGenericType(eventHandler.Method.DeclaringType, typeof(TE));

            var wehConstructor = wehType.GetConstructor([typeof(EventHandler<TE>), typeof(UnregisterCallback<TE>)]);

            IWeakEventHandler<TE> weh = (IWeakEventHandler<TE>)wehConstructor.Invoke([eventHandler, unregister ]);

            return weh.Handler;
        }
    }
}
