namespace States.Character
{
    public class IdleCharacterState : CharacterState
    {
        public IdleCharacterState(CharacterBase context) : base(context){}

        public override void Act()
        {
            //do nothing
        }
    }
}