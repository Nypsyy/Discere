using SGoap.Services;

public class Grenade : Item
{
    private void Awake()
    {
        ObjectManager<Grenade>.Add(this);
    }

    private void OnDestroy()
    {
        ObjectManager<Grenade>.Remove(this);
    }

    private void OnDisable()
    {
        ObjectManager<Grenade>.Remove(this);
    }
}
