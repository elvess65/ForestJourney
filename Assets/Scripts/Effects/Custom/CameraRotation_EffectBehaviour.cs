using mytest.ActionTrigger.Effects;
using mytest.ActionTrigger.Events;
using UnityEngine;

public class CameraRotation_EffectBehaviour : MonoBehaviour
{
    [Tooltip("Угол, на который будет повернута камера")]
    public float Angle = 45;
    [Tooltip("Угловая скорость (градусов в секунду). Если не нужно использовать кастомную скорость - оставить -1 ")]
    public float Speed = -1;
    [Tooltip("Вращение по часовой стрелке")]
    public bool Clockwise = true;
    [Tooltip("Включить ли ввод по окончанию вращения")]
    public bool UnlockInputOnRotationFinished = true;
    [Tooltip("Общий объект для грифики (перемещения всех графики в позицию игрока)")]
    public GameObject RotateCameraEffectGrahics;
    [Tooltip("События окончания вращения")]
    public TriggerAction_Event[] OnRotationFinished;

    private iActionTrigger_EffectController m_EffectController;

    public void Rotate(iActionTrigger_EffectController effectController)
    {
        m_EffectController = effectController;

        //Расстояние, которое игрок пройдет по инерции после выключения ввода
        Vector3 offset = GameManager.Instance.GameState.Player.MoveDir * GameManager.Instance.GameState.Player.MoveSpeed * (PlayerController.ReduceSpeedAtLockInputTime / 1.5f);
        //Позиция для перемещения графики
        Vector3 targetPos = GameManager.Instance.GameState.Player.transform.position + offset;
        targetPos.y = RotateCameraEffectGrahics.transform.position.y;
        //Перемещение графики
        RotateCameraEffectGrahics.transform.position = targetPos;

        //Вращение камеры
        GameManager.Instance.CameraController.OnRotationFinished += RotationFinishedHandler;
        GameManager.Instance.CameraController.RotateAroundTarget(Angle, Speed, Clockwise, UnlockInputOnRotationFinished);

        //Показать компас
        GameManager.Instance.GameState.GlobalCompass.Show();
    }

    void RotationFinishedHandler()
    {
        GameManager.Instance.CameraController.OnRotationFinished -= RotationFinishedHandler;

        //Анимировать компас
        GameManager.Instance.GameState.GlobalCompass.Animate();

        //Начать проигрывать эффект окончания вращения
        if (m_EffectController != null)
            m_EffectController.DeactivateEffect_Action();

        //Начать вызывать события окончания вращения
        for (int i = 0; i < OnRotationFinished.Length; i++)
            OnRotationFinished[i].StartEvent();
    }
}
