using ProjectWilson.Lookups;

namespace ProjectWilson.Actions
{
    public class DoubleJump : MovementActionHandler<EmptyContext>
    {
        public DoubleJump(RPGCharacterMovementController movement) : base(movement)
        {
        }

        public override bool CanStartAction(RPGCharacterController controller)
        { return controller.isFalling; }
        //{ return controller.isFalling && movement.canDoubleJump; }

        protected override void _StartAction(RPGCharacterController controller, EmptyContext context)
        { movement.currentState = CharacterState.DoubleJump; }

        public override bool IsActive()
        { return movement.currentState != null && (CharacterState)movement.currentState == CharacterState.DoubleJump; }
    }
}