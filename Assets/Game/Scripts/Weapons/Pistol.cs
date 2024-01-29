namespace Taydogmus
{
    public class Pistol : BaseWeapon
    {
        public override void SetStatus(bool status)
        {
            model.SetActive(status);
        }
    }
}