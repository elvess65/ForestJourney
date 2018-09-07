using System.Collections;
using UnityEngine;

namespace mytest.Main.MiniGames.ChargeGenerator
{
    public class MiniGame_ChargeGenerator : MiniGame_Base
    {
        [Header("General")]
        public GameObject Graphics;
        public Transform ItemsParent;
        [Header("Objects")]
        public Item_Charger ItemCharger;
        public Item_Generator ItemGenerator;
        public Item_ChargeFactory ItemChargeFactoryPrefab;
        [Header("Positions")]
        public Transform[] ItemChargeFactoryPositions;

        private bool m_Active = false;
        private BaseItem.ItemTypes m_LastHitItemType;
        private Item_ChargeFactory m_ItemChargeFactory;

        public override void StartGame(Camera cam, LayerMask layer)
        {
            ItemGenerator.OnCharged += GeneratorChargedhandler;

            ItemGenerator.Init();

            Generate();

            base.StartGame(cam, layer);

            m_Active = true;
        }

        void GeneratorChargedhandler()
        {
            m_Active = false;

            StartCoroutine(Wait());
        }

        IEnumerator Wait()
        {
            yield return new WaitForSeconds(1);
            FinishGame();
        }

        public override void FinishGame()
        {
            Graphics.SetActive(false);

            base.FinishGame();
        }

        void Generate()
        {
            ItemCharger.Init();

            m_ItemChargeFactory = Instantiate(ItemChargeFactoryPrefab, ItemsParent);
            Transform target = ItemChargeFactoryPositions[Random.Range(0, ItemChargeFactoryPositions.Length)];
            m_ItemChargeFactory.transform.position = target.position;
            m_ItemChargeFactory.transform.rotation = target.rotation;
        }


        public override void ProcessGame(float deltaTime)
        {
            if (!m_Active)
                return;

            ItemCharger.UpdateItem(deltaTime);
            ItemGenerator.UpdateItem(deltaTime);
            m_ItemChargeFactory.UpdateItem(deltaTime);

            if (Input.GetKeyDown(KeyCode.F))
                FinishGame();

            if (Input.GetMouseButtonDown(0))
                HandleMouseDown();

            if (Input.GetMouseButton(0))
                HandleMousePress(deltaTime);

            if (Input.GetMouseButtonUp(0))
                HandleMouseUp();
        }

        void HandleMouseDown()
        {
            BaseItem item = Raycast();
            if (item != null)
            {
                m_LastHitItemType = item.ItemType;

                switch (item.ItemType)
                {
                    case BaseItem.ItemTypes.Charger:
                        ItemCharger.AddCharge();
                        break;

                    case BaseItem.ItemTypes.Generator:
                        break;
                   
                    case BaseItem.ItemTypes.ChargeFactory:
                        break;

                    case BaseItem.ItemTypes.Charge:
                        m_ItemChargeFactory.Item.HandleMouseDown();
                        break;
                }
            }
        }

        void HandleMousePress(float deltaTime)
        {
            if (m_ItemChargeFactory.Item != null)
                m_ItemChargeFactory.Item.UpdateItem(deltaTime);
        }

        void HandleMouseUp()
        {
            if (m_ItemChargeFactory.Item != null)
                m_ItemChargeFactory.Item.HandleMouseUp();

            BaseItem item = Raycast();
            if (item != null)
            {
                Debug.Log(m_LastHitItemType);
                Debug.Log(item.ItemType);

                switch (item.ItemType)
                {
                    case BaseItem.ItemTypes.ChargeFactory:
                        if (m_LastHitItemType == BaseItem.ItemTypes.Charger)
                        {
                            if (ItemCharger.CanSpark())
                                m_ItemChargeFactory.CreateCharge(m_Cam);
                        }
                        break;
                    case BaseItem.ItemTypes.Generator:
                        if (m_LastHitItemType == BaseItem.ItemTypes.Charge)
                        {
                            m_ItemChargeFactory.HideCharge();

                            if (!ItemGenerator.AddCharge())
                                Generate();
                        }
                        break;
                }
            }
        }

        BaseItem Raycast()
        {
            RaycastHit hit;
            Ray ray = m_Cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 100, m_Layer))

                return hit.collider.gameObject.GetComponent<BaseItem>();

            return null;
        }
    }
}
