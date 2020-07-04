using System;
using System.IO;
using System.Threading.Tasks;
using BasicHttpServerCore;
using Newtonsoft.Json;

namespace EPGPWeb
{
	class Program
	{
		const string CONFIG_FILE = "config.json";

		private class ServerConfig {
			public string Prefix { get; set; } = "http://localhost:8080";
		}

		private static Logger.LogFunc Log = Logger.BuildClassLogger( "Program" );

		static async Task Main(string[] args)
		{
			Logger.Init( ( line ) => Console.WriteLine( line ) );

			ServerConfig config = new ServerConfig();
			if( !File.Exists( CONFIG_FILE ) ) {
				using( StreamWriter sw = new StreamWriter( new FileStream( CONFIG_FILE, FileMode.CreateNew, FileAccess.Write ) ) ) {
					await sw.WriteLineAsync( JsonConvert.SerializeObject( config ) );
				}

				Log( "Main", $"Created a new config file: { CONFIG_FILE }" );
			}
			else {
				using( StreamReader sr = new StreamReader( new FileStream( CONFIG_FILE, FileMode.Open, FileAccess.Read ) ) ) {
					string json = await sr.ReadToEndAsync();
					config = JsonConvert.DeserializeObject<ServerConfig>( json );
				}
			}

			string baseDir = Environment.CurrentDirectory;
			#if DEBUG
				baseDir += "\\..\\..\\..\\";
			#endif
			baseDir = Path.Combine( baseDir, "wwwroot" );

			BasicHttpServer server = new BasicHttpServer( config.Prefix );
			server.HandleFiles( baseDir );

			await server.Start();
		}
	}
}
