using Mirror;
using UnityEngine;
using TMPro;

namespace MirrorSample
{
    public class PlayerScript : NetworkBehaviour
    {
        private SceneScript sceneScript;

        public TextMeshPro playerNameText;
        public GameObject floatingInfo;

        private Material playerMaterialClone;

        // SyncVars are variable that, well, are kept synced. Will also send all the latest values when a player joins.
        // Should probably only be modified through a command.
        // In this case, also calls OnNameChanged when synced.
        [SyncVar(hook = nameof(OnNameChanged))]
        public string playerName;

        [SyncVar(hook = nameof(OnColorChanged))]
        public Color playerColor = Color.white;

        // The parameters for a syncvar hook **has** to be <Type> _Old and <Type> _New otherwise it will throw errors.
        private void OnNameChanged(string _Old, string _New)
        {
            playerNameText.text = playerName;
        }

        private void OnColorChanged(Color _Old, Color _New)
        {
            // Creates a new material as it would otherwise modify the material used for both players.
            playerNameText.color = _New;
            playerMaterialClone = new Material(GetComponent<Renderer>().material);
            playerMaterialClone.color = _New;
            GetComponent<Renderer>().material = playerMaterialClone;
        }

        void Awake()
        {
            //allow all players to run this.
            sceneScript = GameObject.FindObjectOfType<SceneScript>();
        }

        public override void OnStartLocalPlayer()
        {
            // playerScript in sceneScript is kept local, as such this is fine.
            sceneScript.playerScript = this;

            Camera.main.transform.SetParent(transform);
            Camera.main.transform.localPosition = new Vector3(0, 0, 0);

            // Just some randomized values.
            string name = "Player" + Random.Range(100, 999);
            Color color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
            CmdSetupPlayer(name, color);
        }

        [Command]
        public void CmdSendPlayerMessage()
        {
            if (sceneScript)
            {
                sceneScript.statusText = $"{playerName} says hello {Random.Range(10, 99)}";
            }
        }

        // A command is a function that runs on the server, only has access to syncvars and variables specified as server only.
        [Command]
        public void CmdSetupPlayer(string name, Color color)
        {
            playerName = name;
            playerColor = color;
            sceneScript.statusText = $"{playerName} joined.";
        }

        void Update()
        {
            // IsLocalPlayer is available for all network network identities.
            // Just stops the user from updating the position of both player objects.
            if (!isLocalPlayer) 
            {
                floatingInfo.transform.LookAt(Camera.main.transform);
                return;
            }

            float moveX = Input.GetAxis("Horizontal") * Time.deltaTime * 110.0f;
            float moveZ = Input.GetAxis("Vertical") * Time.deltaTime * 4f;

            transform.Rotate(0, moveX, 0);
            transform.Translate(0, 0, moveZ);
        }
    }
}