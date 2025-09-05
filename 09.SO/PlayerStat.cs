namespace DREADBORN
{
    //UnityEngine
    using UnityEngine;

    [CreateAssetMenu(fileName = "PlayerStat", menuName = "Scriptable Objects/PlayerStat")]
    public class PlayerStat : ScriptableObject
    {
        [Header("Info")]
        [SerializeField] private CharacterType characterType;
        [SerializeField] private Sprite iconSprite;
        [SerializeField] private float hp;
        [SerializeField] private float moveSpeed;
        [SerializeField] private float sprintSpeed;
        [SerializeField] private float jumpHeight;
        [SerializeField] private float maxDefensePoint;
        [SerializeField] private float maxRecoverDefenseTime; //최대 방어력 회복 시간
        [SerializeField] private float recoverDefenseDelay; //피격 후 방어력 회복 대기 시간
        [SerializeField] private Sprite skillIcon;
        [SerializeField] private float skillDurationTime;
        [SerializeField] private float skillCoolTime;

        //프로퍼티
        public CharacterType CharacterType => characterType;
        public Sprite IconSprite => iconSprite;
        public float Hp => hp;
        public float MoveSpeed => moveSpeed;
        public float SprintSpeed => sprintSpeed;
        public float JumpHeight => jumpHeight;
        public float MaxDefensePoint => maxDefensePoint;
        public float MaxRecoverDefenseTime => maxRecoverDefenseTime;
        public float RecoverDefenseDelay => recoverDefenseDelay;
        public Sprite SkillIcon => skillIcon;
        public float SkillDurationTime => skillDurationTime;
        public float SkillCoolTime => skillCoolTime;
    }
}
