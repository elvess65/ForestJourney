using UnityEngine;

public class KeyController : Item_Base
{
    public enum KeyTypes
    {
        Key1,
        Key2,
        Key3
    }

    public KeyTypes Type = KeyTypes.Key1;

    protected override void Start()
    {
    }

    protected override void Update()
    {
    }

    public override void Interact()
    {
        base.Interact();

        Use();
    }
    public override void Use()
    {
        base.Use();

        try
        {
            GetComponent<EventAction_DynamicMazeSimple>().GenerateMaze();
        }
        catch
        {

        }

        GameManager.Instance.AddKey(Type);

        Destroy(gameObject);
    }
}
