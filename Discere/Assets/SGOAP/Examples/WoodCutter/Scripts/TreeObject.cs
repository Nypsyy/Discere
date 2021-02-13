using SGoap;
using SGoap.Services;
using UnityEngine;

namespace SGoap.Example
{
    public class TreeObject : MonoBehaviour
    {
        public CharacterStatusController Status;
        public int Wood = 5;

        private void Awake()
        {
            Wood = Random.Range(2, 4);
            Status.Hp = Wood;
            Status.MaxHp = Wood;
        }

        private void OnEnable()
        {
            ObjectManager<TreeObject>.Add(this);
        }

        private void OnDisable()
        {
            ObjectManager<TreeObject>.Remove(this);
        }

        public void TakeWood()
        {
            if(Status != null)
                Status.TakeDamage(1);
            
            Wood--;
        }
    }
}