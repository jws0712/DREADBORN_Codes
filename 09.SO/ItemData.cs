namespace DREADBORN
{
    //System
    using System;
    using System.Collections;
    using System.Collections.Generic;

    //UnityEngine
    using UnityEngine;


    [Serializable]
    public enum ItemType { Weapon, Armor, Potion, Junk }

    [Serializable]
    public enum ItemGrade { Common, Rare, Epic, Legendary }

    [CreateAssetMenu(fileName = "ItemData", menuName = "Inventory/ItemData")]
    public class ItemData : ScriptableObject
    {
        [Header("Item Info")]
        public int id;                  //고유번호
        public string itemName;         //이름
        public string description;      //설명
        public int price;               //가격


        [Header("Category")]
        public ItemType type;           //아이템 종류
        public ItemGrade grade;         //아이템 등급

        [Header("Size")]
        public int width;     //아이템 가로
        public int height;     //아이템 세로

        [Header("UI")]
        public Sprite icon;             //아이템 아이콘
        public Color backgroundColor;   //배경 색

        [Header("Stack")]
        public bool isStackable;        //스택 가능 여부
        public int maxStack;            //최대 스택 수
    }
}
