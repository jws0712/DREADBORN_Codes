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

        public const string Dead = nameof(Dead);
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
        public const string IsJump = nameof(IsJump);
        public const string IsGround = nameof(IsGround);
        public const string IsSprint = nameof(IsSprint);
        public const string IsDefend = nameof(IsDefend);

        //FadeMangaer
        public const string Out = nameof(Out);
        public const string In = nameof(In);
    }

    //씬 이름
    public static class SceneName
    {
        public const string TitleScene = nameof(TitleScene);
        public const string InGameScene = nameof(InGameScene);
        public const string Stage1 = nameof(Stage1);
        public const string LobbyScene = nameof(LobbyScene);
        public const string StageLoadingScene = nameof(StageLoadingScene);
    }

    //커스텀 프로퍼티 키
    public static class CustomPropertyKey
    {
        public const string SelectCharacterType = nameof(SelectCharacterType);
    }
}

