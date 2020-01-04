using GitWebApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using TL539WebApi.Entities;
using TL539WebApi.ResourceParameters;


namespace TL539WebApi.Controllers
{
	[ApiController]
	[Route("api/WinNumbers")]
	public class GetWinNumberController : ControllerBase
	{
		private readonly ILogger _logger;
		private readonly IWinNumberRepository _winNumberRepository;
		private readonly IHttpClientFactory _httpClientFactory;

		public GetWinNumberController(ILogger<GetWinNumberController> logger, IWinNumberRepository winNumberRepository, IHttpClientFactory httpClientFactory)
		{
			_httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(_httpClientFactory));
			_winNumberRepository = winNumberRepository ?? throw new ArgumentNullException(nameof(_winNumberRepository));
			_logger = logger;
		}
		/// <summary>
		/// 抓取開獎資料
		/// </summary>
		/// <param name="winNumberResourceParameters"></param>
		/// <returns></returns>
		[HttpGet()]
		public async Task<ActionResult<IEnumerable<WinNumber>>> GetWinNumbersAsync([FromQuery] WinNumberResourceParameters winNumberResourceParameters)
		{
			try
			{
				_winNumberRepository.DeleteAll();
				_winNumberRepository.ReSeedTo0();
				var client = _httpClientFactory.CreateClient();
				var res1 = await client.GetStringAsync($"https://www.pilio.idv.tw/lto539/list.asp?indexpage=1&orderby=new");
				var TotalPage = int.Parse(res1
					.Split(new string[] { "<a target=\"_self\" href=\"list.asp?indexpage=", "&orderby=new\" style=\"font-size: 48px; font-weight: bold\">最末頁</a>" }, StringSplitOptions.RemoveEmptyEntries)[3]);


				for (int v = 1; v < TotalPage + 1; v++)
				{
					var res = await client.GetStringAsync($"https://www.pilio.idv.tw/lto539/list.asp?indexpage={v}&orderby=old");
					var DrawTable = res.Split(new string[] { "<table class=\"auto-style1\">", "</table>" }, StringSplitOptions.RemoveEmptyEntries)[3];
					var DrawDateAndNemberArray = DrawTable.Replace("<tr style=\"text-align:center; background-color: #FFDBCE;\">\r\n", "")
						.Replace("\r\n", "")
						.Replace("<br />", "/")
						.Replace("</tr>                    <tr style=\"text-align:center; \">", "")
						.Replace("</tr>", "")
						.Replace("                        ", "")
						.Split(new string[] { "<td style=\"font-size: 32px; font-weight: bold; color: #000000;border-bottom-style: dotted; border-bottom-color: #CCCCCC\">", "</td>", "<td style=\"font-size: 48px; font-weight: bold; color: #000000;border-bottom-style: dotted; border-bottom-color: #CCCCCC; word-break: break-all\">", "</td>" }, StringSplitOptions.RemoveEmptyEntries);
					//var container = new List<WinNumber>();
					var numberContainer = new string[] { };
					for (int i = 1; i < DrawDateAndNemberArray.Length / 3; i++)
					{
						numberContainer = DrawDateAndNemberArray[i * 3 + 1].Trim().Split(",&nbsp;", StringSplitOptions.RemoveEmptyEntries);
						//container.Add(
						//	new WinNumber
						//	{
						//		Date = DrawDateAndNemberArray[i * 3],
						//		ASC1 = numberContainer[0],
						//		ASC2 = numberContainer[1],
						//		ASC3 = numberContainer[2],
						//		ASC4 = numberContainer[3],
						//		ASC5 = numberContainer[4]
						//	}
						//	);
						_winNumberRepository.Add(new WinNumber
						{
							Date = int.Parse(DrawDateAndNemberArray[i * 3].Replace("/", "").Split(new string[] { "(" }, StringSplitOptions.RemoveEmptyEntries)[0]),
							DayOfWeek = DrawDateAndNemberArray[i * 3].Split(new string[] { "/" }, StringSplitOptions.RemoveEmptyEntries)[3],
							ASC1 = numberContainer[0],
							ASC2 = numberContainer[1],
							ASC3 = numberContainer[2],
							ASC4 = numberContainer[3],
							ASC5 = numberContainer[4]
						});
					}

				}
				_winNumberRepository.Save();


				return Ok(new { ok = "OKOKOK" });
			}
			catch (Exception)
			{

				throw;
			}


		}

		/// <summary>
		/// 返回要抓取的總頁數
		/// </summary>
		/// <param name="winNumberResourceParameters"></param>
		/// <returns></returns>
		[HttpGet("TotalPage")]
		public async Task<ActionResult<IEnumerable<WinNumber>>> GetTatolPageAsync([FromQuery] WinNumberResourceParameters winNumberResourceParameters)
		{
			try
			{
				var client = _httpClientFactory.CreateClient();

				var res = await client.GetStringAsync($"https://www.pilio.idv.tw/lto539/list.asp?indexpage=1&orderby=new");
				var TotalPage = int.Parse(res
					.Split(new string[] { "<a target=\"_self\" href=\"list.asp?indexpage=", "&orderby=new\" style=\"font-size: 48px; font-weight: bold\">最末頁</a>" }, StringSplitOptions.RemoveEmptyEntries)[3]);

				return Ok(new { ok = "OKOKOK" });
			}
			catch (Exception)
			{

				throw;
			}


		}

		/// <summary>
		/// 抓取近幾期的中獎號碼
		/// </summary>
		/// <param name="winNumberResourceParameters"></param>
		/// <returns></returns>
		[HttpGet("GetlasterPeriod")]
		public ActionResult<IEnumerable<WinNumber>> GetlasterPeriod([FromQuery] WinNumberResourceParameters winNumberResourceParameters)
		{

			try
			{
				var Queryresult = _winNumberRepository.GetlasterPeriod(winNumberResourceParameters.lasterPeriod);


				return Ok(new { ok = Queryresult });
			}
			catch (Exception)
			{

				throw;
			}

		}

		/// <summary>
		/// 抓取近幾期至近幾期的中獎號碼
		/// </summary>
		/// <param name="winNumberResourceParameters"></param>
		/// <returns></returns>
		[HttpGet("GetPeriodFromoAndTo")]
		//參數類別化
		public ActionResult<IEnumerable<WinNumber>> GetPeriodFromoAndTo([FromQuery] WinNumberResourceParameters winNumberResourceParameters)
		{

			try
			{
				var Queryresult = _winNumberRepository.GetPeriodFromoAndTo(winNumberResourceParameters.FormPeriod, winNumberResourceParameters.ToPeriod);


				return Ok(new { ok = Queryresult });
			}
			catch (Exception)
			{

				throw;
			}

		}

		/// <summary>
		/// 抓取近幾期內 開出包含某數字 的 後面第幾期(單一期) 開出數字的統計數
		/// </summary>
		/// <param name="winNumberResourceParameters"></param>
		/// <returns></returns>
		[HttpGet("GetEasyDrawNumberStatisticsInlasterPeriodByNumber")]
		public ActionResult<IEnumerable<WinNumber>> GetEasyDrawNumberStatisticsInlasterPeriodByNumber([FromQuery] WinNumberResourceParameters winNumberResourceParameters)
		{
			//winNumberResourceParameters.lasterPeriod
			//winNumberResourceParameters.EasyDrawNumber1
			//winNumberResourceParameters.NextWhichPeriod
			try
			{
				if (winNumberResourceParameters.lasterPeriod == 0)
				{
					return Ok(new { ok = "請檢查是否輸入完整" });
				}
				if (winNumberResourceParameters.NextWhichPeriod == 0)
				{
					return Ok(new { ok = "請檢查是否輸入完整" });
				}
				var Queryresult = _winNumberRepository.GetlasterPeriod(winNumberResourceParameters.lasterPeriod);

				#region 處理傳入的EasyDrawNumber1陣列
				if (winNumberResourceParameters.EasyDrawNumber1 != null)
				{
					var EasyDrawNumber1List = new List<string>();//當期開獎要同時包含的數字們
					foreach (var item in winNumberResourceParameters.EasyDrawNumber1)
					{
						var EasyDrawNumber1ToString = item.ToString();
						if (item.ToString().Length == 1)
						{
							EasyDrawNumber1ToString = "0" + item.ToString();
						}
						EasyDrawNumber1List.Add(EasyDrawNumber1ToString);
					}
					var NextWhichPeriodIndexList = new List<int>();//要統計的期數List
					foreach (var item in Queryresult)
					{
						var ASCsList = new List<String>();
						ASCsList.Add(item.ASC1);
						ASCsList.Add(item.ASC2);
						ASCsList.Add(item.ASC3);
						ASCsList.Add(item.ASC4);
						ASCsList.Add(item.ASC5);

						var allcontain = false;
						var containcount = 0;
						foreach (var item1 in EasyDrawNumber1List)
						{
							if (ASCsList.Contains(item1))
							{
								containcount++;
							}

						}
						if (containcount == EasyDrawNumber1List.Count)
						{
							allcontain = true;
						}

						if (allcontain)
						{
							//如果本期開獎包含了 某數字，就去找他的後幾期(超過總期數不計入考慮)
							if (!((Queryresult.IndexOf(item) + winNumberResourceParameters.NextWhichPeriod) > (Queryresult.Count - 1)))
							{
								NextWhichPeriodIndexList.Add(Queryresult.IndexOf(item) + winNumberResourceParameters.NextWhichPeriod);
							}

						}

					}
					#endregion
					#region 單一數字的
					//var EasyDrawNumber1ToString = winNumberResourceParameters.EasyDrawNumber1.ToString();
					//if (winNumberResourceParameters.EasyDrawNumber1.ToString().Length == 1)
					//{
					//	EasyDrawNumber1ToString = "0" + winNumberResourceParameters.EasyDrawNumber1.ToString();
					//}
					//var NextWhichPeriodIndexList = new List<int>();//要統計的期數List
					//foreach (var item in Queryresult)
					//{
					//	if (item.ASC1.Equals(EasyDrawNumber1ToString) ||
					//		item.ASC2.Equals(EasyDrawNumber1ToString) ||
					//		item.ASC3.Equals(EasyDrawNumber1ToString) ||
					//		item.ASC4.Equals(EasyDrawNumber1ToString) ||
					//		item.ASC5.Equals(EasyDrawNumber1ToString))
					//	{
					//		//如果本期開獎包含了 某數字，就去找他的後幾期(超過總期數不計入考慮)
					//		if (!((Queryresult.IndexOf(item) + winNumberResourceParameters.NextWhichPeriod) > (Queryresult.Count - 1)))
					//		{
					//			NextWhichPeriodIndexList.Add(Queryresult.IndexOf(item) + winNumberResourceParameters.NextWhichPeriod);
					//		}

					//	}

					//}
					#endregion
					//開始統計
					var allNumberWithoutCount = new List<String>();
					foreach (var item in NextWhichPeriodIndexList)
					{
						allNumberWithoutCount.Add(Queryresult[item].ASC1);
						allNumberWithoutCount.Add(Queryresult[item].ASC2);
						allNumberWithoutCount.Add(Queryresult[item].ASC3);
						allNumberWithoutCount.Add(Queryresult[item].ASC4);
						allNumberWithoutCount.Add(Queryresult[item].ASC5);
					}
					var countDic = new Dictionary<string, int>();
					for (int i = 1; i < 40; i++)
					{
						var iString = i.ToString();
						if (iString.Length == 1)
						{
							iString = "0" + i.ToString();
						}
						if (allNumberWithoutCount.Contains(iString))
						{
							var howManyTimes = 0;
							foreach (var item in allNumberWithoutCount)
							{
								if (item == iString)
								{
									howManyTimes++;
								}
							}
							countDic.Add(iString, howManyTimes);
						}
					}
					return Ok(new { ok = countDic });
				}
				return Ok(new { ok = "請檢查是否輸入完整" });
			}
			catch (Exception)
			{

				throw;
			}

		}

		/// <summary>
		/// 抓取近幾期內 開出包含某尾數 的 後面第幾期(單一期) 開出數字的統計數
		/// </summary>
		/// <param name="winNumberResourceParameters"></param>
		/// <returns></returns>
		[HttpGet("GetEasyDrawNumberStatisticsInlasterPeriodByNumberTail")]
		public ActionResult<IEnumerable<WinNumber>> GetEasyDrawNumberStatisticsInlasterPeriodByNumberTail([FromQuery] WinNumberResourceParameters winNumberResourceParameters)
		{
			//winNumberResourceParameters.lasterPeriod
			//winNumberResourceParameters.EasyDrawNumber1  (這時候取尾數)
			//winNumberResourceParameters.NextWhichPeriod
			try
			{
				if (winNumberResourceParameters.lasterPeriod == 0)
				{
					return Ok(new { ok = "請檢查是否輸入完整" });
				}
				if (winNumberResourceParameters.NextWhichPeriod == 0)
				{
					return Ok(new { ok = "請檢查是否輸入完整" });
				}
				var Queryresult = _winNumberRepository.GetlasterPeriod(winNumberResourceParameters.lasterPeriod);

				#region 處理傳入的EasyDrawNumber1陣列
				if (winNumberResourceParameters.EasyDrawNumber1 != null)
				{
					var EasyDrawNumber1List = new List<string>();//當期開獎要同時包含的數字們
					foreach (var item in winNumberResourceParameters.EasyDrawNumber1)
					{
						var EasyDrawNumber1ToString = item.ToString();
						if (item.ToString().Length == 2)
						{
							EasyDrawNumber1ToString = item.ToString().Substring(1);
						}
						EasyDrawNumber1List.Add(EasyDrawNumber1ToString);
					}
					var NextWhichPeriodIndexList = new List<int>();//要統計的期數List
					foreach (var item in Queryresult)
					{
						var ASCsList = new List<String>();
						ASCsList.Add(item.ASC1.Substring(1));
						ASCsList.Add(item.ASC2.Substring(1));
						ASCsList.Add(item.ASC3.Substring(1));
						ASCsList.Add(item.ASC4.Substring(1));
						ASCsList.Add(item.ASC5.Substring(1));

						var allcontain = false;
						var containcount = 0;
						foreach (var item1 in EasyDrawNumber1List)
						{
							if (ASCsList.Contains(item1))
							{
								containcount++;
							}

						}
						if (containcount == EasyDrawNumber1List.Count)
						{
							allcontain = true;
						}

						if (allcontain)
						{
							//如果本期開獎包含了 某數字，就去找他的後幾期(超過總期數不計入考慮)
							if (!((Queryresult.IndexOf(item) + winNumberResourceParameters.NextWhichPeriod) > (Queryresult.Count - 1)))
							{
								NextWhichPeriodIndexList.Add(Queryresult.IndexOf(item) + winNumberResourceParameters.NextWhichPeriod);
							}

						}

					}
					#endregion
					#region 單一數字的
					//var EasyDrawNumber1ToString = winNumberResourceParameters.EasyDrawNumber1.ToString();
					//if (winNumberResourceParameters.EasyDrawNumber1.ToString().Length == 2)
					//{
					//	EasyDrawNumber1ToString = winNumberResourceParameters.EasyDrawNumber1.ToString().Substring(1);
					//}
					//var NextWhichPeriodIndexList = new List<int>();//要統計的期數List
					//foreach (var item in Queryresult)
					//{
					//	if (item.ASC1.Substring(1).Equals(EasyDrawNumber1ToString) ||
					//		item.ASC2.Substring(1).Equals(EasyDrawNumber1ToString) ||
					//		item.ASC3.Substring(1).Equals(EasyDrawNumber1ToString) ||
					//		item.ASC4.Substring(1).Equals(EasyDrawNumber1ToString) ||
					//		item.ASC5.Substring(1).Equals(EasyDrawNumber1ToString))
					//	{
					//		//如果本期開獎包含了 某尾數，就去找他的後幾期(超過總期數不計入考慮)
					//		if (!((Queryresult.IndexOf(item) + winNumberResourceParameters.NextWhichPeriod) > (Queryresult.Count - 1)))
					//		{
					//			NextWhichPeriodIndexList.Add(Queryresult.IndexOf(item) + winNumberResourceParameters.NextWhichPeriod);
					//		}

					//	}

					//}
					#endregion
					//開始統計
					var allNumberWithoutCount = new List<String>();
					foreach (var item in NextWhichPeriodIndexList)
					{
						allNumberWithoutCount.Add(Queryresult[item].ASC1);
						allNumberWithoutCount.Add(Queryresult[item].ASC2);
						allNumberWithoutCount.Add(Queryresult[item].ASC3);
						allNumberWithoutCount.Add(Queryresult[item].ASC4);
						allNumberWithoutCount.Add(Queryresult[item].ASC5);
					}
					var countDic = new Dictionary<string, int>();
					for (int i = 1; i < 40; i++)
					{
						var iString = i.ToString();
						if (iString.Length == 1)
						{
							iString = "0" + i.ToString();
						}
						if (allNumberWithoutCount.Contains(iString))
						{
							var howManyTimes = 0;
							foreach (var item in allNumberWithoutCount)
							{
								if (item == iString)
								{
									howManyTimes++;
								}
							}
							countDic.Add(iString, howManyTimes);
						}
					}
					return Ok(new { ok = countDic });
				}
				return Ok(new { ok = "請檢查是否輸入完整" });
			}
			catch (Exception)
			{

				throw;
			}

		}
		/// <summary>
		/// 查詢連莊的號碼 下一期開什麼
		/// </summary>
		/// <param name="winNumberResourceParameters"></param>
		/// <returns></returns>
		[HttpGet("GetSameDealerNumber")]
		public ActionResult<IEnumerable<WinNumber>> GetSameDealerNumber([FromQuery] WinNumberResourceParameters winNumberResourceParameters)
		{
			//winNumberResourceParameters.lasterPeriod
			//winNumberResourceParameters.EasyDrawNumber1  
			//winNumberResourceParameters.SameDealerNumberHowManyTimes (連莊期數)
			try
			{
				if (winNumberResourceParameters.lasterPeriod == 0)
				{
					return Ok(new { ok = "請檢查是否輸入完整" });
				}
				if (winNumberResourceParameters.SameDealerNumberHowManyTimes == 0)
				{
					return Ok(new { ok = "請檢查是否輸入完整" });
				}
				var Queryresult = _winNumberRepository.GetlasterPeriod(winNumberResourceParameters.lasterPeriod);

				#region 處理傳入的EasyDrawNumber1陣列
				if (winNumberResourceParameters.EasyDrawNumber1 != null)
				{
					var EasyDrawNumber1List = new List<string>();//當期開獎要同時包含的數字們
					foreach (var item in winNumberResourceParameters.EasyDrawNumber1)
					{
						var EasyDrawNumber1ToString = item.ToString();
						if (item.ToString().Length == 1)
						{
							EasyDrawNumber1ToString = "0" + item.ToString();
						}
						EasyDrawNumber1List.Add(EasyDrawNumber1ToString);
					}
					var NextWhichPeriodIndexList = new List<int>();//要統計的期數List

					for (int i = 0; i < Queryresult.Count(); i++)
					{
						if ((i + winNumberResourceParameters.SameDealerNumberHowManyTimes) < Queryresult.Count())
						{

							//var howManyTimesDic = new Dictionary<String, List<string>>();
							var matchCondiction = false;
							var allcontain = 0;
							for (int howManyTimes = 0; howManyTimes < winNumberResourceParameters.SameDealerNumberHowManyTimes; howManyTimes++)
							{
								var ASCsList = new List<String>();
								ASCsList.Add(Queryresult[i + howManyTimes].ASC1);
								ASCsList.Add(Queryresult[i + howManyTimes].ASC2);
								ASCsList.Add(Queryresult[i + howManyTimes].ASC3);
								ASCsList.Add(Queryresult[i + howManyTimes].ASC4);
								ASCsList.Add(Queryresult[i + howManyTimes].ASC5);
								//howManyTimesDic.Add("ASCsList" + howManyTimes, ASCsList);

								//allcontain = 0;
								var containcount = 0;
								foreach (var item1 in EasyDrawNumber1List)
								{
									if (ASCsList.Contains(item1))
									{
										containcount++;
									}

								}
								if (containcount == EasyDrawNumber1List.Count)
								{
									allcontain++;
								}

							}

							if (allcontain == winNumberResourceParameters.SameDealerNumberHowManyTimes)
							{
								matchCondiction = true;
							}
							if (matchCondiction)
							{
								NextWhichPeriodIndexList.Add(i + winNumberResourceParameters.SameDealerNumberHowManyTimes);
							}
						}

					}
					//
					#endregion
					#region 單一數字的
					//var EasyDrawNumber1ToString = winNumberResourceParameters.EasyDrawNumber1.ToString();
					//if (winNumberResourceParameters.EasyDrawNumber1.ToString().Length == 2)
					//{
					//	EasyDrawNumber1ToString = winNumberResourceParameters.EasyDrawNumber1.ToString().Substring(1);
					//}
					//var NextWhichPeriodIndexList = new List<int>();//要統計的期數List
					//foreach (var item in Queryresult)
					//{
					//	if (item.ASC1.Substring(1).Equals(EasyDrawNumber1ToString) ||
					//		item.ASC2.Substring(1).Equals(EasyDrawNumber1ToString) ||
					//		item.ASC3.Substring(1).Equals(EasyDrawNumber1ToString) ||
					//		item.ASC4.Substring(1).Equals(EasyDrawNumber1ToString) ||
					//		item.ASC5.Substring(1).Equals(EasyDrawNumber1ToString))
					//	{
					//		//如果本期開獎包含了 某尾數，就去找他的後幾期(超過總期數不計入考慮)
					//		if (!((Queryresult.IndexOf(item) + winNumberResourceParameters.NextWhichPeriod) > (Queryresult.Count - 1)))
					//		{
					//			NextWhichPeriodIndexList.Add(Queryresult.IndexOf(item) + winNumberResourceParameters.NextWhichPeriod);
					//		}

					//	}

					//}
					#endregion
					//開始統計
					var allNumberWithoutCount = new List<String>();
					foreach (var item in NextWhichPeriodIndexList)
					{
						allNumberWithoutCount.Add(Queryresult[item].ASC1);
						allNumberWithoutCount.Add(Queryresult[item].ASC2);
						allNumberWithoutCount.Add(Queryresult[item].ASC3);
						allNumberWithoutCount.Add(Queryresult[item].ASC4);
						allNumberWithoutCount.Add(Queryresult[item].ASC5);
					}
					var countDic = new Dictionary<string, int>();
					for (int i = 1; i < 40; i++)
					{
						var iString = i.ToString();
						if (iString.Length == 1)
						{
							iString = "0" + i.ToString();
						}
						if (allNumberWithoutCount.Contains(iString))
						{
							var howManyTimes = 0;
							foreach (var item in allNumberWithoutCount)
							{
								if (item == iString)
								{
									howManyTimes++;
								}
							}
							countDic.Add(iString, howManyTimes);
						}
					}
					return Ok(new { ok = countDic });
				}
				return Ok(new { ok = "請檢查是否輸入完整" });
			}
			catch (Exception)
			{

				throw;
			}

		}

	}
}
