using UnityEngine;
using System;
using System.IO;
using System.Data;
using System.Collections.Generic;
using ExcelDataReader;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace Krafton.SP2.X1.Lib
{
	public class JsonConverter 
	{	
		public delegate void ConversionToJsonSuccessfullHandler();
		public event ConversionToJsonSuccessfullHandler ConversionToJsonSuccessfull = delegate {};
		
		public delegate void ConversionToJsonFailedHandler();
		public event ConversionToJsonFailedHandler ConversionToJsonFailed = delegate {};
		
		/// <summary>
		/// Converts all excel and csv files in the input folder to json and saves them in the output folder.
		/// Each sheet within an excel file is saved to a separate json file with the same name as the sheet name.
		/// Files, sheets and columns whose name begin with '~' are ignored.
		/// </summary>
		/// <param name="inputFolderPath">Input folder path.</param>
		/// <param name="outputFolderPath">Output folder path.</param>
		/// <param name="convertOnlyModifiedFiles">If set to <c>true</c>, will only process modified files only.</param>
		public void ConvertFilesToJson(string inputFolderPath, string outputFolderPath, bool convertOnlyModifiedFiles = false)
		{
			List<string> excelFilePaths = GetExcelFilePathsInFolder(inputFolderPath);
			List<string> csvFilePaths = GetCSVFilePathsInFolder(inputFolderPath);
			Debug.Log($"Json Converter: {excelFilePaths.Count.ToString()} excel files, {csvFilePaths.Count.ToString()} csv files found.");
			
			if(convertOnlyModifiedFiles)
			{
				excelFilePaths = RemoveUnmodifiedExcelFiles(excelFilePaths, outputFolderPath);
				csvFilePaths = RemoveUnmodifiedCSVFiles(csvFilePaths, outputFolderPath);
				
				if(excelFilePaths.Count == 0)
					Debug.Log("Json Converter: No updates to excel files since last conversion.");
				else
					Debug.Log($"Json Converter: {excelFilePaths.Count.ToString()} excel files updated/added since last conversion.");

				if(csvFilePaths.Count == 0)
					Debug.Log("Json Converter: No updates to csv files since last conversion.");
				else
					Debug.Log($"Json Converter: {csvFilePaths.Count.ToString()} csv files updated/added since last conversion.");
			}
			
			bool isSuccess = true;
			for(int i = 0 ; i < excelFilePaths.Count; i++)
			{
				if(!ConvertExcelFileToJson(excelFilePaths[i], outputFolderPath))
				{
					isSuccess = false;
					break;
				}
			}
			for(int i = 0 ; i < csvFilePaths.Count; i++)
			{
				if(!ConvertCSVFileToJson(csvFilePaths[i], outputFolderPath))
				{
					isSuccess = false;
					break;
				}
			}
			if(isSuccess)
			{
				ConversionToJsonSuccessfull();
			}
			else
			{
				ConversionToJsonFailed();
			}
		}
		
		/// <summary>
		/// Gets all the file names in the specified folder
		/// </summary>
		/// <returns>The excel file names in folder.</returns>
		/// <param name="folderPath">Folder path.</param>
		private List<string> GetExcelFilePathsInFolder(string folderPath)
		{
			// Regular expression to match against 3 excel file types (xls & xlsx & xlsb), ignoring
			// files with extension .meta and starting with ~$ (temp file created by excel when fie
			Regex excelFileNameRegex = new Regex(@"^((?!(~\$)).*\.(xlsx|xlsb|xls$))$");
			
			List<string> excelFilePaths = new List<string>();

			string[] filePaths = Directory.GetFiles(folderPath, "*.xls*");
			for(int i = 0; i < filePaths.Length; i++)
			{
				string fileName = filePaths[i].Substring(filePaths[i].LastIndexOf('\\') + 1);				
				if(excelFileNameRegex.IsMatch(fileName))
					excelFilePaths.Add(filePaths[i]);
			}
			
			return excelFilePaths;
		}
		
		/// <summary>
		/// Removes files which have not been modified since they were last processed
		/// from the process list
		/// </summary>
		/// <param name="excelFilePaths">Excel file paths.</param>
		private List<string> RemoveUnmodifiedExcelFiles(List<string> excelFilePaths, string outputFolderPath)
		{	
			// ignore sheets whose name starts with '~'
			Regex skipSheetNameRegex = new Regex(@"^~.*$");
			
			for(int i = excelFilePaths.Count - 1; i >= 0; i--)
			{
				List<string> excelSheetNames = GetExcelSheetNamesInExcelFile(excelFilePaths[i]);

				bool isModified = false;
				for(int j = 0; j < excelSheetNames.Count; j++)
				{
					if(skipSheetNameRegex.IsMatch(excelSheetNames[j]))
						continue;
					
					string outputFile = $"{outputFolderPath}/{excelSheetNames[j]}.json";
					if(!File.Exists(outputFile) || File.GetLastWriteTimeUtc(outputFile) < File.GetLastWriteTimeUtc(excelFilePaths[i]))
					{
						isModified = true;
						break;
					}
				}				
				if(!isModified)
					excelFilePaths.RemoveAt(i);
			}
			
			return excelFilePaths;
		}
		
		/// <summary>
		/// Gets the list of sheet names in the specified excel file
		/// </summary>
		/// <returns>The sheet names in file.</returns>
		/// <param name="excelFilePath">Excel File path.</param>
		private List<string> GetExcelSheetNamesInExcelFile(string excelFilePath)
		{
			// Create regular expressions to detect the type of excel file
			Regex xlsRegex = new Regex(@"^(.*\.(xls$))");
			Regex xlsxRegex = new Regex(@"^(.*\.(xlsx$))");
			Regex xlsbRegex = new Regex(@"^(.*\.(xlsb$))");
			
			if(!xlsRegex.IsMatch(excelFilePath) && !xlsxRegex.IsMatch(excelFilePath) && !xlsbRegex.IsMatch(excelFilePath))
			{
				Debug.LogError($"Json Converter: Unexpected files type: {excelFilePath}");
				return null;
			}

			using(FileStream stream = File.Open(excelFilePath, FileMode.Open, FileAccess.Read))
			{
				List<string> excelSheetNames = new List<string>();
				
				// Create the excel data reader
				IExcelDataReader excelDataReader = ExcelReaderFactory.CreateReader(stream);
				if(excelDataReader == null)
					return excelSheetNames;
				
				do
				{
					// Add the sheet name to the list
					string sheetName = excelDataReader.Name.Replace(" ", string.Empty);
					int index = sheetName.IndexOf('~');
					if(index >= 0)
						sheetName = sheetName.Substring(0, index);
					excelSheetNames.Add(sheetName);
				}
				while(excelDataReader.NextResult()); // Read the next sheet

				return excelSheetNames;
			}
		}
		
		/// <summary>
		/// Converts each sheet in the specified excel file to json and saves them in the output folder.
		/// The name of the processed json file will match the name of the excel sheet. Ignores
		/// sheets whose name begin with '~'. Also ignores columns whose names begin with '~'.
		/// </summary>
		/// <returns><c>true</c>, if excel file was successfully converted to json, <c>false</c> otherwise.</returns>
		/// <param name="excelFilePath">Excel file path.</param>
		/// <param name="outputFolderPath">Output folder path.</param>
		public bool ConvertExcelFileToJson(string excelFilePath, string outputFolderPath)
		{
			Debug.Log($"Json Converter: Processing: {excelFilePath}");

			DataSet excelDataSet = GetExcelDataSet(excelFilePath);
			if(excelDataSet == null)
			{
				Debug.LogError($"Json Converter: Failed to process file: {excelFilePath}");
				return false;
			}

			Dictionary<string, List<DataTable>> dataTables = new Dictionary<string, List<DataTable>>();
			for(int i = 0; i < excelDataSet.Tables.Count; i++)
			{
				string tableName = excelDataSet.Tables[i].TableName.Replace(" ", string.Empty);
				int index = tableName.IndexOf('~');
				if(index >= 0)
					tableName = tableName.Substring(0, index);
				if(!dataTables.ContainsKey(tableName))
					dataTables.Add(tableName, new List<DataTable>());
				dataTables[tableName].Add(excelDataSet.Tables[i]);
			}
			foreach(var pair in dataTables)
			{
				for(int i = 1; i < pair.Value.Count; i++)
				{
					if(pair.Value[i].Rows.Count == 0)
						continue;
					
					int count = Mathf.Min(pair.Value[0].Columns.Count, pair.Value[i].Columns.Count);
					DataTable dataTable = null;
					for(int j = 0; j < count; j++)
					{
						if(pair.Value[i].Columns[j].DataType == typeof(System.Object) && pair.Value[i].Columns[j].DataType != pair.Value[0].Columns[j].DataType)
						{
							if(dataTable == null)
								dataTable = pair.Value[0].Clone();
							dataTable.Columns[j].DataType = typeof(System.Object);
						}
					}
					if(dataTable != null)
					{
						dataTable.Merge(pair.Value[0], true, MissingSchemaAction.Ignore);
						pair.Value[0] = dataTable;
					}
					pair.Value[0].Merge(pair.Value[i], true, MissingSchemaAction.Ignore);
				}
			}

			// Regex skipSheetNameRegex = new Regex(@"^~.*$");
			// Process Each SpreadSheet in the excel file
			foreach(var pair in dataTables)
			{
				//if(skipSheetNameRegex.IsMatch(pair.Key))
				//	continue;

				string spreadSheetJson = GetSpreadSheetJson(pair.Value[0]);
				if(String.IsNullOrEmpty(spreadSheetJson))
				{
					Debug.LogError($"Json Converter: Failed to covert Spreadsheet '{pair.Key}' to json.");
					return false;
				}
				else
				{
					WriteTextToFile(spreadSheetJson, $"{outputFolderPath}/{pair.Key}.json");
					Debug.Log($"Json Converter: {pair.Key} successfully written to file.");
				}
			}
			
			return true;
		}
		
		/// <summary>
		/// Gets the Excel data from the specified file
		/// </summary>
		/// <returns>The excel data set or null if file is invalid.</returns>
		/// <param name="excelFilePath">Excel file path.</param>
		private DataSet GetExcelDataSet(string excelFilePath)
		{
			// Create regular expressions to detect the type of excel file
			Regex xlsRegex = new Regex(@"^(.*\.(xls$))");
			Regex xlsxRegex = new Regex(@"^(.*\.(xlsx$))");
			Regex xlsbRegex = new Regex(@"^(.*\.(xlsb$))");

			if(!xlsRegex.IsMatch(excelFilePath) && !xlsxRegex.IsMatch(excelFilePath) && !xlsbRegex.IsMatch(excelFilePath))
			{
				Debug.LogError($"Json Converter: Unexpected files type: {excelFilePath}");
				return null;
			}

			using(FileStream fileStream = File.Open(excelFilePath, FileMode.Open, FileAccess.Read))
			{
				// Create the excel data reader
				IExcelDataReader excelDataReader = ExcelReaderFactory.CreateReader(fileStream);
				if(excelDataReader == null)
					return null;
			
				Regex skipNameRegex = new Regex(@"^~.*$");

				// Get the data from the excel file
				DataSet dataSet = excelDataReader.AsDataSet(new ExcelDataSetConfiguration()
				{
					FilterSheet = (tableReader, sheetIndex) =>
					{
						if(skipNameRegex.IsMatch(tableReader.Name))
							return false;
						return true;
					},
					ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
					{
						UseHeaderRow = true,
						FilterColumn = (rowReader, columnIndex) =>
						{
							string value = rowReader.GetString(columnIndex);
							if(string.IsNullOrEmpty(value) || skipNameRegex.IsMatch(value))
								return false;
							return true;
						},
						FilterRow = (rowReader) =>
						{
							bool skip = true;
							for(int i = 0; i < rowReader.FieldCount; i++)
							{
								object value = rowReader.GetValue(i);
								if(value != null)
								{
									skip = false;
									break;
								}
							}
							if(skip)
								return false;
							return true;
						},
					}
				});
				
				return dataSet;
			}
		}
		
		/// <summary>
		/// Gets the json data for the specified spreadsheet in the specified DataSet
		/// </summary>
		/// <returns>The spread sheet json.</returns>
		/// <param name="dataTable">Data table.</param>
		private string GetSpreadSheetJson(DataTable dataTable)
		{
			// Serialze the data table to json string
			return JsonConvert.SerializeObject(dataTable, Formatting.Indented);
		}
		
		/// <summary>
		/// Gets all the file names in the specified folder
		/// </summary>
		/// <returns>The CSV file names in folder.</returns>
		/// <param name="folderPath">Folder path.</param>
		private List<string> GetCSVFilePathsInFolder(string folderPath)
		{
			// Regular expression to match against 3 excel file types (xls & xlsx & xlsb), ignoring
			// files with extension .meta and starting with ~$ (temp file created by excel when fie
			Regex csvFileNameRegex = new Regex(@"^((?!(~\$)).*\.csv)$");

			List<string> csvFilePaths = new List<string>();
			
			string[] filePaths = Directory.GetFiles(folderPath, "*.csv");
			for(int i = 0; i < filePaths.Length; i++)
			{
				string fileName = filePaths[i].Substring(filePaths[i].LastIndexOf('\\') + 1);				
				if(csvFileNameRegex.IsMatch(fileName))
					csvFilePaths.Add(filePaths[i]);
			}
			
			return csvFilePaths;
		}

		/// <summary>
		/// Removes files which have not been modified since they were last processed
		/// from the process list
		/// </summary>
		/// <param name="csvFilePaths">CSV file paths.</param>
		private List<string> RemoveUnmodifiedCSVFiles(List<string> csvFilePaths, string outputFolderPath)
		{
			for(int i = csvFilePaths.Count - 1; i >= 0; i--)
			{
				string csvFileName = csvFilePaths[i].Substring(csvFilePaths[i].LastIndexOf('\\') + 1).Replace(".csv", string.Empty).Replace(".CSV", string.Empty);
				string outputFile = $"{outputFolderPath}/{csvFileName}.json";

				if(File.Exists(outputFile) && File.GetLastWriteTimeUtc(outputFile) > File.GetLastWriteTimeUtc(csvFilePaths[i]))
					csvFilePaths.RemoveAt(i);
			}
			
			return csvFilePaths;
		}

		/// <summary>
		/// Converts csv file to json and saves them in the output folder.
		/// The name of the processed json file will match the name of the csv file.
		/// Ignores columns whose names begin with '~'.
		/// </summary>
		/// <returns><c>true</c>, if csv file was successfully converted to json, <c>false</c> otherwise.</returns>
		/// <param name="csvFilePath">File path.</param>
		/// <param name="outputFolderPath">Output path.</param>
		public bool ConvertCSVFileToJson(string csvFilePath, string outputFolderPath)
		{
			Debug.Log($"Json Converter: Processing: {csvFilePath}");

			List<Dictionary<string, object>> csvDataSet = GetCSVDataSet(csvFilePath);
			if(csvDataSet == null)
			{
				Debug.LogError($"Json Converter: Failed to process file: {csvFilePath}");
				return false;
			}

			string csvJson = GetCSVJson(csvDataSet);
			if(String.IsNullOrEmpty(csvJson))
			{
				Debug.LogError($"Json Converter: Failed to covert CSV '{csvFilePath}' to json.");
				return false;
			}
			else
			{
				string fileName = csvFilePath.Substring(csvFilePath.LastIndexOf('\\') + 1).Replace(".csv", string.Empty).Replace(".CSV", string.Empty);
				WriteTextToFile(csvJson, $"{outputFolderPath}/{fileName}.json");
				Debug.Log($"Json Converter: {csvFilePath} successfully written to file.");
			}
			
			return true;
		}

		/// <summary>
		/// Gets the CSV data from the specified file
		/// </summary>
		/// <returns>The csv data set or null if file is invalid.</returns>
		/// <param name="csvFilePath">CSV file path.</param>
		private List<Dictionary<string, object>> GetCSVDataSet(string csvFilePath)
		{
			// Create regular expressions to detect the type of excel file
			Regex csvRegex = new Regex(@"^(.*\.csv)");
				
			if(!csvRegex.IsMatch(csvFilePath))
			{
				Debug.LogError("Json Converter: Unexpected files type: " + csvFilePath);
				return null;
			}

			var csv = new List<List<string>>();
			var lines = File.ReadAllLines(csvFilePath);
			foreach(var line in lines)
			{
				csv.Add(new List<string>());

				string[] splitedLine = line.Split(',');
				string wholeLine = string.Empty;
				for(int i = 0; i < splitedLine.Length; i++)
				{
					wholeLine += splitedLine[i];
					if((wholeLine[0] != '"' && wholeLine[wholeLine.Length - 1] != '"'))
					{
						csv[csv.Count - 1].Add(wholeLine);
						wholeLine = string.Empty;
					}
					else if((wholeLine[0] == '"' && wholeLine[wholeLine.Length - 1] == '"'))
					{
						csv[csv.Count - 1].Add(wholeLine.Substring(1, wholeLine.Length - 2));
						wholeLine = string.Empty;
					}
					else
						wholeLine += ",";
				}
			}

			List<Dictionary<string, object>> csvDataSet = new List<Dictionary<string, object>>();
			for(int i = 1; i < csv.Count; i++)
			{
				if(csv[0].Count != csv[i].Count)
					return null;

				Dictionary<string, object> data = new Dictionary<string, object>();
				for(int j = 0; j < csv[0].Count; j++)
				{
					if(csv[0][j][0] == '~')
						continue;
					float result;
					if(float.TryParse(csv[i][j], out result))
						data.Add(csv[0][j], result);
					else
						data.Add(csv[0][j], csv[i][j]);
				}
				csvDataSet.Add(data);
			}
				
			return csvDataSet;
		}		
		
		/// <summary>
		/// Gets the json data for the specified spreadsheet in the specified DataSet
		/// </summary>
		/// <returns>The spread sheet json.</returns>
		/// <param name="dataSet">Data set.</param>
		private string GetCSVJson(List<Dictionary<string, object>> dataSet)
		{
			// Serialze the data table to json string
			return JsonConvert.SerializeObject(dataSet, Formatting.Indented);
		}
		
		/// <summary>
		/// Writes the specified text to the specified file, overwriting it.
		/// Creates file if it does not exist.
		/// </summary>
		/// <param name="text">Text.</param>
		/// <param name="filePath">File path.</param>
		private void WriteTextToFile(string text, string filePath)
		{
			System.IO.File.WriteAllText(filePath, text);
		}
	}
}