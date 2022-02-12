namespace FrontendApp.Models
{
    public class CategoryModel
    {
        public string Key { get; set; }
        public string Icon { get; set; }
        public List<CategoryModel> SubCategories { get; set; }
    }
}
