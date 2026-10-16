using UnityEngine;

namespace My3DGame
{
    /// <summary>
    /// 아이템 오브젝트들을 관리하는 스크립터블 오브젝트
    /// </summary>
    [CreateAssetMenu(fileName = "new ItemDataBase", menuName = "ItemSystem/Item DataBase")]
    public class ItemDataBaseSO : ScriptableObject
    {
        #region Variables
        public ItemObjectSO[] itemObjects;  //아이템 오브젝트 목록
        #endregion

        #region Unity Even Method
        //인스펙터창에서 값이 변경시 호출된다
        private void OnValidate()
        {
            //아이템 오브젝트의 아이디 셋팅
            for (int i = 0; i < itemObjects.Length; i++)
            {
                //아이템 오브젝트 체크
                if (itemObjects[i] == null)
                    continue;

                itemObjects[i].data.id = i;
            }
        }
        #endregion
    }
}