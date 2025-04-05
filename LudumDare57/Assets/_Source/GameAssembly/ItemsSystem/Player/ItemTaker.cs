using InputSystem;
using Services;
using UnityEngine;
using VContainer;

namespace ItemsSystem.Player
{
    public class ItemTaker : MonoBehaviour
    {
        [SerializeField] private LayerMask itemLayer;

        private GameObject _currentAvailableCarryObject;
        private PlayerInput _input;
        private ItemHolder _itemHolder;

        [Inject]
        private void Construct(PlayerInput input, ItemHolder itemHolder)
        {
            _input = input;
            _itemHolder = itemHolder;
        }

        private void Start() => Bind();

        private void OnDestroy() => Expose();

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!LayerService.CheckLayersEquality(other.gameObject.layer, itemLayer))
                return;

            _currentAvailableCarryObject = other.gameObject;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!LayerService.CheckLayersEquality(other.gameObject.layer, itemLayer) ||
                _currentAvailableCarryObject != other.gameObject)
                return;

            _currentAvailableCarryObject = null;
        }

        private void OnInteract()
        {
            if (!_currentAvailableCarryObject ||
                (_itemHolder.CurrentObject && _itemHolder.CurrentObject.ItemType == ItemType.OBJECT))
                return;

            var carry = _currentAvailableCarryObject!.GetComponent<CarryObject>();
            _itemHolder.TakeItem(carry);
            _currentAvailableCarryObject = null;
        }


        private void Bind() => _input.OnInteract += OnInteract;

        private void Expose() => _input.OnInteract -= OnInteract;
    }
}