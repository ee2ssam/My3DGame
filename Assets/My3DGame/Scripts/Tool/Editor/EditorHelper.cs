using UnityEditor;
using UnityEngine;
using My3DGame.GameData;
using UnityObject = UnityEngine.Object;

namespace My3DGame.Tool
{
    /// <summary>
    /// 모든 툴에서 사용되는 공통 기능 구현
    /// </summary>
    public class EditorHelper
    {
        //데이터 툴의 상단 레이어 UI (데이터 추가, 복사, 제거 버튼 구성)
        public static void EditToolTopLayer(BaseData data, ref int seletion,  
            ref UnityObject source, int uiWidth)
        {
            EditorGUILayout.BeginHorizontal();
            {
                //추가 버튼
                if(GUILayout.Button("ADD", GUILayout.Width(uiWidth)))
                {
                    //데이터 추가 처리
                    data.AddData("New Data");
                    seletion = data.GetDataCount() - 1;
                    source = null;
                }
                //복사 버튼
                if (GUILayout.Button("COPY", GUILayout.Width(uiWidth)))
                {
                    //선택된 데이터를 복사하여 새로 추가한다
                    data.CopyData(seletion);
                    seletion = data.GetDataCount() - 1;
                    source = null;
                }
                //제거 버튼 - 데이터가 두개 이상일때만 제거가 가능하다
                if(data.GetDataCount() > 1)
                {
                    if (GUILayout.Button("REMOVE", GUILayout.Width(uiWidth)))
                    {
                        source = null;
                        data.RemoveData(seletion);
                    }
                }
                //인덱스 예외 처리
                if(seletion > (data.GetDataCount()-1))
                {
                    seletion = data.GetDataCount() - 1;
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        //데이터 이름 리스트 레이어
        public static void EditorToolListLayer(BaseData data, ref int selection,
            ref UnityObject source, int uiWidth, ref Vector2 scrollPosition)
        {
            EditorGUILayout.BeginVertical(GUILayout.Width(uiWidth));
            {
                EditorGUILayout.Separator();    //구분 공간 주기
                EditorGUILayout.BeginVertical("box");
                {
                    scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
                    {
                        if(data.GetDataCount() > 0)
                        {
                            int lastSelection = selection;
                            selection = GUILayout.SelectionGrid(selection, data.GetNameList(true), 1);
                            if(lastSelection != selection)
                            {
                                source = null;
                            }
                        }
                    }
                    EditorGUILayout.EndScrollView();
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndVertical();
        }
    }
}