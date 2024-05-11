namespace Connect4.Frontend.Shared
{
    public delegate Task AsyncEventHandler<in T>(object sender, T e) where T : EventArgs;
}