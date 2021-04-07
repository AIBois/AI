namespace States.Character
{
    public abstract class CharacterState : State
    {
        public CharacterBase context;

        protected CharacterState(CharacterBase context)
        {
            this.context = context;
        }
    }
}