
namespace UINavigator.Menus
{
    public interface ITopMenuItem
    {
        public bool IsTopMenu { get; }
        public string? Id { get; }
        public string? Name { get; }
        public List<IMenuItem> Items { get ; set; }
    }
}
