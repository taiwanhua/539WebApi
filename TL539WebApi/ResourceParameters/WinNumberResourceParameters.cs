using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TL539WebApi.ResourceParameters
{
	public class WinNumberResourceParameters
	{
		public int GetDataTotalPage { get; set; }
		/// <summary>
		/// 取得近幾期
		/// </summary>
		public int lasterPeriod { get; set; }
		/// <summary>
		/// 從幾期開始
		/// </summary>
		public int FormPeriod { get; set; }
		/// <summary>
		/// 到幾期
		/// </summary>
		public int ToPeriod { get; set; }
		/// <summary>
		/// 容易開出的數字
		/// </summary>
		public int[] EasyDrawNumber1 { get; set; }
		/// <summary>
		/// 容易開出的數字的 後 的第幾期 (單一期)
		/// </summary>
		public int NextWhichPeriod { get; set; }
		/// <summary>
		/// 連莊幾期
		/// </summary>
		public int SameDealerNumberHowManyTimes { get; set; }
	}
}
