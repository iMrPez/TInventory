namespace TInventory
{
    public interface IManageable
    {
        public object GetModel();

        public bool LoadModel(string modelJson);
    }
}