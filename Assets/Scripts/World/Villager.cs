using UnityEngine;

public class Villager : WorldCharacter
{

    private void Update()
    {
        
    }

    public void StartTalking()
    {
        if(!world)
        {
            Debug.LogError($"{transform.name} No world!");
            return;
        }

        if (done) return;

        if (!world.IsQuestGoal(this))
        {
            // Not targeted
            Debug.LogError($"{transform.name} -> Is not quest goal");
            return;
        }

        // If is the quest goal
        done = true;
        world.StartDialog(this, dialogData);
    }

    public override void DialogEnded()
    {
        Debug.Log($"{transform.name} -> Dialog ended");
        done = true;

        if (hasSomethingToGive)
        {
            // Give -> gift
            world.ReceiveGiftFromChara(this, gift);
        }
    }

    // Events

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        StartTalking();
    }

    private void OnDrawGizmos()
    {
        CoreUtility.DrawText(transform.position + Vector3.up*3, PublicName, Color.black, size:15);
    }
}
