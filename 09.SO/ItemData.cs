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
        public int id;                  //������ȣ
        public string itemName;         //�̸�
        public string description;      //����
        public int price;               //����


        [Header("Category")]
        public ItemType type;           //������ ����
        public ItemGrade grade;         //������ ���

        [Header("Size")]
        public int width;     //������ ����
        public int height;     //������ ����

        [Header("UI")]
        public Sprite icon;             //������ ������
        public Color backgroundColor;   //��� ��

        [Header("Stack")]
        public bool isStackable;        //���� ���� ����
        public int maxStack;            //�ִ� ���� ��
    }
}
