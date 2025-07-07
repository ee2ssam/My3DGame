using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Xml;

namespace My3DGame
{
    //데이터 파일을 읽어서 대화 정보 가져오기
    //대화창 그리기
    public class DrawDialog : MonoBehaviour
    {
        #region Variables
        //데이터 파일 읽기
        private string xmlFile = "Dialog";        //데이터 파일 이름

        private XmlNodeList allNodes;                //XML node 리스트
        #endregion

        #region Unity Event Method
        private void Start()
        {
            //데이터 읽기
            LoadDialogXml(xmlFile);

        }
        #endregion

        #region Custom Method
        //xml 파일 읽어서 XmlNodeList에 담기
        private void LoadDialogXml(string filename)
        {
            var xmlTextFile = Resources.Load<TextAsset>("Dialog/filename");

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlTextFile.text);
            allNodes = xmlDoc.SelectNodes("root/dialog");
        }
        #endregion
    }
}
