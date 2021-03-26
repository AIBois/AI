using UnityEngine;

namespace States.Character
{
    public class CombatCharacterState : CharacterState
    {
        private SquadBase enemySquad;

        public CombatCharacterState(SquadBase enemySquad)
        {
            this.enemySquad = enemySquad;
        }

        protected override void Act()
        {
            var closestEnemy = GetClosestEnemy();
            if (InRangedRange(closestEnemy.transform.position)) RangedAttack(closestEnemy); 
            else if (InMeleeRange(closestEnemy.transform.position)) MeleeAttack(closestEnemy); 
            else context.MoveTo(closestEnemy.transform.position);
        }

        CharacterBase GetClosestEnemy()
        {
            CharacterBase result = null;
            var minDistance = Mathf.Infinity;
            var currentPos = transform.position;
            foreach (var enemy in enemySquad.Units)
            {
                var distance = Vector3.Distance(enemy.transform.position, currentPos);
                if (distance > minDistance) continue;
                result = enemy;
                minDistance = distance;
            }
            return result;
        }

        private bool InRangedRange(Vector3 enemyPosition)
        {
            if (!context.IsRanged) return false;
            var distance = Vector3.Distance(enemyPosition, transform.position);
            return distance <= context.RangedAttackLongDistance &&
                distance >= context.RangedAttackShortDistance;
        }

        private void RangedAttack(CharacterBase closestEnemy)
        {
            if(context.ReadyToRangedAttack()) closestEnemy.TakeDamage(context.RangedDamage);
        }
        
        private bool InMeleeRange(Vector3 position)
        {
            return Vector3.Distance(position, transform.position) <= context.MeleeAttackDistance;
        }

        private void MeleeAttack(CharacterBase closestEnemy)
        {
            if(context.ReadyToAttack()) closestEnemy.TakeDamage(context.MeleeDamage);
        }
    }
}