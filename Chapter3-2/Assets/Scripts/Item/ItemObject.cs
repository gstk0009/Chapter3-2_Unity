using UnityEngine;

public interface IInteractable
{
    public string GetInteractPrompt();

    public void OnInteract();
}

public class ItemObject : MonoBehaviour, IInteractable
{
    public ItemData itemData;
    
    public string GetInteractPrompt()
    {
        string str = $"{itemData.DisplayName}\n{itemData.Description}";
        return str;
    }

    public void OnInteract()
    {
        PlayerManager.Instance.Player.itemData = itemData;
        PlayerManager.Instance.Player.addItem?.Invoke();
        Destroy(gameObject);
    }

    // �� ��ư ��ȣ �ۿ� �� �� �ִϸ��̼� ����
}
