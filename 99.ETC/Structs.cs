namespace DREADBORN
{
    //System
    using System;

    //UnityEngine
    using UnityEngine;
    using UnityEngine.UI;
    

    [Serializable]
    //플레이어 클래스 타입과 모델
    public struct CharacterModelEntry
    {
        public CharacterType type;
        public GameObject model;
    }

    [Serializable]
    //플레이어 클래스 타입과 아이콘
    public struct CharacterIconEntry
    {
        public CharacterType type;
        public Sprite icon;
    }
}


