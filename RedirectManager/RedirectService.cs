namespace RedirectManager;

public class RedirectService
{
    public List<RedirectData> GetRedirectData()
    {
        // Mock data; replace this with actual API calls
        return new List<RedirectData>
        {
            new RedirectData
            {
                RedirectUrl = "/campaignA",
                TargetUrl = "/campaigns/targetcampaign",
                RedirectType = 302,
                UseRelative = false
            },
            new RedirectData
            {
                RedirectUrl = "/campaignB",
                TargetUrl = "/campaigns/targetcampaign/channelB",
                RedirectType = 302,
                UseRelative = false
            },
            new RedirectData
            {
                RedirectUrl = "/product-directory",
                TargetUrl = "/products",
                RedirectType = 301,
                UseRelative = true
            }
        };
    }
}

