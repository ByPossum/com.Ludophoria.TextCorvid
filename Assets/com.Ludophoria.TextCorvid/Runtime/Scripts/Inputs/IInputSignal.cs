namespace TextCorvid
{
    public interface IInputSignal
    {
        void FireInput();
        void ToggleInput();
        bool GetDone();
    }
}
