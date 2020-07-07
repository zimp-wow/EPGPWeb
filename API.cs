using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BasicHttpServerCore;

namespace EPGPWeb
{
	public class API
	{
		private Logger.LogFunc Log = Logger.BuildClassLogger( "API" );

		[APIHandler( "POST", "uploadSnapshot" )]
		public async Task UploadSnapshot( HttpListenerContext context ) {
			Dictionary<string, string> receivedFiles = await FileUpload.HandleUpload( context );
			foreach( KeyValuePair<string, string> pair in receivedFiles ) {
				Log( "UploadSnapshot", $"{pair.Key} -> {pair.Value}" );
			}
		}
	}
}
