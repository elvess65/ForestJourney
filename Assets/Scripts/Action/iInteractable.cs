/// <summary>
/// Базовый интерфейс для всех объектов, с которыми можно взаимодейтсовать 
/// </summary>
public interface iInteractable
{
    event System.Action OnInteract;
    event System.Action OnInteractionFinished;

    void Interact();
}

