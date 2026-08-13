using UnityEngine;
using System.Collections.Generic;
using System.IO;

namespace My3DGame
{
    /// <summary>
    /// 이펙트 데이터 리스트를 관리하는 ScriptableObject
    /// 속성 : 이펙트 데이터 리스트
    /// 기능 : 데이터 저장하기, 불러오기 
    /// </summary>
    public class EffectData : BaseData
    {
        #region Variables
        public List<EffectClip> clips;      //이펙트 데이터 리스트

        //파일 (xml, json)
        //리소스 폴더 이하 경로 - Resources.Load 경로
        public const string dataPath = "Data";       
        public const string fileName = "EffectData.json";
        #endregion

        //생성자
        public EffectData() { }

        //데이터(이펙트 데이터 리스트) 저장하기 
        public void SaveData()
        {
            //json
            //클립리스트에 있는 이름을 툴 이름 목록 리스트로 저장
            int length = GetDataCount();
            for (int i = 0; i < length; i++)
            {
                clips[i].id = i;
                clips[i].name = names[i];
            }

            //파일에 저장할 데이터 준비
            EffectDatabase database = new EffectDatabase();
            database.clips = clips;
            //저장할 데이터를 json 타입의 텍스트로 변경
            string jsonOutput = JsonUtility.ToJson(database.clips);
            //파일 저장
            string filePath = Application.dataPath + dataPath_Asset + fileName;
            File.WriteAllText(filePath, jsonOutput);
        }

        //데이터(이펙트 데이터 리스트) 불러오기
        public void LoadData()
        {
            TextAsset asset = ResourcesManager.Load<TextAsset>(dataPath);
            if(asset == null || asset.text == null)
            {
                return;
            }

            //json
            EffectDatabase database = JsonUtility.FromJson<EffectDatabase>(asset.text);
            clips = database.clips;

            int length = clips.Count;
            names = new List<string>();
            for (int i = 0; i < length; i++)
            {
                names.Add(clips[i].name);
            }
        }

        //데이터 추가하기 - 추가 후 데이터 목록 갯수 반환
        public override int AddData(string newName)
        {
            //데이터가 하나도 없을때
            if(names == null)
            {
                //리스트 새로 생성하고 데이터 추가
                names = new List<string>() { newName };
                clips = new List<EffectClip>() { new EffectClip() };
            }
            else
            {
                names.Add(newName); //이름 목록에 새로운 이름 추가
                clips.Add(new EffectClip());    //이펙트 데이터 리스트 추가
            }

            return GetDataCount();
        }

        //데이터 복사하기
        public override void CopyData(int index)
        {

        }

        //데이터 제거하기
        public override void RemoveData(int index)
        {

        }
    }
}