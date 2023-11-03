using OpenQA.Selenium;
using UINavigator.Menus.Administration;
using UINavigator.Models.Enums.Site;

namespace UINavigator.Menus
{
    /// <summary>
    /// Top menu fabric
    /// </summary>
    public class TopMenu
    {
        public TopMenu() { }

        public static ITopMenuItem? GetTopMenu(TopMenuOption menuName)
        {
            ITopMenuItem? menu = null;
            switch (menuName)
            {
                case TopMenuOption.Administration:
                    menu = new AdministrationMenu();
                    break;
                case TopMenuOption.MyTeam:
                    menu = new AdministrationMenu();
                    break;
                case TopMenuOption.MySelf:
                    menu = new AdministrationMenu();
                    break;
                default:
                    throw new NotFoundException();
            }
            return menu;
        }
    }
}
