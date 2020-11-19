using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JET.Launcher.Structures
{
	public class AccountInfo
	{
		public string id;
		public string nickname;
		public string email;
		public string password;
		public bool wipe;

		public AccountInfo()
		{
			id = "";
			nickname = "";
			email = "";
			password = "";
			wipe = false;
		}
	}
}
