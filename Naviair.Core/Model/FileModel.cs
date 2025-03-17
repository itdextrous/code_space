using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Naviair.Core.Model
{
	public class FileModel
	{
        public string FileName { get; set; }
		public IFormFile File { get; set; }
    }
}
