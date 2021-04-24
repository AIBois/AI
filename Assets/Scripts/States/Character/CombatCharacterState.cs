using UnityEngine;

namespace States.Character
{
    public class CombatCharacterState : CharacterState
    {
        private SquadBase enemySquad;

        public CombatCharacterState(CharacterBase context, SquadBase enemySquad) : base(context)
        {
            this.enemySquad = enemySquad;
        }

        public override void Act()
        {
            //TODO check handle if closest enemy is null (Squad dead) and change to the appropriate state.

            var closestEnemy = GetClosestEnemy();
            if (context.InRangedRange(closestEnemy.transform.position)) context.RangedAttack(closestEnemy, enemySquad); 
            else if (context.InMeleeRange(closestEnemy.transform.position)) context.MeleeAttack(closestEnemy, enemySquad); 
            else context.MoveTo(closestEnemy.transform.position);
        }

        CharacterBase GetClosestEnemy()
        {
            CharacterBase result = null;
            var minDistance = Mathf.Infinity;
            var currentPos = context.transform.position;
            foreach (var enemy in enemySquad.Units)
            {
                var distance = Vector3.Distance(enemy.transform.position, currentPos);
                if (distance > minDistance) continue;
                result = enemy;
                minDistance = distance;
            }
            return result;
        }
    }
}