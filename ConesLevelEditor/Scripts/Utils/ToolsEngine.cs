using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Itibsoft.Utils.ToolsEngine
{
	public static class ToolsEngine
	{
		public static bool IsNull(object content)
		{
			if (content != null) return true;
			else return false;
		}
	}
}
