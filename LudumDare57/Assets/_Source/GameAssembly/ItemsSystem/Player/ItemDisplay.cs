using System;
using DG.Tweening;
using UnityEngine;
using VContainer;

namespace ItemsSystem.Player
{
    public sealed class ItemDisplay : MonoBehaviour
    {
        [SerializeField] private Transform objectPoint;
        [SerializeField] private float objectTakeDuration = 0.325f;

        private ItemHolder _itemHolder;

        [Inject]
        private void Construct(ItemHolder itemHolder) => _itemHolder = itemHolder;

        private void Start() => Bind();

        private void OnDestroy() => Expose();

        private void DisplayItem(ACarryObject item)
        {
            if(!item)
                return;
            
            switch (item.ItemType)
            {
                case ItemType.WEAPON:
                    DisplayWeapon(item);
                    break;
                case ItemType.OBJECT:
                    DisplayObjectItem(item);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void DisplayWeapon(ACarryObject weapon)
        {
            weapon.gameObject.SetActive(true);
        }

        private void DisplayObjectItem(ACarryObject item)
        {
            item.transform.parent = objectPoint;
            item.DisablePhysics();
            item.transform.DOLocalMove(
                new Vector3(0, 0, item.transform.localPosition.z),
                objectTakeDuration).SetEase(Ease.OutBack).SetId(item.gameObject);
        }

        private void Bind() => _itemHolder.OnCurrentItemChanged += DisplayItem;

        private void Expose() => _itemHolder.OnCurrentItemChanged -= DisplayItem;
    }
}