namespace DREADBORN
{
    public interface IInteractable
    {
        //상호작용 시간
        public float InteractTime { get; }

        //상호작용
        public void Interaction();
    }
}