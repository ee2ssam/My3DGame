using UnityEngine;
using System;

namespace My3DGame
{
    /// <summary>
    /// 아이템을 관리하는 클래스
    /// 속성: 아이디(아이템데이터베이스 인덱스), 이름, 능력치
    /// </summary>
    [Serializable]
    public class Item
    {
        #region Variables
        public int id;
        public string name;

        //아이템 능력치
        public ItemBuff[] buffs;
        #endregion

        #region Constructor
        //아이템 리셋, 비어있는 아이템, 아이템 정보 제거
        public Item()
        {
            id = -1;
            name = null;
        }

        //게임에서 사용하는 아이템 생성
        //아이템 오브젝트를(정보:아이디, 이름, 능력치 최대, 최소) 가져와서 생성
        public Item(ItemObjectSO itemObect)
        {
            id = itemObect.data.id;
            name = itemObect.data.name;

            buffs = new ItemBuff[itemObect.data.buffs.Length];
            for (int i = 0; i < buffs.Length; i++)
            {
                buffs[i] = new ItemBuff(itemObect.data.buffs[i].Min,
                    itemObect.data.buffs[i].Max)
                {
                    stat = itemObect.data.buffs[i].stat
                };
            }
        }
        #endregion
    }
}