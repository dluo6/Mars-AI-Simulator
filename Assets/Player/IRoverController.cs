using UnityEngine;
public interface IRoverController
{
    void SetInputs(float horizontal, float vertical);
    float GetMotorForce();
    void SetMotorForce(float newForce);
    void ApplyBraking();
    bool useAI { get; set; }
    Transform transform { get; }
}