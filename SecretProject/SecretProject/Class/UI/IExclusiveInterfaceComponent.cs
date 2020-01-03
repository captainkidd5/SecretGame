namespace SecretProject.Class.UI
{
    public interface IExclusiveInterfaceComponent
    {
        bool IsActive { get; set; }
        bool FreezesGame { get; set; }
    }
}
