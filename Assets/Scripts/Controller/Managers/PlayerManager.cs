using Model.Modules;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

namespace Controller.Managers
{
    /// <summary>
    /// Classe qui controle le joueur, nottament son déplacement.
    /// </summary>
    /// <remarks>
    /// Le code présent sert de placeholder simple pour tester le déplacement du joueur.
    /// </remarks>
    public class PlayerManager : MonoBehaviour
    {
        [Header("Affectation inspecteur"), Space(30)]
        [Header("Hiérarchie")]
        public GameObject indicateurClick, indicateurSouris;
        public TileManager tileManager;
        [Header("Ajustement inspecteur")]
        [Tooltip("Si false, utilise mvt avec souris (raycast)")]
        public bool useTileMvt;

        Camera camera;
        NavMeshAgent agent;
        Vector3 worldPosition = Vector3.zero;
        Ray ray = new Ray();
        RaycastHit hit;

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
        }
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            camera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        }
        // Update is called once per frame
        void Update()
        {

            if (Mouse.current != null)
            {
                ray = camera.ScreenPointToRay(Mouse.current.position.ReadValue());

                if (useTileMvt)
                {
                    if (hit.collider != null)
                    {
                        //Debug.Log(hit.transform);
                        //Debug.Log(hit.transform.gameObject);
                        hit.transform.TryGetComponent<Tile>(out var tileElement);

                        if (Mouse.current.leftButton.wasPressedThisFrame)
                        {
                            //tileManager.UpdateTilesMaterial(hit.transform.gameObject, isTargeted: true);
                            tileManager.SetTargetedTile(tileElement);
                        }
                        else
                        {
                            //tileManager.UpdateTilesMaterial(hit.transform.gameObject, true);
                            tileManager.SetHoveredTile(tileElement);
                        }
                    }
                    else tileManager.SetHoveredTile(null);
                }
                else
                {
                    indicateurSouris.transform.position = worldPosition;

                    if (Mouse.current.leftButton.wasPressedThisFrame)
                    {
                        MovePlayer();
                        indicateurClick.transform.position = worldPosition;
                    }
                }
            }
        }
        private void FixedUpdate()
        {
            if (Physics.Raycast(ray, out hit)) worldPosition = hit.point;
        }



        /// <summary>
        /// Déplace le joueur vers la position de la souris, convertie en point 3D dans le monde.
        /// </summary>
        void MovePlayer()
        {
            agent.destination = worldPosition;
        }
    }
}