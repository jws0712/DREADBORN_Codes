namespace DREADBORN
{
    //UnityEngine
    using UnityEngine;

    //TMP
    using TMPro;

    public class DamageTextEffect : Effect
    {
        [Header("DamageTextInfo")]
        [SerializeField] private Color color;

        private TextMeshProUGUI damageText;
        private Canvas canvas;

        private void Awake()
        {
            damageText = GetComponentInChildren<TextMeshProUGUI>();
            canvas = GetComponent<Canvas>();
        }

        private void Start()
        {
            damageText.faceColor = color;
            canvas.worldCamera = Camera.main;
        }

        private void Update()
        {
            transform.forward = Camera.main.transform.forward;
        }

        public void SetDamageText(float damage)
        {
            damageText.text = damage.ToString();
        }
    }
}
