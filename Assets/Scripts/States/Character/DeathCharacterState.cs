namespace States.Character
{
    public class DeathCharacterState : CharacterState
    {
        public DeathCharacterState(CharacterBase context) : base(context){ }
        
        public override void Act()
        {
            //Destroy character
        }
    }
}