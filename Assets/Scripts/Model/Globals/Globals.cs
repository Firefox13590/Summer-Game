using UnityEngine;

namespace Model.Globals
{
    /// <summary>
    /// Classe triviale pour permttre d'intégrer aisémment le pattern de singleton
    /// </summary>
    /// <remarks>
    /// Hérite de <see cref="MonoBehaviour"/>
    /// </remarks>
    /// <typeparam name="T">Type de la classe enfant</typeparam>
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance;
        public static T Instance { get { return instance; } }

        protected virtual void Awake()
        {
            //Debug.Log("Interface singleton");
            if (Instance == null)
            {
                instance = this as T;
            }
            else Destroy(gameObject);
        }
    }
}