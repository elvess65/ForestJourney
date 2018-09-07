using mytest.Main.MiniGames.ChargeGenerator.UI;
using UnityEngine;

namespace mytest.Main.MiniGames.ChargeGenerator
{
    public abstract class BaseItem : MonoBehaviour
    {
        public enum ItemTypes
        {
            Generator,
            Charger,
            ChargeFactory,
            Charge
        }

        public ItemTypes ItemType;

        public abstract void UpdateItem(float deltaTime);
    }

    public abstract class Item<T> : BaseItem where T: UI_Base
    {
        public T ItemUI;
    }
}

namespace mytest.Main.MiniGames.ChargeGenerator.UI
{
    public abstract class UI_Base : MonoBehaviour
    { }
}
