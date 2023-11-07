
namespace UINavigator.Models.UIModels
{
    public class UINavigation
    {
        public string? Context  { get; set; }

        public string? MainMenu { get; set; }

        public string? SubMenuPath { get; set; }

        public string? Handler { get; set; }

        public string? Url { get; set; }

        public string? Path { get; set; }

        public bool IsPopUp { get; set; }

        public string? RightMenuPath { get; set; }
    }
}
