using UnityEngine;
using UnityEngine.XR;
using GorillaLocomotion;
using System.Linq;
using Steamworks;

namespace DevHoldableEngine
{
    public class DevHoldable : HoldableObject
    {
        public bool InHand;
        public bool InLeftHand;
        public bool PickUp;
        public Rigidbody Rigidbody;

        public float Distance = 0.2f;
        public float ThrowForce = 1.75f;

        public virtual void OnGrab(bool isLeft)
        {
            if (Rigidbody != null)
            {
                Rigidbody.isKinematic = true;
                Rigidbody.useGravity = false;
            }
        }

        public virtual void OnDrop(bool isLeft)
        {
            if (Rigidbody != null)
            {
                GorillaVelocityEstimator gorillaVelocityEstimator = (isLeft ? Player.Instance.leftHandTransform.GetComponentInChildren<GorillaVelocityEstimator>() : Player.Instance.rightHandTransform.GetComponentInChildren<GorillaVelocityEstimator>()) ?? null;

                if (gorillaVelocityEstimator != null)
                {
                    Rigidbody.isKinematic = false;
                    Rigidbody.useGravity = true;
                    Rigidbody.velocity = (gorillaVelocityEstimator.linearVelocity * ThrowForce) + Player.Instance.GetComponent<Rigidbody>().velocity;
                    Rigidbody.angularVelocity = gorillaVelocityEstimator.angularVelocity;
                }
            }
        }

        public void Update()
        {
            InputDevices.GetDeviceAtXRNode(XRNode.LeftHand).TryGetFeatureValue(CommonUsages.grip, out float left);
            InputDevices.GetDeviceAtXRNode(XRNode.RightHand).TryGetFeatureValue(CommonUsages.grip, out float right);

            bool leftGrip = left >= 0.65f;
            bool rightGrip = right >= 0.65f;

            if (PickUp && leftGrip && Vector3.Distance(Player.Instance.leftHandTransform.position, transform.position) < Distance && !InHand && EquipmentInteractor.instance.leftHandHeldEquipment == null)
            {
                // Hold logic
                InLeftHand = true;
                InHand = true;
                transform.SetParent(GorillaTagger.Instance.offlineVRRig.leftHandTransform.parent);

                // Other logic
                GorillaTagger.Instance.StartVibration(true, 0.1f, 0.05f);
                EquipmentInteractor.instance.leftHandHeldEquipment = this;

                // Callback
                OnGrab(true);
            }
            else if (!leftGrip && InHand && InLeftHand)
            {
                // Drop logic
                InLeftHand = true;
                InHand = false;
                transform.SetParent(null);

                // Other logic
                GorillaTagger.Instance.StartVibration(true, 0.1f, 0.05f);
                EquipmentInteractor.instance.leftHandHeldEquipment = null;

                // Callback
                OnDrop(true);
            }

            if (PickUp && rightGrip && Vector3.Distance(Player.Instance.rightHandTransform.position, transform.position) < Distance && !InHand && EquipmentInteractor.instance.rightHandHeldEquipment == null)
            {
                // Hold logic
                InLeftHand = false;
                InHand = true;
                transform.SetParent(GorillaTagger.Instance.offlineVRRig.rightHandTransform.parent);

                // Other logic
                GorillaTagger.Instance.StartVibration(false, 0.1f, 0.05f);
                EquipmentInteractor.instance.rightHandHeldEquipment = this;

                // Callback
                OnGrab(false);
            }
            else if (!rightGrip && InHand && !InLeftHand)
            {
                // Drop logic
                InLeftHand = false;
                InHand = false;
                transform.SetParent(null);

                // Other logic
                GorillaTagger.Instance.StartVibration(false, 0.1f, 0.05f);
                EquipmentInteractor.instance.rightHandHeldEquipment = null;

                // Callback
                OnDrop(false);
            }
        }
    }
}
