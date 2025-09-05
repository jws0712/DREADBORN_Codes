namespace DREADBORN
{
    //System
    using System;

    //UnityEngine
    using UnityEngine;
    using UnityEngine.UI;
    

    [Serializable]
    //�÷��̾� Ŭ���� Ÿ�԰� ��
    public struct CharacterModelEntry
    {
        public CharacterType type;
        public GameObject model;
    }

    [Serializable]
    //�÷��̾� Ŭ���� Ÿ�԰� ������
    public struct CharacterIconEntry
    {
        public CharacterType type;
        public Sprite icon;
    }
}


