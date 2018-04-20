/// <summary>
/// Базовых интерфейс для всех объектов которые можно использовать (ключи, оружие)
/// </summary>
public interface iUsable 
{
    event System.Action OnUse;

    void Use();
}
