namespace LinqToDB.DataProvider.DB2iSeries {
	public enum DB2iSeriesNamingConvention {
		Sql = 0,
		System = 1
	}

	public static class DB2iSeriesNamingConventionExtensions {

		public static string Delimiter(this DB2iSeriesNamingConvention naming) => naming == DB2iSeriesNamingConvention.Sql ? "." : "/";

		public static string DummyTableName(this DB2iSeriesNamingConvention naming) => $"SYSIBM{naming.Delimiter()}SYSDUMMY1";

	}
}
