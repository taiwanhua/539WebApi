using TL539WebApi.DbContexts;
using TL539WebApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

namespace GitWebApi.Services
{
	public class WinNumberRepository : IWinNumberRepository
	{
		private readonly TL539WebApiContext _context;

		public WinNumberRepository(TL539WebApiContext context)
		{
			_context = context ?? throw new ArgumentNullException(nameof(context));
		}

		public int Add(WinNumber winNumber)
		{
			if (winNumber == null)
			{
				throw new ArgumentNullException(nameof(winNumber));
			}
			_context.WinNumbers.Add(winNumber);
			return winNumber.WinNumberID;
		}

		public void DeleteAll()
		{
			_context.RemoveRange(_context.WinNumbers.Where(x => (1 == 1)));
		}

		public void ReSeedTo0()
		{
			_context.Database.ExecuteSqlCommand("DBCC CHECKIDENT (\"WinNumbers\", RESEED, 0);");
		}

		public List<WinNumber> GetlasterPeriod(int lasterPeriod)
		{
			if (lasterPeriod == null)
			{
				throw new ArgumentNullException(nameof(lasterPeriod));
			}
			var lasterPeriodSqlParameter = new SqlParameter("lasterPeriod", lasterPeriod);
			var result = _context.WinNumbers.FromSqlRaw("SELECT TOP (@lasterPeriod) * FROM[TL539WebApi].[dbo].[WinNumbers] ORDER BY [Date] Desc", lasterPeriodSqlParameter).OrderBy(p => p.Date).ToList();
			return result;
		}

		public List<WinNumber> GetPeriodFromoAndTo(int From, int To)
		{
			if (From == null)
			{
				throw new ArgumentNullException(nameof(From));
			}
			if (To == null)
			{
				throw new ArgumentNullException(nameof(To));
			}
			var FromPeriodSqlParameter = new SqlParameter("From", From);
			var result = _context.WinNumbers.FromSqlRaw("SELECT TOP (@From) * FROM[TL539WebApi].[dbo].[WinNumbers] ORDER BY [Date] Desc", FromPeriodSqlParameter).OrderBy(p => p.Date).ToList();
			var filter = new List<WinNumber>();
			for (int i = 0; i < result.Count - To; i++)
			{
				filter.Add(result[i]);
			}
			return filter;
		}

		//--------------------------

		//public string Exists(string RemoteRepositoryUrl)
		//{
		//	if (string.IsNullOrEmpty(RemoteRepositoryUrl))
		//	{
		//		throw new ArgumentNullException(nameof(RemoteRepositoryUrl));
		//	}
		//	if (_context.remoteRepositories.Any(a => a.RemoteRepositoryUrl == RemoteRepositoryUrl.Trim()))
		//	{
		//		var remoteRepository = _context.remoteRepositories.Where(a => a.RemoteRepositoryUrl == RemoteRepositoryUrl.Trim()).Select(a => new RemoteRepository { ServerPath = a.ServerPath });
		//		return remoteRepository.FirstOrDefault().ServerPath;
		//	}
		//	else
		//	{
		//		return "";
		//	}
		//}



		//public IEnumerable<RemoteRepository> GetAll()
		//{
		//	return _context.remoteRepositories.ToList<RemoteRepository>();
		//}



		//public void Delete(ChangeFormRecord changeFormRecord)
		//{

		//	if (changeFormRecord == null)
		//	{
		//		throw new ArgumentNullException(nameof(changeFormRecord));
		//	}

		//	_context.changeFormRecords.Remove(changeFormRecord);
		//}

		//public ChangeFormRecord Get(Guid ChangeFormRecordId)
		//{
		//	if (ChangeFormRecordId == Guid.Empty)
		//	{
		//		throw new ArgumentNullException(nameof(ChangeFormRecordId));
		//	}
		//	return _context.changeFormRecords.FirstOrDefault(a => a.ChangeFormRecordID == ChangeFormRecordId);
		//}



		public bool Save()
		{
			return (_context.SaveChanges() >= 0);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				// dispose resources when needed
			}
		}

	}
}
