#nullable enable
namespace LinqToDB.DataProvider.DB2iSeries {
	public class DB2iSeriesProviderOptions {
		public static DB2iSeriesProviderOptions DefaultInstance = new DB2iSeriesProviderOptions();

		public DB2iSeriesProviderOptions(string providerName) : this(
			providerName,
			DB2iSeriesProviderName.GetProviderType(providerName),
			DB2iSeriesProviderName.GetDB2iSeriesVersion(providerName)
			) {
		}

		public DB2iSeriesProviderOptions(
			string? providerName = null,
			DB2iSeriesProviderType providerType = DB2iSeriesProviderType.Odbc,
			DB2iSeriesVersion version = DB2iSeriesVersion.V7_1,
			bool? mapGuidAsString = null
			) {
			ProviderName = providerName ?? DB2iSeriesProviderName.GetProviderName(version, providerType, false);
			ProviderType = providerType;
			DB2iSeriesVersion = version;
			MapGuidAsString = mapGuidAsString  ?? ProviderName.Contains("GAS");
		}

		public string ProviderName { get; set; }
		public DB2iSeriesProviderType ProviderType { get; } = DB2iSeriesProviderType.Odbc;
		public DB2iSeriesVersion DB2iSeriesVersion { get; set; } = DB2iSeriesVersion.V7_1;
		public DB2iSeriesNamingConvention NamingConvention { get; set; }
		public bool MapGuidAsString { get; set; }
	}
}
