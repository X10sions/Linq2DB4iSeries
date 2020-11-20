using System;

namespace LinqToDB.DataProvider.DB2iSeries {
	public enum DB2iSeriesVersion {
		V5_4 = -2,
		//V6_1 = -1,
		V7_1 = 0,
		V7_2 = 1,
		V7_3 = 2,
		//V7_4 = 3
	}

	public static class DB2iSeriesVersionExtensions {

		public static bool SupportsArrayType(this DB2iSeriesVersion version) => version >= DB2iSeriesVersion.V7_1;

		public static bool SupportsDecFloatTypes(this DB2iSeriesVersion version) => version > DB2iSeriesVersion.V5_4;

		public static bool SupportsOffsetClause(this DB2iSeriesVersion version) => version > DB2iSeriesVersion.V7_2;

		public static bool SupportsTruncateTable(this DB2iSeriesVersion version) => version > DB2iSeriesVersion.V7_1;

		public static bool SupportsMergeStatement(this DB2iSeriesVersion version) => version > DB2iSeriesVersion.V5_4;

		public static bool SupportsNCharTypes(this DB2iSeriesVersion version) => version > DB2iSeriesVersion.V5_4;

		public static bool SupportsXmlType(this DB2iSeriesVersion version) => version >= DB2iSeriesVersion.V7_1;

		public static int MaxTimestampPrecision(this DB2iSeriesVersion version) => version < DB2iSeriesVersion.V7_2 ? 6:12;

		public static DB2iSeriesVersion GetDB2iSeriesVersion(this Version version) => version switch {
			//var x when x >= new Version(7, 4) => DB2iSeriesVersion.V7_4,
			var x when x >= new Version(7, 3) => DB2iSeriesVersion.V7_3,
			var x when x >= new Version(7, 2) => DB2iSeriesVersion.V7_2,
			var x when x >= new Version(7, 1) => DB2iSeriesVersion.V7_1,
			//var x when x >= new Version(6, 1) => DB2iSeriesVersion.V6_1,
			var x when x >= new Version(5, 4) => DB2iSeriesVersion.V5_4,
			_ => DB2iSeriesVersion.V7_1
		};

	}
}
