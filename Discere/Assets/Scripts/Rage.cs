public class Rage : EntityResource
{
    public float multiplier = .5f;

    public void IncreaseRage() {
        ChangeValue(-multiplier);
    }
}