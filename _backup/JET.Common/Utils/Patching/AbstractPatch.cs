using System.Reflection;

namespace JET.Common.Utils.Patching
{
	public abstract class AbstractPatch
	{
		public string methodName;
		public BindingFlags flags;

		public abstract MethodInfo TargetMethod();
	}
}
