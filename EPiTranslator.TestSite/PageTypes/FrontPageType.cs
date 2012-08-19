using PageTypeBuilder;

namespace EPiTranslator.TestSite.PageTypes
{
    [PageType(Name = "Front page",
        Description = "Front page of the site",
        Filename = "~/Templates/Pages/FrontPage.aspx",
        AvailablePageTypes = new[] {typeof(TextPageType), typeof(SectionPageType)})]
    public class FrontPageType : TypedPageData
    {
    }
}