using mytest.Effects.Custom;
using System.Collections;
using System.Collections.Generic;
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
        public Item_ChargeFactory ItemChargeFactory;

        private bool m_Active = false;
        private BaseItem.ItemTypes m_LastHitItemType;
        private World2CameraMoveHehaviour m_Behaviour;

        public override void StartGame(Camera cam, LayerMask layer)
        {
            Graphics.gameObject.SetActive(true);

            //Генератор инициализируеться только раз
            ItemGenerator.OnCharged += GeneratorChargedhandler;
            ItemGenerator.Init();

            //Генерация объектов на каждой итерации
            Generate();

            //Данные для анимации объектов (движение к камере)
            float animationTime = 2;
            float distance = 7;

            //Объекты, которые нужно переместить
            List<Transform> objectsToMove = new List<Transform>();
            objectsToMove.Add(ItemCharger.transform);
            objectsToMove.Add(ItemGenerator.transform);
            objectsToMove.Add(ItemChargeFactory.transform);

            //Координаты на экране
            List<Vector3> destinationViewport = new List<Vector3>();
            destinationViewport.Add(new Vector3(0.75f, 0.5f, distance));
            destinationViewport.Add(new Vector3(0.25f, 0.5f, distance));
            destinationViewport.Add(new Vector3(0.5f, 0.5f, distance));

            //Анимация
            World2CameraMoveHehaviour.MoveData mData = new World2CameraMoveHehaviour.MoveData(AppearAnimationFinishedHandler, animationTime, objectsToMove.ToArray(), destinationViewport.ToArray());
            m_Behaviour = World2CameraMoveHehaviour.CreateWorld2CameraMoveHehaviour();
            m_Behaviour.Move(mData);

            base.StartGame(cam, layer);
        }

        public override void FinishGame()
        {
            Graphics.SetActive(false);

            base.FinishGame();
        }


        public override void ProcessGame(float deltaTime)
        {
            if (!m_Active)
                return;

            ItemCharger.UpdateItem(deltaTime);
            ItemGenerator.UpdateItem(deltaTime);
            ItemChargeFactory.UpdateItem(deltaTime);

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
                        ItemChargeFactory.Item.HandleMouseDown();
                        break;
                }
            }
        }

        void HandleMousePress(float deltaTime)
        {
            if (ItemChargeFactory.Item != null)
                ItemChargeFactory.Item.UpdateItem(deltaTime);
        }

        void HandleMouseUp()
        {
            if (ItemChargeFactory.Item != null)
                ItemChargeFactory.Item.HandleMouseUp();

            BaseItem item = Raycast();
            if (item != null)
            {
                switch (item.ItemType)
                {
                    case BaseItem.ItemTypes.ChargeFactory:
                        if (m_LastHitItemType == BaseItem.ItemTypes.Charger)
                        {
                            if (ItemCharger.CanSpark())
                                ItemChargeFactory.CreateCharge(m_Cam);
                        }
                        break;
                    case BaseItem.ItemTypes.Generator:
                        if (m_LastHitItemType == BaseItem.ItemTypes.Charge)
                        {
                            ItemChargeFactory.HideCharge();

                            if (!ItemGenerator.AddCharge())
                                Generate();
                        }
                        break;
                }
            }
        }


        void Generate()
        {
            ItemCharger.Init();
        }


        void AppearAnimationFinishedHandler()
        {
            ItemCharger.ItemUI.ShowUI(true);
            ItemGenerator.ItemUI.ShowUI(true);

            m_Active = true;
        }

        void DissappearAnimationFinishedHandler()
        {
            FinishGame();
        }

        void GeneratorChargedhandler()
        {
            m_Active = false;

            StartCoroutine(WaitBeforeFinish());
        }


        IEnumerator WaitBeforeFinish()
        {
            yield return new WaitForSeconds(1);

            m_Behaviour.MoveReserve(DissappearAnimationFinishedHandler);
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
