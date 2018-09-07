using UnityEngine;

namespace mytest.Main.MiniGames.ChargeGenerator
{
    public class Item_ChargeFactory : BaseItem
    {
        public Item_Charge ChargeItemPrefab;

        private Item_Charge m_ItemCharge;

        public Item_Charge Item
        {
            get { return m_ItemCharge; }
        }

        public void CreateCharge(Camera cam)
        {
            gameObject.SetActive(false);

            m_ItemCharge = Instantiate(ChargeItemPrefab, transform.position, Quaternion.identity, transform.parent);
            m_ItemCharge.Init(cam);
        }

        public void HideCharge()
        {
            Destroy(m_ItemCharge.gameObject);
        }

        public override void UpdateItem(float deltaTime)
        {
        }
    }
}
