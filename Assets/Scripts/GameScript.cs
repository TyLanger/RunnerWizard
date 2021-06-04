using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameScript : MonoBehaviour
{

    public float startRoomRadius = 6.5f;
    public Vector3 startRoomPos;

    public float endRoomRadius = 2.5f;
    public Vector3 endRoomPos;

    public MapGrid map;

    public RunnerBrain runner;
    public RangerBrain ranger;

    public GunPickup gunPickupPrefab;

    public Rule[] rules;

    public CameraFollow cam;

    // Tutorial
    int triggersTouched = 0;
    public Transform wandTrans;
    public Transform gunSpawn;
    bool gunPickedUp = false;
    public PlayerInput player;
    bool wandStolen = false;

    public Transform chatBox;
    public TextMeshProUGUI chatPrefab;

    public Image blackout;

    // Start is called before the first frame update
    void Start()
    {
        //cam = FindObjectOfType<CameraFollow>();

        HealRule.OnHealStarted += BreakTime;

        StartCoroutine(DigStartingRooms());
        //Invoke("StartRunner", 3);

        StartCoroutine(StartTutorial());

        runner.OnRoomCreated += RoomCreated;
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator DigStartingRooms()
    {
        yield return null;
        map.MoveCircle(startRoomPos, startRoomRadius, false);
        map.MoveCircle(endRoomPos, endRoomRadius, false);
        map.MoveCircle(runner.transform.position, 1, false);
        runner.SetupStartRoom(startRoomPos, startRoomRadius);
    }

    void StartRunner()
    {
        runner.stateMachine.SetState(new DigTunnel(runner));
    }

    void RoomCreated(Vector3 center, float radius, int roomNumber)
    {
        if (roomNumber < rules.Length)
        {
            if (rules[roomNumber] != null)
            {
                runner.CreateRule(rules[roomNumber]);
            }
        }

        switch (roomNumber)
        {
            case 0:
                runner.SpawnMinions(1);
                break;
            case 1:
                runner.SpawnMinions(3);
                break;
            case 2:
                //runner.CreateRule(runner.dropRule);
                runner.SpawnMinions(4);
                break;

            case 3:
                runner.SpawnMinions(4);
                SpawnGunsAroundRoom(center, radius);
                break;
            case 4:
                Rule[] minionRuleRoom4 = { null, rules[2], null };
                runner.SpawnMinions(3, minionRuleRoom4);
                runner.SpawnRangedMinions(2);
                break;
            case 5:
                Rule[] minionRules = { rules[5], null, rules[2], null, rules[5] };

                runner.SpawnMinions(5, minionRules);
                break;
            case 6:
                Rule[] minionRulesRoom6 = { rules[6], rules[3], rules[6] };

                runner.SpawnMinions(3, minionRulesRoom6);
                runner.SpawnRangedMinions(3);
                SpawnGunsAroundRoom(center, radius);
                // room larger for next room
                runner.roomRadius += 2;
                break;
            case 7:
                StartCoroutine(PeriodicScreenShake());
                // if I don't spawn at least 1 enemy, the runner won't swap to digTunnel automatically
                runner.stateMachine.SetState(new WandBroken(runner, center, radius));
                WandBrokenEvent();
                break;
            case 8:
                break;

        }
    }

    void SpawnGunsAroundRoom(Vector3 center, float radius)
    {
        int gunsToSpawn = 4;
        // spawn near the edge. 0.9x the way from the center to the edge
        radius *= map.spacing * 0.9f;
        for (int i = 0; i < gunsToSpawn; i++)
        {
            // sin(pi) = 0
            // sin(2pi) = 0
            Vector3 spawnPoint = center + new Vector3(radius * Mathf.Sin(Mathf.PI * i * 0.5f), 0.5f, radius * Mathf.Cos(Mathf.PI * i * 0.5f));
            SpawnWeaponPickup(spawnPoint);
        }
    }

    void SpawnWeaponPickup(Vector3 position)
    {
        GunPickup p = Instantiate(gunPickupPrefab, position, transform.rotation);
        int r = Random.Range(0, p.guns.Length);
        p.SetGun(r);
    }

    void WandBrokenEvent()
    {
        string message = "Did the wand just break? This can't be good";
        Chat(message);
    }

    IEnumerator PeriodicScreenShake()
    {
        Debug.Log("Periodic Screenshake");
        string message = "Uh oh. That rumbling feels like the whole place is coming down.";
        Chat(message);
        while (true)
        {
            // vary it a little
            cam.AddShake(0.5f);
            yield return new WaitForSeconds(0.5f);
            cam.AddShake(0.2f);
            yield return new WaitForSeconds(0.2f);
        }
    }

    public void TriggerTouched()
    {
        triggersTouched++;
    }

    public void GunPickedUp()
    {
        gunPickedUp = true;
    }

    public void WandStolen()
    {
        wandStolen = true;
    }

    IEnumerator DestroyChat(TextMeshProUGUI chatToDestroy)
    {
        //Debug.Log("DEstroy thing");
        yield return new WaitForSeconds(6);
        for (int i = 0; i < 15; i++)
        {
            chatToDestroy.color += new Color(-0.05f, -0.05f, -0.05f, 0);
            //chatToDestroy.color = Color.Lerp(chatToDestroy.color, new Color(0, 0, 0, 1), (float)i / 15);
            yield return new WaitForSeconds(0.5f);
        }
        //Debug.Log("DEstroy thing 10s");

        Destroy(chatToDestroy.gameObject);
    }

    void Chat(string message)
    {
        TextMeshProUGUI chat0 = Instantiate(chatPrefab, chatBox);
        chat0.text = message;
        StartCoroutine(DestroyChat(chat0));
    }

    IEnumerator StartTutorial()
    {
        yield return null;
        // what is the tutorial?
        // Walk around the plinth to check for traps
        // 4 colliders I need to touch

        string message0 = "Ah I fell into this cave and now I'm stuck.";
        string message1 = "Look at that cool wand on the plinth. I had better walk around it once and check for traps";
        string message2 = "Walk around the plinth. Look where you're going and keep going in the same direction to build up speed";
        //Debug.Log(message0);
        //Debug.Log(message1);
        //Debug.Log(message2);

        Chat(message0);
        Chat(message1);
        Chat(message2);

        yield return new WaitForSeconds(1.1f);
        cam.AddShake(1f);


        while (triggersTouched < 4)
        {
            yield return null;
        }
        GunPickup p = Instantiate(gunPickupPrefab, gunSpawn.position, gunSpawn.rotation);
        p.OnGunPickedUp += GunPickedUp;
        p.SetGun(0);

        string message3 = "Oh that's where my gun went. I'd better go pick it up.";
        string message4 = "Walk over the gun to pick it up";
        //Debug.Log(message3);
        //Debug.Log(message4);

        Chat(message3);
        Chat(message4);

        // go pick up a gun
        while (!gunPickedUp)
        {
            yield return null;
        }

        string message5 = "Let's see if it still works.";
        string message6 = "Left click to shoot. R to reload. Right click or Q to drop it again.";
        //Debug.Log(message5);
        //Debug.Log(message6);
        Chat(message5);
        Chat(message6);

        // give the player some time to play with the gun.
        // I don't really want to check to see if they press all the buttons
        // Hopefully they don't drop the gun and forget about it and use the handgun
        yield return new WaitForSeconds(6);
        map.MoveCircle(runner.transform.position, 2.5f, false);
        yield return new WaitForSeconds(2);
        // runner comes and steals the wand?
        // map.move a tunnel for the runner
        // runner.digTunnel room 0 (this room)

        string message7 = "What is that? Is that a kobold? A kobold wizard? Did he fall down here with me?";
        string message8 = "He just went and stole the wand! Without even checking for traps!";

        player.CanMove(false);
        StealWand state = new StealWand(runner, wandTrans.position);
        state.OnWandStolen += WandStolen;
        runner.stateMachine.SetState(state);
        //Debug.Log(message7);
        Chat(message7);

        // camera follow the runner?
        // what if the player is in the way?
        while (!wandStolen)
        {
            yield return null;
        }
        wandTrans.gameObject.SetActive(false);
        //Debug.Log(message8);
        Chat(message8);

        player.CanMove(true);

        // spawn 1 enemy (legs are broken?)
        // run away digTunnel
        // have to shoot the chain in the forcefield to advance
        string message9 = "That wand can animate golems? Uh oh.";
        //Debug.Log(message9);
        Chat(message9);

        yield return new WaitForSeconds(4);
        string message10 = "I don't think I can just walk through that forcefield.";
        string message11 = "But maybe my bullets can get through and break that chain holding that rule there.";
        Chat(message10);
        Chat(message11);
    }

    public void EndTriggerTouched()
    {
        // EndGame();
        StartCoroutine(EndSequence());
        
    }

    IEnumerator EndSequence()
    {
        string messge0 = "And the knight and kobold wizard put their differences aside and left the cave together.";
        string message1 = "In the end, they weren't really enemies; just both victims of their circumstances.";
        string message2 = "Turns out the real enemy was cursed wands and golems and the real treasure was the friends made along the way.";
        Chat(messge0);
        Chat(message1);
        Chat(message2);
        for (int i = 0; i < 21; i++)
        {
            blackout.color += new Color(0, 0, 0, i * 0.05f);
            yield return new WaitForSeconds(0.4f);
        }
        yield return new WaitForSeconds(6);
        string message3 = "The End";
        string message4 = "Thanks for playing.";
        string message5 = "The knight and kobold wizard ended up as great friends and went on many adventures together.";
        Chat(message3);
        yield return new WaitForSeconds(3);
        Chat(message4);
        yield return new WaitForSeconds(3);
        Chat(message5);
        yield return new WaitForSeconds(3);

    }

    public void BreakTime()
    {
        string message = "It seems the little guy got tuckered out and need a break.";
        string message2 = "Nice of him to make this little fire and invite me to sit with him.";
        Chat(message);
        Chat(message2);
    }
}
