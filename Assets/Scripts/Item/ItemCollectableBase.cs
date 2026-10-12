using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Items
{
    public class ItemCollectableBase : MonoBehaviour
    {
        public ItemType itemType;

        public ParticleSystem particleSystem;
        public string compareTag = "Player";
        public GameObject graphicItem;
        public float timeToHide = 1;

        public Collider collider;

        [Header("Sounds")]
        public AudioSource audioSource;


        private void OnTriggerEnter(Collider collision)
        {
            if (collision.transform.CompareTag(compareTag))
            {
                Collect();
            }

        }

        protected virtual void Collect()
        {
            if(collider != null) collider.enabled = false; //CHECK IF WORKING (BUG DOUBLE COLLISION)
            if (graphicItem != null) graphicItem.SetActive(false);
            Invoke(nameof(HideItem), timeToHide);
            OnCollect();
        }

        private void HideItem()
        {
            gameObject.SetActive(false);
        }

        protected virtual void OnCollect()
        {
            if (particleSystem != null) particleSystem.Play();
            if (audioSource != null) audioSource.Play();
            ItemManager.Instance.AddByType(itemType);
        }
    }
}
