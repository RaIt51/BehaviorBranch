using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace AI.UI
{
    public class CommandInput : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent<string> onCommand;

        [SerializeField]
        private KeyCode keyStartingCommand = KeyCode.Return;

        [SerializeField]
        private InputField inputField;

        private bool flagCancelingInput = false;

        private void Update()
        {
            if (!flagCancelingInput && Input.GetKeyDown(keyStartingCommand))
            {
                StartCommanding();
            }
            else if (flagCancelingInput)
            {
                flagCancelingInput = false;
            }
        }

        private void StartCommanding()
        {
            //stop time
            Time.timeScale = 0;

            //forcus on inputfield
            inputField.interactable = true;
            inputField.ActivateInputField();
        }

        public void Command()
        {
            //revert time
            Time.timeScale = 1;

            //get command
            string command = inputField.text;

            //invoke event
            onCommand.Invoke(command);

            //clear inputfield
            inputField.text = "";

            //unforcus on inputfield
            inputField.DeactivateInputField();
            inputField.interactable = false;

            flagCancelingInput = true;
        }
    }
}
