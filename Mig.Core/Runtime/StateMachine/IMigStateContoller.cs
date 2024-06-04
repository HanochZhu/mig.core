namespace Mig.Core
{
    public interface IMigStateController
    {
        void Awake();

        void Sleep();

        void Update();
        void UpdateUI();
    }
}
