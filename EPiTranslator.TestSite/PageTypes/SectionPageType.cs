using PageTypeBuilder;

namespace EPiTranslator.TestSite.PageTypes
{
    [PageType(Name = "Section",
        Description = "A section for grouping pages",
        Filename = "~/Templates/Pages/System/NoTemplate.aspx")]
    public class SectionPageType : TypedPageData
    {
    }
}