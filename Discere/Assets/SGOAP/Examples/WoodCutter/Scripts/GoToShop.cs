namespace SGoap.Example
{
    public class GoToShop : NavMeshAction
    {
        public StringReference ItemToSell;
        public StringReference Money;

        public int PricePerItem = 5;

        public override bool PostPerform()
        {
            Sell();
            return base.PostPerform();
        }

        public void Sell()
        {
            var woodCount = States.GetValue(ItemToSell.Value);
            States.ModifyState(Money.Value, woodCount * PricePerItem);
            States.RemoveState(ItemToSell.Value);
        }
    }
}
