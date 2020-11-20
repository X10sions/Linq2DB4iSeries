using System.Data.Common;

namespace LinqToDB.DataProvider.DB2iSeries {
	public enum DB2iSeriesProviderType {
		Odbc,
		OleDb,
		DB2,
#if NETFRAMEWORK
		AccessClient
#endif
	}

	public static class DB2iSeriesProviderTypeExtensions {

		public static DB2iSeriesProviderType GetProviderType(this DbConnectionStringBuilder csb)
			=> csb.ContainsKey("DRIVER") ? DB2iSeriesProviderType.Odbc
			: csb.ContainsKey("PROVIDER") ? DB2iSeriesProviderType.OleDb
			: csb.ContainsKey("SERVER") ? DB2iSeriesProviderType.DB2
#if NETFRAMEWORK
			: csb.ContainsKey("DATA SOURCE") ? DB2iSeriesProviderType.AccessClient
#endif
			: throw ExceptionHelper.InvalidConnectionString();

		public static bool IsAccessClient(this DB2iSeriesProviderType providerType)
			=>
#if NETFRAMEWORK
			providerType == DB2iSeriesProviderType.AccessClient;
#else
			false;
#endif

		public static bool IsDB2(this DB2iSeriesProviderType providerType)
			=> providerType == DB2iSeriesProviderType.DB2;

		public static bool IsIBM(this DB2iSeriesProviderType providerType)
			=> providerType == DB2iSeriesProviderType.DB2
#if NETFRAMEWORK
			|| providerType == DB2iSeriesProviderType.AccessClient
#endif
			;

		public static bool IsOdbc(this DB2iSeriesProviderType providerType)
			 => providerType == DB2iSeriesProviderType.Odbc;

		public static bool IsOleDb(this DB2iSeriesProviderType providerType)
			 => providerType == DB2iSeriesProviderType.OleDb;

		public static bool IsOdbcOrOleDb(this DB2iSeriesProviderType providerType)
			 => providerType == DB2iSeriesProviderType.Odbc
			|| providerType == DB2iSeriesProviderType.OleDb;

	}
}
