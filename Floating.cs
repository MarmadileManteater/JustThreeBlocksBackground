using Godot;
using System;

/// <summary>
/// A rigidbody floating at Y = 0
/// which can not move in the X or Z directions
/// </summary>
public class Floating : RigidBody
{
	private readonly Random RandomGenerator = new Random();
	private Vector3 InitialTranslation;
	private readonly float MaxSpeed = 1.0f;
	private readonly float RotationMultiplier = 0.5f;
	private readonly float BobMultiplier = 0.15f;

	public override void _Ready()
	{
		// Save the initial translation to prevent movement
		InitialTranslation = new Vector3(Translation);
	}

	public override void _PhysicsProcess(float delta)
	{
		if (GlobalTranslation.y < 0)
		{
			float torque = (((RandomGenerator.Next(100) / 100.0f) - 0.5f) / 100.0f) * RotationMultiplier;
			float force = RandomGenerator.Next(100) / 75.0f;
			// Randomly bob and rotate this rigidbody
			int torqueDimension = RandomGenerator.Next(9) / 3;
			switch (torqueDimension) {
				case 1:
					ApplyTorqueImpulse(new Vector3(torque * RotationMultiplier, 0, 0));
					break;
				default:
				case 2:
					ApplyTorqueImpulse(new Vector3(0, torque * RotationMultiplier, 0));
					break;
				case 3:
					ApplyTorqueImpulse(new Vector3(0, 0, torque * RotationMultiplier));
					break;
			}
			ApplyCentralImpulse(new Vector3(0, force * BobMultiplier, 0));
		}
	}

	public override void _IntegrateForces(PhysicsDirectBodyState state)
	{
		base._IntegrateForces(state);
		// Keep this rigid body at the same x, and z coords
		Translation = new Vector3(InitialTranslation.x, Translation.y, InitialTranslation.z);
		if (Translation.y < -0.5)
		{
			LinearVelocity = new Vector3(0, MaxSpeed / 2, 0);
		}
		if (LinearVelocity.y > MaxSpeed)
		{
			LinearVelocity = new Vector3(0, MaxSpeed, 0);
		}
	}
}
