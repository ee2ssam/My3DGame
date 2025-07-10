using UnityEditor;
using UnityEngine;
using My3DGame.GameData;
using UnityObject = UnityEngine.Object;

namespace My3DGame.Tool
{
    /// <summary>
    /// 이펙트 데이터를 관리하는 툴 제작
    /// </summary>
    public class EffectTool : EditorWindow
    {
        #region Variables
        //이펙트 데이터
        private static EffectData effectData;

        //선택된 이펙트 클립 이펙트 오브젝트
        private GameObject effectSource = null;

        //Editor UI
        public int uiWidthLarge = 300;  //툴 창의 폭
        public int uiWidthMiddle = 200;
        private int selection = 0;      //선택된 데이터의 현재 인덱스
        private Vector2 SP1 = Vector2.zero;
        private Vector2 SP2 = Vector2.zero;
        #endregion

        //툴 Window 불러오기(show)
        [MenuItem("Tools/Effect Tool")]
        static void Init()
        {
            //EffectData 객체 생성하고 데이터 가져오기
            effectData = ScriptableObject.CreateInstance<EffectData>();
            effectData.LoadData();

            //Tool Window 열기
            EffectTool window = GetWindow<EffectTool>(false, "Effect Tool");
            window.Show();
        }


        #region Unity Event Method
        //툴 Window UI 구성

        private void OnGUI()
        {
            //effectData 체크
            if (effectData == null)
                return;

            EditorGUILayout.BeginVertical();
            {
                UnityObject source = effectSource;

                //데이터 툴의 상단 레이어 (데이터 추가, 복사, 제거 버튼 구성)
                EditorHelper.EditToolTopLayer(effectData, ref selection, ref source,
                    uiWidthMiddle);
                effectSource = (GameObject)source;

                //데이터 부분
                EditorGUILayout.BeginHorizontal();
                {
                    //데이터 이름 리스트 레이어
                    EditorHelper.EditorToolListLayer(effectData, ref selection, ref source,
                        uiWidthLarge, ref SP1);
                    effectSource = (GameObject)source;

                    //선택된 데이터 설정 레이어
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();
        }
        #endregion
    }
}
