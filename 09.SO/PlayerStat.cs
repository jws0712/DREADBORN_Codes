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
        [SerializeField] private float maxRecoverDefenseTime; //�ִ� ���� ȸ�� �ð�
        [SerializeField] private float recoverDefenseDelay; //�ǰ� �� ���� ȸ�� ��� �ð�
        [SerializeField] private Sprite skillIcon;
        [SerializeField] private float skillDurationTime;
        [SerializeField] private float skillCoolTime;

        //������Ƽ
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
