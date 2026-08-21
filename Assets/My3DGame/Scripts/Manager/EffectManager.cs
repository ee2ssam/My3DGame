using My3DGame;
using UnityEngine;

namespace My3DGame
{
    public class EffectManager : Singleton<EffectManager>
    {

        #region Variables
        public EffectData effectDataSo;
        #endregion

        #region Custom Method
        //이펙트 데이터를 가져다가 이펙트 플레이 시킨다
        public GameObject EffectOneShot(EffectList effectList, Vector3 position)
        {
            EffectClip clip = effectDataSo.GetClip((int)effectList);
            GameObject effectGo = clip.InstantiateEffect(position);
            effectGo.SetActive(true);
            //Destroy(effectGo, clip.lifetime);

            return effectGo;
        }
        #endregion
    }
}