using UnityEngine;
using System.Linq;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace My3DGame
{
    /// <summary>
    /// 인벤토리를 관리하는 스크립터블 오브젝트
    /// 속성 : 인벤토리 컨테이너, 아이템 데이터 베이스, 인벤토리 타입
    /// 기능 : 인벤토리에 아이템 추가, 아이템 바꿔치기, 아이템 사용하기, 아이템 장착하기
    /// </summary>
    [CreateAssetMenu(fileName = "New Inventory", menuName = "ItemSystem/Inventory")]
    public class InventorySO : ScriptableObject
    {
        #region Variables        
        public Inventory container = new Inventory();   //인벤토리 목록

        public InventoryType type;          //인벤토로 종류
        public ItemDataBaseSO dataBase;     //아이템 오브젝트 목록을 가진 오브젝트
        #endregion

        #region Property
        //인벤토리 목록
        public ItemSlot[] Slots => container.slots;

        //인벤 풀 체크 - 
        public int EmptySlotCount
        {
            get
            {
                int count = 0;
                foreach (var slot in Slots)
                {
                    if(slot.item.id < 0)
                    {
                        count++;
                    }
                }

                return count;
            }
        }
        #endregion

        #region Custom Method
        //아이템 아이디로 아이템을 생성하여 아이템 추가하기
        public bool AddItem(int itemId)
        {
            Item newItem = new Item(dataBase.itemObjects[itemId]);
            return AddItem(newItem, 1);
        }

        //인벤토리에 아이템 추가
        public bool AddItem(Item item, int amont)
        {
            //null 체크
            if(item == null)
            {
                return false;
            }

            //item 있는 슬롯 찾기
            ItemSlot slot = FindItemInInventory(item);

            if (dataBase.itemObjects[item.id].isStackable == false
                || slot == null)
            {
                //빈슬롯을 찾아 아이템 추가
                if(EmptySlotCount <= 0)
                {
                    return false;
                }

                ItemSlot emptySlot = GetEmptySlot();
                emptySlot.UpdateSlot(item, amont);
            }
            else
            {
                //기존 슬롯에 수량만 추가
                slot.AddAmount(amont);
            }

            return true;
        }

        //아이템 사용하기
        public bool UseItem(ItemSlot useSlot)
        {
            //슬롯 체크
            if(useSlot.item.id <= -1 || useSlot.amount <= 0)
            {
                return false;
            }

            useSlot.AddAmount(-1);
            return true;
        }

        //선택된 아이템 판매하기
        public bool SellItem(ItemSlot sellSlot)
        {
            //슬롯 체크
            if (sellSlot.item.id <= -1 || sellSlot.amount <= 0)
            {
                return false;
            }

            //아이템 수량 1개 제거
            sellSlot.AddAmount(-1);

            return true;
        }

        //아이템 바꿔치기
        public void SwapItems(ItemSlot itemA, ItemSlot itemB)
        {
            //슬롯 체크
            if (itemA == itemB)
                return;

            //장착 가능 여부 체크
            if(itemA.CanPlaceInSlot(itemB.ItemObject)
                && itemB.CanPlaceInSlot(itemA.ItemObject))
            {
                //스왑
                ItemSlot tempSlot = new ItemSlot(itemA.item, itemA.amount);
                itemA.UpdateSlot(itemB.item, itemB.amount);
                itemB.UpdateSlot(tempSlot.item, tempSlot.amount);
            }
        }

        //아이템 장착
        public void EquipItem(ItemSlot itemSlot)
        {
            //장착될 위치를 찾아 슬롯에 있는 아이템을 장착한다
            foreach (var slot in Slots)
            {
                //장착 가능 여부 체크
                if(slot.CanPlaceInSlot(itemSlot.ItemObject))
                {
                    //가져온 아이템과 장착된 아이템을 바꿔치기
                    SwapItems(itemSlot, slot);
                    break;
                }
            }
        }

        //매개변수로 들어온 아이템이 있는 슬롯 찾기
        public ItemSlot FindItemInInventory(Item item)
        {
            return Slots.FirstOrDefault(i => i.item.id == item.id);
        }

        //매개변수로 들어온 아이템오브젝트의 아이템이 있는 슬롯 찾기
        public ItemSlot FindItemInInventory(ItemObjectSO itemObject)
        {
            return FindItemInInventory(itemObject.data);
        }

        //빈 슬롯 찾기
        public ItemSlot GetEmptySlot()
        {
            return Slots.FirstOrDefault(i => i.item.id <= -1);
        }
        #endregion

        //인벤토리 데이터 json 파일 저장하기, 불러오기
        #region Save/Load
        public string savePath = "/Inventory.json"; //저장파일 경로

        //인벤토리 데이터(container) json 파일 저장하기
        [ContextMenu("Save")]
        public void Save()
        {
            //저장할 경로
            string path = Application.persistentDataPath + savePath;

            //데이터 2진화 준비
            BinaryFormatter bf = new BinaryFormatter();
            //저장할 파일 접근
            FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write);
            //저장할 데이터를 준비
            string saveData = JsonUtility.ToJson(container, true);
            //데이터를 이진화해서 저장
            bf.Serialize(fs, saveData);

            //파일 닫기
            fs.Close();
        }

        //인벤토리 데이터(container) json 파일 불러오기
        [ContextMenu("Load")]
        public void Load()
        {
            //저장할 경로
            string path = Application.persistentDataPath + savePath;

            //파일 존재 여부 체크
            if(File.Exists(path))
            {
                //데이터 2진화 준비
                BinaryFormatter bf = new BinaryFormatter();
                //저장할 파일 접근
                FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
                //이진화된 데이터를 읽어서 역진화 한다
                string loadData = bf.Deserialize(fs).ToString();
                //json 데이터를 container 형식으로 읽는다
                JsonUtility.FromJsonOverwrite(loadData, container);

                //파일 닫기
                fs.Close();
            }
        }

        //인벤토리 비우기
        [ContextMenu("Clear")]
        public void Clear()
        {
            container.Clear();
        }
        #endregion
    }
}