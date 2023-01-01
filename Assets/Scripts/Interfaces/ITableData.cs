using System.Threading.Tasks;

namespace ProjectWilson
{
	interface ITableData
	{
		void Load();
		Task Wait();
		void Unload();
		void Validate();
	}
}