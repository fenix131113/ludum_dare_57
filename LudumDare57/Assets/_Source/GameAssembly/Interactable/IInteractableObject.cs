namespace Interactable
{
    public interface IInteractableObject
    {
        bool CanInteract();
        virtual void Interact()
        {
        }
    }
}