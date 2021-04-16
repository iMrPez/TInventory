namespace TInventory.ContextMenu.Action
{
    public interface IOption
    {   
        public string GetName();
        
        public bool CanAct();
        
        public void Act();
    }
}
