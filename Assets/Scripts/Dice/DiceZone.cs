using UnityEngine;

public class DiceZone : MonoBehaviour
{
    [Header("Dice's Rigidbody")]
    public Rigidbody diceRigidBody;
    private int diceNumber;
    public BoxCollider boxCollider;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (!GameController.instance.canRoll)
        {
            if (diceRigidBody.velocity.x == 0f && diceRigidBody.velocity.y == 0f && diceRigidBody.velocity.z == 0f)
            {
                switch (other.gameObject.name)
                {
                    case "Side_01":
                        boxCollider.enabled = false;
                        diceNumber = 6;
                        GameController.instance.DoneRolling(diceNumber);
                        break;

                    case "Side_02":
                        boxCollider.enabled = false;
                        diceNumber = 5;
                        GameController.instance.DoneRolling(diceNumber);
                        break;

                    case "Side_03":
                        boxCollider.enabled = false;
                        diceNumber = 4;
                        GameController.instance.DoneRolling(diceNumber);
                        break;

                    case "Side_04":
                        boxCollider.enabled = false;
                        diceNumber = 3;
                        GameController.instance.DoneRolling(diceNumber);
                        break;

                    case "Side_05":
                        boxCollider.enabled = false;
                        diceNumber = 2;
                        GameController.instance.DoneRolling(diceNumber);
                        break;

                    case "Side_06":
                        boxCollider.enabled = false;
                        diceNumber = 1;
                        GameController.instance.DoneRolling(diceNumber);
                        break;
                }
            }

        }
    }
}
