using System.Collections.Generic;
using TL539WebApi.Entities;

namespace GitWebApi.Services
{
	public interface IWinNumberRepository
	{
		int Add(WinNumber winNumber);
		void DeleteAll();
		void Dispose();
		List<WinNumber> GetlasterPeriod(int lasterPeriod);
		List<WinNumber> GetPeriodFromoAndTo(int From, int To);
		void ReSeedTo0();
		bool Save();
	}
}