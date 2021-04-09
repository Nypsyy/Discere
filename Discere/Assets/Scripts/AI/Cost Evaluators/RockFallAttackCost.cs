public class RockFallAttackCost : ShockwaveAttackCost
{
    public override void Touched() {
        TimesAttacked += .4f;
    }
}