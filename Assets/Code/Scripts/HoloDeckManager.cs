using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoloDeckManager : MonoBehaviour
{
    // All possible warning states
    public enum WarningState {NoPower, Fire, Quarantine, None};

    // The warning state the holodecks are currently in
    public WarningState currentWarningState;

    // Text mesh component on warning text child object
    public TextMesh warningText;

    // Floats for changing the color of the warning text
    float speed = 0.75f;
    float t;




    private void Update()
    {
        // Changing color of text
        t = Mathf.PingPong(Time.time * speed, 2.0f);
        warningText.color = Color.Lerp(new Color(1, 0, 0, 1), new Color(0.25f, 0, 0, 1), Mathf.PingPong(Time.time * speed, 1));


        if(currentWarningState == WarningState.None)
        {
            warningText.gameObject.SetActive(false);
        }

        if(currentWarningState == WarningState.NoPower)
        {
            if(t > 1)
            {
                warningText.text = ("WARNING: NO POWER");
            }
            else
            {
                warningText.text = ("WARNING: QUARANTINE ACTIVE");
            }
        }

        if (currentWarningState == WarningState.Fire)
        {
            if (t > 1)
            {
                warningText.text = ("WARNING: FIRE");
            }
            else
            {
                warningText.text = ("WARNING: QUARANTINE ACTIVE");
            }
        }

        if(currentWarningState == WarningState.Quarantine)
        {
            warningText.text = ("WARNING: QUARANTINE ACTIVE");
        }

    }
}