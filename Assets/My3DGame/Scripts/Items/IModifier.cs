
namespace My3DGame
{
    /// <summary>
    /// 능력치 더하기 기능 강제
    /// </summary>
    public interface IModifier
    {
        //내가 가진 능력치를 참조 매개변수에 더해준다
        public void AddValue(ref int baseValue);
    }
}