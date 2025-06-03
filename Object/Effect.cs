namespace DREADBORN
{
    //System
    using System.Collections;

    //UnityEngine
    using UnityEngine;

    public class Effect : MonoBehaviour
    {
        [Header("Info")]
        [SerializeField] private float durationTime = default;

        private void OnEnable()
        {
            StartCoroutine(Co_Destroy());
        }

        private IEnumerator Co_Destroy()
        {
            yield return new WaitForSeconds(durationTime);

            ObjectPoolManager.Instance.ReturnObject(this.gameObject);
        }
    }
}

