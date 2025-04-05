namespace InputSystem
{
    public interface IInputValueCallback<in T>
    {
        void InputCallback(T value);
    }
}