using System;
using System.Text;
using Microsoft.CodeAnalysis;

namespace ElectronicObserver.SourceGenerators
{
	[Generator]
	public class BuildInfoGenerator : ISourceGenerator
	{
		public void Execute(GeneratorExecutionContext context)
		{
			StringBuilder sourceBuilder = new StringBuilder($@"
using System;

namespace ElectronicObserver.Generated
{{
    public static class BuildInfo
    {{
        public static long TimeStamp => {DateTime.Now.Ticks};
    }}
}}");

			// inject the created source into the users compilation
			context.AddSource("ElectronicObserver.Generated", sourceBuilder.ToString());
		}

		public void Initialize(GeneratorInitializationContext context)
		{
			// No initialization required for this one
		}
	}
}
