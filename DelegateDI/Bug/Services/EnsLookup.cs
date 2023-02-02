using Nethereum.Util;
using Nethereum.Web3;


namespace DelegateDI.Bug.Services;

public class EnsLookup
{
	public static async Task<string?> ResolveAddress(string? accountAddress)
	{
		if (accountAddress is null)
			return null;

		if (!AddressUtil.Current.IsValidEthereumAddressHexFormat(accountAddress))
			return null;

		var    web3       = new Web3("https://mainnet.infura.io/v3/c6b0cf8ac8594918b51d04f3946d7b6f");
		var    ensService = new Nethereum.ENS.ENSService(web3);
		string ensDomain  = await ensService.ReverseResolveAsync(accountAddress);
		return ensDomain;
	}
}