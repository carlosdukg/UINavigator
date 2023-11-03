
namespace UINavigator.Menus
{
    public interface IMenuItem
    {
        /// <summary>
        /// Menu html id.
        /// </summary>
        public string? Id { get; }

        /// <summary>
        /// Menu html nmae.
        /// </summary>
        public string? Name { get; }

        /// <summary>
        /// 
        /// </summary>
        List<MenuElement> Items { get; set; }
    }
}
