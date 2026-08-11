using UnityEngine;
using System.Collections.Generic;

namespace My3DGame
{
    /// <summary>
    /// Data툴에서 생산되는 Data 기본(부모) 클래스
    /// 공통 속성 : 이름 목록(리스트)
    /// 공통 기능 : 데이터 갯수 가져오기, 이름 리스트 가져오기, 데이터 추가, 복사, 제거하기
    /// </summary>
    public class BaseData : ScriptableObject
    {
        #region Variables
        public List<string> names;              //이름 목록(리스트)
        public const string dataPath = "/My3DGame/Resources/Data";      //데이터 파일 경로
        #endregion

        //생성자
        public BaseData() { }

        //데이터 갯수 가져오기


        //이름 리스트 가져오기


        //데이터 추가하기

        //데이터 복사하기

        //데이터 제거하기
    }
}