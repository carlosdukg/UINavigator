namespace UINavigator.Menus.Administration
{
    internal class EmployeeAdminMenu : IMenuItem
    {
        private List<MenuElement> _items;
        private readonly string _name;
        private readonly string _id;

        public EmployeeAdminMenu()
        {
            _name = "EmployeAdmin";
            _id = "346";
            _items = new List<MenuElement>
            {
                new MenuElement
                {
                    Id = "424",
                    Name = "My Employes"
                }
            };
        }

        public bool IsTopMenu => false;

        public string? Id => _id;

        public string? Name => _name;

        public List<MenuElement> Items
        {
            get => _items; set { _items = value; }
        }
    }
}
