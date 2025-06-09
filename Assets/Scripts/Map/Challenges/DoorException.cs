using UnityEngine;

public class DoorException : Door
{
    private void OnEnable()
    {
        LeverManager.doorOpen += HandleDoorOpen;
    }

    private void OnDisable()
    {
        LeverManager.doorOpen -= HandleDoorOpen;
    }

    protected override void Update()
    {
        if (shouldMove)
        {
            VerticalMove();
        }
    }
    private void HandleDoorOpen(GameObject doorToOpen)
    {
        if (doorToOpen == this.gameObject)
        {
            shouldMove = true;
        }
    }
}
