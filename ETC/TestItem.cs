namespace DREADBORN
{
    using UnityEngine;

    public class TestItem : MonoBehaviour, IInteractable
    {
        public void Interaction()
        {
            Destroy(gameObject);
        }
    }
}

