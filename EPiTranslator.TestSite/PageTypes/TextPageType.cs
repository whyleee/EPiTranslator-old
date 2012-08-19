using PageTypeBuilder;

namespace EPiTranslator.TestSite.PageTypes
{
    [PageType(Name = "Text page",
        Description = "Ordinary text page",
        Filename = "~/Templates/Pages/TextPage.aspx")]
    public class TextPageType : TypedPageData
    {
    }
}