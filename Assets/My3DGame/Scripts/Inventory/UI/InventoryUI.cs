using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace My3DGame
{
    /// <summary>
    /// 인벤토리 UI를 관리하는 클래스들의 부모 (추상) 클래스
    /// 속성 : 인벤토리 오브젝트, UI에 있는 슬롯 오브젝트를 관리하는 목록(Dictionary)
    /// 필수(abstract) 기능 : 슬롯 오브젝트 생성(인벤토리에 있는 슬롯 숫자만큼)
    /// 기능 : 아이템 다른 인벤토리로 옮기기 :  드래그 앤 드랍 기능
    /// </summary>
    [RequireComponent(typeof(EventTrigger))]
    public abstract class InventoryUI : MonoBehaviour
    {

    }
}