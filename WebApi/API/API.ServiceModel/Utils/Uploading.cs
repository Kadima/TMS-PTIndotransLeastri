using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using ServiceStack.ServiceHost;

namespace WebApi.ServiceModel.Utils
{
				[Route("/upload", "POST")]
				public class Uploading : IRequiresRequestStream
				{
								public Stream RequestStream { get; set; }
				}
}
