using UnityEngine;
using System.Collections.Generic;

namespace My3DGame
{
    /// <summary>
    /// 이펙트 데이터 리스트 정의 - 파일 불러오기,저장하기에서 이용
    /// </summary>
    public class EffectDatabase
    {
        public List<EffectClip> clips;
    }

    /// <summary>
    /// 이펙트 데이터 정의
    /// </summary>
    public class EffectClip
    {
        #region Variable
        public int id;                  //id
        public string name;             //데이터 이름
        public EffectType effectType;   //이펙트 종류
        public string effectPath;       //이펙트 파일 경로
        public string effectName;       //이펙트 파일 이름
        #endregion
    }
}