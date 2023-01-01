namespace ProjectWilson
{
	interface ISaveData
	{
		void Init();
		void Reset();
		object Serialize();
		void Deserialize(object objectData);
	}
}