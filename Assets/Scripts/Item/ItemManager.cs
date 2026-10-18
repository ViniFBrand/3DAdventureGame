using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ebac.Core.Sigleton;
using Items;

namespace Items
{
    public enum ItemType
    {
        COIN,
        LIFE_PACK
    }

    public class ItemManager : Singleton<ItemManager>
    {
        public List<ItemSetup> itemSetups;


        protected override void Awake()
        {
            base.Awake();
            Reset();
        }

        [NaughtyAttributes.Button]
        private void Reset()
        {
            foreach(var i in itemSetups)
            {
                i.soInt.value = 0;
            }
        }

        public void AddByType(ItemType itemType, int amount = 1)
        {
            if (amount < 0) return;

            itemSetups.Find(i => i.itemType == itemType).soInt.value += amount;
        }

        public void RemoveByType(ItemType itemType, int amount = -1)
        {
            if (amount > 0) return;

            var item = itemSetups.Find(i => i.itemType == itemType);
            item.soInt.value += amount;

            if (item.soInt.value < 0) item.soInt.value = 0;
        }

        #region BUTTONS
        [NaughtyAttributes.Button]
        private void AddCoin()
        {
            AddByType(ItemType.COIN);
        }

        [NaughtyAttributes.Button]
        private void AddLifePack()
        {
            AddByType(ItemType.LIFE_PACK);
        }

        [NaughtyAttributes.Button]
        private void RemoveCoin()
        {
            RemoveByType(ItemType.COIN);
        }

        [NaughtyAttributes.Button]
        private void RemoveLifePack()
        {
            RemoveByType(ItemType.LIFE_PACK);
        }

        #endregion

    }

    [System.Serializable]
    public class ItemSetup
    {
        public ItemType itemType;
        public SOInt soInt;
        public Sprite icon;
    }

}


