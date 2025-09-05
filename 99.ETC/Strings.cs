namespace DREADBORN
{
    //애니매이션 클립 이름
    public static class AnimationClipName
    {
        public const string Light_Attack1 = nameof(Light_Attack1);
        public const string Light_Attack2 = nameof(Light_Attack2);
        public const string Light_Attack3 = nameof(Light_Attack3);

        public const string Heavy_Attack1 = nameof(Heavy_Attack1);
        public const string Heavy_Attack2 = nameof(Heavy_Attack2);
        public const string Heavy_Attack3 = nameof(Heavy_Attack3);

        public const string Die = nameof(Die);
        public const string Hit = nameof(Hit);
        public const string DefendHit = nameof(DefendHit);
        public const string StandUp = nameof(StandUp);
    }

    //애니매이터 파라메터 이름
    public static class AnimatorParameter
    {
        //Player
        public const string Horizontal = nameof(Horizontal);
        public const string Vertical = nameof(Vertical);
        public const string IsGround = nameof(IsGround);
        public const string IsSprint = nameof(IsSprint);
        public const string IsDefend = nameof(IsDefend);

        //FadeMangaer
        public const string Out = nameof(Out);
        public const string In = nameof(In);

        //TitleManager
        public const string EnableJoinRoomPanel = nameof(EnableJoinRoomPanel);
        public const string DisableJoinRoomPanel = nameof(DisableJoinRoomPanel);
        public const string GameStart = nameof(GameStart);
    }

    //씬 이름
    public static class SceneName
    {
        public const string SplashScene = nameof(SplashScene);
        public const string TitleScene = nameof(TitleScene);
        public const string LoadingScene = nameof(LoadingScene);
        public const string LobbyScene = nameof(LobbyScene);
        public const string Stage1 = nameof(Stage1);
        public const string ResultScene = nameof(ResultScene);
        public const string GameTestScene = nameof(GameTestScene);
    }

    //테그 이름
    public static class TagName
    {
        public const string MissionTable = nameof(MissionTable);
    }

    //테그 이름
    public static class LayerName
    {
        public const string Player = nameof(Player);
    }

    //커스텀 프로퍼티 키
    public static class CustomPropertyKey
    {
        public const string PlayerType = nameof(PlayerType);
        public const string MaxHp = nameof(MaxHp);
        public const string CurrentHp = nameof(CurrentHp);
        public const string IsDead = nameof(IsDead);
        public const string Level = nameof(Level);
    }

    //오브젝트 풀 오브젝트 이름
    public static class ObjectPoolObjectName
    {
        public const string HitEffect = nameof(HitEffect);
        public const string FootStepEffect = nameof(FootStepEffect);
        public const string ReviveEffect = nameof(ReviveEffect);
        public const string DamageTextEffect = nameof(DamageTextEffect);
        public const string DefendEffect = nameof(DefendEffect);
        public const string BerserkerRageEffect = nameof(BerserkerRageEffect);
        public const string VanguardSkill = nameof(VanguardSkill);
    }
    
}

