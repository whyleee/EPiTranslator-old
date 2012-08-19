using EPiServer;
using EPiServer.Core;

namespace EPiTranslator.EPiServer
{
    /// <summary>
    /// Testable wrapper for EPiServer CMS core framework.
    /// </summary>
    public class EPiServerWrapper
    {
        /// <summary>
        /// Gets the page by its link.
        /// </summary>
        /// <param name="pageLink">The link to the page.</param>
        /// <returns>Page got from EPiServer by provided link, or <c>null</c> if no page was found.</returns>
        public virtual PageData GetPage(PageReference pageLink)
        {
            try
            {
                return DataFactory.Instance.GetPage(pageLink);
            }
            catch (PageNotFoundException)
            {
                return null;
            }
        }
    }
}
