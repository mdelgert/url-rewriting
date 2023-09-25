namespace RedirectManager;

public class RedirectService
{
    public static IEnumerable<RedirectData> GetRedirectData()
    {
        // Mock data; replace this with actual API calls
        return new List<RedirectData>
        {
            new()
            {
                RedirectUrl = "/campaignA",
                TargetUrl = "/campaigns/targetcampaign",
                RedirectType = 302,
                UseRelative = false
            },
            new()
            {
                RedirectUrl = "/campaignB",
                TargetUrl = "/campaigns/targetcampaign/channelB",
                RedirectType = 302,
                UseRelative = false
            },
            new()
            {
                RedirectUrl = "/product-directory",
                TargetUrl = "/products",
                RedirectType = 301,
                UseRelative = true
            }
        };
    }
}