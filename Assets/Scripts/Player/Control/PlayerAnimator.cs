using UnityEngine;

namespace XVerse.Player.Control
{
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimator : MonoBehaviour
    {
        [Header("PlayerRoot")]
        [SerializeField]
        private PlayerController playerController;

        private Animator animator;
    }
}