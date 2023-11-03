namespace UINavigator.Menus.Administration
{
    internal class AdministrationMenu : ITopMenuItem
    {
        private List<IMenuItem> _menuElements;
        private readonly string _name;
        private readonly string _id;
        private readonly bool _isTopMenu;

        public AdministrationMenu()
        {
            _isTopMenu = true;
            _id = "menu_admin";
            _name = "Administration";

            _menuElements = new List<IMenuItem>
            {
                new EmployeeAdminMenu()
            };
        }

        public bool IsTopMenu { get => _isTopMenu; }
        public string? Id { get => _id; }
        public string? Name { get => _name; }
        public List<IMenuItem> Items { get => _menuElements; set => _menuElements = value; }
    }
}
