using UnityEngine;
public class HeadController : PlayerController
{
    public override void Die()
    {
        PlayerManager.instance.OnSelectChange(PlayerManager.PlayerPart.head);
        Recall();
    }
}
