public interface Interactable
{
    public enum GEAR
    {
        NULL,
        KEY1,
        KEY2VALID,
        KEY2INVALID,
        KNIFE
    }

    void Interact();

    void Interact(bool gear) { }

    void Pickup() { }

    bool Use() { return true; }

    void GearUse() { }

    GEAR Type() { return GEAR.NULL; }

}
