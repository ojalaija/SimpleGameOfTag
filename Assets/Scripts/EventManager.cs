using UnityEngine.Events;

public static class EventManager
{
    public static event UnityAction ChangeRoles;

    public static void OnChangeRoles() => ChangeRoles?.Invoke();
}
