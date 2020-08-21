using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformingCharacter : Mobile, IInputReader
{
    public InputToken InputToken { get; set; }

    public PlatformingCharacterProperties Properties;

    public LayerMask Player;

    int cyoteTime = 5;
    int wallSlideTime = 5;
    public event System.Action OnJump;
    public event System.Action OnWallJump;
    public event System.Action<PlatformingCharacter> OnStomp;
    public bool WallSliding => VMomentum < 0f && wallSlideTime > 0;
    public bool Climbing { get; set; }
    int ForceJumpFrames = 0;
    Vector2 ForceMove;
    int ForceMoveFrames = 0;
    int rootDuration = 0;

    bool CanWallJump => Properties.WalljumpForce > 0f && !Grounded && wallSlideTime > 0 && allowWallJump;

    public bool allowWallJump = true;
    public bool allowClimb = true;

    public bool Disabled { get; set; }

    // Update is called once per frame
    new protected void FixedUpdate()
    {
        if (InputToken == null)
        {
            base.FixedUpdate();
            return;
        }
        if(Disabled)
        {
            base.FixedUpdate();
            HMomentum *= .8f;
            return;
        }

        InputSnapshot input = InputToken.GetSnapshot();
        var data = SimulateFrame(input, cyoteTime);
        cyoteTime = data.cyoteTime;
        if (data.jumpConsumed)
            InputToken.ConsumeJump();
    }

    public void Resimulate(float time, Vector2 from, bool stomp = false)
    {
        ResimulateFrames(Mathf.RoundToInt(time / Time.fixedDeltaTime), from, stomp);
    }

    public void ResimulateFrames(int frames, Vector2 from, bool stomp = false)
    {
        if (frames > 12)
            frames = 12;
        var snapshot = GetSnapshotFor(frames);
        transform.position = from;
        HMomentum = snapshot.momentum.x;
        VMomentum = snapshot.momentum.y;
        if (stomp)
            VMomentum = Mathf.Min(VMomentum - 2f, -2f);

        for (int i = frames; i > 0; i--)
        {
            snapshot = GetSnapshotFor(frames);
            var info = SimulateFrame(snapshot.input, snapshot.cyoteTime);
            cyoteTime = info.cyoteTime;
            // missing proper resimulation and useage of the jump buffer
        }
    }

    public (bool jumpConsumed, int cyoteTime) SimulateFrame(InputSnapshot input, int cyoteTime)
    {
        bool jumpConsumed = false;
        ForceJumpFrames--;
        ForceMoveFrames--;
        rootDuration--;

        // useful variables
        var peakJump = (VMomentum < 1f && VMomentum > -1f) && !Grounded;
        var x = input.direction.x;
        var currentSpeed = Mathf.Abs(HMomentum);
        bool jumpHeld = ForceJumpFrames > 0 || input.jumpHeld;
        if (ForceMoveFrames > 0)
            x = ForceMove.x;

        // facing
        if (x < .25f && x > -.25f)
            x = 0f;
        if (x > .75f)
            x = 1f;
        if (x < -.75f)
            x = -1f;
        if (x != 0f && (Grounded || Properties.airTurn) && rootDuration < 0)
            FaceRight = x > 0f;

        // horiontal movement
        var desiredSpeed = x * Properties.MaxSpeed;
        if (Grounded && rootDuration > 0)
            desiredSpeed = 0f;
        bool breaking = (Mathf.Abs(desiredSpeed) < Mathf.Abs(HMomentum) || Mathf.Sign(desiredSpeed) != Mathf.Sign(HMomentum));
        var accel = Properties.AccelerationCurve.Evaluate(breaking ? -currentSpeed : currentSpeed);
        if (peakJump)
            accel *= Properties.PeakAirControl;
        else if (!Grounded)
            accel *= Properties.AirControl;
        HMomentum = Mathf.Clamp(desiredSpeed, HMomentum - accel, HMomentum + accel);
        if (Grounded && breaking)
            HMomentum *= Properties.Traction;

        // jump resolver
        if (Grounded)
            cyoteTime = Properties.CyoteTime + 1;
        else
            cyoteTime--;
        if(allowClimb && InputToken.ClimbHeld && Properties.ClimbSpeed > 0f && !Grounded && TouchingWall)
        {
            if(input.direction.y < 0f && Properties.ClimbSpeed < Properties.MaxWallslideSpeed)
                VMomentum = VMomentum * .5f + Properties.MaxWallslideSpeed * .5f * input.direction.y;
            else
                VMomentum = VMomentum * .5f + Properties.ClimbSpeed * .5f * input.direction.y;
            HMomentum = TouchingWallDirection * 3f;
            FaceRight = TouchingWallDirection < 0;
            wallSlideTime = Properties.CyoteTime + 1;
            Climbing = true;
            ForceMove = new Vector2(TouchingWallDirection, 0f);
        }
        else if (Properties.WalljumpForce > 0f && !Grounded && TouchingWall && TouchingWallDirection == System.Math.Sign(input.direction.x))
        {
            if(WallSliding)
                FaceRight = TouchingWallDirection < 0;
            wallSlideTime = Properties.CyoteTime + 1;
            Climbing = false;
        }
        else
        {
            if(Climbing == true)
            {
                if (input.direction.y > .5f && InputToken.ClimbHeld)
                {
                    VMomentum = 10f;
                    ForceJumpFrames = 15;
                    ForceMoveFrames = 10;
                    FaceRight = ForceMove.x > 0f;
                }
            }
            wallSlideTime--;
            Climbing = false;
        }

        if (cyoteTime > 0 && input.jumpBufferTimer >= 0f)
        {
            VMomentum = Properties.JumpForce;
            OnJump?.Invoke();
            cyoteTime = 0;
            jumpConsumed = true;
        }
        else if(input.jumpBufferTimer >= 0f && CanWallJump)
        {
            if(TouchingWall)
                FaceRight = TouchingWallDirection < 0;
            VMomentum = Properties.WalljumpForce;
            HMomentum = Properties.WallpushForce * Forward;

            OnWallJump?.Invoke();
            wallSlideTime = 0;
            jumpConsumed = true;
            ForceMove = new Vector2(-TouchingWallDirection, 0f);
            ForceMoveFrames = 5;
        }
        else if (!Grounded && VMomentum < 0f && Properties.HeadBonkForce > 0f)
        {
            // jump on player here
            var hits = Physics2D.BoxCastAll(
                origin: new Vector2(transform.position.x, transform.position.y - radius + .08f),
                size: new Vector2(radius * .9f, .02f),
                angle: 0f,
                direction: Vector2.down,
                distance: -VMomentum * Time.fixedDeltaTime,
                layerMask: Player
                );
            if (hits.Length > 1)
            {
                foreach (var hit in hits)
                {
                    if (hit.transform != transform && hit.transform.position.y < transform.position.y)
                    {

                        VMomentum = Properties.HeadBonkForce;
                        var other = hit.transform.GetComponent<PlatformingCharacter>();
                        other.VMomentum = Mathf.Min(VMomentum - Properties.HeadBonkForce, -Properties.HeadBonkForce);
                        OnStomp?.Invoke(other);
                        break;
                    }
                }
            }
        }

        if (Grounded == false && jumpHeld == false && VMomentum > 0f)
        {
            VMomentum *= Properties.JumpCap;
        }

        // Gravity Manipulation
        if (Climbing)
        {
            Gravity = 0f;
        }
        else if (peakJump && jumpHeld)
        {
            Gravity = Properties.Gravity * Properties.PeakJumpGravity;
        }
        else if (!Grounded && Properties.slowfallTrottle > 0f && jumpHeld && VMomentum < -Properties.slowfallSpeed)
        {
            VMomentum += (1f - Properties.slowfallTrottle) * (-VMomentum - Properties.slowfallSpeed);
        }
        else if (TouchingWall && WallSliding)
        {
            VMomentum += .2f * (-VMomentum - Properties.MaxWallslideSpeed);
            Gravity = 0f;
        }
        else if (VMomentum < -Properties.MaxFallSpeed)
            Gravity = 0f;
        else
            Gravity = Properties.Gravity;



        base.FixedUpdate();

        return (jumpConsumed, cyoteTime);
    }

    public void Root(int duration)
    {
        rootDuration = Mathf.Max(rootDuration, duration);
    }

    PlatformingCharacterSnapshot[] history = new PlatformingCharacterSnapshot[60];
    int snapshothead;
    void SaveSnapshot()
    {
        snapshothead++;
        snapshothead %= 60;
        history[snapshothead] = new PlatformingCharacterSnapshot()
        {
            input = InputToken.GetSnapshot(),
            momentum = new Vector2(HMomentum, VMomentum),
            cyoteTime = cyoteTime,
            position = transform.position,
        };
    }
    PlatformingCharacterSnapshot GetSnapshotFor(int framesAgo) => history[(60 + snapshothead - framesAgo) % 60];

    [System.Serializable]
    public struct PlatformingCharacterSnapshot
    {
        public InputSnapshot input;
        public Vector2 momentum;
        public int cyoteTime;
        public Vector2 position;
    }
}
