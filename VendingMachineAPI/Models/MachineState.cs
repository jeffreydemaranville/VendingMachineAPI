namespace VendingMachineAPI.Models
{
    public class MachineState
    {
        public int Coins { get; set; }
        public List<Item> Items { get; set; }        

        //FQ Ctor needed to allow instantiation within the Singleton
        public MachineState(int coins, List<Item> items)
        {
            Coins = coins;
            Items = items;            
        }
    }
}
