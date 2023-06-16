using System;
using CesarJZO.Input;
using UnityEngine;

namespace CesarJZO.Player
{
    public class PlayerInteractionHandler : MonoBehaviour
    {
        public event Action<Transform> InteractableChanged;
        [SerializeField] private Transform interactionSensor;
        [SerializeField] private LayerMask interactableLayerMask;

        private Collider2D _currentInteractable;
        public Collider2D HasInteractable => Physics2D.OverlapBox(
            point: interactionSensor.position,
            size: interactionSensor.lossyScale,
            angle: interactionSensor.rotation.z,
            layerMask: interactableLayerMask
        );

        private void Start()
        {
            PlayerInput.Instance.InteractPerformed += OnInteractPerformed;
        }

        private void OnInteractPerformed()
        {
            if (!HasInteractable) return;

            if (HasInteractable.TryGetComponent(out IInteractable interactable))
                interactable.Interact();
        }

        // ReSharper disable Unity.PerformanceAnalysis
        private void Update()
        {
            Collider2D interactable = HasInteractable;
            if (interactable == _currentInteractable) return;

            _currentInteractable = interactable;
            InteractableChanged?.Invoke(interactable ? interactable.transform : null);
        }

        private void OnDrawGizmos()
        {
            if (!interactionSensor) return;
            Gizmos.color = Color.green;
            Gizmos.matrix = Matrix4x4.TRS(
                interactionSensor.position,
                interactionSensor.rotation,
                interactionSensor.lossyScale
            );
            Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
        }
    }
}
