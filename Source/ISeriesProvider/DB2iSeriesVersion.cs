namespace LinqToDB.DataProvider.DB2iSeries {
	public enum DB2iSeriesVersion {
		V5_4 = -2,
		//V6_1 = -1,
		V7_1 = 0,
		V7_2 = 1,
		V7_3 = 2,
		V7_4 = 3
	}

	public static class DB2iSeriesVersionExtensions {

		public static bool SupportsDecFloatTypes(this DB2iSeriesVersion version) => version > DB2iSeriesVersion.V5_4;

		public static bool SupportsOffsetClause(this DB2iSeriesVersion version) => version > DB2iSeriesVersion.V7_2;

		public static bool SupportsTruncateTable(this DB2iSeriesVersion version) => version > DB2iSeriesVersion.V7_1;

		public static bool SupportsMergeStatement(this DB2iSeriesVersion version) => version > DB2iSeriesVersion.V5_4;

		public static bool SupportsNCharTypes(this DB2iSeriesVersion version) => version > DB2iSeriesVersion.V5_4;

	}
}
